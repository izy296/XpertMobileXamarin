using Acr.UserDialogs;
using Newtonsoft.Json;
using Plugin.Connectivity;
using Plugin.FirebasePushNotification;
using Plugin.Multilingual;
using Rg.Plugins.Popup.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Linq;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Xpert.Common.WSClient.Model;
using XpertMobileApp.Api;
using XpertMobileApp.Api.Managers;
using XpertMobileApp.Api.Models;
using XpertMobileApp.Api.Models.Interfaces;
using XpertMobileApp.Api.Services;
using XpertMobileApp.DAL;
using XpertMobileApp.Data;
using XpertMobileApp.Helpers;
using XpertMobileApp.Models;
using XpertMobileApp.Services;
using XpertMobileApp.ViewModels;
using XpertMobileApp.Views;


namespace XpertMobileApp
{
    public partial class App : Application
    {
        
        private static string LOCAL_DB_NAME = Constants.LOCAL_DB_NAME;
        public static User User { get; internal set; }
        public static string XPharmMainColor = "#52B2A7";
        public static string XDISTMainColor = "#FF9500";
        public static bool Online = false;
        public static bool showReconnectMessage { get; set; } = true;
        public static bool showPrefixConfigurationMessage { get; set; } = true;
        public static string PrefixCodification { get; internal set; }
        public static string CODE_MAGASIN { get; internal set; }
        public static decimal PARAM_FIDELITE_TIERS { get; internal set; } = 0;

        public static MsgCenter MsgCenter = new MsgCenter();

        static TokenDatabaseControler tokenDatabase;
        static UserDatabaseControler userDatabase;
        static ClientDatabaseControler clientDatabase;
        static SettingsDatabaseControler settingsDatabase;
        static WebServiceClient resteService;
        private static Settings settings;
        public SettingsModel settingsViewModel;
        private static bool isThereNotification = false;

        public readonly ILocalNotificationsService localNotificationsService;
        public static bool IsThereNotification { get { return isThereNotification; } set { isThereNotification = value; } }

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

        public static bool IsInForeground { get; set; } = false;

        //Toast information
        public bool IsToastExitConfirmation
        {
            get => Preferences.Get(nameof(IsToastExitConfirmation), false);
            set => Preferences.Set(nameof(IsToastExitConfirmation), value);
        }
        private void InitApp()
        {
            // Vérification de la licence
            LicenceInfos licenceInfos = LicActivator.GetLicenceInfos();
            LicState licState = LicActivator.CheckLicence(licenceInfos).Result;
            PreventLinkerFromStrippingCommonLocalizationReferences();
            this.IsToastExitConfirmation = false;

            if (licState == LicState.Valid && Constants.AppName != Apps.XAGRI_Mob)
            {
                Settings.Mobile_Edition = licenceInfos != null ? licenceInfos.Mobile_Edition : Mobile_Edition.Lite;
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
                if (Constants.AppName == Apps.XCOM_Livraison || Constants.AppName == Apps.X_DISTRIBUTION)
                {
                    MainPage = new ActivationPage(licState);

                }
                else
                    MainPage = new ActivationPage(licState);
            }

        }

        private static void PreventLinkerFromStrippingCommonLocalizationReferences()
        {
            _ = new System.Globalization.GregorianCalendar();
            _ = new System.Globalization.PersianCalendar();
            _ = new System.Globalization.UmAlQuraCalendar();
            _ = new System.Globalization.ThaiBuddhistCalendar();
        }
        private bool isNotifiedForUpdate(string title, string message)
        {
            bool res = false;
            var notificationList = new SettingsModel().getNotificationAsync();
            foreach (var notif in notificationList)
            {
                if (notif.Title == title && notif.Message == message)
                    res = true;
            }
            return res;
        }
        public static string GetAppName()
        {
            return Constants.AppName;
        }

