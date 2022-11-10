using Acr.UserDialogs;
using Rg.Plugins.Popup.Services;
using Syncfusion.XForms.Buttons;
using System;
using System.Linq;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using XpertMobileApp.DAL;
using XpertMobileApp.Helpers;
using XpertMobileApp.ViewModels;

namespace XpertMobileApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class TiersPage : ContentPage
    {
        TiersViewModel viewModel;
        private TiersSelector itemSelector;
        public GoogleVisionBS gvsScannedBarcode;
        public string CurrentStream = Guid.NewGuid().ToString();
        public TiersPage()
        {
            InitializeComponent();

            BindingContext = viewModel = new TiersViewModel();
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();
        }

        async void OnItemSelected(object sender, SelectedItemChangedEventArgs args)
        {
            /*
            var item = args.SelectedItem as View_TRS_TIERS;
            if (item == null)
                return;

            await Navigation.PushAsync(new VenteDetailPage(item));

            // Manually deselect item.
            ItemsListView.SelectedItem = null;                        
           */
        }
        async void AddItem_Clicked(object sender, EventArgs e)
        {
            NewTiersPage form = new NewTiersPage(null);
            await Navigation.PushModalAsync(new NavigationPage(form));
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            if (viewModel.Items.Count == 0)
                viewModel.LoadItemsCommand.Execute(null);

            if (viewModel.Familles.Count == 0)
                viewModel.LoadExtrasDataCommand.Execute(null);
        }

        private async void btn_Select_Clicked(object sender, EventArgs e)
        {
            await PopupNavigation.Instance.PushAsync(itemSelector);
        }

        private void rg_solde_StateChanged(object sender, Syncfusion.XForms.Buttons.StateChangedEventArgs e)
        {
            if (e.IsChecked.HasValue && e.IsChecked.Value)
            {
                viewModel.SoldOperator = (sender as SfRadioButton).ClassId;
            }
        }

        private async void ShowHideFilter(object sender, EventArgs e)
        {
            TiersPopupFilter filter = new TiersPopupFilter(viewModel);
            //Load data for the first time ...
            await PopupNavigation.Instance.PushAsync(filter);
        }

        private void ApplyFilter(object sender, EventArgs e)
        {
            // clearing the tiersScanned
            viewModel.TiersScanned = null;
            viewModel.LoadItemsCommand.Execute(null);
        }

        private void SortAToZ(object sender, EventArgs e)
        {
            //sortLabelZA.IsVisible = true;
            //sortLabelAZ.IsVisible = false;
            viewModel.sortAtoZ = false;
            viewModel.LoadItemsCommand.Execute(null);
        }

        private void SortZToA(object sender, EventArgs e)
        {
            //sortLabelZA.IsVisible = false;
            //sortLabelAZ.IsVisible = true;
            viewModel.sortAtoZ = true;
            viewModel.LoadItemsCommand.Execute(null);
        }

        private async void EmployeeView_SelectionChanged(object sender, Xamarin.Forms.SelectionChangedEventArgs e)
        {
            try
            {
                if (e.CurrentSelection.Count != 0)
                {
                    var item = e.CurrentSelection[0] as View_TRS_TIERS;
                    if (item != null)
                    {
                        //UserDialogs.Instance.AlertAsync(); 
                        await PopupNavigation.Instance.PushAsync(new TierDetailPage(item));
                        ClientsView.SelectedItem = null;
                    }
                }


            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        private void ScanQrCode_Clicked(object sender, EventArgs e)
        {
            try
            {
                gvsScannedBarcode = new GoogleVisionBS();
                MainPage RootPage = Application.Current.MainPage as MainPage;
                var detail = RootPage.Detail;
                gvsScannedBarcode.UserSubmitted += async (_, tiersScanned) =>
                {

                    // une solution pour la fonction Load More après un changement dans le contenu de listView
                    // enregistrez la fonction responsable OnCanLoadMore avant de l'écraser pour bloquer le chargement
                    // d'autres éléments(en appelant la fonction LoadMore) dans la liste après la numérisation d'un code-barres
                    // afin de limiter l'affichage du seul produit numérisé

                    var func = viewModel.Items.OnCanLoadMore;
                    viewModel.Items.OnCanLoadMore = () =>
                    {
                        return false;
                    };

                    viewModel.TiersScanned = tiersScanned;
                    await detail.Navigation.PopAsync();
                    viewModel.LoadItemsCommand.Execute(null);
                };
                detail.Navigation.PushAsync(gvsScannedBarcode);
        }
            catch (Exception ex)
            {

                throw ex;
            }
        }
    }
}