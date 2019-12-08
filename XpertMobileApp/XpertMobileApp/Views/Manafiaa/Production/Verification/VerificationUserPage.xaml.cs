using Acr.UserDialogs;
using Rg.Plugins.Popup.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Xpert.Common.WSClient.Helpers;
using XpertMobileApp.Api.ViewModels;
using XpertMobileApp.DAL;
using XpertMobileApp.Helpers;
using XpertMobileApp.Models;
using XpertMobileApp.Services;
using XpertMobileApp.ViewModels;
using ZXing.Net.Mobile.Forms;

namespace XpertMobileApp.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class VerificationUserPage : ContentPage
	{
        VerifTiersViewModel viewModel;

        public VerificationUserPage(string codeTiers)
        {
            InitializeComponent();
            TiersSelector = new TiersSelector();
            BindingContext = this.viewModel = new VerifTiersViewModel();

            MessagingCenter.Subscribe<TiersSelector, View_TRS_TIERS>(this, MCDico.ITEM_SELECTED, async (obj, selectedItem) =>
            {
                Device.BeginInvokeOnMainThread(() =>
                {
                    viewModel.Tiers = selectedItem;
                });
            });
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
        }

        private async void DisplayProductionDetails(string text)
        {
            try
            {
                string[] str = text.Split('-');
                if (str.Count() > 0)
                {
                    string CodeTiers = str[1];
                    string CodeDoc = str[0];
                    await viewModel.ExecuteLoadProdInfosCommand(CodeDoc);
                    if(viewModel.ProductionInfos != null)
                    { 
                        btn_Livrer.IsVisible = viewModel.ProductionInfos.Delevred;
                        if (viewModel.ProductionInfos?.CODE_TIERS == viewModel.Tiers?.CODE_TIERS && viewModel.Tiers != null
                            && viewModel.ProductionInfos.TOTAL_RESTE <= 0)
                        {
                            Header_RowScan.BackgroundColor = Color.FromHex("#87D37C");
                        }
                        else
                        { 
                          Header_RowScan.BackgroundColor = Color.FromHex("#ff4d4d");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                await UserDialogs.Instance.AlertAsync(WSApi2.GetExceptionMessage(ex), AppResources.alrt_msg_Alert,
    AppResources.alrt_msg_Ok);
            }
        }

        private async void DisplayTiersDetails(string text)
        {
            string[] str = text.Split('-');
            if(str.Count() > 0)
            {
                string CodeTiers = str[1];
                string CodeDoc = str[0];
                await viewModel.ExecuteLoadTiersCommand(CodeTiers);
            }
        }

        private TiersSelector TiersSelector;
        private async void btn_Select_Clicked(object sender, EventArgs e)
        {
            TiersSelector.SearchedType = "CF";
            await PopupNavigation.Instance.PushAsync(TiersSelector);
        }

        private async void btn_ScanTiers_Clicked(object sender, EventArgs e)
        {

        }

        private void BidonScan_Clicked(object sender, EventArgs e)
        {
            var scaner = new ZXingScannerPage();
            Navigation.PushAsync(scaner);
            scaner.OnScanResult += (result) =>
            {
                scaner.IsScanning = false;
                Device.BeginInvokeOnMainThread(async () =>
                {
                    await Navigation.PopAsync();

                    DisplayProductionDetails(result.Text);
                });
            };
        }

        private async void btn_Scan_Clicked(object sender, EventArgs e)
        {
            var scaner = new ZXingScannerPage();
            await Navigation.PushAsync(scaner);
            scaner.OnScanResult += (result) =>
            {
                scaner.IsScanning = false;
                Device.BeginInvokeOnMainThread(async () =>
                {
                    await Navigation.PopAsync();

                    DisplayTiersDetails(result.Text);

                });
            };
        }

        private async void btn_RowScanTiers_Clicked(object sender, EventArgs e)
        {
            var scaner = new ZXingScannerPage();
            await Navigation.PushAsync(scaner);
            scaner.OnScanResult += (result) =>
            {
                scaner.IsScanning = false;
                Device.BeginInvokeOnMainThread(async () =>
                {
                    await Navigation.PopAsync();

                    DisplayTiersDetails(result.Text);

                });
            };
        }

        private async void btn_Livrer_Clicked(object sender, EventArgs e)
        {
            bool result = false;

            Button btn = (sender as Button);

            string codeDocRecept = viewModel.ProductionInfos.CODE_DOC_RECEPTION;
            string codeDocDetail = viewModel.ProductionInfos.CODE_DOC_DETAIL;
            
            if (IsBusy)
                return;

            IsBusy = true;

            try
            {
                UserDialogs.Instance.ShowLoading(AppResources.txt_Loading);

                result = await WebServiceClient.LivrerProduction(codeDocRecept, codeDocDetail);

                if (result)
                {
                    btn.IsVisible = false;
                    // await UserDialogs.Instance.AlertAsync("La quantité produite a bien été enregistré!", AppResources.alrt_msg_Alert, AppResources.alrt_msg_Ok);
                }
            }
            catch (Exception ex)
            {
                await UserDialogs.Instance.AlertAsync(WSApi2.GetExceptionMessage(ex), AppResources.alrt_msg_Alert,
                    AppResources.alrt_msg_Ok);
            }
            finally
            {
                UserDialogs.Instance.HideLoading();
                IsBusy = false;
            }
        }
    }
}