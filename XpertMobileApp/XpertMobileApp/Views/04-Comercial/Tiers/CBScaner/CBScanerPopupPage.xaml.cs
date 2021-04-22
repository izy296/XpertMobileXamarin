using Acr.UserDialogs;
using Rg.Plugins.Popup.Pages;
using Rg.Plugins.Popup.Services;
using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Xpert.Common.WSClient.Helpers;
using XpertMobileApp.Api;
using XpertMobileApp.Api.Managers;
using XpertMobileApp.Api.Services;
using XpertMobileApp.DAL;
using XpertMobileApp.Helpers;
using XpertMobileApp.Services;
using XpertMobileApp.ViewModels;
using ZXing.Net.Mobile.Forms;
using static XpertMobileApp.DAL.SYS_CONFIG_TIROIRS;

namespace XpertMobileApp.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class CBScanerPopupPage : PopupPage
    {
        public event EventHandler<CBScanedEventArgs> CBScaned;
        public CBScanerPopupPage(View_TRS_TIERS item = null)
		{
			InitializeComponent ();
            BindingContext = this;
        }
        protected virtual void OnCBScaned(CBScanedEventArgs e)
        {
            EventHandler<CBScanedEventArgs> handler = CBScaned;
            if (handler != null)
            {
                handler(this, e);
            }
        }
        protected override void OnAppearing()
        {
            base.OnAppearing();

        }

        private async void Handle_OnScanResult(ZXing.Result result)
        {
            try
            {
                string cb = result.Text;
                Device.BeginInvokeOnMainThread(async () =>
                {
                    XpertHelper.PeepScan();
                    CBScanedEventArgs eventArgs = new CBScanedEventArgs();
                    eventArgs.CodeBarre = cb;
                    OnCBScaned(eventArgs);
                    await PopupNavigation.Instance.PopAsync();
                });
            }
            catch (Exception ex)
            {
                await UserDialogs.Instance.AlertAsync(WSApi2.GetExceptionMessage(ex), AppResources.alrt_msg_Alert, AppResources.alrt_msg_Ok);
            }
        }
    }
}
 