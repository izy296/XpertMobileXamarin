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
using XpertMobileApp.Api;
using XpertMobileApp.Api.Services;
using XpertMobileApp.DAL;
using XpertMobileApp.Services;
using XpertMobileApp.SQLite_Managment;
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

            HideFields();
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

        private void HideFields()
        {
            try
            {
                if (Constants.AppName == Apps.XCOM_Mob)
                {
                    labelShp.IsVisible = false;
                    labelPpa.IsVisible = false;
                }
            }
            catch (Exception ex)
            {
                throw ex;
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
                List<View_ACH_DOCUMENT_DETAIL> itemsC = new List<View_ACH_DOCUMENT_DETAIL>();
                if (App.Online)
                {
                    itemsC = await WebServiceClient.GetAchatsDetails(this.Item.CODE_DOC);
                    this.viewModel.Title = this.Item.TIERS_TITLE;
                    foreach (var item in itemsC)
                    {
                        viewModel.ItemRows.Add(item);
                    }
                }
                else
                {
                    // Get the details from the sqlite ....
                    var AchatList = await SQLite_Manager.GetAchatDetails(this.Item.CODE_DOC);
                    foreach (var item in AchatList)
                    {
                        viewModel.ItemRows.Add(XpertHelper.CloneObject<View_ACH_DOCUMENT_DETAIL>(item));
                    }
                }
                UserDialogs.Instance.HideLoading();
            }
            catch (Exception ex)
            {
                UserDialogs.Instance.HideLoading();
                await UserDialogs.Instance.AlertAsync(WSApi2.GetExceptionMessage(ex), AppResources.alrt_msg_Alert, AppResources.alrt_msg_Ok);
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