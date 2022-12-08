using Acr.UserDialogs;
using Rg.Plugins.Popup.Services;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using XpertMobileApp.Api.Models;
using XpertMobileApp.DAL;
using XpertMobileApp.SQLite_Managment;
using XpertMobileApp.ViewModels;
using XpertMobileApp.Views._03_CommonPages.Synchronisation;
using XpertMobileApp.Views.Templates;

namespace XpertMobileApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class TourneesPage : ContentPage
    {
        TourneesViewModel viewModel;

        public TourneesPage()
        {
            InitializeComponent();

            BindingContext = viewModel = new TourneesViewModel();

            MessagingCenter.Subscribe<TourneePopup, View_LIV_TOURNEE>(this, "UpdateTourneeStatus", async (sender, selectedItem) =>
            {
                await viewModel.UpdateTourneStatus(selectedItem);
                await viewModel.ExecuteLoadItemsCommand();

            });

            MessagingCenter.Subscribe<TourneePopup, View_LIV_TOURNEE>(this, "TourneeDetails", async (sender, selectedItem) =>
            {
                await Navigation.PushAsync(new TourneesDetailsPage(selectedItem.CODE_TOURNEE));
            });

            MessagingCenter.Subscribe<TourneesViewModel, View_LIV_TOURNEE>(this, "TourneeDetails", async (sender, selectedItem) =>
            {
                await Navigation.PushAsync(new TourneesDetailsPage(selectedItem.CODE_TOURNEE));
            });
        }
        TourneePopup popup;
        async void OnItemSelected(object sender, SelectedItemChangedEventArgs args)
        {
            var item = args.SelectedItem as View_LIV_TOURNEE;
            if (item == null)
                return;

            popup=new TourneePopup(item);
            await PopupNavigation.Instance.PushAsync(popup);
            //await Navigation.PushAsync(new TourneesDetailsPage(item.CODE_TOURNEE));

            // Manually deselect item.
            ItemsListView.SelectedItem = null;
        }

        async void AddItem_Clicked(object sender, EventArgs e)
        {
            // await Navigation.PushModalAsync(new NavigationPage(new NewEncaissementPage(null, viewModel.EncaissDisplayType)));
        }

        // protected override void OnAppearing()

        protected async override void OnAppearing()
        {
            base.OnAppearing();
            if (App.CODE_MAGASIN == null && App.User.UserGroup != "AD")
            {
                var obj = await SQLite_Manager.GetInstance().Table<View_LIV_TOURNEE>().ToListAsync();
                if (obj != null && obj.Count > 0)
                {
                    App.CODE_MAGASIN = obj[0].CODE_MAGASIN;
                }
                if (App.CODE_MAGASIN==null)
                {
                    await UserDialogs.Instance.AlertAsync("Aucun Magasin est assigner a vous ,s'il vous plait synchroniser les tournes", AppResources.alrt_msg_Alert,
    AppResources.alrt_msg_Ok);
                    await ((MainPage)App.Current.MainPage).NavigateFromMenu((int)MenuItemType.Synchronisation);
                }

            }
            //Addd();
            //List<View_LIV_TOURNEE> tournee = TourneeServices.getTrounee();


            //viewModel.Items = new Xamarin.Forms.Extended.InfiniteScrollCollection<View_LIV_TOURNEE>();
            //viewModel.Items.AddRange(tournee);


            //viewModel.Items.LoadMoreAsync();
            if (viewModel.Items.Count == 0)
                LoadData();

            //if (viewModel.Familles.Count == 0)
            //    viewModel.LoadExtrasDataCommand.Execute(null);
        }

    private void TypeFilter_Clicked(object sender, EventArgs e)
    {
        LoadData();
    }

    private void LoadData()
    {
        viewModel.LoadItemsCommand.Execute(null);
    }

        private async void ShowHideFilter(object sender, EventArgs e)
        {
            TourneesPopupFilter filter = new TourneesPopupFilter(viewModel);
            //Load data for the first time ...
            await PopupNavigation.Instance.PushAsync(filter);
        }
    }
}