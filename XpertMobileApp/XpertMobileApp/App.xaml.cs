using Acr.UserDialogs;
using Plugin.Connectivity;
using Plugin.FirebasePushNotification;
using Plugin.Multilingual;
using System;
using System.Globalization;
using System.Threading;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using XpertMobileApp.Api;
using XpertMobileApp.Api.Services;
using XpertMobileApp.Data;
using XpertMobileApp.Helpers;
using XpertMobileApp.Models;
using XpertMobileApp.Services;
using XpertMobileApp.Views;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
namespace XpertMobileApp
{
    public partial class App : Application
    {

        private static string LOCAL_DB_NAME = Constants.LOCAL_DB_NAME;
        public static User User { get; internal set; }

        static TokenDatabaseControler tokenDatabase;
        static UserDatabaseControler userDatabase;
        static SettingsDatabaseControler settingsDatabase;
        static WebServiceClient resteService;
        private static Settings settings;

        public App()
        {
            Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense("NDgzNjlAMzEzNjJlMzMyZTMwYW9KSjRINjJybys3QVhUM0pvelpCMkhaV0kwcHM0ZkJ5cmpGYWJLZTFLTT0=;NDgzNzBAMzEzNjJlMzMyZTMwa09wVkRFcVFLTVgza1p6MHdEVDRtUkJ4d252NG5iTDZMTTEwT1Rxc054Zz0=");

            InitializeComponent();

            App.SetAppLanguage(Settings.Language);

            MainPage = new LoginPage();

            // Vérification de la licence
            LicState licState = LicActivator.CheckLicence().Result;
            if (licState != LicState.Valid)
            {                
                MainPage = new ActivationPage(licState);
            }

            /*
            string currentVersion = AppInfo.Version.ToString();
            if (App.Settings != null && App.Settings.ShouldUpdate && string.Compare(App.Settings.DestinationVersion , currentVersion) >= 0)
            {
                MainPage = new UpdatePage();
            }
            else
            {
                MainPage = new LoginPage();
            } 
            */
        }

        protected override void OnStart()
        {
            // Handle when your app starts

            CrossFirebasePushNotification.Current.OnNotificationReceived += (s, p) =>
            {
                string currentVerision = AppInfo.Version.ToString();
                object CriticalVersion;

                if (p.Data.TryGetValue("CriticalVersion", out CriticalVersion) && !string.IsNullOrEmpty(Convert.ToString(CriticalVersion)))
                {
                    if (String.Compare(Convert.ToString(CriticalVersion),currentVerision) > 0)
                    {
                        App.Settings.ShouldUpdate = true;
                        App.Settings.DestinationVersion = Convert.ToString(CriticalVersion);
                        App.SettingsDatabase.SaveItemAsync(App.Settings);
                    }
                }

            };

            CrossFirebasePushNotification.Current.OnNotificationOpened += (s, p) =>
            {
                try
                {
                    object url;
                    // Case action is page to open
                    if (p.Data.TryGetValue("urlPage", out url) && !string.IsNullOrEmpty(Convert.ToString(url)))
                    {
                        Device.OpenUri(new Uri(Convert.ToString(url)));
                    }
                }
                catch (Exception e)
                {
                }
            };
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }

        public static void SetAppLanguage(string language)
        {
            try
            {
                string val = language;
                if (string.IsNullOrEmpty(val))
                {
                    // Set the device language
                    val = CrossMultilingual.Current.DeviceCultureInfo.TwoLetterISOLanguageName;
                }

                Settings.Language = val;

                var culture = new CultureInfo(val);
                AppResources.Culture = culture;
                CrossMultilingual.Current.CurrentCultureInfo = culture;
  
            }
            catch (Exception)
            {
            }
        }

        public static bool IsConected
        {
            get
            {
                return CrossConnectivity.Current.IsConnected;
            }
        }

        public static string RestServiceUrl
        {
            get
            {
                return Settings.ServiceUrl + ServiceUrlDico.BASE_URL;
            }
        }

        public static TokenDatabaseControler TokenDatabase
        {
            get
            {
                if (tokenDatabase == null)
                {
                    tokenDatabase = new TokenDatabaseControler(DependencyService.Get<IFileHelper>().GetLocalFilePath(LOCAL_DB_NAME));
                }
                return tokenDatabase;
            }
        }

        public static UserDatabaseControler UserDatabase
        {
            get
            {
                if (userDatabase == null)
                {
                    userDatabase = new UserDatabaseControler(DependencyService.Get<IFileHelper>().GetLocalFilePath(LOCAL_DB_NAME));
                }
                return userDatabase;
            }
        }

        public static SettingsDatabaseControler SettingsDatabase
        {
            get
            {
                if (settingsDatabase == null)
                {
                    settingsDatabase = new SettingsDatabaseControler(DependencyService.Get<IFileHelper>().GetLocalFilePath(LOCAL_DB_NAME));
                }
                return settingsDatabase;
            }
        }

        public static WebServiceClient RestService
        {
            get
            {
                if (resteService == null)
                {
                    resteService = new WebServiceClient();
                }
                return resteService;
            }
        }

        public static Settings Settings
        {
            get
            {
                if (settings == null)
                {
                    settings = App.SettingsDatabase.GetFirstItemAsync().Result;
                    if (settings == null)
                    {
                        settings = new Settings();
                    }
                }
                return settings;
            }

            set
            {
                settings = value;
            }

        }

        // ------ Internet connexion infos ---------
        #region Verification de la connexion internet et affichage d'une alerte en cas de deconnexion

        private static bool alertDisplayed = false;
        private static Label labelInfo;
        private static Page currentPage;
        private static Timer timer;

        public static void StatrtCheckIfInternet(Label label, Page page)
        {
            labelInfo = label;
            label.Text = "Vous n'êtes pas connecté à internet";
            label.IsVisible = false;

            currentPage = page;

            if (timer == null)
            {
                timer = new Timer((e) =>
                {
                    CheckIfInternetOverTimeAsync();
                }, null, 10, (int)TimeSpan.FromSeconds(3).TotalMilliseconds);
            }
        }

        private static void CheckIfInternetOverTimeAsync()
        {
            if (!App.IsConected)
            {
                Device.BeginInvokeOnMainThread(async () =>
                {
                       labelInfo.IsVisible = true;
                       if (!alertDisplayed)
                       {
                            alertDisplayed = true;
                            await ShowDisplayAlert();
                       }
                });
            }
            else
            {
                Device.BeginInvokeOnMainThread(() => {
                    labelInfo.IsVisible = false;
                    // Remettre l'affichage de l'alerte a false quand internet revient
                    alertDisplayed = false;
                });
            }
        }

        private static async Task ShowDisplayAlert()
        {
            await currentPage.DisplayAlert("Internet", "Vous n'êtes pas connecté à internet", "Ok");
        }

        #endregion
    }
}
