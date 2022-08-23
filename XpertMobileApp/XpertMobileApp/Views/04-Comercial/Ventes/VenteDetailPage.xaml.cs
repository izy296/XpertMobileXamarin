using Acr.UserDialogs;
using Newtonsoft.Json;
using Rg.Plugins.Popup.Services;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Xpert.Common.WSClient.Helpers;
using XpertMobileApp.DAL;
using XpertMobileApp.Helpers;
using XpertMobileApp.Services;
using XpertMobileApp.SQLite_Managment;
using XpertMobileApp.ViewModels;
using XpertMobileApp.Views.Helper;
using XpertMobileSettingsPage.Helpers.Services;
using XpertWebApi.Models;

namespace XpertMobileApp.Views.Encaissement
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class VenteDetailPage : ContentPage
    {
        ItemRowsDetailViewModel<View_VTE_VENTE, View_VTE_JOURNAL_DETAIL> viewModel;

        private View_VTE_VENTE item;
        public View_VTE_VENTE Item
        {
            get { return item; }
            set { item = value; }
        }

        public string CodeVente = null;

        public List<View_VTE_VENTE> itemByCodeVente;
        public List<View_VTE_VENTE_LOT> Printerdetails { get; set; }
        public VenteDetailPage(View_VTE_VENTE vente, string codeVente = null)
        {
            InitializeComponent();

            CodeVente = codeVente;
            this.Item = vente;

            BindingContext = this.viewModel = new ItemRowsDetailViewModel<View_VTE_VENTE, View_VTE_JOURNAL_DETAIL>(vente, vente.CODE_VENTE);

            this.viewModel.LoadRowsCommand = new Command(async () => await ExecuteLoadRowsCommand());

            // TODO put into th generic view model 
            MessagingCenter.Subscribe<SessionsViewModel, View_VTE_VENTE>(this, MCDico.REFRESH_ITEM, async (obj, item) =>
            {
                // viewModel.Item = item;
            });
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            viewModel.LoadRowsCommand.Execute(null);
        }

        async Task ExecuteLoadRowsCommand()
        {

            if (IsBusy)
                return;

            IsBusy = true;

            try
            {
                // une partie de code qui permet d'écrasé l'objet vente pour afficher les informations
                // envoyé par la notification avec le remplacement d'Item avec l'objet avec
                // le codeVente donné et le changement du Titre de la page.
                // Le deuxiéme appelle
                // de la fonction ExecuteLoadRowsCommand() pour la recuperation de détail du nouvelle object
                if (CodeVente != null)
                {
                    UserDialogs.Instance.ShowLoading(AppResources.txt_Loading);
                    itemByCodeVente = await WebServiceClient.GetVente(CodeVente);
                    this.Item = itemByCodeVente[0];
                    viewModel.Item = this.Item;
                    viewModel.ItemId = CodeVente;
                    viewModel.Title = CodeVente;
                    CodeVente = null;
                    IsBusy = false;
                    await ExecuteLoadRowsCommand();
                    UserDialogs.Instance.HideLoading();
                    return;
                }
                //------------------------------------
                UserDialogs.Instance.ShowLoading(AppResources.txt_Loading);
                viewModel.ItemRows.Clear();
                List<View_VTE_JOURNAL_DETAIL> itemsC = new List<View_VTE_JOURNAL_DETAIL>();
                List<View_VTE_VENTE_LOT> itemsFromOffline = new List<View_VTE_VENTE_LOT>();
                if (App.Online)
                {
                    itemsC = await WebServiceClient.GetVenteDetails(this.Item.CODE_VENTE);

                }
                else
                {
                    itemsFromOffline = await SQLite_Manager.getVenteDetails(this.Item.CODE_VENTE);
                }



                UpdateItemIndex(itemsC);
                ClearPrinterDetails();
                if (App.Online)
                {
                    foreach (var itemC in itemsC)
                    {
                        if (itemC.PSYCHOTHROPE == 1)
                        {
                            InfosPsyco.IsVisible = true;
                        }
                        viewModel.ItemRows.Add(itemC);
                        AddItemPrinterDetails(itemC);
                    }
                }
                else
                {
                    foreach (var itemC in itemsFromOffline)
                    {
                        if (itemC.PSYCHOTHROPE == 1)
                        {
                            InfosPsyco.IsVisible = true;
                        }
                        viewModel.ItemRows.Add(new View_VTE_JOURNAL_DETAIL
                        {
                            DESIGNATION = itemC.DESIGNATION_PRODUIT,
                            QUANTITE = itemC.QUANTITE,
                            PRIX_VENTE = itemC.PRIX_VTE_TTC,
                            MT_VENTE = itemC.MT_TTC,
                        });
                        AddItemPrinterDetails(itemC);
                    }
                }


                UserDialogs.Instance.HideLoading();
            }
            catch (Exception ex)
            {
                UserDialogs.Instance.HideLoading();
                await UserDialogs.Instance.AlertAsync(WSApi2.GetExceptionMessage(ex), AppResources.alrt_msg_Alert,
                    AppResources.alrt_msg_Ok);
            }
            finally
            {
                UserDialogs.Instance.HideLoading();
                IsBusy = false;
            }
        }

        private void AddItemPrinterDetails(View_VTE_JOURNAL_DETAIL itemC)
        {
            Printerdetails.Add(new View_VTE_VENTE_LOT
            {
                DESIGNATION_PRODUIT = itemC.DESIGNATION,
                QUANTITE = itemC.QUANTITE,
                PRIX_VTE_TTC = itemC.PRIX_VENTE,
                MT_TTC = itemC.MT_VENTE
            });
        }

        private void AddItemPrinterDetails(View_VTE_VENTE_LOT itemC)
        {
            Printerdetails.Add(itemC);
        }

        private void ClearPrinterDetails()
        {
            if (this.Printerdetails == null)
            {
                this.Printerdetails = new List<View_VTE_VENTE_LOT>();
            }
            else
            {
                this.Printerdetails.Clear();
            }
        }

        private async void PrintAsync(object sender, EventArgs e)
        {
            View_VTE_VENTE vente = viewModel.Item;
            if (vente != null)
            {
                try
                {
                    string printerToUse = App.Settings.PrinterName;
                    if (App.Settings.EnableMultiPrinter)
                    {
                        List<XPrinter> Liste;
                        if (Manager.isJson(App.Settings.MultiPrinterList))
                        {
                            Liste = JsonConvert.DeserializeObject<List<XPrinter>>(App.Settings.MultiPrinterList);

                            if (Liste != null && Liste.Count != 0)
                            {
                                var popupPrinter = new MultiPrinterSelector(Liste);
                                await PopupNavigation.Instance.PushAsync(popupPrinter);
                                var resPop = await popupPrinter.PopupClosedTask;
                                if (resPop != "Null")
                                    printerToUse = resPop;
                            }
                            else await Application.Current.MainPage.DisplayAlert(AppResources.alrt_msg_Alert, AppResources.txt_Msg_List_Impremant_Vide, AppResources.alrt_msg_Ok);
                        }
                        else
                        {
                            await Application.Current.MainPage.DisplayAlert(AppResources.alrt_msg_Alert, AppResources.txt_Msg_List_Impremant_Vide, AppResources.alrt_msg_Ok);
                        }
                    }
                    UserDialogs.Instance.ShowLoading(AppResources.txt_Loading);
                    bool res = await WebServiceClient.printTicket(vente.CODE_VENTE, printerToUse);
                    UserDialogs.Instance.HideLoading();
                    if (res)
                    {
                        await UserDialogs.Instance.AlertAsync(AppResources.vdp_ImpressionSuccess, AppResources.alrt_msg_Alert,
        AppResources.alrt_msg_Ok);
                    }
                    else
                    {
                        await UserDialogs.Instance.AlertAsync(AppResources.vdp_ImpressionError, AppResources.alrt_msg_Alert,
        AppResources.alrt_msg_Ok);

                    }
                    //vente.Details = Printerdetails;
                    //PrinterHelper.PrintBL(vente);
                }
                catch (Exception ex)
                {
                    UserDialogs.Instance.HideLoading();
                    await UserDialogs.Instance.AlertAsync(WSApi2.GetExceptionMessage(ex), AppResources.alrt_msg_Alert,
AppResources.alrt_msg_Ok);

                }
            }
        }

        private void UpdateItemIndex<T>(List<T> items)
        {
            int i = 0;
            foreach (var item in items)
            {
                i += 1;
                (item as BASE_CLASS).Index = i;
            }
        }

        private async void RefreshView_Refreshing(object sender, EventArgs e)
        {
            await ExecuteLoadRowsCommand();
        }

        private async void CollectionView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var selected = e.CurrentSelection[0] as View_VTE_JOURNAL_DETAIL;
            ProduitDetailPage produitDetailPage = new ProduitDetailPage(selected.CODE_PRODUIT);
            await Navigation.PushAsync(produitDetailPage);
        }
    }
}