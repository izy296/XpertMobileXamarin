using XpertMobileApp.ViewModels;
using Plugin.Connectivity;
using System;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using System.Collections.ObjectModel;
using XpertMobileApp.Model;

namespace XpertMobileApp.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class SettingsPage : ContentPage
	{

        public ObservableCollection<Language> Languages { get; }

        public SettingsModel viewModel;

        public SettingsPage()
        {
            InitializeComponent();
            BindingContext = viewModel = new SettingsModel();

            LanguagesPicker.SelectedItem = viewModel.GetLanguageElem(viewModel.Settings.Language);
        }


        public SettingsPage (bool isModal = false)
		{
			InitializeComponent();
            CommandeGrid.IsVisible = isModal;
            BindingContext = viewModel = new SettingsModel();

            LanguagesPicker.SelectedItem = viewModel.GetLanguageElem(viewModel.Settings.Language);
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            LanguagesPicker.SelectedIndexChanged += LanguagesPicker_SelectedIndexChanged;
        }

        private void LanguagesPicker_SelectedIndexChanged(object sender, EventArgs e)
        {
            var language = viewModel.Languages[LanguagesPicker.SelectedIndex];

            App.SetAppLanguage(language.ShortName);

            lbl_LanguageSelector.Text = AppResources.sp_lbl_SelectLanguage;
            lbl_ServerName.Text       = AppResources.sp_lbl_ServerName;
            lbl_PortNumber.Text       = AppResources.sp_lbl_PortNumber;
            Btn_CloseSettings.Text    = AppResources.btn_Close;
            Btn_SaveSettings.Text     = AppResources.sp_btn_SaveSettings;
            Btn_TestCnx.Text          = AppResources.sp_btn_TestConnection;
            lbl_ConnexionInfos.Text   = AppResources.sp_lbl_ConnexionInfos;
        }

        protected override async void OnDisappearing()
        {
            base.OnDisappearing();

            if (viewModel.Settings.isModified)
            { 
                var action = await DisplayAlert(AppResources.alrt_msg_title_Settings, AppResources.alrt_msg_SaveSettings,
                    AppResources.alrt_msg_Ok, AppResources.alrt_msg_Cancel);
                if (action)
                {
                    viewModel.SaveSettings();
                }
            }
        }

        protected void SettingsClose_Clicked(object sender, EventArgs e)
        {
            this.Navigation.PopModalAsync(true);
        }

        protected void SaveSettings_Clicked(object sender, EventArgs e)
        {
            viewModel.SaveSettings();
            DisplayAlert(AppResources.alrt_msg_Info, AppResources.alrt_msg_SettingsSaved, AppResources.alrt_msg_Ok);
        }

        protected async void TestConnexion_Clicked(object sender, EventArgs e)
        {
            // Check if the WebService is configured
            if (string.IsNullOrEmpty(App.RestServiceUrl))
            {
                await DisplayAlert(AppResources.alrt_msg_Info, AppResources.alrt_msg_MissingServerInfos, AppResources.alrt_msg_Ok);
                return;
            }

            int port;
            if (string.IsNullOrEmpty(App.Settings.Port))
                port = 80;
            else
                port = Convert.ToInt32(App.Settings.Port);

            bool reachable = await this.IsBlogReachableAndRunning(App.Settings.ServerName, port);
            if (reachable)
            {
                await DisplayAlert(AppResources.alrt_msg_Info, AppResources.alrt_msg_ConnectionSucces, AppResources.alrt_msg_Ok);
            }
            else
            {
                await DisplayAlert(AppResources.alrt_msg_Info, AppResources.alrt_msg_ConnectionError, AppResources.alrt_msg_Ok);
            }
        }

        public async Task<bool> IsBlogReachableAndRunning(string host, int port = 80, int msTimeout = 5000)
        {
            var connectivity = CrossConnectivity.Current;
            if (!connectivity.IsConnected)
                return false;

            var reachable = await connectivity.IsRemoteReachable(host, port, msTimeout);

            return reachable;
        }
    }
}