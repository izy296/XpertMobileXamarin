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

            menuItems = new List<HomeMenuItem>
            {
                new HomeMenuItem {Id = MenuItemType.Home, Image = "", Title=AppResources.pn_home },
                new HomeMenuItem {Id = MenuItemType.Encaissements, Image = "", Title=AppResources.pn_encaissement },
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
            App.User = null;
            Application.Current.MainPage = new LoginPage();
        }
    }
}