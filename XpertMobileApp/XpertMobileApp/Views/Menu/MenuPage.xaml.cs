using XpertMobileApp.Models;
using System;
using System.Collections.Generic;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using XpertMobileApp.Services;

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
            /*
                new HomeMenuItem {Id = MenuItemType.rfid, Image = "", Title=AppResources.pn_RfidScan },
                new HomeMenuItem {Id = MenuItemType.invrfid, Image = "",Title= AppResources.pn_rfid_inventaire },
            */
            menuItems = new List<HomeMenuItem>
            {
                new HomeMenuItem {Id = MenuItemType.Home, Image = "", Title=AppResources.pn_home },
                new HomeMenuItem {Id = MenuItemType.Encaissements, Image = "", Title=AppResources.pn_encaissement },
                new HomeMenuItem {Id = MenuItemType.Achats, Image = "", Title=AppResources.pn_Achats },
                new HomeMenuItem {Id = MenuItemType.Reception, Image = "", Title=AppResources.pn_NewReception },
                
                new HomeMenuItem {Id = MenuItemType.Ventes, Image = "", Title=AppResources.pn_Ventes },
                new HomeMenuItem {Id = MenuItemType.Catalogues, Image = "", Title=AppResources.pn_Catalogues },
                new HomeMenuItem {Id = MenuItemType.Commandes, Image = "", Title=AppResources.pn_Commandes },
                new HomeMenuItem {Id = MenuItemType.Tiers, Image = "", Title=AppResources.pn_Tiers },
                new HomeMenuItem {Id = MenuItemType.Produits, Image = "", Title=AppResources.pn_Produits },
                new HomeMenuItem {Id = MenuItemType.rfid, Image = "", Title=AppResources.pn_RfidScan },
                new HomeMenuItem {Id = MenuItemType.invrfid, Image = "",Title= AppResources.pn_rfid_inventaire },
                new HomeMenuItem {Id = MenuItemType.Manquants, Image = "", Title=AppResources.pn_Manquants },
                new HomeMenuItem {Id = MenuItemType.EncAnalyses, Image = "", Title=AppResources.pn_Analyses },
                new HomeMenuItem {Id = MenuItemType.About, Image = "", Title=AppResources.pn_About }
            };

            ListViewMenu.ItemsSource = menuItems;

            ListViewMenu.SelectedItem = menuItems[0];

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
            try { 
            if(App.User != null && App.User.Token != null)
                App.TokenDatabase.DeleteItemAsync(App.User.Token);
            }
            catch
            {
            }

            App.User = null;
            Application.Current.MainPage = new LoginPage();
        }
    }
}