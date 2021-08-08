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

            lbl_MenuUser.Text = App.User?.UserName;

            if (Constants.AppName == Apps.XCOM_Mob || Constants.AppName == Apps.XPH_Mob)
            {
                menuItems = new List<HomeMenuItem>();

                menuItems.Add(new HomeMenuItem { Id = MenuItemType.Home, Image = FontAwesome.FontAwesomeIcons.Home, Title = AppResources.pn_home });
                /*
                if (AppManager.HasAdmin) 
                { 

                }*/
                menuItems.Add(new HomeMenuItem
                {
                    Id = MenuItemType.Ventes,
                    Title = AppResources.pn_Ventes,
                    CodeObjet = XpertObjets.VTE_VENTE,
                    Action = XpertActions.AcSelect,
                    Image = FontAwesome.FontAwesomeIcons.Sell
                });

                //{x:Static fontawesome:FontAwesomeIcons.Search}
                menuItems.Add(new HomeMenuItem
                {
                    Id = MenuItemType.Encaissements,
                    Title = AppResources.pn_encaissement,
                    CodeObjet = XpertObjets.TRS_DECAISS,
                    Action = Xpert.XpertActions.AcSelect,
                    Image = FontAwesome.FontAwesomeIcons.MoneyTransaction
                });


                if (App.Settings.Mobile_Edition >= Mobile_Edition.Standard)
                {
                    //menuItems.Add(new HomeMenuItem { Id = MenuItemType.Livraison, Image = "", Title = AppResources.pn_Livraison });
                    menuItems.Add(new HomeMenuItem
                    {
                        Id = MenuItemType.VenteComptoir,
                        Title = AppResources.pn_VteComptoir,
                        CodeObjet = XpertObjets.VTE_COMPTOIR,
                        Image = FontAwesome.FontAwesomeIcons.Sell,
                        Action = XpertActions.AcSelect
                    });

                    if (AppManager.HasAdmin)
                    {
                        menuItems.Add(new HomeMenuItem
                        {
                            Id = MenuItemType.Sessions,
                            Title = AppResources.pn_session,
                            CodeObjet = XpertObjets.TRS_RESUME_SESSION,
                            Action = XpertActions.AcSelect,
                            Image = FontAwesome.FontAwesomeIcons.Session,
                        });

                        menuItems.Add(new HomeMenuItem
                        {
                            Id = MenuItemType.Tresorerie,
                            Title = AppResources.pn_Tresorerie,
                            CodeObjet = XpertObjets.BSE_COMPTE,
                            Image = FontAwesome.FontAwesomeIcons.Tresorerie,
                            Action = XpertActions.AcSelect
                        });

                        menuItems.Add(new HomeMenuItem
                        {
                            Id = MenuItemType.Commandes,
                            Title = AppResources.pn_Commandes,
                            CodeObjet = XpertObjets.VTE_COMMANDE,
                            Image = FontAwesome.FontAwesomeIcons.Shopping_basket,
                            Action = XpertActions.AcSelect
                        });

                        if (Constants.AppName == Apps.XPH_Mob)
                        {
                            menuItems.Add(new HomeMenuItem
                            {
                                Id = MenuItemType.Psychotrop,
                                Title = AppResources.pn_VtePsychotrop,
                                CodeObjet = XpertObjets.VTE_PSYCHOTROP,
                                Image = FontAwesome.FontAwesomeIcons.Pills,
                                Action = XpertActions.AcSelect
                            });

                            menuItems.Add(new HomeMenuItem
                            {
                                Id = MenuItemType.Bordereaux,
                                Title = AppResources.pn_Bordereaux,
                                CodeObjet = XpertObjets.CFA_BORDEREAU,
                                Image = FontAwesome.FontAwesomeIcons.File_invoice_dollar,
                                Action = XpertActions.AcSelect
                            });
                        }
                    }
                    menuItems.Add(new HomeMenuItem { Id = MenuItemType.Tiers, Image = FontAwesome.FontAwesomeIcons.Handshake, Title = AppResources.pn_Tiers });
                    menuItems.Add(new HomeMenuItem { Id = MenuItemType.Produits, Image = FontAwesome.FontAwesomeIcons.Box_alt, Title = AppResources.pn_Produits });

                    menuItems.Add(new HomeMenuItem
                    {
                        Id = MenuItemType.Manquants,
                        Title = AppResources.pn_Manquants,
                        CodeObjet = XpertObjets.ACH_MANQUANTS,
                        Image = FontAwesome.FontAwesomeIcons.Boxes,
                        Action = XpertActions.AcSelect
                    });

                    if (AppManager.HasAdmin)
                    {
                        menuItems.Add(new HomeMenuItem
                        {
                            Id = MenuItemType.SimpleIndicators,
                            Image = FontAwesome.FontAwesomeIcons.Poll,
                            Title = "Indicteurs",
                            CodeObjet = XpertObjets.TDB_ANALYSES,
                            Action = XpertActions.AcSelect
                        }); // AppResources.pn_Simpleindicator

                        menuItems.Add(new HomeMenuItem
                        {
                            Id = MenuItemType.EncAnalyses,
                            Title = AppResources.pn_Analyses,
                            CodeObjet = XpertObjets.TDB_ANALYSES,
                            Image = FontAwesome.FontAwesomeIcons.Poll_h,
                            Action = XpertActions.AcSelect

                        });
                    }
                }
                if (AppManager.HasAdmin)
                {
                    menuItems.Add(new HomeMenuItem { Id = MenuItemType.Settings, Image = FontAwesome.FontAwesomeIcons.Cog, Title = AppResources.pn_Settings });
                }
                menuItems.Add(new HomeMenuItem { Id = MenuItemType.About, Image = FontAwesome.FontAwesomeIcons.About, Title = AppResources.pn_About });

                // new HomeMenuItem {Id = MenuItemType.rfid, Image = "", Title=AppResources.pn_RfidScan },
                // new HomeMenuItem {Id = MenuItemType.invrfid, Image = "",Title= AppResources.pn_rfid_inventaire },
            }
            else if (Constants.AppName == Apps.XCOM_Livraison)
            {
                menuItems = new List<HomeMenuItem>();
                menuItems.Add(new HomeMenuItem { Id = MenuItemType.Home, Image = FontAwesome.FontAwesomeIcons.Home, Title = AppResources.pn_home });
                menuItems.Add(new HomeMenuItem { Id = MenuItemType.Livraison, Image = FontAwesome.FontAwesomeIcons.Shipping_timed, Title = AppResources.pn_Livraison });
                menuItems.Add(new HomeMenuItem { Id = MenuItemType.Tournee, Image = FontAwesome.FontAwesomeIcons.Boxes, Title = "Mes tournées" });
                menuItems.Add(new HomeMenuItem { Id = MenuItemType.Encaissements, Image = FontAwesome.FontAwesomeIcons.MoneyTransaction, Title = AppResources.pn_encaissement });
                menuItems.Add(new HomeMenuItem { Id = MenuItemType.Tiers, Image = FontAwesome.FontAwesomeIcons.Handshake, Title = AppResources.pn_Tiers });
                menuItems.Add(new HomeMenuItem { Id = MenuItemType.Settings, Image = FontAwesome.FontAwesomeIcons.Cog, Title = AppResources.pn_Settings });
                menuItems.Add(new HomeMenuItem { Id = MenuItemType.About, Image = FontAwesome.FontAwesomeIcons.About, Title = AppResources.pn_About });
            }
            else if (Constants.AppName == Apps.XAGRI_Mob)
            {
                menuItems = new List<HomeMenuItem>
                {
                    new HomeMenuItem {Id = MenuItemType.Achats, Image = FontAwesome.FontAwesomeIcons.Shopping_basket, Title=AppResources.pn_Achats },
                    new HomeMenuItem {Id = MenuItemType.AchatsProduction, Image = FontAwesome.FontAwesomeIcons.Money_check_edit_alt, Title=AppResources.pn_AchatsProduction },
                    new HomeMenuItem {Id = MenuItemType.OrdresProduction, Image = FontAwesome.FontAwesomeIcons.Sort_size_up_alt, Title=AppResources.pn_OrdresProduction },
                    new HomeMenuItem {Id = MenuItemType.Tiers, Image = FontAwesome.FontAwesomeIcons.Handshake, Title=AppResources.pn_Tiers },
                    new HomeMenuItem {Id = MenuItemType.Settings, Image = FontAwesome.FontAwesomeIcons.Cog, Title=AppResources.pn_Settings },
                    new HomeMenuItem {Id = MenuItemType.About, Image = FontAwesome.FontAwesomeIcons.About, Title=AppResources.pn_About }
                };

                if (AppManager.HasAdmin)
                {
                    menuItems.Insert(4,
                        new HomeMenuItem { Id = MenuItemType.AchatAgroAnalyses, Image = FontAwesome.FontAwesomeIcons.chart_line, Title = AppResources.pn_Analyses });

                    menuItems.Insert(3, new HomeMenuItem
                    {
                        Id = MenuItemType.Encaissements,
                        Title = AppResources.pn_encaissement,
                        CodeObjet = XpertObjets.TRS_DECAISS,
                        Image = FontAwesome.FontAwesomeIcons.MoneyTransaction,
                        Action = Xpert.XpertActions.AcSelect
                    });

                    menuItems.Insert(3, new HomeMenuItem
                    {
                        Id = MenuItemType.Tresorerie,
                        Title = AppResources.pn_Tresorerie,
                        CodeObjet = XpertObjets.BSE_COMPTE,
                        Image = FontAwesome.FontAwesomeIcons.Tresorerie,
                        Action = XpertActions.AcSelect
                    });
                }
            }
            else if (Constants.AppName == Apps.XACATALOG_Mob)
            {
                menuItems = new List<HomeMenuItem>
                {
                    new HomeMenuItem {Id = MenuItemType.MyCommandes, Image = FontAwesome.FontAwesomeIcons.Hand_holding_box , Title=AppResources.pn_MyCommandes },
                    new HomeMenuItem {Id = MenuItemType.Catalogues, Image = FontAwesome.FontAwesomeIcons.Newspaper, Title=AppResources.pn_Catalogues },
                    new HomeMenuItem {Id = MenuItemType.About, Image = FontAwesome.FontAwesomeIcons.About, Title=AppResources.pn_About }
                };
            }
            else if (Constants.AppName == Apps.X_BOUTIQUE)
            {
                menuItems = new List<HomeMenuItem>
                {
                    new HomeMenuItem {Id = MenuItemType.XBoutiqueHome, Image =  FontAwesome.FontAwesomeIcons.Home, Title=AppResources.pn_home },
                    new HomeMenuItem {Id = MenuItemType.XBoutique, Image = FontAwesome.FontAwesomeIcons.Store_alt, Title=AppResources.pn_Catalogues },
                    new HomeMenuItem {Id = MenuItemType.XMyCommandes, Image =  FontAwesome.FontAwesomeIcons.Hand_holding_box , Title=AppResources.pn_MyCommandes, VisibleToGuest=false },
                    new HomeMenuItem {Id = MenuItemType.XWishList, Image = FontAwesome.FontAwesomeIcons.Clipboard_list_check, Title="Wish List", VisibleToGuest=false },
                    new HomeMenuItem {Id = MenuItemType.XPurchased, Image = FontAwesome.FontAwesomeIcons.box_check, Title="Produits achetés", VisibleToGuest=false },

                    new HomeMenuItem {Id = MenuItemType.XProfile, Image = FontAwesome.FontAwesomeIcons.users, Title="Profile", VisibleToGuest=false },
                    new HomeMenuItem {Id = MenuItemType.About, Image = FontAwesome.FontAwesomeIcons.About, Title=AppResources.pn_About }
                };
            }

            ListViewMenu.ItemSelected += async (sender, e) =>
            {
                if (e.SelectedItem == null)
                    return;

                var id = (int)((HomeMenuItem)e.SelectedItem).Id;
                await RootPage.NavigateFromMenu(id);
            };

            MessagingCenter.Subscribe<SignUpPageViewModel, string>(this, "RELOAD_MENU", async (obj, str) =>
            {
                ReloadMenu();
            });

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
                if (Constants.AppName == Apps.X_BOUTIQUE)
                {
                    if (App.User?.Token == null)
                        foreach (var item in menuItems)
                        {
                            menus = menuItems.Where(x => x.VisibleToGuest == true).ToList();
                        }
                }
                ListViewMenu.ItemsSource = menus;
            });
        }

        private void Logout_Tapped(object sender, EventArgs e)
        {
            if (App.User != null && App.User.Token != null)
                App.TokenDatabase.DeleteItemAsync(App.User.Token);

            App.User = null;
            AppManager.permissions = null;
            // Application.Current.MainPage = new LoginPage();
            if (Constants.AppName == Apps.X_BOUTIQUE)
            {
                Application.Current.MainPage = new MainPage();
            }
            else
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
                if (Constants.AppName != Apps.X_BOUTIQUE)
                {
                    var param = await AppManager.GetSysParams();
                    var permissions = await AppManager.GetPermissions();
                    menus = menuItems.Where(x => x.HasPermission == true).ToList();
                }
                else
                {
                    if (App.User?.Token == null)
                    {
                        foreach (var item in menuItems)
                        {
                            menus = menuItems.Where(x => x.VisibleToGuest == true).ToList();
                        }
                    }
                }

                ListViewMenu.ItemsSource = menus;

                if (menus.Count > 0)
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