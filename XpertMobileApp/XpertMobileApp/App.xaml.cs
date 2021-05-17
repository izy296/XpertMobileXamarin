using Acr.UserDialogs;
using Plugin.Connectivity;
using Plugin.FirebasePushNotification;
using Plugin.Multilingual;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.IO;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Xpert.Common.WSClient.Model;
using XpertMobileApp.Api;
using XpertMobileApp.Api.Managers;
using XpertMobileApp.Api.Services;
using XpertMobileApp.DAL;
using XpertMobileApp.Data;
using XpertMobileApp.Helpers;
using XpertMobileApp.Models;
using XpertMobileApp.Services;
using XpertMobileApp.Views;

//[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
namespace XpertMobileApp
{
    public partial class App : Application


    {
        static VTE_VENTE_Database vteVentedatabase;

        // Create the database connection as a singleton.
        public static VTE_VENTE_Database VteVentedatabase
        {
            get
            {
                if (vteVentedatabase == null)
                {
                    vteVentedatabase = new VTE_VENTE_Database(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "Notes.db3"));
                }
                return vteVentedatabase;
            }
        }

        private static string LOCAL_DB_NAME = Constants.LOCAL_DB_NAME;
        public static User User { get; internal set; }

        public static bool Online = false;

        public static MsgCenter MsgCenter = new MsgCenter();

        static TokenDatabaseControler tokenDatabase;
        static UserDatabaseControler userDatabase;
        static ClientDatabaseControler clientDatabase;
        static SettingsDatabaseControler settingsDatabase;
        static WebServiceClient resteService;
        private static Settings settings;

        private static View_VTE_VENTE currentSales;
        public static View_VTE_VENTE CurrentSales
        {
            get
            {
                if (currentSales == null)
                {
                    currentSales = new View_VTE_VENTE();
                }
                return currentSales;
            }
            set
            {
                currentSales = value;
            }
        }

        public static FlowDirection PageFlowDirection
        {
            get
            {
                if (App.Settings.Language == "ar")
                {
                    return FlowDirection.RightToLeft;
                }
                else
                {
                    return FlowDirection.LeftToRight;
                }
            }
        }

        private void InitApp()
        {
            // Vérification de la licence
            LicenceInfos licenceInfos = LicActivator.GetLicenceInfos();
            LicState licState = LicActivator.CheckLicence(licenceInfos).Result;

            if (licState == LicState.Valid)
            {
                Settings.Mobile_Edition = licenceInfos.Mobile_Edition;

                string currentVersion = AppInfo.Version.ToString();
                if (App.Settings != null && App.Settings.ShouldUpdate && string.Compare(App.Settings.DestinationVersion, currentVersion) >= 0)
                {
                    MainPage = new UpdatePage();
                }
                else
                {
                    Token token = App.TokenDatabase.GetFirstItemAsync().Result;
                    if (token != null && DateTime.Now <= token.expire_Date)
                    {
                        App.User = new User(token.userName, "");
                        App.User.UserName = token.userID;
                        App.User.CODE_TIERS = token.CODE_TIERS;
                        App.User.UserGroup = token.UserGroup;
                        App.User.GroupName = token.GroupName;
                        App.User.ClientId = licenceInfos.ClientId;
                        App.User.Token = token;

                        MainPage = new MainPage();
                    }
                    else
                    {
                        if (Constants.AppName == Apps.X_BOUTIQUE)
                        {
                            MainPage = new MainPage();
                        }
                        else
                        {
                            MainPage = new LoginPage();
                        }
                    }
                }
            }
            else
            {
                 MainPage = new LoginPage();
                //MainPage = new ActivationPage(licState);
            }
        }

        public App()
        {
            Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense("Mjk3ODAxQDMxMzgyZTMyMmUzMFdHQ3FLK0tXSmRMUnFiUm5CelZnQ3kxRTV3Q2ZXWXEraExTemNSVjJwQ2M9");

            InitializeComponent();

            App.SetAppLanguage(Settings.Language);

            this.InitApp();

        }