        public App()
        {
            Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense("NDYxNTc2QDMxMzkyZTMxMmUzMER2U3c5d05XeFJzbm04aTJwVnB6ejgxRU1wNTNRYTBVOUt6SnhrWk1ZQm89");

            InitializeComponent();

            App.SetAppLanguage(Settings.Language);
            settingsViewModel = new SettingsModel();

            this.InitApp();
            localNotificationsService = DependencyService.Get<ILocalNotificationsService>();
            new Task(
                async () =>
                {
                    try
                    {
                        if (App.User != null && App.User.Token != null && App.Online)
                        {
                            var xml = await WebServiceClient.GetNewVersion();
                            XDocument docWebApiXml = XDocument.Parse(xml);
                            XElement itemWebApiXml = docWebApiXml.Element("item");
                            var newVersion = itemWebApiXml.Element("version").Value;
                            var Data = new Dictionary<string, string>()
                                    {
                                        { "title", AppResources.Update_Notification_Header },
                                        { "body", AppResources.Update_Notification_Text + " " + newVersion },
                                        { "moduleName", MenuItemType.About.ToString() },
                                        { "user", "XpertSoft Mobile" },
                                        { "timeNotification", DateTime.Now.ToString() }
                                    };

                            Version newVersionHolder = new Version(newVersion);
                            Version currentVersionHolder = new Version(VersionTracking.CurrentVersion);
                            if (newVersionHolder > currentVersionHolder)
                            {
                                bool isNotified = isNotifiedForUpdate(Data["title"].ToString(), Data["body"].ToString());

                                if (!isNotified)
                                {
                                    localNotificationsService.ShowNotification(AppResources.Update_Notification_Header, AppResources.Update_Notification_Text + " " + newVersion, Data);

                                    new SettingsModel().setNotificationAsync(new Notification()
                                    {
                                        Title = Data["title"].ToString(),
                                        Message = Data["body"].ToString(),
                                        Module = Data["moduleName"].ToString(),
                                        User = Data["user"].ToString(),
                                        Extras = Data.ContainsKey("extras") ? Data["extras"] : null,
                                        TimeNotification = DateTime.Parse(Data["timeNotification"])
                                    });
                                    // code responable a la refresh de badge de notification
                                    MessagingCenter.Send(this, "RELOAD_MENU", "");
                                    MessagingCenter.Send(this, "RELOAD_NOTIF", "");
                                }

                            }
                        }
                    }
                    catch (Exception ex)
                    {

                        //await UserDialogs.Instance.AlertAsync(ex.Message.ToString(), AppResources.alrt_msg_Alert,
                        //AppResources.alrt_msg_Ok);
                    }
                }).Start();
        }

