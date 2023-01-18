using XpertMobileApp.Models;
using System;
using System.Collections.Generic;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using XpertMobileApp.Services;
using XpertMobileApp.Api;
using Acr.UserDialogs;
using Xpert.Common.WSClient.Helpers;
using System.Linq;
using Xpert;
using XpertMobileApp.ViewModels.XLogin;
using XpertMobileApp.Api.Models;

namespace XpertMobileApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MenuPage : ContentPage
    {
        MainPage RootPage { get => Application.Current.MainPage as MainPage; }
        List<HomeMenuItem> menuItems;
        public MenuPage()
        {
            InitializeComponent();

            lbl_MenuUser.Text = string.IsNullOrEmpty(App.User?.Token?.fullName) ? "" : App.User.Token.fullName;

           
             if (Constants.AppName == Apps.XCOM_Livraison) 
            {
                menuItems = new List<HomeMenuItem>();
                menuItems.Add(new HomeMenuItem { Id = MenuItemType.Home, Image = "", Title = AppResources.pn_home });
                menuItems.Add(new HomeMenuItem { Id = MenuItemType.Livraison, Image = "", Title = AppResources.pn_Livraison });
                menuItems.Add(new HomeMenuItem { Id = MenuItemType.entreeStock, Image = "", Title = "Bon de retour" });
                menuItems.Add(new HomeMenuItem { Id = MenuItemType.Tournee, Image = "", Title = "Mes tournées" });
                menuItems.Add(new HomeMenuItem { Id = MenuItemType.Encaissements, Image = "", Title = AppResources.pn_encaissement });
                menuItems.Add(new HomeMenuItem { Id = MenuItemType.Tiers, Image = "", Title = AppResources.pn_Tiers });
                menuItems.Add(new HomeMenuItem { Id = MenuItemType.Settings, Image = "", Title = AppResources.pn_Settings });
                menuItems.Add(new HomeMenuItem { Id = MenuItemType.About, Image = "", Title = AppResources.pn_About });
                menuItems.Add(new HomeMenuItem { Id = MenuItemType.synchronisation, Image = "", Title = "Synchronisation" });
                menuItems.Insert(3, new HomeMenuItem {  Id = MenuItemType.Tresorerie, Title = AppResources.pn_Tresorerie, CodeObjet = XpertObjets.BSE_COMPTE, Action = XpertActions.AcSelect
                
                });
              
            }
             else if (Constants.AppName == Apps.XAGRI_Mob)
            {
                menuItems = new List<HomeMenuItem>
                {
                    new HomeMenuItem {Id = MenuItemType.Achats, Image = "", Title=AppResources.pn_Achats },
                    new HomeMenuItem {Id = MenuItemType.AchatsProduction, Image = "", Title=AppResources.pn_AchatsProduction },
                    new HomeMenuItem {Id = MenuItemType.OrdresProduction, Image = "", Title=AppResources.pn_OrdresProduction },
                    new HomeMenuItem {Id = MenuItemType.Tiers, Image = "", Title=AppResources.pn_Tiers },
                    new HomeMenuItem {Id = MenuItemType.Settings, Image = "", Title=AppResources.pn_Settings },

                    new HomeMenuItem {Id = MenuItemType.About, Image = "", Title=AppResources.pn_About }
                };

                if (AppManager.HasAdmin)
                {
                    menuItems.Insert(4,
                        new HomeMenuItem { Id = MenuItemType.AchatAgroAnalyses, Image = "", Title = AppResources.pn_Analyses });

                    menuItems.Insert(3, new HomeMenuItem
                    {
                        Id = MenuItemType.Encaissements,
                        Title = AppResources.pn_encaissement,
                        CodeObjet = XpertObjets.TRS_DECAISS,
                        Action = Xpert.XpertActions.AcSelect
                    });

                    menuItems.Insert(3, new HomeMenuItem
                    {
                        Id = MenuItemType.Tresorerie,
                        Title = AppResources.pn_Tresorerie,
                        CodeObjet = XpertObjets.BSE_COMPTE,
                        Action = XpertActions.AcSelect
                    });
                }
            }
             else if (Constants.AppName == Apps.XCOM_Abattoir)
            {
                menuItems = new List<HomeMenuItem>
                {
                    //new HomeMenuItem {Id = MenuItemType.Achats, Image = "", Title=AppResources.pn_Achats },
                    new HomeMenuItem {Id = MenuItemType.AchatsProduction, Image = "", Title=AppResources.pn_AchatsProduction },
                    //new HomeMenuItem {Id = MenuItemType.OrdresProduction, Image = "", Title=AppResources.pn_OrdresProduction },
                    new HomeMenuItem{ Id = MenuItemType.synchronisation, Image = "", Title = "Synchronisation" },
                    new HomeMenuItem {Id = MenuItemType.Tiers, Image = "", Title=AppResources.pn_Tiers },
                    new HomeMenuItem {Id = MenuItemType.Settings, Image = "", Title=AppResources.pn_Settings },
                    new HomeMenuItem {Id = MenuItemType.About, Image = "", Title=AppResources.pn_About }
                };
            }

            ListViewMenu.ItemSelected += async (sender, e) =>
            {
                if (e.SelectedItem == null)
                    return;

                var id = (int)((HomeMenuItem)e.SelectedItem).Id;
                await RootPage.NavigateFromMenu(id);
            };

            //MessagingCenter.Subscribe<SignUpPageViewModel, string>(this, "RELOAD_MENU", async (obj, str) =>
            //{
            //    ReloadMenu();
            //});

            MessagingCenter.Subscribe<LoginPageViewModel, string>(this, "RELOAD_MENU", async (obj, str) =>
            {
                ReloadMenu();
            });
        }

        private void ReloadMenu() 
        {
            Device.BeginInvokeOnMainThread(() =>
            {
                var menus = menuItems;
                
                ListViewMenu.ItemsSource = menus;
            });
        }

        private void btn_Disconnect_Clicked(object sender, EventArgs e)
        {

            if (App.User != null && App.User.Token != null)
                App.TokenDatabase.DeleteItemAsync(App.User.Token);

            App.User = null;
            AppManager.permissions = null;
            // Application.Current.MainPage = new LoginPage();
            {
                Application.Current.MainPage = new LoginPage();
            }
        }

        protected async override void OnAppearing()
        {
            base.OnAppearing();

            try
            {
                var menus = menuItems;
                UserDialogs.Instance.ShowLoading(AppResources.txt_Loading);
                {
                    var param = await AppManager.GetSysParams();
                    var permissions = await AppManager.GetPermissions();
                    menus = menuItems.Where(x => x.HasPermission == true).ToList();
                }

                ListViewMenu.ItemsSource = menus;

                if(menus.Count > 0 )
                    ListViewMenu.SelectedItem = menus[0];

                UserDialogs.Instance.HideLoading();
            }
            catch (Exception ex)
            {
                UserDialogs.Instance.HideLoading();
                await UserDialogs.Instance.AlertAsync(WSApi2.GetExceptionMessage(ex), AppResources.alrt_msg_Alert,
                    AppResources.alrt_msg_Ok);
            }

        }
    }
}