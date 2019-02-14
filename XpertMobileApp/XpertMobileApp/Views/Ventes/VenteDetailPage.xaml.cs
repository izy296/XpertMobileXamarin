using Acr.UserDialogs;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Xpert.Pharm.DAL;
using XpertMobileApp.DAL;
using XpertMobileApp.Helpers;
using XpertMobileApp.Models;
using XpertMobileApp.Services;
using XpertMobileApp.ViewModels;

namespace XpertMobileApp.Views.Encaissement
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class VenteDetailPage : ContentPage
	{
        ItemRowsDetailViewModel<View_VTE_VENTE, View_VTE_VENTE_LOT> viewModel;

        private View_VTE_VENTE item;
        public View_VTE_VENTE Item
        {
            get { return item; }
            set { item = value; }
        }

        public VenteDetailPage(View_VTE_VENTE vente)
        {
            InitializeComponent();

            this.Item = vente;

            BindingContext = this.viewModel = new ItemRowsDetailViewModel<View_VTE_VENTE, View_VTE_VENTE_LOT>(vente, vente.CODE_VENTE);

            this.viewModel.LoadRowsCommand = new Command(async () => await ExecuteLoadRowsCommand());

            // TODO put into th generic view model 
            MessagingCenter.Subscribe<EncaissementsViewModel, View_VTE_VENTE>(this, MCDico.REFRESH_ITEM, async (obj, item) =>
            {
                // viewModel.Item = item;
            });
        }

        public VenteDetailPage()
        {
            InitializeComponent();

            var item = new View_VTE_VENTE
            {
                NUM_VENTE = "Item 1",
                TITRE_VENTE = "This is a compte.."
            };

            viewModel = new ItemRowsDetailViewModel<View_VTE_VENTE, View_VTE_VENTE_LOT>(item, "");
            BindingContext = viewModel;
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
                viewModel.ItemRows.Clear();
                var itemsC = await WebServiceClient.GetVenteDetails(this.Item.CODE_VENTE);
                foreach (var itemC in itemsC)
                {
                    viewModel.ItemRows.Add(itemC);
                }
            }
            catch (Exception ex)
            {
                await UserDialogs.Instance.AlertAsync(ex.Message, AppResources.alrt_msg_Alert,
                    AppResources.alrt_msg_Ok);
            }
            finally
            {
                IsBusy = false;
            }
        }
    }
}