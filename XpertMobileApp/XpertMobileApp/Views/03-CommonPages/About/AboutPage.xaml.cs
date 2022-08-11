using System;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using XpertMobileApp.Api;
using XpertMobileApp.ViewModels;

namespace XpertMobileApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AboutPage : ContentPage
    {
        AboutViewModel viewModel;

        public AboutPage()
        {
            InitializeComponent();

            XpertVersion.Text = Mobile_Edition.GetEditionTitle(App.Settings.Mobile_Edition) + VersionTracking.CurrentVersion;
            CurrentVersion.Text = VersionTracking.CurrentVersion;
            Lbl_AppFullName.Text = Constants.AppFullName.Replace(" ", "\n");
            BindingContext = viewModel = new AboutViewModel();
            Update_btn.IsEnabled = false;
            googlePlay_btn.IsEnabled = false;
            googlePlay_btn.Opacity = 0.3;
            Update_btn.BackgroundColor = Color.FromHex("#ddd");

            handleVersions();
        }

        /// <summary>
        /// Check if there is new version or not 
        /// </summary>
        /// <returns></returns>
        private async Task<bool> CheckForNewUpdates()
        {
            string newVersion = await viewModel.GetNewVersion(NewVersion);
            return newVersion.ToString() != VersionTracking.CurrentVersion ? true : false;
        }

        /// <summary>
        /// Send the command to initiat Update process
        /// </summary>
        /// <returns></returns>
        private async Task<bool> UpdateToNewVersion()
        {
            string result = await viewModel.UpdateVersion();
            if (result != null)
                if (result.Contains("OK"))
                    return true;
                else return false;
            else return false;
        }
        /// <summary>
        /// Check if the current version is up to date or not and enable button to update it ....
        /// </summary>
        async void handleVersions()
        {
            //if (await CheckForNewUpdates())
            //{
            //    Update_btn.IsEnabled = true;
            //    Update_btn.BackgroundColor = Color.FromHex("#7EC384");

            //    googlePlay_btn.IsEnabled = true;
            //    googlePlay_btn.Opacity = 1;
            //} 
            //else
            //{
            //    Update_btn.IsEnabled = false;

            //    googlePlay_btn.IsEnabled = false;
            //    googlePlay_btn.Opacity = 0.3;
            //}
        }
        protected override void OnAppearing()
        {
            base.OnAppearing();
        }

        private async void CheckForUpdate(object sender, EventArgs e)
        {

            if (await CheckForNewUpdates())
            {
                googlePlay_btn.IsEnabled = true;
                googlePlay_btn.Opacity = 1;
                await DisplayAlert(AppResources.alrt_msg_Alert, AppResources.ap_new_update, AppResources.alrt_msg_Ok);
                await animateButton(googlePlay_btn);
            }
            else
            {
                googlePlay_btn.IsEnabled = false;
                googlePlay_btn.Opacity = 0.3;
                if (!await CheckForWebApiUpdate())
                {
                    Update_btn.IsEnabled = true;
                    Update_btn.BackgroundColor = Color.FromHex("#7EC384");
                    await DisplayAlert(AppResources.alrt_msg_Alert, AppResources.ap_new_update_web, AppResources.alrt_msg_Ok);
                    await animateButton(Update_btn);
                }
                else
                {
                    Update_btn.IsEnabled = false;
                    Update_btn.BackgroundColor = Color.FromHex("#ddd");
                    await DisplayAlert(AppResources.alrt_msg_Alert, AppResources.ap_updated, AppResources.alrt_msg_Ok);
                }
            }
        }

        private async Task<bool> CheckForWebApiUpdate()
        {
            bool res = await viewModel.GetNewWebApiVersion(NewVersion);
            return res;
        }

        private async
        Task
        animateButton(Button btn)
        {
            var scaleOriginal = btn.Scale;
            await btn.ScaleTo(btn.Scale * 1.3, 500);
            await btn.ScaleTo(scaleOriginal, 500);
            await btn.ScaleTo(btn.Scale * 1.3, 500);
            await btn.ScaleTo(scaleOriginal, 500);
        }

        [Obsolete]
        private void GoToPlayStore(object sender, EventArgs e)
        {
            Device.OpenUri(new Uri("market://details?id=" + AppInfo.PackageName));
        }

        private async void Update(object sender, EventArgs e)
        {
            if (await UpdateToNewVersion())
            {
                Update_btn.IsEnabled = false;
                Update_btn.BackgroundColor = Color.FromHex("#ddd");
                await DisplayAlert(AppResources.alrt_msg_Alert, AppResources.ap_update_success, AppResources.alrt_msg_Ok);
            }
            else
            {
                await DisplayAlert(AppResources.alrt_msg_Alert, AppResources.ap_update_failed, AppResources.alrt_msg_Ok);
            }
        }
    }
}