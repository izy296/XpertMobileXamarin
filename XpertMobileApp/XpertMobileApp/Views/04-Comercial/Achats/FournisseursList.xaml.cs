using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using XpertMobileApp.Base;
using XpertMobileApp.DAL;
using XpertMobileApp.SQLite_Managment;
using XpertMobileApp.ViewModels;

namespace XpertMobileApp.Views._04_Comercial.Achats
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class FournisseursList : XBasePage
    {
        private TiersSelectorViewModel viewModel { get; }

        public FournisseursList()
        {
            InitializeComponent();
            viewModel = new TiersSelectorViewModel();
            BindingContext = viewModel;
        }
        protected async override void OnAppearing()
        {
            base.OnAppearing();
            await GetSuppliersList();
        }

        public async Task GetSuppliersList()
        {
            try
            {
                if (viewModel.Items.Count == 0)
                {
                    viewModel.SearchedType = "F";
                    viewModel.LoadItemsCommand.Execute(null);
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public override void SearchCommand()
        {
            base.SearchCommand();
            viewModel.SearchedText = SearchBarText;
            viewModel.ExecuteLoadItemsCommand();
        }
        private async void OnItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            try
            {
                var selectedSuppliers = e.SelectedItem as View_TRS_TIERS;
                if (selectedSuppliers == null)
                    return;

                await Navigation.PushAsync(new NewAchatPage(selectedSuppliers));

                // Manually deselect item.
                ItemsListView.SelectedItem = null;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
    }
}