using System;
using Rg.Plugins.Popup.Pages;
using Xamarin.Forms;
using Rg.Plugins.Popup.Services;
using XpertMobileApp.ViewModels;
using XpertMobileApp.Helpers;
using System.Linq;
using System.Threading.Tasks;
using XpertMobileApp.Api.Services;
using XpertMobileApp.DAL;

namespace XpertMobileApp.Views
{
    public partial class VteValidationPage : PopupPage
    {

        VteValidationViewModel viewModel;
        public string ParentStream { get; set; }
        public VteValidationPage(string stream)
        {
            InitializeComponent();
            ParentStream = stream;
            BindingContext = viewModel = new VteValidationViewModel();

            MessagingCenter.Subscribe<TiersSelector, View_TRS_TIERS>(this, viewModel.CurrentStream, async (obj, selectedItem) =>
            {
                viewModel.SelectedTiers = selectedItem;
            });
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
        }


        private void btnCancel_Clicked(object sender, EventArgs e)
        {

        }

        private async void btnValidate_Clicked(object sender, EventArgs e)
        {
            viewModel.ValidateVte(viewModel.Item);
           // XpertHelper.SendAction(this, CurrentStream, "", MCDico.ITEM_SELECTED, viewModel.Item);

            await PopupNavigation.Instance.PopAsync();
        }

        private TiersSelector itemSelector;
        private async void btn_Select_Clicked(object sender, EventArgs e)
        {
            if(itemSelector == null)
                itemSelector = new TiersSelector(viewModel.CurrentStream);

            await PopupNavigation.Instance.PushAsync(itemSelector);
        }
    }
}
