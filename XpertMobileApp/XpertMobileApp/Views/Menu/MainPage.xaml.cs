using XpertMobileApp.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using SampleBrowser.SfListView;
using XpertMobileApp.Api;
using XpertMobileApp.ViewModels;
using XpertMobileApp.DAL;

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
            if (Constants.AppName == Apps.XCOM_Mob)
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
                    case (int)MenuItemType.Encaissements:
                        MenuPages.Add(id, new NavigationPage(new EncaissementsPage()));
                        break;
                    case (int)MenuItemType.Achats:
                        MenuPages.Add(id, new NavigationPage(new AchatsPage()));
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
                    case (int)MenuItemType.Psychotrop:
                        MenuPages.Add(id, new NavigationPage(new VentesPage(VentesTypes.VentePSYCO)));
                        break;
                    case (int)MenuItemType.Bordereaux:
                        MenuPages.Add(id, new NavigationPage(new BordereauxPage()));
                        break;
                    case (int)MenuItemType.Catalogues:
                        MenuPages.Add(id, new NavigationPage(new Paging()));
                        break;
                    case (int)MenuItemType.MyCommandes:
                        MenuPages.Add(id, new NavigationPage(new Paging()));
                        break;
                    case (int)MenuItemType.Commandes:
                        MenuPages.Add(id, new NavigationPage(new CommandesPage()));
                        break;
                    case (int)MenuItemType.Produits:
                        MenuPages.Add(id, new NavigationPage(new ProduitsPage()));
                        break;
                    case (int)MenuItemType.Manquants:
                        MenuPages.Add(id, new NavigationPage(new ManquantsPage()));
                        break;
                    case (int)MenuItemType.rfid:
                        MenuPages.Add(id, new NavigationPage(new RfidScanPage()));
                        break;
                    case (int)MenuItemType.invrfid:
                        MenuPages.Add(id, new NavigationPage(new RfidScanInventairePage()));
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
                    case (int)MenuItemType.About:
                        MenuPages.Add(id, new NavigationPage(new AboutPage()));
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