using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace XpertMobileApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class UpdatePage : ContentPage
    {
        public UpdatePage()
        {
            InitializeComponent();
        }

        private async void btn_VersionUpdate_Clicked(object sender, EventArgs e)
        {
            var url = string.Empty;
            var appId = string.Empty;

            if (Device.RuntimePlatform == Device.iOS)
             {
                appId = AppInfo.PackageName;
                url = $"itms-apps://itunes.apple.com/WebObjects/MZStore.woa/wa/viewContentsUserReviews?id={appId}&amp;onlyLatestVersion=true&amp;pageNumber=0&amp;sortOrdering=1&amp;type=Purple+Software";
            }
            else if (Device.RuntimePlatform == Device.Android)
            {
                appId = AppInfo.PackageName;
                url = $"https://play.google.com/store/apps/details?id={appId}";
            }

            if (string.IsNullOrWhiteSpace(url))
                return;

            await Task.Run(() => { Device.OpenUri(new Uri(url)); });
        }
    }
}