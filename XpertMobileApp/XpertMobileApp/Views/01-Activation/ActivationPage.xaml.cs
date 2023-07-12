using Plugin.FirebasePushNotification;
using Rg.Plugins.Popup.Services;
using System;
using System.Linq;
using System.Net.Mail;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using XpertMobileApp.Api;
using XpertMobileApp.Api.Services;
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
            InitializeComponent();

            currentLicenceState = state;

            BindingContext = viewModel = new ActivationViewModel();

            Lbl_AppFullName.Text = Constants.AppFullName.Replace(" ", "\n");

            NavigationPage.SetHasNavigationBar(this, false);

            Init();
        }

        private void Init()
        {
            // App.StatrtCheckIfInternet(Lbl_NoInternet, this);

            Ent_UserEemail.Text = "";
            Ent_ClientId.Text = "";
            Ent_UserEemail.Completed += (s, e) => Ent_ClientId.Focus();
            Ent_ClientId.Completed += (s, e) => ActivateUserAsync(s, e);

            Ent_UserEemail.TextChanged += Ent_UserEemail_TextChanged;
            Ent_UserPhone.TextChanged += Ent_UserPhone_TextChanged;

            var DInfos = DependencyService.Get<IDeviceInfos>();
            if (!DInfos.HasPermission())
            {
                DInfos.RequestPermissions();
            }
        }

        private void Ent_UserPhone_TextChanged(object sender, TextChangedEventArgs e)
        {
            VerifyPhoneNumber(sender, null);
        }

        private void Ent_UserEemail_TextChanged(object sender, TextChangedEventArgs e)
        {
            VerifyEmail(sender,null);
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

        async void ActivateUserAsync(object sender, EventArgs e)
        {
            if (viewModel.IsBusy)
                return;

            if (!viewModel.PhoneFormatCorrect || !viewModel.EmailFormatCorrect)
            {
                lbl_check_warning.IsVisible = true;
                return;
            }
            lbl_check_warning.IsVisible = false;
            var DInfos = DependencyService.Get<IDeviceInfos>();
            /*
            if (!DInfos.HasPermission())
            {
                var res = await DisplayAlert(AppResources.msg_Confirmation, AppResources.alrt_msg_MissingDeviceInfos, AppResources.alrt_msg_Ok, AppResources.alrt_msg_Cancel);
                if (res)
                {
                    DInfos.RequestPermissions();
                    return;
                }
            }
            */
            // Check if the WebService is configured
            if (string.IsNullOrEmpty(App.RestServiceUrl))
            {
                CustomPopup AlertPopup = new CustomPopup(AppResources.alrt_msg_Alert, trueMessage: AppResources.alrt_msg_Ok);
                await PopupNavigation.Instance.PushAsync(AlertPopup);
                return;
            }

            if (this.CheckUser(viewModel.Client))
            {
                try
                {
                    Client result = await viewModel.ActivateClient();

                    // Client result = await viewModel.ActivateClient();

                    if (result == null)
                        return;

                    DateTime LicenceEndDate = await viewModel.GetEndDate(result.LicenceTxt);
                    int nbrDays = Convert.ToInt32(LicenceEndDate.Subtract(DateTime.Now).TotalDays);
                    if (nbrDays > 0)
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
                    CustomPopup AlertPopup = new CustomPopup("Erreur lors de l'activation : " + ex.Message, trueMessage: AppResources.alrt_msg_Ok);
                    await PopupNavigation.Instance.PushAsync(AlertPopup);
                }
            }
            else
            {
                CustomPopup AlertPopup = new CustomPopup("Informations d'activation manquantes", trueMessage: AppResources.alrt_msg_Ok);
                await PopupNavigation.Instance.PushAsync(AlertPopup);
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

        private void EnterInDemoMode(object sender, Syncfusion.XForms.Buttons.StateChangedEventArgs e)
        {
            try
            {
                if (e.IsChecked.HasValue && e.IsChecked.Value)
                {
                    Ent_ClientId.IsEnabled = false;
                    Ent_ClientId.IsPassword = true;
                    viewModel.Client.ClientId = "1540";
                    viewModel.Client.DemoMode = true;
                }
                else
                {
                    Ent_ClientId.IsEnabled = true;
                    Ent_ClientId.IsPassword = false;
                    viewModel.Client.ClientId = String.Empty;
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        private void VerifyEmail(object sender, FocusEventArgs e)
        {
            try
            {
                string email = Ent_UserEemail.Text.ToString();
                Regex regex = new Regex(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$");
                Match match = regex.Match(email);
                if (match.Success)
                {
                    lbl_email_warning.IsVisible = false;
                    viewModel.EmailFormatCorrect = true;
                }

                else
                {
                    lbl_email_warning.IsVisible = true;
                    viewModel.EmailFormatCorrect = true;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void VerifyPhoneNumber(object sender, FocusEventArgs e)
        {
            try
            {
                string phone = string.Empty;
                if (Ent_UserPhone.Text != null)
                {
                    phone = Ent_UserPhone.Text;
                }

                var regex = new Regex(@"^(00213|\+213|0)(5|6|7)[0-9]{8}$");
                Match match = regex.Match(phone);
                if (match.Success)
                {
                    lbl_phone_warning.IsVisible = false;
                    viewModel.PhoneFormatCorrect = true;
                }
                else
                {
                    lbl_phone_warning.IsVisible = true;
                    viewModel.PhoneFormatCorrect = false;
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
    }
}