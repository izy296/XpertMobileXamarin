using Rg.Plugins.Popup.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using XpertMobileApp.DAL;
using XpertMobileApp.ViewModels;
using XpertMobileApp.Views.ViewModels;

namespace XpertMobileApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class BeneficiaresDetailPage : ContentPage
    {
        public BeneficiaresDetailViewModel viewModel { get; set; }
        View_CFA_MOBILE_DETAIL_FACTURE Item;
        public BeneficiaresDetailPage(View_CFA_MOBILE_DETAIL_FACTURE item)
        {
            InitializeComponent();
            Item = item;
            BindingContext = viewModel = new BeneficiaresDetailViewModel(Item);
        }

        protected async override void OnAppearing()
        {
            base.OnAppearing();
            await viewModel.ExecuteLoadItemsCommand();
        }

        private void ExecutePullToRefresh(object sender, EventArgs e)
        {

        }

        private void OnItemSelected(object sender, SelectionChangedEventArgs e)
        {
            ItemsListView.SelectedItem = null;
        }

        private async void SMS_Btn_Clicked(object sender, EventArgs e)
        {
            PhonePopup popup = new PhonePopup("Choisi le num telephone", "Annuler", "Ok",listPhones:new List<string>(){ viewModel.Tier.TEL1_TIERS, viewModel.Tier.TEL2_TIERS });
            await PopupNavigation.Instance.PushAsync(popup);
            if (await popup.PopupClosedTask)
            {
                Device.OpenUri(new Uri($"sms:{popup.Result}&body=YourTextHere"));
            }
        }

        private async void Appel_Btn_Clicked(object sender, EventArgs e)
        {
            PhonePopup popup = new PhonePopup("Choisi le num telephone", "Annuler", "Ok", listPhones: new List<string>() { viewModel.Tier.TEL1_TIERS, viewModel.Tier.TEL2_TIERS });
            await PopupNavigation.Instance.PushAsync(popup);
            if (await popup.PopupClosedTask)
            {
                Device.OpenUri(new Uri($"tel:{popup.Result}"));

            }
        }

        private void Facts_Btn_Clicked(object sender, EventArgs e)
        {

        }
    }
}