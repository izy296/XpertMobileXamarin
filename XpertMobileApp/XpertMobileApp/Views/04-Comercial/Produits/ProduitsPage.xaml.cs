using Rg.Plugins.Popup.Services;
using Syncfusion.XForms.Buttons;
using System;
using System.Collections.Generic;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using XpertMobileApp.Api;
using XpertMobileApp.Base;
using XpertMobileApp.DAL;
using XpertMobileApp.Helpers;
using XpertMobileApp.Models;
using XpertMobileApp.ViewModels;

namespace XpertMobileApp.Views
{

    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ProduitsPage : XBasePage
    {
        ProduitsViewModel viewModel;
        public static bool displayGrid { get; set; } = App.Settings.DisplayType;
        private bool opened = false;
        public ProduitsPage()
        {
            InitializeComponent();

            // Get the display mode of the products form sqlite...
            BindingContext = viewModel = new ProduitsViewModel();

            if (displayGrid == false)
            {
                ItemsListView.ItemsLayout = LinearItemsLayout.Vertical;
                changeDisplayItem.IconImageSource = "row_display.png";
                changeDisplayItem.Text = "Affichage par Grille";
            }
            else
            {
                ItemsListView.ItemsLayout = new GridItemsLayout(2, ItemsLayoutOrientation.Vertical);
                changeDisplayItem.IconImageSource = "grid_display.png";
                changeDisplayItem.Text = "Affichage par Ligne";
            }

        }
        private void FilterScroll_Focused(object sender, FocusEventArgs e)
        {
            ItemsListView.Opacity = 0;
        }

        async void OnItemSelected(object sender, Xamarin.Forms.SelectionChangedEventArgs args)
        {
            if (args.CurrentSelection.Count != 0)
                if (App.Online)
                {
                    if (opened)
                    {
                        ItemsListView.Opacity = 1;
                        ItemsListView.SelectedItem = null;
                        return;
                    }
                    var item = args.CurrentSelection[0] as STK_PRODUITS;

                    if (item == null)
                        return;

                    await Navigation.PushAsync(new ProduitDetailPage(item));

                    // Manually deselect item.
                    ItemsListView.SelectedItem = null;
                }
        }

        async void AddItem_Clicked(object sender, EventArgs e)
        {
            // await Navigation.PushModalAsync(new NavigationPage(new NewEncaissementPage(null, viewModel.EncaissDisplayType)));
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            Label label = new Label();
            if (viewModel.Items.Count == 0)
                LoadStats();

            if (viewModel.Familles.Count == 0)
                viewModel.LoadExtrasDataCommand.Execute(null);
        }

        private void TypeFilter_Clicked(object sender, EventArgs e)
        {
            LoadStats();
        }

        private void LoadStats()
        {
            viewModel.LoadItemsCommand.Execute(null);
        }

        private void Filter_Clicked(object sender, EventArgs e)
        {

        }

        private void btn_ApplyFilter_Clicked(object sender, EventArgs e)
        {
            viewModel.LoadItemsCommand.Execute(null);
        }

        private void ComptePicker_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        /// <summary>
        /// function pour scanner les code barres et retriever un produite
        /// </summary>
        public GoogleVisionBS gvsScannedBarcode;


        private void RowScan_Clicked(object sender, EventArgs e)
        {
            gvsScannedBarcode = new GoogleVisionBS();
            MainPage RootPage = Application.Current.MainPage as MainPage;
            var detail = RootPage.Detail;
            gvsScannedBarcode.UserSubmitted += async (_, scannedPassword) =>
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
                viewModel.BareCode = scannedPassword;
                await detail.Navigation.PopAsync();
                await viewModel.GetScanedProduct();

            };

            detail.Navigation.PushAsync(gvsScannedBarcode);
        }

        public override void SearchCommand()
        {
            base.SearchCommand();
            viewModel.SearchedText = SearchBarText;
            viewModel.ExecuteLoadItemsCommand();
        }

        private void CheckBox_StateChangedSM(object sender, StateChangedEventArgs e)
        {
            viewModel.CheckBoxSM = (bool)e.IsChecked;
        }

        private void CheckBox_StateChangedS(object sender, StateChangedEventArgs e)
        {
            viewModel.CheckBoxS = (bool)e.IsChecked;
        }

        private void CheckBox_StateChangedR(object sender, StateChangedEventArgs e)
        {
            viewModel.CheckBoxR = (bool)e.IsChecked;
        }

        private void FedeliteAll_StateChanged(object sender, StateChangedEventArgs e)
        {
            if (e.IsChecked.HasValue && e.IsChecked.Value)
            {
                viewModel.FidelOperator = (sender as SfRadioButton).ClassId;
            }
        }
        private void etatAll_StateChanged(object sender, StateChangedEventArgs e)
        {
            if (e.IsChecked.HasValue && e.IsChecked.Value)
            {
                viewModel.EtatOperator = (sender as SfRadioButton).ClassId;
            }
        }

        private async void ShowHideFilter(object sender, EventArgs e)
        {
            ProduitsPopupFilter filter = new ProduitsPopupFilter(viewModel);
            //Load data for the first time ...
            await PopupNavigation.Instance.PushAsync(filter);
        }

        private void Switch1_Toggled(object sender, ToggledEventArgs e)
        {
            try
            {
                viewModel.Items.Clear();
                viewModel.ItemsWithQteMagasin.Clear();
                viewModel.LoadItemsCommand.Execute(null);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        private async void SwitchDisplayMode(object sender, EventArgs e)
        {
            if (!displayGrid)
            {
                ItemsListView.ItemsLayout = new GridItemsLayout(2, ItemsLayoutOrientation.Vertical);
                changeDisplayItem.IconImageSource = "grid_display.png";
                changeDisplayItem.Text = "Affichage par Ligne";
            }
            else
            {
                ItemsListView.ItemsLayout = LinearItemsLayout.Vertical;
                changeDisplayItem.IconImageSource = "row_display.png";
                changeDisplayItem.Text = "Affichage par Grille";
            }
            displayGrid = !displayGrid;

            // save the display mode in the sqlite.. 
            App.Settings.DisplayType = displayGrid;
            await App.SettingsDatabase.SaveItemAsync(App.Settings);
            viewModel.Items.Clear();
            viewModel.ItemsWithQteMagasin.Clear();
            viewModel.LoadItemsCommand.Execute(null);
        }
    }
}