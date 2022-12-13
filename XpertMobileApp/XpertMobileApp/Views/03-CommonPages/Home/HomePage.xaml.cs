using Acr.UserDialogs;
using System;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Xpert.Common.WSClient.Helpers;
using XpertMobileApp.Api;
using XpertMobileApp.Api.Models;
using XpertMobileApp.DAL;
using XpertMobileApp.Models;
using XpertMobileApp.SQLite_Managment;
using XpertMobileApp.ViewModels;

namespace XpertMobileApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class HomePage : ContentPage
    {
        HomeViewModel viewModel;
        private bool KeepAnimate { get; set; } = false;
        public HomePage()
        {
            InitializeComponent();
            // Title = AppResources.pn_home;
            lblUser.Text = App.User?.Token.userName;
            lblClientName.Text = App.Settings.ClientName;

            var userGroup = App.User.UserGroup;
            if (userGroup == "AD")
                notificationBell.IsVisible = true;
            BindingContext = viewModel = new HomeViewModel();
            if (App.Settings.Language == "ar")
            {
                notif_container.Padding = new Thickness(0, 0, 0, 0);
                lblClientName.Margin = new Thickness(45, 0, 90, 0);
            }
            if (App.Online)
            {
                if (Constants.AppName == Apps.X_DISTRIBUTION)
                {
                    Application.Current.Resources["NavigationPrimary"] = "#F9B208 "; //orange foncé
                    Application.Current.Resources["MenuAccent"] = "#F9B208";
                    Application.Current.Resources["MenuItemGroup"] = "#FBEEC9"; //orange Claire
                }
                else if (Constants.AppName == Apps.XPH_Mob)
                {
                    Application.Current.Resources["NavigationPrimary"] = "#087565"; // Vert foncé
                    Application.Current.Resources["MenuAccent"] = "#087565";
                    Application.Current.Resources["MenuItemGroup"] = "#ACD1CB"; // vert claire
                }
                else
                {
                    Application.Current.Resources["NavigationPrimary"] = "#2196F3";
                    Application.Current.Resources["MenuAccent"] = "#96d1ff";
                }

                connectionStatus.Text = AppResources.txt_online;
            }

            else
            {
                if (Constants.AppName == Apps.X_DISTRIBUTION)
                {
                    Application.Current.Resources["MenuItemGroup"] = "#FBEEC9"; //orange Claire
                    Application.Current.Resources["NavigationPrimary"] = "#5B5B5B";
                    Application.Current.Resources["MenuAccent"] = "#5B5B5B";
                }
                else if (Constants.AppName == Apps.XPH_Mob)
                {
                    Application.Current.Resources["NavigationPrimary"] = "#087565"; // Vert foncé
                    Application.Current.Resources["MenuAccent"] = "#087565";
                    Application.Current.Resources["MenuItemGroup"] = "#ACD1CB"; // vert claire
                }
                connectionStatus.Text = AppResources.txt_offline;
            }

        }

        protected async override void OnAppearing()
        {
            base.OnAppearing();
            CheckIfUserWantReconnect();
            CHeckIfPrefixIsConfigured();
            try
            {
                UserDialogs.Instance.ShowLoading(AppResources.txt_Loading);
                if (Constants.AppName != Apps.X_BOUTIQUE)
                {
                    var param = await AppManager.GetSysParams();
                    var permissions = await AppManager.GetPermissions();
                }
                UserDialogs.Instance.HideLoading();
            }
            catch (Exception ex)
            {
                UserDialogs.Instance.HideLoading();
                await UserDialogs.Instance.AlertAsync(WSApi2.GetExceptionMessage(ex), AppResources.alrt_msg_Alert,
                    AppResources.alrt_msg_Ok);
            }

            if (viewModel.Items.Count == 0)
                viewModel.LoadItemsCommand.Execute(null);
            if (viewModel.Sessions.Count == 0)
                viewModel.LoadSessionsCommand.Execute(null);

            MessagingCenter.Subscribe<HomeMenuItem, string>(this, "refreshBell", async (objet, e) =>
            {
                if (App.IsThereNotification)
                {
                    notificationBell.ImageSource = "notificationRed36.png";
                    await notificationBell.RotateTo(-50, 300, easing: Easing.Linear);
                    await notificationBell.RotateTo(0, 300, easing: Easing.Linear);
                }
                else notificationBell.ImageSource = "notification36.png";
            });

            if (App.IsThereNotification)
            {
                notificationBell.ImageSource = "notificationRed36.png";
                await Task.Delay(500);
                await notificationBell.RotateTo(-50, 100, easing: Easing.Linear);
                await notificationBell.RotateTo(0, 100, easing: Easing.Linear);
            }
            else notificationBell.ImageSource = "notification36.png";

            if (((NavigationPage)((MasterDetailPage)App.Current.MainPage).Detail).CurrentPage == this)
            {
                new MenuPage("1");
            }

            //Synchroniser les données si la connexion existe !
            await SyncDataIfDbEmpty();
        }

        private void OnItemSelected(object sender, SelectedItemChangedEventArgs e)
        {

        }
        private void listView_SelectionChanged(object sender, Syncfusion.ListView.XForms.ItemSelectionChangedEventArgs e)
        {

        }

        private void btn_Refresh_Clicked(object sender, EventArgs e)
        {
            viewModel.LoadItemsCommand.Execute(null);
        }

        private async void Btn_Menu_Clicked(object sender, EventArgs e)
        {
            MainPage RootPage = Application.Current.MainPage as MainPage;
            string id = ((sender as Button).Parent.Parent.Parent.BindingContext as TDB_SIMPLE_INDICATORS).CODE_ANALYSE;
            if (id == "31324")
            {
                //await Upload();
            }
            else if (id == "5611")
            {
                //await Download();
            }
            else
            {
                //var page = RootPage.GetMenuPage(Convert.ToInt32(id));
                await RootPage.NavigateFromMenu(Convert.ToInt32(id));
                //await Navigation.PushAsync(page);
                // await RootPage.NavigateFromMenu(Convert.ToInt32(id));
            }
        }

        /// <summary>
        /// Quand la connexion existe !
        /// Uploader les nouveaux données aux sqlserver 
        /// télécharger les nouveaux données depuis sqlserver
        /// </summary>
        /// <returns></returns>
        public async Task SyncDataIfDbEmpty()
        {
            try
            {
                var number = await SQLite_Manager.GetInstance().Table<View_TRS_TIERS>().ToListAsync();
                if (App.Online)
                {
                    var checkDbEmpty = await SQLite_Manager.CheckAllTablesIfEmpty();
                    if (checkDbEmpty)
                        await Download();
                }
            }
            catch (Exception ex)
            {
                await UserDialogs.Instance.AlertAsync(WSApi2.GetExceptionMessage(ex), AppResources.alrt_msg_Alert, AppResources.alrt_msg_Ok);
            }
        }

        public async Task Download()
        {
            await SQLite_Manager.SynchroniseDownload();
        }

        public async Task Upload()
        {
            await SQLite_Manager.synchroniseUpload();
        }

        private async void btn_Notification(object sender, EventArgs e)
        {
            KeepAnimate = false;
            await ((MainPage)App.Current.MainPage).NavigateFromMenu((int)MenuItemType.Notification);
        }
        public async static void CheckIfUserWantReconnect()
        {
            try
            {
                if (App.showReconnectMessage == true && App.Online == false)
                {
                    if (Constants.AppName == Apps.XCOM_Mob || Constants.AppName == Apps.XPH_Mob)
                    {
                        bool answer = await App.Current.MainPage.DisplayAlert("Reconnexion", AppResources.msg_Reconnect, AppResources.exit_Button_Yes, AppResources.exit_Button_No);
                        if (answer)
                        {
                            bool response = await App.IsConected();
                            if (!response)
                                await UserDialogs.Instance.AlertAsync("Veuillez vous connectez a l'internet !", AppResources.alrt_msg_Alert, AppResources.alrt_msg_Ok);
                        }
                    }
                    App.showReconnectMessage = false;
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async static void CHeckIfPrefixIsConfigured()
        {
            try
            {
                if (Constants.AppName == Apps.X_DISTRIBUTION)
                {
                    if (App.showPrefixConfigurationMessage == true && App.Online == false)
                    {
                        if (string.IsNullOrEmpty(App.PrefixCodification))
                        {
                            bool answer = await App.Current.MainPage.DisplayAlert("Configuration du prefix !", AppResources.txt_ConfigPrefix, AppResources.exit_Button_Yes, AppResources.exit_Button_No);
                            if (answer)
                            {
                                await SQLite_Manager.AssignPrefix();
                                if (string.IsNullOrEmpty(App.PrefixCodification))
                                    await UserDialogs.Instance.AlertAsync(AppResources.txt_erreurProduit, AppResources.alrt_msg_Alert, AppResources.alrt_msg_Ok);
                                else
                                    await UserDialogs.Instance.AlertAsync(AppResources.txt_prefixConfSucce, AppResources.alrt_msg_Alert, AppResources.alrt_msg_Ok);

                            }
                        }
                    }
                    App.showPrefixConfigurationMessage = false;
                }
            }
            catch (Exception ex)
            {

                UserDialogs.Instance.AlertAsync(ex.Message, AppResources.alrt_msg_Alert, AppResources.alrt_msg_Ok); ;
            }
        }
    }
}