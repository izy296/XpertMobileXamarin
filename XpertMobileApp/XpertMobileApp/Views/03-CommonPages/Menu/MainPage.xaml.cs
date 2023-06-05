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
using XpertMobileApp.ViewModels.Entree;
using XpertMobileApp.Views.Entree;
using XpertMobileAppManafiaa.Views._06_Manafiaa.DaytimeDelivery;
using XpertMobileAppManafiaa.Views._06_Manafiaa.Stock;
using XpertMobileAppManafiaa.Views._03_CommonPages.Synchronisation;
using XpertMobileAppManafiaa.Views._06_Manafiaa.Statistics;

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
            if (Constants.AppName == Apps.XCOM_Livraison)
            {
                this.Detail = new NavigationPage(new HomePage());
                MenuPages.Add((int)MenuItemType.Home, (NavigationPage)Detail);


            }
            else if (Constants.AppName == Apps.XAGRI_Mob)
            {
                this.Detail = new NavigationPage(new AchatsPage());
                MenuPages.Add((int)MenuItemType.Achats, (NavigationPage)Detail);
            }
        }

        public ContentPage GetMenuPage(int idPage)
        {
            switch (idPage)
            {
                case (int)MenuItemType.Home:
                    return new HomePage();
                case (int)MenuItemType.Sessions:
                    return new DaytimeDelivery_DashBord();
                case (int)MenuItemType.Tresorerie:
                    return new TresoreriePage();
                case (int)MenuItemType.Encaissements:
                    return new EncaissementsPage();
                case (int)MenuItemType.Achats:
                    return new AchatsPage();
                case (int)MenuItemType.AchatsProduction:
                case (int)MenuItemType.AchatsPrestation:
                    {
                        if (Constants.AppName == Apps.XCOM_Abattoir)
                            if (idPage == (int)MenuItemType.AchatsProduction)
                                return new AchatsOHPageAbattoire(PesageMotifs.PesageForProduction);
                            else return new AchatsOHPageAbattoire(PesageMotifs.PesagePrestation);
                        else
                            return new AchatsOHPage(AchRecMotifs.PesageForProduction);
                    }
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
                case (int)MenuItemType.Tournee:
                    return new TourneesPage();
                case (int)MenuItemType.MyCommandes:
                //   return new Paging();
                case (int)MenuItemType.Commandes:
                    return new CommandesPage();
                case (int)MenuItemType.Produits:
                    return new StockPage();
                case (int)MenuItemType.Manquants:
                    return new ManquantsPage();
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
                case (int)MenuItemType.entreeStock:
                    return new EntreesPage();
                case (int)MenuItemType.statistics:
                    return new StatisticsPage();
                //return new EntreeFormPage(null,null);
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
                        MenuPages.Add(id, new NavigationPage(new DaytimeDelivery_DashBord()));
                        break;
                    case (int)MenuItemType.Tresorerie:
                        MenuPages.Add(id, new NavigationPage(new TresoreriePage()));
                        break;
                    case (int)MenuItemType.Encaissements:
                        MenuPages.Add(id, new NavigationPage(new EncaissementsPage()));
                        break;
                    case (int)MenuItemType.Achats:
                        MenuPages.Add(id, new NavigationPage(new AchatsPage()));
                        break;
                    case (int)MenuItemType.AchatsProduction:
                    case (int)MenuItemType.AchatsPrestation:
                        {
                            if (Constants.AppName == Apps.XCOM_Abattoir)
                                if (id == (int)MenuItemType.AchatsProduction)
                                    MenuPages.Add(id, new NavigationPage(new AchatsOHPageAbattoire(PesageMotifs.PesageForProduction)));
                                else MenuPages.Add(id, new NavigationPage(new AchatsOHPageAbattoire(PesageMotifs.PesagePrestation)));
                            else
                                MenuPages.Add(id, new NavigationPage(new AchatsOHPage(AchRecMotifs.PesageForProduction)));
                            break;
                        }
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
                    //case (int)MenuItemType.Bordereaux:
                    //    MenuPages.Add(id, new NavigationPage(new BordereauxPage()));
                    //    break;
                    case (int)MenuItemType.Catalogues:
                        break;
                    case (int)MenuItemType.MyCommandes:
                        break;
                    case (int)MenuItemType.Commandes:
                        MenuPages.Add(id, new NavigationPage(new CommandesPage()));
                        break;

                    case (int)MenuItemType.Produits:
                        MenuPages.Add(id, new NavigationPage(new StockPage()));
                        break;
                    case (int)MenuItemType.Manquants:
                        MenuPages.Add(id, new NavigationPage(new ManquantsPage()));
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
                    case (int)MenuItemType.entreeStock:
                        MenuPages.Add(id, new NavigationPage(new EntreesPage()));
                        break;
                    case (int)MenuItemType.synchronisation:
                        MenuPages.Add(id, new NavigationPage(new SynchronisationPage()));
                        break;
                    case (int)MenuItemType.statistics:
                        MenuPages.Add(id, new NavigationPage(new StatisticsPage()));
                        break;
                }
            }

            var newPage = MenuPages[id];

            if (newPage != null && Detail != newPage)
            {
                Detail = newPage;

                if (Device.RuntimePlatform == Device.Android)
                    await Task.Delay(100);

                IsPresented = false;
            }
        }
    }
}