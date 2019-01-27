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
            Ent_ClientId.Completed += (s, e) => ActivateUserAsync(s, e);

            var DInfos = DependencyService.Get<IDeviceInfos>();
            if (!DInfos.HasPermission())
            {
                DInfos.RequestPermissions();
            }
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            string message = LicActivator.GetLicenceMsgFromState(currentLicenceState);
            lic_Message.Text = message;

            // User if translation if changed
            Lbl_UserEemail.Text = AppResources.lp_lbl_UserEemail;
            Lbl_ClientId.Text = AppResources.lp_lbl_ClientId;
            Ent_UserEemail.Placeholder = AppResources.lp_ph_UserEemail;
            Ent_ClientId.Placeholder = AppResources.lp_ph_ClientId;
            Btn_LogIn.Text = AppResources.lp_btn_Activate;
        }

        async Task ActivateUserAsync(object sender, EventArgs e)
        {
            if (viewModel.IsBusy)
                return;

            var DInfos = DependencyService.Get<IDeviceInfos>();
            if (!DInfos.HasPermission())
            {
                var res = await DisplayAlert(AppResources.msg_Confirmation, AppResources.alrt_msg_MissingDeviceInfos, AppResources.alrt_msg_Ok, AppResources.alrt_msg_Cancel);
                if (res)
                {
                    DInfos.RequestPermissions();
                    return;
                }
            }

            // Check if the WebService is configured
            if (string.IsNullOrEmpty(App.RestServiceUrl))
            {
                await DisplayAlert(AppResources.alrt_msg_Alert, AppResources.alrt_msg_MissingServerInfos, AppResources.alrt_msg_Ok);
                return;
            }

            if (this.CheckUser(viewModel.Client))
            {
                try
                {
                    Client result = await viewModel.ActivateClient();
                    if (result == null)
                        return;

                    DateTime LicenceEndDate = await viewModel.GetEndDate(result.LicenceTxt);
                    int nbrDays = Convert.ToInt32(LicenceEndDate.Subtract(DateTime.Now).TotalDays);
                    if(nbrDays > 0)
                    {
                        await DisplayAlert(AppResources.alrt_msg_Info, 
                            String.Format(AppResources.msg_ActivationSucces), AppResources.alrt_msg_Ok);

                        if (Device.RuntimePlatform == Device.Android)
                        {
                            Application.Current.MainPage = new LoginPage();
                        }
                        else if (Device.RuntimePlatform == Device.iOS)
                        {
                            await Navigation.PushModalAsync(new LoginPage(), false);
                        }
                        else
                        {
                            Application.Current.MainPage = new LoginPage();
                        }
                    }

                }
                catch (Exception ex)
                {
                    await DisplayAlert(AppResources.lp_Login, "Erreur lors de l'activation : " + ex.Message, AppResources.alrt_msg_Ok);
                }
            }
            else
            {
                await DisplayAlert(AppResources.lp_Login, "Informations d'activation manquantes", AppResources.alrt_msg_Ok);
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
    }
}