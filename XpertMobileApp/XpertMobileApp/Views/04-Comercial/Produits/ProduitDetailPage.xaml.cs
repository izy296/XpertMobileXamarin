using Acr.UserDialogs;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Xpert.Common.WSClient.Helpers;
using XpertMobileApp.DAL;
using XpertMobileApp.Models;
using XpertMobileApp.Services;
using XpertMobileApp.ViewModels;

namespace XpertMobileApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ProduitDetailPage : ContentPage
    {
        ItemRowsDetailViewModel<View_AssistantCommandes, View_STK_STOCK> viewModel;

        private STK_PRODUITS item;
        public STK_PRODUITS Item
        {
            get { return item; }
            set { item = value; }
        }

        public ProduitDetailPage(STK_PRODUITS prod)
        {
            InitializeComponent();

            this.Item = prod;
        }

        public ProduitDetailPage(string codeProd)
        {
            InitializeComponent();

            Item = new STK_PRODUITS();
            Item.CODE_PRODUIT = codeProd;
        }

        private string getFormatedValue(object value)
        {
            if (value == null) return "";

            string result = value.ToString();

            if (value is DateTime)
            {
                result = string.Format("{0:dd/MM/yyyy HH:mm}", value);
            }
            else if (value is decimal)
            {
                result = string.Format("{0:F2} DA", value);
            }
            else
            {
                result = string.Format("{0}", value);
            }

            return result;
        }

        async void Cancel_Clicked(object sender, EventArgs e)
        {
            await Navigation.PopAsync();
        }

        protected async override void OnAppearing()
        {
            base.OnAppearing();
            BindingContext = this;
            try
            {
                if (this.viewModel == null)
                {
                    var ProdInfos = await WebServiceClient.GetProduitDetails(this.Item.CODE_PRODUIT);

                    BindingContext = this.viewModel = new ItemRowsDetailViewModel<View_AssistantCommandes, View_STK_STOCK>(ProdInfos, Item.CODE_PRODUIT);

                    this.viewModel.LoadRowsCommand = new Command(async () => await ExecuteLoadRowsCommand());
                }
                viewModel.LoadRowsCommand.Execute(null);
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

        async Task ExecuteLoadRowsCommand()
        {
            if (IsBusy)
                return;

            IsBusy = true;

            try
            {
                UserDialogs.Instance.ShowLoading(AppResources.txt_Loading);
                viewModel.ItemRows.Clear();
                var itemsC = await WebServiceClient.GetLots(this.Item.CODE_PRODUIT);

                UpdateItemIndex(itemsC);

                foreach (var itemC in itemsC)
                {
                    viewModel.ItemRows.Add(itemC);
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

        private void UpdateItemIndex<T>(List<T> items)
        {
            int i = 0;
            foreach (var item in items)
            {
                i += 1;
                (item as BASE_CLASS).Index = i;
            }
        }

        private async void RefBtn_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new ProduitRefDetailPage(viewModel.Item.REFERENCE));
        }

        private async void RefreshView_Refreshing(object sender, EventArgs e)
        {
            await ExecuteLoadRowsCommand();
        }
    }
}