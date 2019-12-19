using XpertMobileApp.ViewModels;
using Plugin.Connectivity;
using System;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using System.Collections.ObjectModel;
using XpertMobileApp.Model;
using XpertMobileApp.Api.Services;
using XpertMobileApp.Models;
using XpertMobileApp.Helpers;

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
            //CommandeGrid.IsVisible = isModal;
            BindingContext = viewModel = new SettingsModel();

            LanguagesPicker.SelectedItem = viewModel.GetLanguageElem(viewModel.Settings.Language);
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            LanguagesPicker.SelectedIndexChanged += LanguagesPicker_SelectedIndexChanged;

            Client client = App.ClientDatabase.GetFirstItemAsync().Result;
            if(client != null)
            {
                DateTime expirationDate = LicActivator.GetLicenceEndDate(client.LicenceTxt).Result;
                lbl_ExperationDate.Text = string.Format("{0} : {1}", TranslateExtension.GetTranslation("msg_ExpireOn"),
                    expirationDate.ToString("dd/MM/yyyy"));
            }
        }

        private void LanguagesPicker_SelectedIndexChanged(object sender, EventArgs e)
        {
            var language = viewModel.Languages[LanguagesPicker.SelectedIndex];

            App.SetAppLanguage(language.ShortName);

            lbl_LanguageSelector.Text = AppResources.sp_lbl_SelectLanguage;
            lbl_ServiceName.Text      = AppResources.sp_lbl_ServiceName;
            Btn_CloseSettings.Text    = AppResources.btn_Close;
            Btn_SaveSettings.Text     = AppResources.sp_btn_SaveSettings;
            Btn_TestCnx.Text          = AppResources.sp_btn_TestConnection;
            lbl_ConnexionInfos.Text   = AppResources.sp_lbl_ConnexionInfos;
            Btn_RemoveLicence.Text    = AppResources.sp_btn_RemoveLicence;
            lbl_LicenceInfos.Text     = AppResources.sp_lbl_LicenceInfos;
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

            try
            {
                string serviceUrl = et_ServiceName.Text?.Trim();
                bool reachable = await this.IsBlogReachableAndRunning(serviceUrl);
                if (reachable)
                {
                    await DisplayAlert(AppResources.alrt_msg_Info, AppResources.alrt_msg_ConnectionSucces, AppResources.alrt_msg_Ok);
                }
                else
                {
                    await DisplayAlert(AppResources.alrt_msg_Info, AppResources.alrt_msg_ConnectionError, AppResources.alrt_msg_Ok);
                }
            }
            catch 
            {
                await DisplayAlert(AppResources.alrt_msg_Info, AppResources.alrt_msg_CantTestConnexionSettings, AppResources.alrt_msg_Ok);
            }
        }

        public async Task<bool> IsBlogReachableAndRunning(string url, int msTimeout = 5000)
        {
            try
            { 
                var connectivity = CrossConnectivity.Current;
                if (!connectivity.IsConnected)
                    return false;

                var uri = new System.Uri(url);
                TimeSpan ts = new TimeSpan(0, 0, 0, msTimeout);
                bool reachable = await connectivity.IsRemoteReachable(uri, ts);

                return reachable;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        private async void Btn_RemoveLicence_Clicked(object sender, EventArgs e)
        {
            var res = await DisplayAlert(AppResources.msg_Confirmation, AppResources.alrt_msg_CondifirmDesactivateLicence, AppResources.alrt_msg_Ok, AppResources.alrt_msg_Cancel);
            if (res)
            {
                bool result = await viewModel.DeactivateClient();
                if (result)
                {
                    await DisplayAlert(AppResources.alrt_msg_Info,
                                String.Format(AppResources.msg_DeactivationSucces), AppResources.alrt_msg_Ok);

                    if (Device.RuntimePlatform == Device.Android)
                    {
                        Application.Current.MainPage = new ActivationPage();
                    }
                    else if (Device.RuntimePlatform == Device.iOS)
                    {
                        await Navigation.PushModalAsync(new ActivationPage(), false);
                    }
                    else
                    {
                        Application.Current.MainPage = new ActivationPage();
                    }
                }
                else
                {

                }
            }
        }
    }
}