        protected override void OnStart()
        {
            IsInForeground = true;
            CrossFirebasePushNotification.Current.OnNotificationOpened += async (s, p) =>
            {
                try
                {
                    //Sauvegarde de notification dans l'événement d'ouverture et
                    //l'affichage de detaile dans le cas de donnes additionnelles
                    if (p.Data["title"].ToString() != AppResources.Update_Notification_Header)
                        new SettingsModel().setNotificationAsync(new Notification()
                        {
                            Title = p.Data["title"].ToString(),
                            Message = p.Data["body"].ToString(),
                            Module = p.Data["moduleName"].ToString(),
                            User = p.Data["user"].ToString(),
                            Extras = p.Data.ContainsKey("extras") ? p.Data["extras"] : null,
                            TimeNotification = DateTime.Parse(p.Data["timeNotification"].ToString())
                        });
                    // code responable a la refresh de badge de notification
                    MessagingCenter.Send(this, "RELOAD_MENU", "");
                    MessagingCenter.Send(this, "RELOAD_NOTIF", "");
                    MainPage RootPage = App.Current.MainPage as MainPage;
                    Enum.TryParse(p.Data["moduleName"].ToString(), out MenuItemType result);
                    if (result != MenuItemType.None)
                    {
                        await RootPage.NavigateFromMenu(Convert.ToInt32(result));
                        if (p.Data.ContainsKey("extras"))
                            MessagingCenter.Send(this, "ExtraData", p.Data["extras"]);
                    }
                    else await App.Current.MainPage.DisplayAlert(AppResources.txt_alert, AppResources.lp_txt_alert_url_manquant, AppResources.alrt_msg_Ok);
                }
                catch (Exception ex)
                {
                    await UserDialogs.Instance.AlertAsync(ex.Message.ToString(), AppResources.alrt_msg_Alert,
    AppResources.alrt_msg_Ok);
                }

                //if (p.Data.TryGetValue("moduleName", out moduleName) && !string.IsNullOrEmpty(Convert.ToString(moduleName)))
                //{
                //    string module = Convert.ToString(moduleName);

                //    // Get menu from moduleName
                //    // Open menu

                //    /*
                //    object idDoc;
                //    if (p.Data.TryGetValue("idDoc", out idDoc) && !string.IsNullOrEmpty(Convert.ToString(idDoc)))
                //    {
                //        string codeDoc = Convert.ToString(idDoc);
                //    }
                //    */
                //}
            };

            //Handle when your app starts
            CrossFirebasePushNotification.Current.OnNotificationReceived += async (s, p) =>
         {
             if (IsInForeground)
             {

             }
             bool saveSettings = false;
             try
             {
                 //sauvegard de notification dans le sqlite avec les données nécehomssaire
                 settingsViewModel.setNotificationAsync(new Notification()
                 {
                     Title = p.Data["title"].ToString(),
                     Message = p.Data["body"].ToString(),
                     Module = p.Data["moduleName"].ToString(),
                     User = p.Data["user"].ToString(),
                     Extras = p.Data.ContainsKey("extras") ? p.Data["extras"] : null,
                     TimeNotification = DateTime.Parse(p.Data["timeNotification"].ToString())
                 });

                 // code responable a la refresh de badge de notification
                 MessagingCenter.Send(this, "RELOAD_MENU", "");
                 MessagingCenter.Send(this, "RELOAD_NOTIF", "");
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
                 {
                     App.SettingsDatabase.SaveItemAsync(App.Settings);
                 }

             }
             catch (Exception ex)
             {
                 await UserDialogs.Instance.AlertAsync(ex.Message.ToString(), AppResources.alrt_msg_Alert, AppResources.alrt_msg_Ok);
             }

         };



            //CrossFirebasePushNotification.Current.OnNotificationOpened += (s, p) =>
            //{
            //    try
            //    {
            //        object url;
            //        // Case action is page to open
            //        if (p.Data.TryGetValue("urlPage", out url) && !string.IsNullOrEmpty(Convert.ToString(url)))
            //        {
            //            Device.OpenUri(new Uri(Convert.ToString(url)));
            //        }
            //    }
            //    catch (Exception e)
            //    {
            //    }
            //};

        }
        protected override void OnSleep()
        {
            // Handle when your app sleeps}
            IsInForeground = false;
        }

        protected async override void OnResume()
        {
            // Handle when your app resumes
            IsInForeground = true;

            // Handle when your app resumes
            if (!await IsConected())
            {
                CustomPopup AlertPopup = new CustomPopup("Connexion introuvable, voulez-vous vous connecter  ? ", falseMessage: AppResources.exit_Button_No, trueMessage: "Oui");
                await PopupNavigation.Instance.PushAsync(AlertPopup);
                if (await AlertPopup.PopupClosedTask)
                {
                    if (AlertPopup.Result)
                    {
                        DependencyService.Get<ISettingsStart>().StartSettings();
                    }
                }
            }
            else
            {
                //CustomPopup AlertPopup = new CustomPopup("", trueMessage: AppResources.alrt_msg_Ok);
                //await PopupNavigation.Instance.PushAsync(AlertPopup);
                DependencyService.Get<Interfaces.IMessage>().LongAlert("Connexion etablie avec succée");
            }
        }
        //Methode to set url services 
        public async static void SetUrlServices(UrlService urlService)
        {
            try
            {
                List<UrlService> liste;
                liste = JsonConvert.DeserializeObject<List<UrlService>>(Settings.ServiceUrl);
                // set all the services to false 
                foreach (UrlService service in liste)
                {
                    service.Selected = false;
                }
                // set true to the selected service ....
                for (int i = 0; i < liste.Count; i++)
                {
                    if (liste[i].DisplayUrlService == urlService.DisplayUrlService)
                    {
                        liste[i].Selected = true;
                    }
                }
                Settings.ServiceUrl = JsonConvert.SerializeObject(liste);
            }
            catch (Exception e)
            {
                await UserDialogs.Instance.AlertAsync(e.Message.ToString(), AppResources.alrt_msg_Alert, AppResources.alrt_msg_Ok);

            }
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
            List<UrlService> liste = new List<UrlService>();
            liste = JsonConvert.DeserializeObject<List<UrlService>>(Settings.ServiceUrl);
            if (liste.Count == 0)
            {
                await App.Current.MainPage.DisplayAlert(AppResources.txt_alert, AppResources.lp_txt_alert_url_manquant, AppResources.alrt_msg_Ok);
                return Online;
            }
            string temp = "";
            foreach (var item in liste)
            {
                if (item.Selected == true)
                {
                    temp = item.DisplayUrlService;
                }

            }
            string url = temp;

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
            //if (!isReachable)
            //    await ShowDisplayAlert();
            Online = isReachable;
            return isReachable;

        }

