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
using XpertMobileApp.Helpers;
using System.Collections.ObjectModel;
using Xamarin.Essentials;
using System.ComponentModel;
using XpertMobileApp.Views.Helper;
using Newtonsoft.Json;
using XpertMobileApp.Api.Services;

namespace XpertMobileApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MenuPage : ContentPage
    {
        MainPage RootPage { get => Application.Current.MainPage as MainPage; }
        List<HomeMenuItem> menuItems;
        ObservableCollection<Grouping<string, int, HomeMenuItem>> menuItemsGrouped;
        private int numberOfNotifications;
        public int NumberOfNotifications
        {
            get { return numberOfNotifications; }
            set
            {
                numberOfNotifications = value;
                OnPropertyChanged("numberOfNotifications");
            }
        }
        public MenuPage(string id)
        {
            MessagingCenter.Send(this, "ChangeListIndex", id);
        }
        public MenuPage()
        {
            InitializeComponent();
            // Vérification de la licence
            LicenceInfos licenceInfos = LicActivator.GetLicenceInfos();

            lbl_MenuUser.Text = string.IsNullOrEmpty(App.User?.Token?.fullName) ? "" : App.User.Token.fullName;

            XpertVersion.Text = Mobile_Edition.GetEditionTitle(App.Settings.Mobile_Edition) + VersionTracking.CurrentVersion;
            ClientId.Text = "ID : " + App.Settings.ClientId;
            Lbl_AppFullName.Text = Constants.AppFullName.Replace(" ", "\n");
            XpertExpireDate.Text = "Exp: " + licenceInfos.ExpirationDate.GetDateTimeFormats().First().ToString();

            GetNumberOfNotifications();

            // Initialisation de la liste de menu ...
            menuItems = new List<HomeMenuItem>();

            /* Menu Commun entre (XCOM && XPH_mob && X_DISTRIBUTION) */
            if (Constants.AppName == Apps.XCOM_Mob || Constants.AppName == Apps.XPH_Mob || Constants.AppName == Apps.X_DISTRIBUTION)
            {
                /* Page Home */
                menuItems.Add(new HomeMenuItem
                {
                    Id = MenuItemType.Home,
                    ItemGroup = MenuItemGroup.Home,
                    Image = "home.png",
                    Title = AppResources.pn_home
                });

                /* Page Encaissement */
                menuItems.Add(new HomeMenuItem
                {
                    Id = MenuItemType.Encaissements,
                    ItemGroup = MenuItemGroup.Tresorerie,
                    Title = AppResources.pn_encaissement,
                    CodeObjet = XpertObjets.TRS_DECAISS,
                    Action = Xpert.XpertActions.AcSelect,
                    Image = "encaisss.png"
                });

                /* Tiers */
                menuItems.Add(new HomeMenuItem
                {
                    Id = MenuItemType.Tiers,
                    ItemGroup = MenuItemGroup.Ventes,
                    Image = "user.png",
                    Title = AppResources.pn_Client
                });

                /* Produit */
                menuItems.Add(new HomeMenuItem
                {
                    Id = MenuItemType.Produits,
                    ItemGroup = MenuItemGroup.Stock,
                    Image = "produit.png",
                    Title = AppResources.pn_Produits
                });

                /* Page Settings */
                menuItems.Add(new HomeMenuItem
                {
                    Id = MenuItemType.Settings,
                    ItemGroup = MenuItemGroup.Parametres,
                    Image = "setting.png",
                    Title = AppResources.pn_Settings
                });

                /* About */
                menuItems.Add(new HomeMenuItem
                {
                    Id = MenuItemType.About,
                    ItemGroup = MenuItemGroup.Parametres,
                    Image = "information.png",
                    Title = AppResources.pn_About
                });

                // Modules qui existe dans la version Pro Seulement 
                if (App.Settings.Mobile_Edition >= Mobile_Edition.Pro)
                {
                    /* Page tresorerie */
                    menuItems.Add(new HomeMenuItem
                    {
                        Id = MenuItemType.Tresorerie,
                        Image = "money.png",
                        ItemGroup = MenuItemGroup.Tresorerie,
                        Title = AppResources.pn_Tresorerie,
                        CodeObjet = XpertObjets.BSE_COMPTE,
                        Action = XpertActions.AcSelect
                    });

                    // Modules qui sont affichés dans la version Pro avec un compte Adiministrateur seulement ...
                    if (AppManager.HasAdmin)
                    {
                        /* Page Notification */
                        menuItems.Add(new HomeMenuItem
                        {
                            Id = MenuItemType.Notification,
                            ItemGroup = MenuItemGroup.Home,
                            Image = "bell.png",
                            Title = AppResources.pn_Notification,
                            NotificationBadgeIsVisible = true,
                            CountOfNotifications = numberOfNotifications
                        });
                        ReloadNumberOfNotifications();

                        /* Commande */
                        menuItems.Add(new HomeMenuItem
                        {
                            Id = MenuItemType.Commandes,
                            ItemGroup = MenuItemGroup.Ventes,
                            Title = AppResources.pn_Commandes,
                            Image = "shopping.png",
                            CodeObjet = XpertObjets.VTE_COMMANDE,
                            Action = XpertActions.AcSelect
                        });
                    }
                } /* fin du bloc de la version pro */

                /* Modules qui exitent dans XCOM ET XPHARM */
                if (Constants.AppName == Apps.XCOM_Mob || Constants.AppName == Apps.XPH_Mob)
                {
                    // new HomeMenuItem {Id = MenuItemType.rfid, Image = "", Title=AppResources.pn_RfidScan },
                    // new HomeMenuItem {Id = MenuItemType.invrfid, Image = "",Title= AppResources.pn_rfid_inventaire },

                    /* page Vente */
                    menuItems.Add(new HomeMenuItem
                    {
                        Id = MenuItemType.Ventes,
                        ItemGroup = MenuItemGroup.Ventes,
                        Title = AppResources.pn_Ventes,
                        CodeObjet = XpertObjets.VTE_VENTE,
                        Action = XpertActions.AcSelect,
                        Image = "vente.png"
                    });

                    if (App.Settings.Mobile_Edition == Mobile_Edition.Pro)
                    {
                        /* Vente au comptoire */
                        menuItems.Add(new HomeMenuItem
                        {
                            Id = MenuItemType.VenteComptoir,
                            ItemGroup = MenuItemGroup.Ventes,
                            Title = AppResources.pn_VteComptoir,
                            CodeObjet = XpertObjets.VTE_COMPTOIR,
                            Action = XpertActions.AcSelect,
                            Image = "vente.png"
                        });

                        if (AppManager.HasAdmin)
                        {
                            /* Transfert du fond */
                            menuItems.Add(new HomeMenuItem
                            {
                                Id = MenuItemType.TransfertDeFond,
                                ItemGroup = MenuItemGroup.Tresorerie,
                                Title = AppResources.pn_TransfertDeFond,
                                Image = "transferdefond.png",
                                CodeObjet = XpertObjets.View_TRS_VIREMENT,
                                Action = XpertActions.AcSelect,
                            });


                            /* Achat */
                            menuItems.Add(new HomeMenuItem
                            {
                                Id = MenuItemType.Achats,
                                ItemGroup = MenuItemGroup.Achats,
                                Title = AppResources.pn_Achats,
                                CodeObjet = XpertObjets.ACH_DOCUMENT,
                                Action = XpertActions.AcSelect,
                                Image = "achat.png"
                            });

                            /* Session */
                            menuItems.Add(new HomeMenuItem
                            {
                                Id = MenuItemType.Sessions,
                                ItemGroup = MenuItemGroup.Tresorerie,
                                Title = AppResources.pn_session,
                                CodeObjet = XpertObjets.TRS_RESUME_SESSION,
                                Action = XpertActions.AcSelect,
                                Image = "transaction.png",
                            });

                            /* Page tresorerie */
                            menuItems.Add(new HomeMenuItem
                            {
                                Id = MenuItemType.Tresorerie,
                                Image = "money.png",
                                ItemGroup = MenuItemGroup.Tresorerie,
                                Title = AppResources.pn_Tresorerie,
                                CodeObjet = XpertObjets.BSE_COMPTE,
                                Action = XpertActions.AcSelect
                            });

                            /* Manquant */
                            menuItems.Add(new HomeMenuItem
                            {
                                Id = MenuItemType.Manquants,
                                ItemGroup = MenuItemGroup.Stock,
                                Title = AppResources.pn_Manquants,
                                CodeObjet = XpertObjets.ACH_MANQUANTS,
                                Action = XpertActions.AcSelect,
                                Image = "manquant.png"
                            });

                            /* Sortie du stock */
                            menuItems.Add(new HomeMenuItem
                            {
                                Id = MenuItemType.Sortie,
                                ItemGroup = MenuItemGroup.Stock,
                                Title = AppResources.pn_Sortie,
                                CodeObjet = XpertObjets.STK_SORTIE,
                                Action = XpertActions.AcSelect,
                                Image = "sortie.png"
                            });


                            /* Rotation des produit */
                            menuItems.Add(new HomeMenuItem
                            {
                                Id = MenuItemType.RotationDesProduits,
                                ItemGroup = MenuItemGroup.Stock,
                                Title = AppResources.pn_RotationDesProduits,
                                CodeObjet = XpertObjets.TDB_ANALYSES,
                                Action = XpertActions.AcSelect,
                                IsNewModule = true,
                                Image = "rotation.png"
                            });

                            /* Indicateur */
                            menuItems.Add(new HomeMenuItem
                            {
                                Id = MenuItemType.SimpleIndicators,
                                ItemGroup = MenuItemGroup.Analyses,
                                Image = "indicateur.png",
                                Title = "Indicateurs",
                                CodeObjet = XpertObjets.TDB_ANALYSES,
                                Action = XpertActions.AcSelect
                            }); // AppResources.pn_Simpleindicator


                            /* Analyse */
                            menuItems.Add(new HomeMenuItem
                            {
                                Id = MenuItemType.EncAnalyses,
                                ItemGroup = MenuItemGroup.Analyses,
                                Title = AppResources.pn_Analyses,
                                CodeObjet = XpertObjets.TDB_ANALYSES,
                                Action = XpertActions.AcSelect,
                                Image = "analyse.png"

                            });
                        }
                    }

                    /* Module qui exite dans XCOM seulement */
                    if (Constants.AppName == Apps.XCOM_Mob)
                    {

                    }

                    /* Modules qui exite dans XPHARM seulement */
                    if (Constants.AppName == Apps.XPH_Mob)
                    {
                        /* Echnage */
                        menuItems.Add(new HomeMenuItem
                        {
                            Id = MenuItemType.Echange,
                            ItemGroup = MenuItemGroup.Stock,
                            Title = AppResources.pn_Echanges,
                            CodeObjet = XpertObjets.View_STK_ECHANGE,
                            Action = XpertActions.AcSelect,
                            IsNewModule = true,
                        });

                        /* Pyschotrope */
                        menuItems.Add(new HomeMenuItem
                        {
                            Id = MenuItemType.Psychotrop,
                            ItemGroup = MenuItemGroup.Psychotrope,
                            Title = AppResources.pn_VtePsychotrop,
                            CodeObjet = XpertObjets.VTE_PSYCHOTROP,
                            Action = XpertActions.AcSelect
                        });

                        /* Chifa */
                        menuItems.Add(new HomeMenuItem
                        {
                            Id = MenuItemType.Bordereaux,
                            ItemGroup = MenuItemGroup.CHIFA,
                            Title = AppResources.pn_Bordereaux,
                            CodeObjet = XpertObjets.CFA_BORDEREAU,
                            Action = XpertActions.AcSelect
                        });
                    }
                }
                else
                if (Constants.AppName == Apps.XCOM_Mob || Constants.AppName == Apps.X_DISTRIBUTION)
                {
                    /* Livraison */
                    menuItems.Add(new HomeMenuItem
                    {
                        Id = MenuItemType.Livraison,
                        ItemGroup = MenuItemGroup.Ventes,
                        Title = AppResources.pn_Livraison,
                        Image = "livraison.png",
                        CodeObjet = XpertObjets.VTE_LIVRAISON,
                        Action = XpertActions.AcSelect
                    });

                    if (Constants.AppName == Apps.X_DISTRIBUTION)
                    {
                        /* Page Chargement / Déchargement */
                        menuItems.Add(new HomeMenuItem
                        {
                            Id = MenuItemType.TransfertStock,
                            ItemGroup = MenuItemGroup.Stock,
                            Image = "transfer.png",
                            Title = AppResources.pn_TransfertStock
                        });

                        /* Page Tournee */
                        menuItems.Add(new HomeMenuItem
                        {
                            Id = MenuItemType.Tournee,
                            ItemGroup = MenuItemGroup.Ventes,
                            CodeObjet = XpertObjets.LIV_TOURNEE,
                            Action = Xpert.XpertActions.AcSelect,
                            Image = "tournee.png",
                            Title = "Tournee"
                        });

                        /* Page Synchronisation */
                        menuItems.Add(new HomeMenuItem
                        {
                            Id = MenuItemType.Synchronisation,
                            ItemGroup = MenuItemGroup.Parametres,
                            Image = "synchron.png",
                            Title = AppResources.pn_synchronisation
                        });
                    }
                }
            }

            //Menu Xpert Livraison
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

            //Xpert AGRI Manafiaa ACHATS
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

            //Xpert CATALOG
            else if (Constants.AppName == Apps.XACATALOG_Mob)
            {
                menuItems = new List<HomeMenuItem>
                {
                    new HomeMenuItem {Id = MenuItemType.MyCommandes, Image = "", Title=AppResources.pn_MyCommandes },
                    new HomeMenuItem {Id = MenuItemType.Catalogues, Image = "", Title=AppResources.pn_Catalogues },
                    new HomeMenuItem {Id = MenuItemType.About, Image = "", Title=AppResources.pn_About }
                };
            }

            //Xpert Boutik
            else if (Constants.AppName == Apps.X_BOUTIQUE)
            {
                menuItems = new List<HomeMenuItem>
                {
                    new HomeMenuItem {Id = MenuItemType.XBoutiqueHome, Image = "", Title=AppResources.pn_home },
                    new HomeMenuItem {Id = MenuItemType.XBoutique, Image = "", Title=AppResources.pn_Catalogues },
                    new HomeMenuItem {Id = MenuItemType.XMyCommandes, Image = "", Title=AppResources.pn_MyCommandes, VisibleToGuest=false },
                    new HomeMenuItem {Id = MenuItemType.XWishList, Image = "", Title="Wish List", VisibleToGuest=false },
                    new HomeMenuItem {Id = MenuItemType.XPurchased, Image = "", Title="Produits achetés", VisibleToGuest=false },

                    new HomeMenuItem {Id = MenuItemType.XProfile, Image = "", Title="Profile", VisibleToGuest=false },
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

            MessagingCenter.Subscribe<MenuPage, string>(this, "ChangeListIndex", async (obj, item) =>
            {
                HomeMenuItem selected = new HomeMenuItem();

                foreach (var itemitem in menuItems)
                {
                    if (itemitem.Id == (MenuItemType)Convert.ToInt32(item))
                        selected = itemitem;
                }
                ListViewMenu.SelectedItem = selected;
                ((MasterDetailPage)App.Current.MainPage).Detail.Focus();
            });


            MessagingCenter.Subscribe<SignUpPageViewModel, string>(this, "RELOAD_MENU", async (obj, str) =>
            {
                ReloadMenu();
            });

            MessagingCenter.Subscribe<LoginPageViewModel, string>(this, "RELOAD_MENU", async (obj, str) =>
            {
                ReloadMenu();
            });

            MessagingCenter.Subscribe<NotificationPage, string>(this, "RELOAD_MENU", async (obj, str) =>
            {
                GetNumberOfNotifications();
                ReloadNumberOfNotifications();
            });
            MessagingCenter.Subscribe<App, string>(this, "RELOAD_MENU", async (obj, str) =>
            {
                GetNumberOfNotifications();
                ReloadNumberOfNotifications();
            });

            MessagingCenter.Subscribe<NotificationPage, string>(this, "RELOAD_MENU", async (obj, str) =>
            {
                GetNumberOfNotifications();
                ReloadNumberOfNotifications();
            });

            /* Styling elements when app is in offline mode or online mode */
            if (!App.Online)
            {
                Lbl_AppFullName.TextColor = Color.White;
                ClientId.TextColor = Color.White;
                XpertVersion.TextColor = Color.White;
                XpertExpireDate.TextColor = Color.White;
                connetionStatus.Text = "mode: " + AppResources.txt_offline;
                connetionStatus.TextColor = Color.White;
            }
            else
            {
                connetionStatus.Text = "mode: " + AppResources.txt_online;
            }
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

        private void btn_Disconnect_Clicked(object sender, EventArgs e)
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

        private void SortMenuItems()
        {
            if (Constants.AppName == Apps.XCOM_Mob || Constants.AppName == Apps.XPH_Mob || Constants.AppName == Apps.X_DISTRIBUTION)
            {
                var menus = menuItems.Where(x => x.HasPermission == true).ToList();
                var sorted = from menu in menus
                             orderby menu.ItemGroup, menu.Id
                             group menu by menu.ItemGroup into menuGroup
                             orderby menuGroup.Key
                             select new Grouping<string, int, HomeMenuItem>(menuGroup.Key.ToString(), (int)menuGroup.Key, menuGroup);

                //create a new collection of groups
                menuItemsGrouped = new ObservableCollection<Grouping<string, int, HomeMenuItem>>(sorted);
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

                SortMenuItems();
                ListViewMenu.ItemsSource = menuItemsGrouped;

                //  je n'ai pas compris pourquoi ce code est ici et est la cause du deuxième lancement ??

                //if (menus.Count > 0)
                //    ListViewMenu.SelectedItem = menus[0];

                UserDialogs.Instance.HideLoading();
            }
            catch (Exception ex)
            {
                UserDialogs.Instance.HideLoading();
                await UserDialogs.Instance.AlertAsync(WSApi2.GetExceptionMessage(ex), AppResources.alrt_msg_Alert,
                    AppResources.alrt_msg_Ok);
            }

        }


        /// <summary>
        /// function pour reloader le menu quand le nombre de notification change
        /// </summary>
        private void ReloadNumberOfNotifications()
        {
            Device.BeginInvokeOnMainThread(() =>
            {
                foreach (var menu in menuItems)
                {
                    if (menu.Id == MenuItemType.Notification)
                    {
                        menu.CountOfNotifications = this.NumberOfNotifications;
                        if (menu.CountOfNotifications == 0)
                        {
                            menu.NotificationBadgeIsVisible = false;
                        }
                        else menu.NotificationBadgeIsVisible = true;
                    }
                }

                SortMenuItems();
                ListViewMenu.ItemsSource = menuItemsGrouped;
            });
        }


        /// <summary>
        /// avoir le nombre de notification stocké dans sqlite
        /// </summary>
        private void GetNumberOfNotifications()
        {
            if (App.Settings.Notifiaction != null)
            {
                if (Manager.isJson(App.Settings.Notifiaction))
                {
                    try
                    {
                        int nbNotification = 0;
                        List<Notification> tempList = JsonConvert.DeserializeObject<List<Notification>>(App.Settings.Notifiaction);
                        foreach (Notification notification in tempList)
                            if (notification.IsUnRead) nbNotification++;
                        if (nbNotification == 0)
                            this.NumberOfNotifications = 0;
                        else this.NumberOfNotifications = nbNotification;
                    }
                    catch (Exception)
                    {

                    }

                }
            }
            else
            {
                this.NumberOfNotifications = 0;
            }
        }

    }

}
