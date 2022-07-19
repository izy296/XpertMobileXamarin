using Rg.Plugins.Popup.Services;
using Syncfusion.XForms.Buttons;
using System;
using System.Collections.Generic;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using XpertMobileApp.Api;
using XpertMobileApp.DAL;
using XpertMobileApp.Helpers;
using XpertMobileApp.Models;
using XpertMobileApp.ViewModels;

namespace XpertMobileApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ProduitsPage : ContentPage
    {
        ProduitsViewModel viewModel;

        private bool opened = false;

        double translateHide = -270;
        double translateShow = 0;
        public ProduitsPage()
        {
            InitializeComponent();

            BindingContext = viewModel = new ProduitsViewModel();
            //if (App.Settings.Language == "ar")
            //{
            //    translateHide = App.Current.MainPage.Width;
            //    translateShow = App.Current.MainPage.Width - 360;
            //}

            if (Constants.AppName == Apps.XCOM_Mob)
            {
                labosLabel.IsVisible = false;
                LaboPicker.IsVisible = false;
                tagsLabel.IsVisible = false;
                tagsPicker.IsVisible = false;
                checkBoxR.IsVisible = false;
            }
            else
            {
                fideleteLabel.IsVisible = false;
                fideleteRadioBtns.IsVisible = false;
            }
            filterLayout.TranslateTo(translateHide, 0);
            FilterScroll.TranslateTo(translateHide, 0);

            MessagingCenter.Subscribe<ProductTagSelector, List<BSE_PRODUIT_TAG>>(this, MCDico.ITEM_SELECTED, async (obj, items) =>
             {
                 viewModel.SelectedTag = items;
                 
                 TagsPicker.Text = "";
                 for (int i =0;i<items.Count;i++)
                 {
                     TagsPicker.Text += items[i].DESIGNATION;
                     if (i != items.Count - 1)
                         TagsPicker.Text += ", ";
                 }
                 //await DisplayAlert("hello", items[0].DESIGNATION, "ok");
            });

            FilterScroll.Focused += FilterScroll_Focused;
        }

        private void FilterScroll_Focused(object sender, FocusEventArgs e)
        {
            ItemsListView.Opacity = 0;
        }

        async void OnItemSelected(object sender, SelectedItemChangedEventArgs args)
        {
            if (App.Online)
            {
                if (opened)
                {
                    ItemsListView.Opacity = 1;
                    hideFilter();
                    ItemsListView.SelectedItem = null;
                    return;
                }
                var item = args.SelectedItem as STK_PRODUITS;

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
            hideFilter();
        }

        private void btn_ApplyFilter_Clicked(object sender, EventArgs e)
        {
            hideFilter();
            viewModel.LoadItemsCommand.Execute(null);
        }

        private void btn_CancelFilter_Clicked(object sender, EventArgs e)
        {
            hideFilter();
            //quantiteG.IsChecked = false;
            //quantiteE.IsChecked = false;
            //quantiteL.IsChecked = false;
            etatAll.IsChecked = false;
            etatActive.IsChecked = false;
            etatNonActive.IsChecked = false;
            TagsPicker.Text = "";
            viewModel.ClearFilters();
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


        private void etatAll_StateChanged(object sender, StateChangedEventArgs e)
        {
            if (e.IsChecked.HasValue && e.IsChecked.Value)
            {
                viewModel.EtatOperator = (sender as SfRadioButton).ClassId;
            }
        }

        private void showHideFilter(object sender, EventArgs e)
        {
            if (opened)
            {
                filterLayout.TranslateTo(translateHide, 0);
                FilterScroll.TranslateTo(translateHide, 0);
            }
            else
            {
                filterLayout.TranslateTo(translateShow, 0);
                FilterScroll.TranslateTo(translateShow, 0);
            }
            opened = !opened;
        }


        private void showFilter()
        {

                filterLayout.TranslateTo(translateShow, 0); 
            FilterScroll.TranslateTo(translateShow, 0);
            opened = !opened;
           
        }

        private void hideFilter()
        {

            filterLayout.TranslateTo(translateHide, 0);
            FilterScroll.TranslateTo(translateHide, 0);
            opened = !opened;

        }

        

        private async void buttonClick(object sender, EventArgs e)
        {
            ProductTagSelector productTagSelector = new ProductTagSelector();
            await PopupNavigation.Instance.PushAsync(productTagSelector);
        }


        private void CheckBox_StateChangedSM(object sender, StateChangedEventArgs e)
        {
            viewModel.CheckBoxSM =(bool) e.IsChecked;
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
    }
}