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
    public partial class EmballageSelector : PopupPage
    {

        EmballageSelectorViewModel viewModel;

        public EmballageSelector()
        {
            InitializeComponent();

            BindingContext = viewModel = new EmballageSelectorViewModel();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            viewModel.LoadItemsCommand.Execute(null);
        }

        private async void OnClose(object sender, EventArgs e)
        {
            MessagingCenter.Send(this, MCDico.ITEM_SELECTED, viewModel.Items.ToList());

            await PopupNavigation.Instance.PopAsync();
        }

        async void btn_Search_Clicked(object sender, EventArgs e)
        {
            viewModel.LoadItemsCommand.Execute(null);
        }

        private void ItemsListView_SelectionChanged(object sender, Syncfusion.ListView.XForms.ItemSelectionChangedEventArgs e)
        {
            btnSelect.IsEnabled = true;
        }
    }
}
