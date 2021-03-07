using System;
using Rg.Plugins.Popup.Pages;
using Xamarin.Forms;
using Rg.Plugins.Popup.Services;
using XpertMobileApp.ViewModels;
using XpertMobileApp.DAL;
using XpertMobileApp.Api.Managers;

namespace XpertMobileApp.Views
{
    public partial class OpenSessionPage : PopupPage
    {
        OpenSessionViewModel viewModel;


        public OpenSessionPage(string stream)
        {
            InitializeComponent();

            BindingContext = viewModel = new OpenSessionViewModel();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            SfN_MONT_DEPART.Focus();
        }

        private async void btnCancel_Clicked(object sender, EventArgs e)
        {
            await PopupNavigation.Instance.PopAsync();
        }

        private async void btnValidate_Clicked(object sender, EventArgs e)
        {
            var res = await viewModel.OpenSession();
            if(res == true)
                await PopupNavigation.Instance.PopAsync();
        }


    }
}