        protected override void OnStart()
        {
            CrossFirebasePushNotification.Current.OnNotificationOpened += (s, p) =>
            {
                object moduleName;
                if (p.Data.TryGetValue("moduleName", out moduleName) && !string.IsNullOrEmpty(Convert.ToString(moduleName)))
                {
                    string module = Convert.ToString(moduleName);

                    MainPage RootPage = Application.Current.MainPage as MainPage;
                    var page = RootPage.GetMenuPage(Convert.ToInt32(module));
                    RootPage.NavigateFromMenu(Convert.ToInt32(module));

                    // Get menu from moduleName
                    // Open menu

                    /*
                    object idDoc;
                    if (p.Data.TryGetValue("idDoc", out idDoc) && !string.IsNullOrEmpty(Convert.ToString(idDoc)))
                    {
                        string codeDoc = Convert.ToString(idDoc);
                    }
                    */
                }
            };

            // Handle when your app starts
            CrossFirebasePushNotification.Current.OnNotificationReceived += (s, p) =>
            {
                bool saveSettings = false;

                // Traitement du message obligant la mise à jour
                string currentVerision = AppInfo.Version.ToString();
                object CriticalVersion;
                if (p.Data.TryGetValue("CriticalVersion", out CriticalVersion) && !string.IsNullOrEmpty(Convert.ToString(CriticalVersion)))
                {
                    if (String.Compare(Convert.ToString(CriticalVersion), currentVerision) > 0)
                    {
                        App.Settings.ShouldUpdate = true;
                        App.Settings.DestinationVersion = Convert.ToString(CriticalVersion);
                        saveSettings = true;
                    }
                }

                // Mise à jour de l'app settings depuis un message push
                foreach (PropertyDescriptor descriptor in TypeDescriptor.GetProperties(App.Settings))
                {
                    string fieldName = descriptor.Name;
                    object currentValue = descriptor.GetValue(App.Settings);
                    currentValue = getFormatedValue(currentValue);

                    object remoteValue = null;
                    if (p.Data.TryGetValue(fieldName, out remoteValue) && !string.IsNullOrEmpty(Convert.ToString(remoteValue)))
                    {
                        descriptor.SetValue(App.Settings, remoteValue);
                        saveSettings = true;
                    }
                }

                if (saveSettings)
                    App.SettingsDatabase.SaveItemAsync(App.Settings);
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

        public async static Task<bool> IsConected()
        {
            // // return CrossConnectivity.Current.IsConnected;
            // string url = App.Settings.ServiceUrl;
            ////var isReachable = await CrossConnectivity.Current.IsReachable(url, 5000);

            //return true;

            string url = App.Settings.ServiceUrl;
            int port = 80;
            Regex r = new Regex(@"^(?<proto>\w+)://[^/]+?(?<port>:\d+)?/", RegexOptions.None, TimeSpan.FromMilliseconds(50));
            Match m = r.Match(url);

            if (m.Success && !string.IsNullOrEmpty(m.Groups["port"]?.Value))
            {
                port = Convert.ToInt32(m.Groups["port"].Value.Replace(":", ""));
            }

            var uri = new System.Uri(url);
            TimeSpan ts = new TimeSpan(0, 0, 0, 50);

            string furl = url.Replace(":" + port.ToString(), "");
            var isReachable = await CrossConnectivity.Current.IsRemoteReachable(furl, port);
            Online = isReachable;
            return isReachable;
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

        public static ClientDatabaseControler ClientDatabase
        {
            get
            {
                if (clientDatabase == null)
                {
                    clientDatabase = new ClientDatabaseControler(DependencyService.Get<IFileHelper>().GetLocalFilePath(LOCAL_DB_NAME));
                }
                return clientDatabase;
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

        private async static void CheckIfInternetOverTimeAsync()
        {
            if (! await App.IsConected())
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

        private string getFormatedValue(object value)
        {
            if (value == null) return "";

            string result = value.ToString();

            if (value is DateTime)
            {
                result = string.Format("{0:dd/MM/yyyy HH:mm}", value);
            }
            else if (value is decimal)
            {
                result = string.Format("{0:F2} DA", value);
            }
            else
            {
                result = string.Format("{0}", value);
            }

            return result;
        }
    }
}
