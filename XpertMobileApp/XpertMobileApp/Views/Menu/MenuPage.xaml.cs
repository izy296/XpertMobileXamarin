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
            lbl_MenuUser.Text = App.User.Token.fullName;

            if (Constants.AppName == Apps.XCOM_Mob || Constants.AppName == Apps.XPH_Mob)
            {
                menuItems = new List<HomeMenuItem>();

                menuItems.Add(new HomeMenuItem { Id = MenuItemType.Home, Image = "", Title = AppResources.pn_home });
                /*
                if (AppManager.HasAdmin) 
                { 

                }*/
                menuItems.Add(new HomeMenuItem
                {
                    Id = MenuItemType.Ventes,
                    Title = AppResources.pn_Ventes,
                    CodeObjet = XpertObjets.VTE_VENTE,
                    Action = XpertActions.AcSelect
                });

                menuItems.Add(new HomeMenuItem
                {
                    Id = MenuItemType.Encaissements,
                    Title = AppResources.pn_encaissement,
                    CodeObjet = XpertObjets.TRS_DECAISS,
                    Action = Xpert.XpertActions.AcSelect
                });


                if (App.Settings.Mobile_Edition >= Mobile_Edition.Standard)
                {
                    //menuItems.Add(new HomeMenuItem { Id = MenuItemType.Livraison, Image = "", Title = AppResources.pn_Livraison });
                    menuItems.Add(new HomeMenuItem { 
                                        Id = MenuItemType.VenteComptoir, 
                                        Title = AppResources.pn_VteComptoir, 
                                        CodeObjet = XpertObjets.VTE_COMPTOIR, 
                                        Action = XpertActions.AcSelect });

                    if (AppManager.HasAdmin)
                    {
                        menuItems.Add(new HomeMenuItem { Id = MenuItemType.Sessions, Title = AppResources.pn_session,
                            CodeObjet = XpertObjets.TRS_RESUME_SESSION,
                            Action = XpertActions.AcSelect
                        });

                        menuItems.Add(new HomeMenuItem { Id = MenuItemType.Tresorerie, Title = AppResources.pn_Tresorerie,
                            CodeObjet = XpertObjets.BSE_COMPTE,
                            Action = XpertActions.AcSelect
                        });

                        menuItems.Add(new HomeMenuItem { Id = MenuItemType.Commandes, Title =   AppResources.pn_Commandes,
                            CodeObjet = XpertObjets.VTE_COMMANDE,
                            Action = XpertActions.AcSelect
                        });

                        if (Constants.AppName == Apps.XPH_Mob)
                        {
                            menuItems.Add(new HomeMenuItem { Id = MenuItemType.Psychotrop,Title =    AppResources.pn_VtePsychotrop,
                              CodeObjet = XpertObjets.VTE_PSYCHOTROP,
                              Action = XpertActions.AcSelect
                            });

                            menuItems.Add(new HomeMenuItem { Id = MenuItemType.Bordereaux, Title = AppResources.pn_Bordereaux,
                                CodeObjet = XpertObjets.CFA_BORDEREAU,
                                Action = XpertActions.AcSelect
                            });
                        }
                    }
                    menuItems.Add(new HomeMenuItem { Id = MenuItemType.Tiers, Image = "", Title = AppResources.pn_Tiers });
                    menuItems.Add(new HomeMenuItem { Id = MenuItemType.Produits, Image = "", Title = AppResources.pn_Produits });

                    menuItems.Add(new HomeMenuItem
                    {
                        Id = MenuItemType.Manquants,
                        Title = AppResources.pn_Manquants,
                        CodeObjet = XpertObjets.ACH_MANQUANTS,
                        Action = XpertActions.AcSelect
                    });

                    if (AppManager.HasAdmin)
                    {
                        menuItems.Add(new HomeMenuItem { Id = MenuItemType.SimpleIndicators, Image = "", Title = "Indicteurs",
                            CodeObjet = XpertObjets.TDB_ANALYSES,
                            Action = XpertActions.AcSelect
                        }); // AppResources.pn_Simpleindicator
                        
                        menuItems.Add(new HomeMenuItem { Id = MenuItemType.EncAnalyses, Title = AppResources.pn_Analyses,
                            CodeObjet = XpertObjets.TDB_ANALYSES,
                            Action = XpertActions.AcSelect
                            
                        });
                    }
                }
                if (AppManager.HasAdmin)
                {
                    menuItems.Add(new HomeMenuItem { Id = MenuItemType.Settings, Image = "", Title = AppResources.pn_Settings });
                }
                menuItems.Add(new HomeMenuItem { Id = MenuItemType.About, Image = "", Title = AppResources.pn_About });

                // new HomeMenuItem {Id = MenuItemType.rfid, Image = "", Title=AppResources.pn_RfidScan },
                // new HomeMenuItem {Id = MenuItemType.invrfid, Image = "",Title= AppResources.pn_rfid_inventaire },
            }
            else if (Constants.AppName == Apps.XCOM_Livraison) 
            {
                menuItems = new List<HomeMenuItem>();
                menuItems.Add(new HomeMenuItem { Id = MenuItemType.Home, Image = "", Title = AppResources.pn_home });
                menuItems.Add(new HomeMenuItem { Id = MenuItemType.Livraison, Image = "", Title = AppResources.pn_Livraison });
                menuItems.Add(new HomeMenuItem { Id = MenuItemType.Tournee, Image = "", Title = "Mes tournées" });
                menuItems.Add(new HomeMenuItem { Id = MenuItemType.Encaissements, Image = "", Title = AppResources.pn_encaissement });
                menuItems.Add(new HomeMenuItem { Id = MenuItemType.Tiers, Image = "", Title = AppResources.pn_Tiers });
                menuItems.Add(new HomeMenuItem { Id = MenuItemType.Settings, Image = "", Title = AppResources.pn_Settings });
                menuItems.Add(new HomeMenuItem { Id = MenuItemType.About, Image = "", Title = AppResources.pn_About });
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
            else if (Constants.AppName == Apps.XACATALOG_Mob)
            {
                menuItems = new List<HomeMenuItem>
                {
                    new HomeMenuItem {Id = MenuItemType.MyCommandes, Image = "", Title=AppResources.pn_MyCommandes },
                    new HomeMenuItem {Id = MenuItemType.Catalogues, Image = "", Title=AppResources.pn_Catalogues },
                    new HomeMenuItem {Id = MenuItemType.About, Image = "", Title=AppResources.pn_About }
                };
            }
            else if (Constants.AppName == Apps.X_BOUTIQUE) 
            {
                menuItems = new List<HomeMenuItem>
                {
                    new HomeMenuItem {Id = MenuItemType.XBoutique, Image = "", Title=AppResources.pn_Catalogues },
                    new HomeMenuItem {Id = MenuItemType.XMyCommandes, Image = "", Title=AppResources.pn_MyCommandes },
                    new HomeMenuItem {Id = MenuItemType.XWishList, Image = "", Title="Wish List" },
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
        }

        private void btn_Disconnect_Clicked(object sender, EventArgs e)
        {

            if (App.User != null && App.User.Token != null)
                App.TokenDatabase.DeleteItemAsync(App.User.Token);

            App.User = null;
            AppManager.permissions = null;
            Application.Current.MainPage = new LoginPage();
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

                ListViewMenu.ItemsSource = menus;

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