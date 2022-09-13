using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using XpertMobileApp.Base;
using XpertMobileApp.Models;
using XpertMobileApp.ViewModels;

namespace XpertMobileApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class RotationDesProduitsPage : XBasePage
    {
        RotationDesProduitsViewModel viewModel;

        private bool opened = false;

        double translateHide = -270;
        double translateShow = 0;
        public RotationDesProduitsPage()
        {
            InitializeComponent();

            BindingContext = viewModel = new RotationDesProduitsViewModel();
            //if (App.Settings.Language == "ar")
            //{
            //    translateHide = App.Current.MainPage.Width;
            //    translateShow = App.Current.MainPage.Width - 360;
            //}

            filterLayout.TranslateTo(translateHide, 0);
            FilterScroll.TranslateTo(translateHide, 0);


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
                var item = args.SelectedItem as RotationDesProduits;

                if (item == null)
                    return;

                await Navigation.PushAsync(new RotationProduitDetailPage(item, viewModel.BeginDate, viewModel.EndDate));

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
            viewModel.SearchedText = "";
            viewModel.ClearFilters();
            viewModel.LoadItemsCommand.Execute(null);
        }

        private void ComptePicker_SelectedIndexChanged(object sender, EventArgs e)
        {

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

        public override void HandleSearchBarTextChanged(object sender, string searchBarText)
        {
            viewModel.SearchedText = searchBarText;
            viewModel.LoadItemsCommand.Execute(null);
        }


    }
}