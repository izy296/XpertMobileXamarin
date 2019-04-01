using System;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using XpertMobileApp.Models;
using XpertMobileApp.Services;
using XpertMobileApp.ViewModels;

namespace XpertMobileApp.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class LoginPage : ContentPage
	{
        LoginViewModel viewModel;

        public LoginPage ()
		{
			InitializeComponent ();

            BindingContext = viewModel = new LoginViewModel();

            NavigationPage.SetHasNavigationBar(this, false);
            Init();
        }

        private void Init()
        {
            App.StatrtCheckIfInternet(Lbl_NoInternet, this);

            Ent_UserName.Text = "";
            Ent_PassWord.Text = "";
            Ent_UserName.Completed += (s, e) => Ent_PassWord.Focus();
            Ent_PassWord.Completed += (s, e) => ConnectUserAsync(s, e);
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            Lbl_UserName.Text = AppResources.lp_lbl_UserName;
            Lbl_PassWord.Text = AppResources.lp_lbl_PassWord;
            Ent_PassWord.Placeholder = AppResources.lp_ph_PassWord;
            Ent_UserName.Placeholder = AppResources.lp_ph_UserName;
            Btn_LogIn.Text = AppResources.lp_btn_Login;
        }

        async Task ConnectUserAsync(object sender, EventArgs e)
        {
            // Check if the WebService is configured
            if (string.IsNullOrEmpty(App.RestServiceUrl))
            {
                await DisplayAlert(AppResources.alrt_msg_Alert, AppResources.alrt_msg_MissingServerInfos, AppResources.alrt_msg_Ok);
                return;
            }

            User user = new User(Ent_UserName.Text, Ent_PassWord.Text);

            if (viewModel.CheckUser(user))
            {
                // Authentification via le WebService
                Token token = await viewModel.Login(user);

                // Cas d'un souci avec le web service 
                if (token == null) return;

                if (token.access_token != null)
                {
                    user.Id = token.userID;
                    user.Token = token;
                    App.User = user;
                    
                    // Alerte apres la connexion
                     DependencyService.Get<ITextToSpeech>().Speak(AppResources.app_speech_Hello + " " + user.UserName + "!");

                    // suavegrade du user et du token en cours dans la bdd local

                    try
                    { 
                       // await App.UserDatabase.SaveItemAsync(user);
                        await App.TokenDatabase.SaveItemAsync(token);
                    }
                    catch(Exception ex)
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
            }
            else
            {
                await DisplayAlert(AppResources.lp_Login, AppResources.lp_login_WrongAcces, AppResources.alrt_msg_Ok);
            }
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