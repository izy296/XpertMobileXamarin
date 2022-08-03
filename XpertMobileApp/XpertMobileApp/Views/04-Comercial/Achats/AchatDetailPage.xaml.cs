using Acr.UserDialogs;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Xpert.Common.WSClient.Helpers;
using XpertMobileApp.DAL;
using XpertMobileApp.Services;
using XpertMobileApp.ViewModels;

namespace XpertMobileApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AchatDetailPage : ContentPage
    {
        ItemRowsDetailViewModel<View_ACH_DOCUMENT, View_ACH_DOCUMENT_DETAIL> viewModel;

        private View_ACH_DOCUMENT item;
        bool showBonusQty;
        public View_ACH_DOCUMENT Item
        {
            get { return item; }
            set { item = value; }
        }
        public AchatDetailPage(View_ACH_DOCUMENT achat)
        {
            InitializeComponent();

            this.Item = achat;

            BindingContext = this.viewModel = new ItemRowsDetailViewModel<View_ACH_DOCUMENT, View_ACH_DOCUMENT_DETAIL>(achat, achat.CODE_DOC);

            this.viewModel.LoadRowsCommand = new Command(async () => await ExecuteLoadRowsCommand());

            //// TODO put into th generic view model 
            //MessagingCenter.Subscribe<SessionsViewModel, View_VTE_VENTE>(this, MCDico.REFRESH_ITEM, async (obj, item) =>
            //{
            //    // viewModel.Item = item;
            //});
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
                UserDialogs.Instance.ShowLoading(AppResources.txt_Loading);

                viewModel.ItemRows.Clear();
                List<View_ACH_DOCUMENT_DETAIL> itemsC = new List<View_ACH_DOCUMENT_DETAIL>();
                //List<View_ACH_DOCUMENT_DETAIL> itemsFromOffline = new List<View_ACH_DOCUMENT_DETAIL>();
                if (App.Online)
                {
                    itemsC = await WebServiceClient.GetAchatsDetails(this.Item.CODE_DOC);
                    this.viewModel.Title = this.Item.TIERS_TITLE;
                }
                else
                {
                    //itemsFromOffline = await SQLite_Manager.getVenteDetails(this.Item.CODE_DOC);
                }

                //UpdateItemIndex(itemsC);
                //ClearPrinterDetails();
                if (App.Online)
                {
                    foreach (var itemC in itemsC)
                    {
                        if (itemC.PSYCHOTHROPE)
                        {
                            //afficher le lot psycho
                            //InfosPsyco.IsVisible = true;
                        }
                        viewModel.ItemRows.Add(itemC);
                        //AddItemPrinterDetails(itemC);
                    }
                }
                else
                {
                    //foreach (var itemC in itemsFromOffline)
                    //{
                    //    if (itemC.PSYCHOTHROPE == 1)
                    //    {
                    //        InfosPsyco.IsVisible = true;
                    //    }
                    //    viewModel.ItemRows.Add(new View_VTE_JOURNAL_DETAIL
                    //    {
                    //        DESIGNATION = itemC.DESIGNATION_PRODUIT,
                    //        QUANTITE = itemC.QUANTITE,
                    //        PRIX_VENTE = itemC.PRIX_VTE_TTC,
                    //        MT_VENTE = itemC.MT_TTC,
                    //    });
                    //    AddItemPrinterDetails(itemC);
                    //}
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

                IsBusy = false;
            }
        }

        private async void RefreshView_Refreshing(object sender, EventArgs e)
        {
            await ExecuteLoadRowsCommand();
        }
    }
}