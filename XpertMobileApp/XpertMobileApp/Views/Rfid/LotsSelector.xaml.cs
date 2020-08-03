using System;
using Rg.Plugins.Popup.Pages;
using Xamarin.Forms;
using Rg.Plugins.Popup.Services;
using XpertMobileApp.ViewModels;
using XpertMobileApp.Helpers;
using System.Linq;
using System.Threading.Tasks;
using XpertMobileApp.Models;
using System.Collections.Generic;

namespace XpertMobileApp.Views
{
    public partial class LotsSelector : PopupPage
    {

        ListViewSelectorLotsViewModel viewModel;

        public LotsSelector(List<View_STK_STOCK> Lots)
        {
            InitializeComponent();

            BindingContext = viewModel = new ListViewSelectorLotsViewModel(Lots);
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
        }

        private async void OnClose(object sender, EventArgs e)
        {
            MessagingCenter.Send(this, MCDico.Lots_SELECTED, viewModel.SelectedItem);

            await PopupNavigation.Instance.PopAsync();
        }

        private void OnItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            btnSelect.IsEnabled = true;
        }


    }
}
