using Acr.UserDialogs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Xpert;
using Xpert.Common.WSClient.Helpers;
using XpertMobileApp.Api;
using XpertMobileApp.Api.Managers;
using XpertMobileApp.DAL;
using XpertMobileApp.Models;
using XpertMobileApp.Services;
using XpertMobileApp.SQLite_Managment;
using XpertMobileApp.ViewModels;

namespace XpertMobileApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class TransfertStockDetail : ContentPage
    {

        private View_STK_TRANSFERT item;
        public View_STK_TRANSFERT Item
        {
            get { return item; }
            set { item = value; }
        }


        ItemRowsDetailViewModel<STK_TRANSFERT, View_STK_TRANSFERT_DETAIL> viewModel;

        public TransfertStockDetail(View_STK_TRANSFERT item)
        {
            InitializeComponent();

            this.Item = item;
            bool hasPermission = AppManager.HasPermission(XpertObjets.STK_TRANSFERT, XpertActions.AcUpdate); 
            if (hasPermission && App.Online)
            {
                validerTransfert.IsVisible = true;
            }
        }

        public TransfertStockDetail(string codeTransfert)
        {
            InitializeComponent();


            this.Item = new View_STK_TRANSFERT();
            this.Item.CODE_TRANSFERT = codeTransfert;

        }

        protected async override void OnAppearing()
        {
            base.OnAppearing();
            try
            {
                if (this.viewModel == null)
                {
                    List<View_STK_TRANSFERT> TransfertInfos = new List<View_STK_TRANSFERT>();
                    if (App.Online)
                    {
                        TransfertInfos = await WebServiceClient.GetTransfertHeader(this.Item.CODE_TRANSFERT);

                    }
                    else
                    {
                        var obj = await SQLite_Manager.GetTransfertHeader(this.Item.CODE_TRANSFERT);
                        TransfertInfos.Add(obj);

                    }

                    STK_TRANSFERT objTransfer = new STK_TRANSFERT();

                    if (TransfertInfos != null && TransfertInfos.Count != 0)
                    {
                        objTransfer = TransfertInfos[0];
                    }

                    BindingContext = this.viewModel = new ItemRowsDetailViewModel<STK_TRANSFERT, View_STK_TRANSFERT_DETAIL>(objTransfer, Item.CODE_TRANSFERT);

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
                if (App.Online)
                {
                    viewModel.ItemRows.Clear();
                    var itemsC = await WebServiceClient.GetTransfertDetail(this.Item.CODE_TRANSFERT);

                    UpdateItemIndex(itemsC);

                    foreach (var itemC in itemsC)
                    {
                        viewModel.ItemRows.Add(itemC);
                    }
                    UserDialogs.Instance.HideLoading();
                }
                else
                {
                    viewModel.ItemRows.Clear();
                    var itemsC = await SQLite_Manager.GetTransfertDetail(this.Item.CODE_TRANSFERT);

                    UpdateItemIndex(itemsC);

                    foreach (var itemC in itemsC)
                    {
                        viewModel.ItemRows.Add(itemC);
                    }
                    UserDialogs.Instance.HideLoading();
                }
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

        private void RefreshView_Refreshing(object sender, EventArgs e)
        {

        }

        private async void ValidateTransfert(object sender, EventArgs e)
        {
            try
            {
                if (App.Online)
                {
                    if (Item != null)
                    {
                        Item.IS_VALIDATE = true;
                        if (AppManager.HasPermission(XpertObjets.STK_TRANSFERT, XpertActions.AcUpdate))
                        {
                            bool result = await CrudManager.StockTransfert.ValidateTransfert(Item);

                        }

                    }
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
    }
}