        public static string RestServiceUrl
        {
            get
            {

                //searching in the json file stocked in the local db a valid url wich is  selected in settings page...
                List<UrlService> liste = new List<UrlService>();
                if (Settings.ServiceUrl != null)
                {
                    liste = JsonConvert.DeserializeObject<List<UrlService>>(Settings.ServiceUrl);
                    foreach (var item in liste)
                    {
                        if (item.Selected == true)
                        {
                            return item.DisplayUrlService + ServiceUrlDico.BASE_URL;
                        }
                    }
                    return "" + ServiceUrlDico.BASE_URL;
                }
                else
                {
                    return ServiceUrlDico.BASE_URL;
                }
            }
        }

        /// <summary>
        /// retourne la table Token, si elle n'existe pas il crée une nouvelle table Token...
        /// </summary>
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

        /// <summary>
        /// retourne la table Client, si elle n'existe pas il crée une nouvelle table client...
        /// </summary>
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

        /// <summary>
        /// retourne la table User, si elle n'existe pas il crée une nouvelle table User...
        /// </summary>
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

        /// <summary>
        /// retourne la table Settings, si elle n'existe pas il crée une nouvelle table Settings...
        /// </summary>
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

        public static void StatrtCheckIfInternet(Page page)
        {
            //labelInfo = label;
            //label.Text = "Vous n'êtes pas connecté à internet";
            //label.IsVisible = false;

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
            if (!await App.IsConected())
            {
                Device.BeginInvokeOnMainThread(async () =>
                {
                    //labelInfo.IsVisible = true;
                    if (!alertDisplayed)
                    {
                        //alertDisplayed = false;
                        await ShowDisplayAlert();
                    }
                });
            }
            else
            {
                Device.BeginInvokeOnMainThread(() =>
                {
                    //labelInfo.IsVisible = false;
                    // Remettre l'affichage de l'alerte a false quand internet revient
                    alertDisplayed = false;
                });
            }
        }

        private static async Task ShowDisplayAlert()
        {

            await App.Current.MainPage.DisplayAlert(AppResources.txt_alert, AppResources.txt_alert_message, AppResources.alrt_msg_Ok);
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

        //check if there are pages on your navigation stack
        public bool PromptToConfirmExit
        {
            get
            {
                bool promptToConfirmExit = false;
                if (MainPage is ContentPage)
                {
                    promptToConfirmExit = true;
                }
                else if (MainPage is Xamarin.Forms.MasterDetailPage masterDetailPage
                    && masterDetailPage.Detail is NavigationPage detailNavigationPage)
                {
                    promptToConfirmExit = detailNavigationPage.Navigation.NavigationStack.Count <= 1;
                }
                else if (MainPage is NavigationPage mainPage)
                {
                    if (mainPage.CurrentPage is TabbedPage tabbedPage
                        && tabbedPage.CurrentPage is NavigationPage navigationPage)
                    {
                        promptToConfirmExit = navigationPage.Navigation.NavigationStack.Count <= 1;
                    }
                    else
                    {
                        promptToConfirmExit = mainPage.Navigation.NavigationStack.Count <= 1;
                    }
                }
                else if (MainPage is TabbedPage tabbedPage
                    && tabbedPage.CurrentPage is NavigationPage navigationPage)
                {
                    promptToConfirmExit = navigationPage.Navigation.NavigationStack.Count <= 1;
                }
                return promptToConfirmExit;
            }
        }
    }

}
