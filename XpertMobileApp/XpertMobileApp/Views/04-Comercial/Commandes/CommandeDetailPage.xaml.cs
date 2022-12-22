using Acr.UserDialogs;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Xpert.Common.WSClient.Helpers;
using XpertMobileApp.Api;
using XpertMobileApp.DAL;
using XpertMobileApp.Helpers;
using XpertMobileApp.Services;
using XpertMobileApp.SQLite_Managment;
using XpertMobileApp.ViewModels;

namespace XpertMobileApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CommandeDetailPage : ContentPage
    {
        ItemRowsDetailViewModel<View_VTE_VENTE, View_VTE_VENTE_LOT> viewModel;

        private View_VTE_VENTE item;
        public View_VTE_VENTE Item
        {
            get { return item; }
            set { item = value; }
        }

        public string codeCommande { get; set; }

        public CommandeDetailPage(View_VTE_VENTE vente, string codeCommande = null)
        {
            InitializeComponent();

            this.Item = vente;
            this.codeCommande = codeCommande;
            if (vente != null)
            {
                BindingContext = this.viewModel = new ItemRowsDetailViewModel<View_VTE_VENTE, View_VTE_VENTE_LOT>(vente, vente.CODE_VENTE);

                viewModel.LoadRowsCommand = new Command(async () => await ExecuteLoadRowsCommand());
            }

            // TODO put into th generic view model 
            MessagingCenter.Subscribe<EncaissementsViewModel, View_VTE_VENTE>(this, MCDico.REFRESH_ITEM, async (obj, item) =>
            {
                // viewModel.Item = item;
            });
        }

        void Handle_ValueChanged(object sender, Syncfusion.SfNumericUpDown.XForms.ValueEventArgs e)
        {
            /*
            if (e.Value.ToString() == "0" || e.Value.ToString() == "0.0")
                appleAddButton.IsEnabled = false;
            else
                appleAddButton.IsEnabled = true;
            this.AppleCost.Text = "$" + Convert.ToDouble(e.Value) * 0.49;
            */
        }

        protected async override void OnAppearing()
        {
            base.OnAppearing();
            if (codeCommande != null)
                if (App.Online)
                {
                    var res = await WebServiceClient.GetCommande(codeCommande);
                    if (res != null && res.Count > 0)
                        this.Item = res[0];
                }
                else
                {
                    this.Item = await SQLite_Manager.GetCommande(codeCommande);
                }

            if (Constants.AppName == Apps.X_DISTRIBUTION)
            {
                ItemHeader.Text = Item.CODE_VENTE;
                DetailsHeader.IsVisible = false;
                Title = AppResources.pn_MyCommandes;
            }

            BindingContext = this.viewModel = new ItemRowsDetailViewModel<View_VTE_VENTE, View_VTE_VENTE_LOT>(Item, Item.CODE_VENTE);

            this.viewModel.LoadRowsCommand = new Command(async () => await ExecuteLoadRowsCommand());

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
                List<View_VTE_VENTE_LOT> itemsCommands = new List<View_VTE_VENTE_LOT>();
                List<View_VTE_VENTE_LIVRAISON> itemsCommandsDistrib = new List<View_VTE_VENTE_LIVRAISON>();

                if (App.Online)
                {
                    if (Constants.AppName == Apps.X_DISTRIBUTION)
                        itemsCommandsDistrib = await WebServiceClient.GetVenteLotLivraisonDetails(this.Item.CODE_VENTE);
                    else
                        itemsCommands = await WebServiceClient.GetCommandeDetails(this.Item.CODE_VENTE);
                }
                else
                {
                    /* Avoir les details de commandes */
                    UserDialogs.Instance.ShowLoading(AppResources.txt_Waiting);
                    itemsCommands = await SQLite_Manager.getVenteDetails(this.Item.CODE_VENTE);
                    UserDialogs.Instance.HideLoading();
                }
                UpdateItemIndex(itemsCommands);
                foreach (var itemC in itemsCommands)
                {
                    viewModel.ItemRows.Add(itemC);
                }
            }
            catch (Exception ex)
            {
                await UserDialogs.Instance.AlertAsync(WSApi2.GetExceptionMessage(ex), AppResources.alrt_msg_Alert,
                    AppResources.alrt_msg_Ok);
            }
            finally
            {
                IsBusy = false;
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

        private async void EditClicked(object sender, EventArgs e)
        {
            var client = await SQLite_Manager.GetClient(Item.CODE_TIERS);
            await Navigation.PushAsync(new NewCommandePage(Item, client));
        }
    }
}