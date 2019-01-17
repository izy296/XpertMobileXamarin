using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Xpert.Pharm.DAL;
using XpertMobileApp.Api.Services;
using XpertMobileApp.Data;
using XpertMobileApp.Helpers;
using XpertMobileApp.Models;
using XpertMobileApp.Services;
using XpertMobileApp.ViewModels;

namespace XpertMobileApp.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class ActivationPage : ContentPage
	{
        ActivationViewModel viewModel;

        private LicState currentLicenceState;

        public ActivationPage(LicState state = LicState.NotActivated)
		{
			InitializeComponent ();

            currentLicenceState = state;

            BindingContext = viewModel = new ActivationViewModel();

            NavigationPage.SetHasNavigationBar(this, false);

            Init();
        }

        private void Init()
        {
            App.StatrtCheckIfInternet(Lbl_NoInternet, this);

            Ent_UserEemail.Text = "";
            Ent_ClientId.Text = "";
            Ent_UserEemail.Completed += (s, e) => Ent_ClientId.Focus();
            Ent_ClientId.Completed += (s, e) => ConnectUserAsync(s, e);
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            string message = LicActivator.GetLicenceMsgFromState(currentLicenceState);

            // User if translation if changed
            Lbl_UserEemail.Text = AppResources.lp_lbl_UserEemail;
            Lbl_ClientId.Text = AppResources.lp_lbl_ClientId;
            Ent_UserEemail.Placeholder = AppResources.lp_ph_UserEemail;
            Ent_ClientId.Placeholder = AppResources.lp_ph_ClientId;
            Btn_LogIn.Text = AppResources.lp_btn_Activate;
        }

        async Task ConnectUserAsync(object sender, EventArgs e)
        {
            // Check if the WebService is configured
            if (string.IsNullOrEmpty(App.RestServiceUrl))
            {
                await DisplayAlert(AppResources.alrt_msg_Alert, AppResources.alrt_msg_MissingServerInfos, AppResources.alrt_msg_Ok);
                return;
            }

            Client client = new Client(Ent_UserEemail.Text, Ent_ClientId.Text);

            if (this.CheckUser(client))
            {
                try
                {
                    Client result = await viewModel.ActivateClient(client);
                    DateTime LicenceEndDate = viewModel.GetEndDate(result.LicenceTxt);
                    int nbrDays = Convert.ToInt32(DateTime.Now.Subtract(LicenceEndDate).TotalDays);
                    if(nbrDays > 0)
                    {
                        await DisplayAlert(AppResources.alrt_msg_Info, 
                            String.Format(AppResources.msg_ActivationSucces), AppResources.alrt_msg_Ok);
                    }

                }
                catch (Exception ex)
                {
                    await DisplayAlert(AppResources.lp_Login, ex.Message, AppResources.alrt_msg_Ok);
                }

                if (Device.RuntimePlatform == Device.Android)
                {
                    Application.Current.MainPage = new MainPage();
                }
                else if (Device.RuntimePlatform == Device.iOS)
                {
                    await Navigation.PushModalAsync(new MainPage(), false);
                }
                else
                {
                    Application.Current.MainPage = new MainPage();
                }
            }
            else
            {
                await DisplayAlert(AppResources.lp_Login, AppResources.lp_login_WrongAcces, AppResources.alrt_msg_Ok);
            }
        }

        private async Task<Token> Login(User user)
        {
            try
            {
                Token result = await WebServiceClient.Login(App.RestServiceUrl, user.UserName, user.PassWord);
                return result != null ? result : new Token();
            }
            catch (XpertException e)
            {
                if(e.Code == XpertException.ERROR_XPERT_INCORRECTPASSWORD)
                { 
                await DisplayAlert(AppResources.alrt_msg_Alert, AppResources.err_msg_IncorrectLoginInfos, AppResources.alrt_msg_Ok);
                } else
                {
                    await DisplayAlert(AppResources.alrt_msg_Alert, e.Message, AppResources.alrt_msg_Ok);
                }
                return null;
            }
        }

        private bool CheckUser(Client user)
        {
            if (user.Email != "" && user.ClientId != "")
            {
                return true;
            }
            return false;
        }

        protected void Settings_Clicked(object sender, EventArgs e)
        {
             SettingsPage sp = new SettingsPage(true);
             this.Navigation.PushModalAsync(new NavigationPage(sp));
        }

        private void Exit_Clicked(object sender, EventArgs e)
        {
            //System.Environment.Exit(1);
            var closer = DependencyService.Get<ICloseApplication>();
            closer?.closeApplication();
        }
    }
}