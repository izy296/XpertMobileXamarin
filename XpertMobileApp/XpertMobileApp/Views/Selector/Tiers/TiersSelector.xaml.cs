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
    public partial class TiersSelector : PopupPage
    {

        public ContentPage pargentPage;

        TiersSelectorViewModel viewModel;

        public string SearchedType { get; set; } = "";

        public TiersSelector()
        {
            InitializeComponent();

            BindingContext = viewModel = new TiersSelectorViewModel();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            viewModel.SearchedType = SearchedType;

            if (viewModel.Items.Count == 0)
                viewModel.LoadItemsCommand.Execute(null);
        }

        private async void OnClose(object sender, EventArgs e)
        {
            string msg = MCDico.ITEM_SELECTED;
            if (pargentPage != null)
            {
                msg = pargentPage.GetType().Name;
            }

            MessagingCenter.Send(this, msg, viewModel.SelectedItem);

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
