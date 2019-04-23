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
	public partial class ProduitRefDetailPage : ContentPage
	{
        ItemRowsDetailViewModel<View_AssistantCommandes, View_STK_STOCK> viewModel;

        private STK_PRODUITS item;
        public STK_PRODUITS Item
        {
            get { return item; }
            set { item = value; }
        }

        public ProduitRefDetailPage(string refProd)
        {
            InitializeComponent();

            Item = new STK_PRODUITS();
            Item.REFERENCE = refProd;
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

            try
            {
                if (this.viewModel == null)
                { 
                    var ProdInfos = await WebServiceClient.GetProduitRefDetails(this.Item.REFERENCE);

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
                viewModel.ItemRows.Clear();
                var itemsC = await WebServiceClient.GetLotsFromRef(this.Item.REFERENCE);

                UpdateItemIndex(itemsC);

                foreach (var itemC in itemsC)
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
    }
}