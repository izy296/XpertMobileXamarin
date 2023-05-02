using Acr.UserDialogs;
using Rg.Plugins.Popup.Services;
using Syncfusion.ListView.XForms;
using System;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Xpert.Common.WSClient.Helpers;
using XpertMobileApp.Api;
using XpertMobileApp.Api.Models;
using XpertMobileApp.DAL;
using XpertMobileApp.Interfaces;
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
                    Application.Current.Resources["NavigationPrimary"] = App.XDISTMainColor; //orange foncé
                    Application.Current.Resources["MenuAccent"] = App.XDISTMainColor;
                }
                else if (Constants.AppName == Apps.XPH_Mob)
                {
                    Application.Current.Resources["NavigationPrimary"] = App.XPharmMainColor; // Vert foncé
                    Application.Current.Resources["MenuAccent"] = App.XPharmMainColor; // vert claire
                    Application.Current.Resources["MenuItemGroup"] =App.XPharmMainColor;
                }
                else
                {
                    Application.Current.Resources["NavigationPrimary"] = "#2196F3";
                    Application.Current.Resources["MenuAccent"] = "#2196F3";
                    Application.Current.Resources["MenuItemGroup"] = "#2196F3"; // vert claire
                }

                //connectionStatus.Text = AppResources.txt_online;
                //connectionStatusIcon.Source = "wifi.png";
            }

            else
            {
                if (Constants.AppName == Apps.X_DISTRIBUTION)
                {
                    Application.Current.Resources["MenuItemGroup"] = App.XDISTMainColor; //orange Claire
                    Application.Current.Resources["NavigationPrimary"] = App.XDISTMainColor;
                    Application.Current.Resources["MenuAccent"] = App.XDISTMainColor;
                }
                else if (Constants.AppName == Apps.XPH_Mob)
                {
                    Application.Current.Resources["NavigationPrimary"] = App.XPharmMainColor; // Vert foncé
                    Application.Current.Resources["MenuAccent"] = App.XPharmMainColor; // vert claire
                    Application.Current.Resources["MenuItemGroup"] = App.XPharmMainColor; // vert claire
                }
                else
                {
                    Application.Current.Resources["NavigationPrimary"] = "#2196F3";
                    Application.Current.Resources["MenuAccent"] = "#2196F3";
                    Application.Current.Resources["MenuItemGroup"] = "#2196F3"; // vert claire
                }
                //connectionStatus.Text = AppResources.txt_offline;
                //connectionStatusIcon.Source = "nowifi.png";
            }

            // handle the witch between offline and online mode...
            //handleOfflineMode();
        }

        //private void handleOfflineMode()
        //{
        //    try
        //    {
        //        if (App.Online)
        //        {
        //            switchHorsLigne.IsOn = true;
        //        }
        //        else
        //        {
        //            switchHorsLigne.IsOn = false;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        throw;
        //    }
        //}
        protected async override void OnAppearing()
        {
            base.OnAppearing();
            UserDialogs.Instance.ShowLoading(AppResources.txt_Loading);
            //CheckIfUserWantReconnect();
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
                CustomPopup AlertPopup = new CustomPopup(WSApi2.GetExceptionMessage(ex), trueMessage: AppResources.alrt_msg_Ok);
                await PopupNavigation.Instance.PushAsync(AlertPopup);
            }

            if (viewModel.Items.Count == 0)
                await viewModel.ExecuteLoadItemsCommand();
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
            //await SyncDataIfDbEmpty();
            /* Custumazing the home menu */

            //if(Constants.AppName == Apps.XCOM_Mob ||Constants.AppName == Apps.XPH_Mob)
            //{
            //    //Get the height of the listview ...
            //    double menuHeight = this.listView.Height;

            //    //calculate the 
            //    int numberOfBtnInHomeMenu = viewModel.Items.Count;

            //    listView.ItemSize = (menuHeight / numberOfBtnInHomeMenu) * 2;
            //    UserDialogs.Instance.HideLoading();
            //}
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
                CustomPopup AlertPopup = new CustomPopup(WSApi2.GetExceptionMessage(ex), trueMessage: AppResources.alrt_msg_Ok);
                await PopupNavigation.Instance.PushAsync(AlertPopup);
            }
        }

        public async Task Download()
        {
            await SQLite_Manager.SynchroniseDownload();
        }

        public async Task Upload()
        {
            //await SQLite_Manager.synchroniseUpload();
        }

        private async void btn_Notification(object sender, EventArgs e)
        {
            KeepAnimate = false;
            await ((MainPage)App.Current.MainPage).NavigateFromMenu((int)MenuItemType.Notification);
        }
        public async void CheckIfUserWantReconnect()
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
                            {
                                CustomPopup AlertPopup = new CustomPopup("Veuillez vous connectez a l'internet !", trueMessage: AppResources.alrt_msg_Ok);
                                await PopupNavigation.Instance.PushAsync(AlertPopup);
                            }
                            else
                            {
                                switchHorsLigne.IsOn = true;
                            }
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
                            CustomPopup customPopup = new CustomPopup(AppResources.txt_ConfigPrefix, AppResources.txt_non, AppResources.txt_oui);
                            await PopupNavigation.Instance.PushAsync(customPopup);
                            if (await customPopup.PopupClosedTask)
                            {
                                if (customPopup.Result)
                                {
                                    await SQLite_Manager.AssignPrefix();
                                    if (string.IsNullOrEmpty(App.PrefixCodification))
                                    {
                                        CustomPopup AlertPopup = new CustomPopup(AppResources.txt_erreurProduit, trueMessage: AppResources.alrt_msg_Ok);
                                        await PopupNavigation.Instance.PushAsync(AlertPopup);
                                    }
                                    else
                                    {
                                        CustomPopup AlertPopup = new CustomPopup(AppResources.txt_prefixConfSucce, trueMessage: AppResources.alrt_msg_Ok);
                                        await PopupNavigation.Instance.PushAsync(AlertPopup);
                                    }
                                }
                            }
                        }
                    }
                    App.showPrefixConfigurationMessage = false;
                }
            }
            catch (Exception ex)
            {
                CustomPopup AlertPopup = new CustomPopup(ex.Message, trueMessage: AppResources.alrt_msg_Ok);
                await PopupNavigation.Instance.PushAsync(AlertPopup);
            }
        }

        private void listView_SelectionChanged(object sender, ItemSelectionChangedEventArgs e)
        {

        }

        private async void State_StateChanging(object sender, Syncfusion.XForms.Buttons.SwitchStateChangingEventArgs e)
        {
            try
            {
                this.switchHorsLigne.IsBusy = true;
                if (switchHorsLigne.IsBusy)
                {
                    if (!App.Online)
                    {
                        bool isConnected = await App.IsConected();
                        if (isConnected)
                        {
                            this.switchHorsLigne.IsOn = true;
                            DependencyService.Get<IMessage>().ShortAlert("Vous avez passé aux mode en ligne");
                        }else
                        {
                            this.switchHorsLigne.IsOn = false;
                        }
                    }
                    else
                    {
                        App.Online = false;
                        this.switchHorsLigne.IsOn = false;
                        DependencyService.Get<IMessage>().ShortAlert("Vous avez passé aux mode hors ligne");
                    }
                    this.switchHorsLigne.IsBusy = false;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {

            }
        }
    }
}