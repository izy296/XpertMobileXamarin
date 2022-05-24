using System;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using XpertMobileApp.Api;

namespace XpertMobileApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AboutPage : ContentPage
    {
        public AboutPage()
        {
            InitializeComponent();

            XpertVersion.Text = Mobile_Edition.GetEditionTitle(App.Settings.Mobile_Edition) + VersionTracking.CurrentVersion;
            Lbl_AppFullName.Text = Constants.AppFullName.Replace(" ", "\n");

        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
        }
    }
}