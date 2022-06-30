using XpertMobileApp.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using XpertMobileApp.Api;
using XpertMobileApp.ViewModels;
using XpertMobileApp.DAL;
using XpertMobileApp.Api.Models;
using Acr.UserDialogs.Infrastructure;
using System.Linq;


namespace XpertMobileApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MainPage : MasterDetailPage
    {
        Dictionary<int, NavigationPage> MenuPages = new Dictionary<int, NavigationPage>();

        public MainPage()
        {
            InitializeComponent();

            FlowDirection = App.PageFlowDirection;

            MasterBehavior = MasterBehavior.Popover;
            if (Constants.AppName == Apps.XCOM_Mob || Constants.AppName == Apps.XCOM_Livraison)
            {
                this.Detail = new NavigationPage(new HomePage());
                MenuPages.Add((int)MenuItemType.Home, (NavigationPage)Detail);
            }
            else if (Constants.AppName == Apps.XAGRI_Mob)
            {
                this.Detail = new NavigationPage(new AchatsPage());
                MenuPages.Add((int)MenuItemType.Achats, (NavigationPage)Detail);
            }
            else if (Constants.AppName == Apps.XACATALOG_Mob)
            {
                this.Detail = new NavigationPage(new MyCommandesPage());
                MenuPages.Add((int)MenuItemType.MyCommandes, (NavigationPage)Detail);
            }
            else if (Constants.AppName == Apps.X_BOUTIQUE)
            {
                this.Detail = new NavigationPage(new ProductHomePage());
                MenuPages.Add((int)MenuItemType.XBoutiqueHome, (NavigationPage)Detail);
            }
        }

        public ContentPage GetMenuPage(int idPage)
        {
            switch (idPage)
            {
                case (int)MenuItemType.Home:
                    return new HomePage();
                case (int)MenuItemType.Sessions:
                    return new SessionsPage();
                case (int)MenuItemType.Tresorerie:
                    return new TresoreriePage();
                case (int)MenuItemType.Encaissements:
                    return new EncaissementsPage();
                case (int)MenuItemType.Achats:
                    return new AchatsListPage();
                case (int)MenuItemType.AchatsProduction:
                    return new AchatsOHPage(AchRecMotifs.PesageForProduction);
                case (int)MenuItemType.OrdresProduction:
                    return new ProductionsPage(AchRecMotifs.PesageForProduction);
                case (int)MenuItemType.Ventes:
                    return new VentesPage(VentesTypes.Vente);
                case (int)MenuItemType.Livraison:
                    return new VentesPage(VentesTypes.Livraison);
                case (int)MenuItemType.VenteComptoir:
                    return new VentesPage(VentesTypes.VenteComptoir);
                case (int)MenuItemType.SimpleIndicators:
                    return new SimpleIndicators();
                case (int)MenuItemType.Psychotrop:
                    return new VentesPage(VentesTypes.VentePSYCO);
                case (int)MenuItemType.Tournee:
                    return new TourneesPage();
                case (int)MenuItemType.Bordereaux:
                    return new BordereauxPage();
                case (int)MenuItemType.Catalogues:
                //   return new Paging(); 
                case (int)MenuItemType.XBoutiqueHome:
                    return new ProductHomePage();
                case (int)MenuItemType.XBoutique:
                    return new CataloguePage();
                case (int)MenuItemType.XMyCommandes:
                    return new XMyCommandesPage();
                case (int)MenuItemType.XWishList:
                    return new WishListPage();
                case (int)MenuItemType.XPurchased:
                    return new PurchasedProdPage();
                case (int)MenuItemType.XProfile:
                    return new ProfilPage();
                case (int)MenuItemType.MyCommandes:
                //   return new Paging(); 
                case (int)MenuItemType.Commandes:
                    return new CommandesPage();
                case (int)MenuItemType.Produits:
                    return new ProduitsPage();
                case (int)MenuItemType.Manquants:
                    return new ManquantsPage();
                case (int)MenuItemType.rfid:
                // TODO    MenuPages.Add(id, (new RfidScanPage())); 
                case (int)MenuItemType.invrfid:
                // TODO    MenuPages.Add(id, (new RfidScanInventairePage())); 
                case (int)MenuItemType.Tiers:
                    return new TiersPage();
                case (int)MenuItemType.EncAnalyses:
                    return new sf_EncAnalysesPage();
                case (int)MenuItemType.AchatAgroAnalyses:
                    return new AchatsAnalysesPage();
                case (int)MenuItemType.Settings:
                    return new SettingsPage();
                case (int)MenuItemType.About:
                    return new AboutPage();
                case (int)MenuItemType.Sortie:
                    return new SortieListPage();
                case (int)MenuItemType.TransfertDeFond:
                    return new TransfertDeFondPage();
                case (int)MenuItemType.Notification:
                    return new NotificationPage();
                default:
                    return new HomePage();
            }
        }

        public async Task NavigateFromMenu(int id)
        {
            if (!MenuPages.ContainsKey(id))
            {
                switch (id)
                {
                    case (int)MenuItemType.Home:
                        MenuPages.Add(id, new NavigationPage(new HomePage()));
                        break;
                    case (int)MenuItemType.Sessions:
                        MenuPages.Add(id, new NavigationPage(new SessionsPage()));
                        break;
                    case (int)MenuItemType.Tresorerie:
                        MenuPages.Add(id, new NavigationPage(new TresoreriePage()));
                        break;
                    case (int)MenuItemType.Encaissements:
                        MenuPages.Add(id, new NavigationPage(new EncaissementsPage()));
                        break;
                    case (int)MenuItemType.Achats:
                        MenuPages.Add(id, new NavigationPage(new AchatsListPage()));
                        break;
                    case (int)MenuItemType.AchatsProduction:
                        MenuPages.Add(id, new NavigationPage(new AchatsOHPage(AchRecMotifs.PesageForProduction)));
                        break;
                    case (int)MenuItemType.OrdresProduction:
                        MenuPages.Add(id, new NavigationPage(new ProductionsPage(AchRecMotifs.PesageForProduction)));
                        break;
                    case (int)MenuItemType.Ventes:
                        MenuPages.Add(id, new NavigationPage(new VentesPage(VentesTypes.Vente)));
                        break;
                    case (int)MenuItemType.Livraison:
                        MenuPages.Add(id, new NavigationPage(new VentesPage(VentesTypes.Livraison)));
                        break;
                    case (int)MenuItemType.VenteComptoir:
                        MenuPages.Add(id, new NavigationPage(new VentesPage(VentesTypes.VenteComptoir)));
                        break;
                    case (int)MenuItemType.SimpleIndicators:
                        MenuPages.Add(id, new NavigationPage(new SimpleIndicators()));
                        break;
                    case (int)MenuItemType.Psychotrop:
                        MenuPages.Add(id, new NavigationPage(new VentesPage(VentesTypes.VentePSYCO)));
                        break;
                    case (int)MenuItemType.Tournee:
                        MenuPages.Add(id, new NavigationPage(new TourneesPage()));
                        break;
                    case (int)MenuItemType.Bordereaux:
                        MenuPages.Add(id, new NavigationPage(new BordereauxPage()));
                        break;
                    case (int)MenuItemType.Catalogues:
                        // MenuPages.Add(id, (new Paging())); 
                        break;
                    case (int)MenuItemType.MyCommandes:
                        // MenuPages.Add(id, (new Paging())); 
                        break;
                    case (int)MenuItemType.Commandes:
                        MenuPages.Add(id, new NavigationPage(new CommandesPage()));
                        break;

                    case (int)MenuItemType.XBoutiqueHome:
                        MenuPages.Add(id, new NavigationPage(new ProductHomePage()));
                        break;
                    case (int)MenuItemType.XBoutique:
                        MenuPages.Add(id, new NavigationPage(new CataloguePage()));
                        break;
                    case (int)MenuItemType.XMyCommandes:
                        MenuPages.Add(id, new NavigationPage(new XMyCommandesPage()));
                        break;
                    case (int)MenuItemType.XWishList:
                        MenuPages.Add(id, new NavigationPage(new WishListPage()));
                        break;
                    case (int)MenuItemType.XPurchased:
                        MenuPages.Add(id, new NavigationPage(new PurchasedProdPage()));
                        break;
                    case (int)MenuItemType.XProfile:
                        MenuPages.Add(id, new NavigationPage(new ProfilPage()));
                        break;
                    case (int)MenuItemType.Produits:
                        MenuPages.Add(id, new NavigationPage(new ProduitsPage()));
                        break;
                    case (int)MenuItemType.Manquants:
                        MenuPages.Add(id, new NavigationPage(new ManquantsPage()));
                        break;
                    //Navigation to the new EntreListPage 
                    //case (int)MenuItemType.Entre:
                    //    MenuPages.Add(id, new NavigationPage(new EntreListPage()));
                    //    break;
                    //Navigation to the new EntreListPage 
                    case (int)MenuItemType.Sortie:
                        MenuPages.Add(id, new NavigationPage(new SortieListPage()));
                        break;
                    case (int)MenuItemType.rfid:
                        // TODO    MenuPages.Add(id, (new RfidScanPage())); 
                        break;
                    case (int)MenuItemType.invrfid:
                        // TODO    MenuPages.Add(id, (new RfidScanInventairePage())); 
                        break;
                    case (int)MenuItemType.Tiers:
                        MenuPages.Add(id, new NavigationPage(new TiersPage()));
                        break;
                    case (int)MenuItemType.EncAnalyses:
                        MenuPages.Add(id, new NavigationPage(new sf_EncAnalysesPage()));
                        break;
                    case (int)MenuItemType.AchatAgroAnalyses:
                        MenuPages.Add(id, new NavigationPage(new AchatsAnalysesPage()));
                        break;
                    case (int)MenuItemType.Settings:
                        MenuPages.Add(id, new NavigationPage(new SettingsPage()));
                        break;
                    case (int)MenuItemType.About:
                        MenuPages.Add(id, new NavigationPage(new AboutPage()));
                        break;
                    case (int)MenuItemType.TransfertDeFond:
                        MenuPages.Add(id, new NavigationPage(new TransfertDeFondPage()));
                        break;
                    case (int)MenuItemType.Notification:
                        MenuPages.Add(id, new NavigationPage(new NotificationPage()));
                        break;
                }
            }

            if (id > 0)
            {
                var newPage = MenuPages[id];

                if (newPage != null && Detail != newPage)
                {
                    //Detail = newPage; 
                    NavigationPage.SetHasNavigationBar(newPage, false);
                    if (Detail.Navigation.NavigationStack.Count > 1)
                    {
                        var page = Detail.Navigation.NavigationStack[Detail.Navigation.NavigationStack.Count - 1];
                        await Detail.Navigation.PushAsync(newPage);
                        Detail.Navigation.RemovePage(page);
                    }
                    else
                    {
                        await Detail.Navigation.PushAsync(newPage);
                    }

                    //Detail.Navigation.NavigationStack.Append(newPage); 

                    if (Device.RuntimePlatform == Device.Android)
                        await Task.Delay(100);

                    IsPresented = false;
                }
            }
        }

    }
}