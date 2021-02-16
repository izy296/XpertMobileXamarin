using Acr.UserDialogs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Xpert.Common.WSClient.Helpers;
using XpertMobileApp.Api;
using XpertMobileApp.DAL;
using XpertMobileApp.Models;
using XpertMobileApp.Services;
using XpertMobileApp.ViewModels;
using XpertMobileApp.Views.Feedback;

namespace XpertMobileApp.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class BtqProductDetailPage : ContentPage
	{
        XprodDetailViewModel viewModel;

        public BtqProductDetailPage(Product prod, bool extraOptions, bool canEval)
        {
            InitializeComponent();

            viewModel = new XprodDetailViewModel(prod, this);
            viewModel.CanEval = canEval;
            BindingContext = viewModel;

            // Hide the menu
            NavigationPage.SetHasNavigationBar(this, false);

        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            if (App.User?.Token != null)
            {
                viewModel.ExecuteLoadPanierCommand(this);
            }
        }

        private async void Btn_NavigateBack_Clicked(object sender, EventArgs e)
        {
            await Navigation.PopAsync();
        }

        private async void cmd_Rate_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new ReviewPage(viewModel.Item));
        }

        private async void allReviews_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new FeedbackPage());
        }

        private void cmd_Buy_Clicked(object sender, EventArgs e)
        {

        }
    }
}