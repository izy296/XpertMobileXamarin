using Acr.UserDialogs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Xpert.Common.WSClient.Helpers;
using XpertMobileApp.DAL;
using XpertMobileApp.Models;
using XpertMobileApp.Services;
using XpertMobileApp.ViewModels;

namespace XpertMobileApp.Views._04_Comercial.Echange
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class EchangeDetailPage : ContentPage
    {
        ItemRowsDetailViewModel<View_STK_ECHANGE, View_VTE_VENTE_LOT> viewModel;
        private View_STK_ECHANGE item;
        public View_STK_ECHANGE Item
        {
            get { return item; }
            set { item = value; }
        }

        public EchangeDetailPage(View_STK_ECHANGE item)
        {
            InitializeComponent();
            this.Item = item;
            BindingContext = this.viewModel = new ItemRowsDetailViewModel<View_STK_ECHANGE, View_VTE_VENTE_LOT>(item, item.CODE_DOCUMENT);
            this.viewModel.LoadRowsCommand = new Command(async () => await LoadRowsCommandAsync());
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            viewModel.LoadRowsCommand.Execute(null);
        }

        async Task LoadRowsCommandAsync()
        {
            if (IsBusy) return;

            IsBusy = true;

            try
            {
                //Show Loading spinner...
                UserDialogs.Instance.ShowLoading(AppResources.txt_Loading);

                viewModel.ItemRows.Clear();

                List<View_VTE_VENTE_LOT> itemsEchangeDetail = new List<View_VTE_VENTE_LOT>();

                if (App.Online)
                {
                    itemsEchangeDetail = await WebServiceClient.GetEchangeDetail(this.Item.CODE_DOCUMENT, this.Item.TYPE_DOC);
                    foreach (var itemEchangeDetail in itemsEchangeDetail)
                    {
                        viewModel.ItemRows.Add(itemEchangeDetail);
                    }
                }

                UserDialogs.Instance.HideLoading();
            }
            catch (Exception e)
            {
                UserDialogs.Instance.HideLoading();
                await UserDialogs.Instance.AlertAsync(WSApi2.GetExceptionMessage(e), AppResources.alrt_msg_Alert, AppResources.alrt_msg_Ok);
            }
            finally
            {
                IsBusy = false;
            }
        }

        private async void RefreshView_Refreshing(object sender, EventArgs e)
        {
            await LoadRowsCommandAsync();
        }
    }
}