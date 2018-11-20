using System;
using System.Net;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using XpertMobileApp.Models;

namespace XpertMobileApp.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class LoginPage : ContentPage
	{
		public LoginPage ()
		{
			InitializeComponent ();

            NavigationPage.SetHasNavigationBar(this, false);
        }

        async Task ConnectUserAsync(object sender, EventArgs e)
        {
            // Check if the WebService is configured
            if (string.IsNullOrEmpty(App.RestServiceUrl))
            {
                await DisplayAlert("Alerte", "Veuillez rensegner les informations du serveur", "Ok");
                return;
            }
            DateTime dlimit = new DateTime(2018, 12, 30);
            if (DateTime.Now > dlimit)
            {
                await DisplayAlert("Erreur", "Veuillez Contacter votre administrateur", "Ok");
                return;
            }

            User user = new User(Ent_UserName.Text, Ent_PassWord.Text);

            if (this.CheckUser(user))
            {
                // Authentification via le WebService
                Token result = await Login(user);

                // Cas d'un souci avec le web service 
                if (result == null) return;

                if (1 != null) // result.access_token != null
                {
                    user.Id = result.userID;
                    App.User = user;
                    
                    // Alerte apres la connexion
                    // DependencyService.Get<ITextToSpeech>().Speak("Hello" + " " + user.UserName + "!");

                    // suavegrade du user et du token en cours dans la bdd local
                    //  await App.UserDatabase.SaveItemAsync(user);
                    //  await App.TokenDatabase.SaveItemAsync(result);

                    App.CurrentUser = user;

                    if (Device.OS == TargetPlatform.Android)
                    {
                        Application.Current.MainPage = new MainPage();
                    }
                    else if (Device.OS == TargetPlatform.iOS)
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
                    await DisplayAlert("Login", "Connexion refusée, veuillez vérifier vos identifiants de connexion!", "Ok");
                }
            }
            else
            {
                await DisplayAlert("Login", "Nom d'utilisateur ou mot de passe incorrect!", "Ok");
            }
        }

        private async Task<Token> Login(User user)
        {
            try
            {
                return new Token();
               // Token result = App.RestService.Login(user.UserName, user.PassWord);
               // return result != null ? result : new Token();
            }
            catch (WebException e)
            {
                await DisplayAlert("Alert", e.Message, "ok");
                return null;
            }
        }


        private bool CheckUser(User user)
        {
            if (user.UserName != "" && user.PassWord != "")
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
    }
}