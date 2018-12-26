using System;
using Rg.Plugins.Popup.Pages;
using Xamarin.Forms;
using Rg.Plugins.Popup.Services;
using XpertMobileApp.ViewModels;
using XpertMobileApp.Helpers;
using System.Linq;

namespace XpertMobileApp.Views
{
    public partial class ItemSelector : PopupPage
    {

        ListViewSelectorViewModel viewModel;

        public ItemSelector()
        {
            InitializeComponent();

            BindingContext = viewModel = new ListViewSelectorViewModel();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            if (viewModel.Items.Count == 0)
                viewModel.LoadItemsCommand.Execute(null);
        }

        private async void OnClose(object sender, EventArgs e)
        {
            MessagingCenter.Send(this, MCDico.ITEM_SELECTED, viewModel.SelectedItem);

            await PopupNavigation.Instance.PopAsync();
        }

        private void OnItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            btnSelect.IsEnabled = true;
        }

        private void btn_Search_Clicked(object sender, EventArgs e)
        {
            string searchedTxt = ent_Filter.Text;
            viewModel.FilterItems(searchedTxt);
        }
    }
}
