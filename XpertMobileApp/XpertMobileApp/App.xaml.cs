using Plugin.Connectivity;
using Plugin.Multilingual;
using System;
using System.Globalization;
using System.Threading;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using XpertMobileApp.Data;
using XpertMobileApp.Models;
using XpertMobileApp.Services;
using XpertMobileApp.Views;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
namespace XpertMobileApp
{
    public partial class App : Application
    {

        private static string LOCAL_DB_NAME = "LinkedResto.db3";
        public static User User { get; internal set; }
        public static User CurrentUser { get; internal set; }

        static TokenDatabaseControler tokenDatabase;
        static UserDatabaseControler userDatabase;
        static SettingsDatabaseControler settingsDatabase;
        static WebServiceClient resteService;
        private static Settings settings;

        public App()
        {
            InitializeComponent();

            App.SetAppLanguage(Settings.Language);
        
            MainPage = new LoginPage(); // MainPage();  
        }

        protected override void OnStart()
        {
            // Handle when your app starts
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

                string result = "";

                if (Settings == null) return result;

                if(!string.IsNullOrEmpty(Settings.ServerName))
                   result += "http://" + Settings.ServerName;

                if (!string.IsNullOrEmpty(Settings.Port))
                    result += ":" + Settings.Port + "/";
                else
                    result += "/";

                return result;
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
