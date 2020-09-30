using System;
using Rg.Plugins.Popup.Pages;
using Xamarin.Forms;
using Rg.Plugins.Popup.Services;
using XpertMobileApp.ViewModels;
using XpertMobileApp.Helpers;
using System.Linq;
using System.Threading.Tasks;

namespace XpertMobileApp.Views
{
    public partial class ProductSelector : PopupPage
    {

        ProductSelectorViewModel viewModel;

        public string CodeTiers
        {
            get
            {
                return viewModel.CodeTiers;
            }
            set
            {
                viewModel.CodeTiers = value;
            }
        }

        public string AutoriserReception
        {
            get
            {
                return viewModel.AutoriserReception;
            }
            set
            {
                viewModel.AutoriserReception = value;
            }
        }

        public ProductSelector()
        {
            InitializeComponent();

            BindingContext = viewModel = new ProductSelectorViewModel();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            if (viewModel.Items.Count == 0)
                viewModel.LoadItemsCommand.Execute(null);
        }

        private async void OnClose(object sender, EventArgs e)
        {
            await PopupNavigation.Instance.PopAsync();
        }

        private void OnItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
           //  btnSelect.IsEnabled = true;
           //  btnRemove.IsEnabled = viewModel.SelectedItem != null && viewModel.SelectedItem.SelectedQUANTITE > 0;
           if(viewModel.SelectedItem != null) 
           {
             viewModel.SelectedItem.SelectedQUANTITE += 1;
           }
        }

        private async void btn_Search_Clicked(object sender, EventArgs e)
        {
            viewModel.LoadItemsCommand.Execute(null);
        }

        private void btnSelect_Clicked(object sender, EventArgs e)
        {
            if(viewModel.SelectedItem != null)
            {
                viewModel.SelectedItem.SelectedQUANTITE = 1; // +=
                btnRemove.IsEnabled = viewModel.SelectedItem.SelectedQUANTITE > 0;
                MessagingCenter.Send(this, MCDico.ITEM_SELECTED, viewModel.SelectedItem);
            }
        }

        private void btnRemove_Clicked(object sender, EventArgs e)
        {
            if (viewModel.SelectedItem != null)
            {
                viewModel.SelectedItem.SelectedQUANTITE = 0; // -=
                btnRemove.IsEnabled = viewModel.SelectedItem.SelectedQUANTITE > 0;
                MessagingCenter.Send(this, MCDico.REMOVE_ITEM, viewModel.SelectedItem);
            }
        }

    }
}
