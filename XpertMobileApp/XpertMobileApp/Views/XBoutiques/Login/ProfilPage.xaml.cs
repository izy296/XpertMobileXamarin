using Acr.UserDialogs;
using Rg.Plugins.Popup.Services;
using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Xpert.Common.WSClient.Helpers;
using XpertMobileApp.Api;
using XpertMobileApp.Api.Managers;
using XpertMobileApp.Api.Services;
using XpertMobileApp.DAL;
using XpertMobileApp.Helpers;
using XpertMobileApp.Services;
using XpertMobileApp.ViewModels;
using XpertMobileApp.ViewModels.XLogin;
using ZXing.Net.Mobile.Forms;

namespace XpertMobileApp.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class ProfilPage : ContentPage
	{
        ProfilePageViewModel viewModel;

        public ProfilPage()
		{
            viewModel = new ProfilePageViewModel();
            viewModel.Title = "Profile";
            BindingContext = viewModel;
            InitializeComponent ();
        }

        protected async override void OnAppearing()
        {
            base.OnAppearing();

            var res = await viewModel.GetBll().GetItemAsync(App.User.Token.userID);
            viewModel.Email = res.Email;
            viewModel.Item = res;

        }
    }
}
 