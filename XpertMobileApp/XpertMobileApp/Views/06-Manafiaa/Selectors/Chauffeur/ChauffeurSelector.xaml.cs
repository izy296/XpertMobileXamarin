using System;
using Rg.Plugins.Popup.Pages;
using Xamarin.Forms;
using Rg.Plugins.Popup.Services;
using XpertMobileApp.ViewModels;
using XpertMobileApp.Helpers;

namespace XpertMobileApp.Views
{
    public partial class ChauffeurSelector : PopupPage
    {

        ChauffeurSelectorViewModel viewModel;

        public ChauffeurSelector()
        {
            InitializeComponent();

            BindingContext = viewModel = new ChauffeurSelectorViewModel();
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

        async void btn_Search_Clicked(object sender, EventArgs e)
        {
            viewModel.LoadItemsCommand.Execute(null);
        }
    }
}
