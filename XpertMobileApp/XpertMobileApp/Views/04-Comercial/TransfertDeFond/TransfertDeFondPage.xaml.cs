using Rg.Plugins.Popup.Services;
using System;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using XpertMobileApp.DAL;
using XpertMobileApp.Helpers;
using XpertMobileApp.ViewModels;
using XpertMobileApp.Views._04_Comercial.TransfertDeFond;

namespace XpertMobileApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class TransfertDeFondPage : ContentPage
    {
        TransfertDeFondPageViewModel viewModel;
        private bool opened = false;

        public TransfertDeFondPage()
        {
            InitializeComponent();

            Title = AppResources.pn_TransfertDeFond;

            BindingContext = viewModel = new TransfertDeFondPageViewModel();
            filterLayout.TranslateTo(-270, 0);
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            if (viewModel.Items.Count == 0)
                viewModel.LoadItemsCommand.Execute(null);
            if (viewModel.Comptes.Count == 0)
                viewModel.LoadExtrasDataCommand.Execute(null);
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            filterLayout.TranslateTo(-270, 0);
        }
        private void CheckBox_CheckedChanged(object sender, CheckedChangedEventArgs e)
        {
            if (sender is CheckBox)
            {
                var checkbox = (CheckBox)sender;
                if (checkbox.IsChecked)
                {
                    viewModel.TransfertCloture = true;
                }
                else
                {
                    viewModel.TransfertCloture = false;
                }
            }
        }
        private void btn_ApplyFilter_Clicked(object sender, EventArgs e)
        {
            viewModel.LoadItemsCommand.Execute(null);
            opened = !opened;
            filterLayout.TranslateTo(-270, 0);
        }
        /// <summary>
        /// To open and close filter section
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Filter_Clicked(object sender, EventArgs e)
        {
            filterLayout.TranslateTo(-270, 0);
        }
        private void ClearFilters()
        {
            compteSourcePicker.SelectedItem = compteSourcePicker.ItemsSource[0];
            compteDestPicker.SelectedItem = compteDestPicker.ItemsSource[0];
            transfertClotureCheckBox.IsChecked = false;
        }
        /// <summary>
        /// Fonction qui permet de fermer la page de filtre 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_CancelFilter_Clicked(object sender, EventArgs e)
        {
            filterLayout.TranslateTo(-270, 0);
            ClearFilters();
            viewModel.LoadItemsCommand.Execute(null);
        }

        private async void OnItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            var item = e.SelectedItem as View_TRS_VIREMENT;
            if (item == null)
                return;
            // Manually deselect item.

            NewTransfertDeFondPopupPage form = new NewTransfertDeFondPopupPage(item);
            await PopupNavigation.Instance.PushAsync(form);

            ItemsListView.SelectedItem = null;
        }
        /// <summary>
        /// Fonction qui permet d'afficher une popup pour l'ajout d'un Transfert de Fond
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        async void AjoutItem_Clicked(object sender, EventArgs e)
        {
            NewTransfertDeFondPopupPage form = new NewTransfertDeFondPopupPage(null);
            await PopupNavigation.Instance.PushAsync(form);
        }


        /// <summary>
        /// Show hide the filter section when clicking to the floating button
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void showHideFilter(object sender, EventArgs e)
        {
            if (opened)
            {
                filterLayout.TranslateTo(-270, 0);
                opened = !opened;
            }
            else
            {
                filterLayout.TranslateTo(0, 0);
                opened = !opened;
            }
        }
    }
}