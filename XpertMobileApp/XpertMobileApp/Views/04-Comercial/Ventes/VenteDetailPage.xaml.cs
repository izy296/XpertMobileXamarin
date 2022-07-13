using Acr.UserDialogs;
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

        public string CodeVente=null;

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
                // une partie de code que permet de crassé l'objet vente pour afficher les informations
                // de vente envoyé par la notification avec la remplacement de Item avec l'objet avec
                // le codeVente donné et le changment de Titre de la page et la deuxiem appele
                // de function ExecuteLoadRowsCommand() pour le recuperation de detaile de nouvelle object
                if (CodeVente != null)
                {
                    UserDialogs.Instance.ShowLoading(AppResources.txt_Loading);
                    itemByCodeVente = await WebServiceClient.GetVente(CodeVente);
                    this.Item = itemByCodeVente[0];
                    viewModel.Item = this.Item;
                    viewModel.ItemId = CodeVente;
                    viewModel.Title = CodeVente;
                    CodeVente =null;
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
                vente.Details = Printerdetails;
                PrinterHelper.PrintBL(vente);
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
    }
}