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
        /// Check if the current version is up to date or not and enable button to update it ....
        /// </summary>
        async void handleVersions()
        {
            if (await CheckForNewUpdates())
            {
                Update_btn.IsEnabled = true;
                Update_btn.BackgroundColor = Color.FromHex("#7EC384");
            }
        }
        protected override void OnAppearing()
        {
            base.OnAppearing();
        }

        private async void CheckForUpdate(object sender, EventArgs e)
        {

            if (await CheckForNewUpdates())
            {
                await DisplayAlert(AppResources.alrt_msg_Alert, "Une nouvelle version est disponible", AppResources.alrt_msg_Ok);
            }
            else
            {
                await DisplayAlert(AppResources.alrt_msg_Alert, "Vous étes à jour ", AppResources.alrt_msg_Ok);
            }
        }

        [Obsolete]
        private void GoToPlayStore(object sender, EventArgs e)
        {
            Device.OpenUri(new Uri("market://details?id=com.xpertsoft.ComMobileApp"));
        }
    }
}