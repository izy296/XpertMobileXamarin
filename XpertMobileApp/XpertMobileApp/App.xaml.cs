using System;
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
        public static string RestServiceUrl { get; internal set; }
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

            MainPage = new LoginPage(); // = new MainPage();
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
    }
}
