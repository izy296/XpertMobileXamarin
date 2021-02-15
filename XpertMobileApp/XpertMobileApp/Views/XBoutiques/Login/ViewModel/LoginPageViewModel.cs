using Acr.UserDialogs;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Internals;
using Xpert.Common.WSClient.Model;
using XpertMobileApp.Api;
using XpertMobileApp.Api.Managers;
using XpertMobileApp.DAL;
using XpertMobileApp.Models;
using XpertMobileApp.Services;


namespace XpertMobileApp.ViewModels.XLogin
{
    /// <summary>
    /// ViewModel for login page.
    /// </summary>
    [Preserve(AllMembers = true)]
    public class LoginPageViewModel : LoginViewModel
    {
        #region Fields

        private string password;

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance for the <see cref="LoginPageViewModel" /> class.
        /// </summary>
        public LoginPageViewModel()
        {
            //this.LoginCommand = new Command(this.LoginClicked);
            this.LoginCommand = new Command(async (object obj) => await this.LoginClicked(obj));
            this.SignUpCommand = new Command(this.SignUpClicked);
            this.ForgotPasswordCommand = new Command(this.ForgotPasswordClicked);
            this.SocialMediaLoginCommand = new Command(this.SocialLoggedIn);
        }

        #endregion

        #region property

        /// <summary>
        /// Gets or sets the property that is bound with an entry that gets the password from user in the login page.
        /// </summary>
        public string Password
        {
            get
            {
                return this.password;
            }

            set
            {
                if (this.password == value)
                {
                    return;
                }
                SetProperty(ref password, value);
            }
        }

        #endregion

        #region Command

        /// <summary>
        /// Gets or sets the command that is executed when the Log In button is clicked.
        /// </summary>
        public Command LoginCommand { get; set; }

        /// <summary>
        /// Gets or sets the command that is executed when the Sign Up button is clicked.
        /// </summary>
        public Command SignUpCommand { get; set; }

        /// <summary>
        /// Gets or sets the command that is executed when the Forgot Password button is clicked.
        /// </summary>
        public Command ForgotPasswordCommand { get; set; }

        /// <summary>
        /// Gets or sets the command that is executed when the social media login button is clicked.
        /// </summary>
        public Command SocialMediaLoginCommand { get; set; }

        #endregion

        #region methods

        /// <summary>
        /// Invoked when the Log In button is clicked.
        /// </summary>
        /// <param name="obj">The Object</param>
        private async Task LoginClicked(object obj)
        {
            try
            {
                bool connected = await ConnectUserAsync(Email, Password);
                popup(obj);
            }
            catch (Exception ex)
            {
                await UserDialogs.Instance.AlertAsync(ex.Message, AppResources.alrt_msg_Info, AppResources.alrt_msg_Ok);
            }
        }

        /// <summary>
        /// Invoked when the Sign Up button is clicked.
        /// </summary>
        /// <param name="obj">The Object</param>
        private void SignUpClicked(object obj)
        {
            // Do something
        }

        /// <summary>
        /// Invoked when the Forgot Password button is clicked.
        /// </summary>
        /// <param name="obj">The Object</param>
        private async void ForgotPasswordClicked(object obj)
        {
            var label = obj as Label;
            label.BackgroundColor = Color.FromHex("#70FFFFFF");
            await Task.Delay(100);
            label.BackgroundColor = Color.Transparent;
        }

        /// <summary>
        /// Invoked when social media login button is clicked.
        /// </summary>
        /// <param name="obj">The Object</param>
        private void SocialLoggedIn(object obj)
        {
            // Do something
        }

        #endregion






        private void popup(object obj)
        {
            Device.BeginInvokeOnMainThread(() =>
            {
                if (obj != null)
                    (obj as ContentPage).Navigation.PopAsync();
            });
        }

        private async Task<bool> ConnectUserAsync(string username, string password)
        {
            // Check if the WebService is configured
            if (string.IsNullOrEmpty(App.RestServiceUrl))
            {
                await UserDialogs.Instance.AlertAsync(AppResources.alrt_msg_NoConnexion, AppResources.alrt_msg_Alert, AppResources.alrt_msg_Ok);
                return false;
            }
            try
            {
                UserDialogs.Instance.ShowLoading(AppResources.txt_Waiting);
                User user = new User(username, password);

                if (CheckUser(user))
                {
                    // Authentification via le WebService
                    Token token = await Login(user);

                    // Cas d'un souci avec le web service 
                    if (token == null)
                    {
                        UserDialogs.Instance.HideLoading();
                        return false;
                    }

                    if (token.access_token != null)
                    {
                        user.Id = token.userID;
                        user.CODE_TIERS = token.CODE_TIERS;
                        user.UserGroup = token.UserGroup;
                        user.GroupName = token.GroupName;
                        user.ClientId = App.Settings.ClientId;
                        user.Token = token;
                        App.User = user;

                        CrudManager.InitServices();

                        // Alerte apres la connexion
                        // DependencyService.Get<ITextToSpeech>().Speak(AppResources.app_speech_Hello + " " + user.UserName + "!");

                        // suavegrade du user et du token en cours dans la bdd local

                        try
                        {
                            if (Constants.AppName != Apps.X_BOUTIQUE)
                            {
                                List<SYS_OBJET_PERMISSION> permissions = await AppManager.GetPermissions();
                            }
                            // await App.UserDatabase.SaveItemAsync(user);
                            await App.TokenDatabase.SaveItemAsync(token);
                        }
                        catch (Exception ex)
                        {
                            await UserDialogs.Instance.AlertAsync(ex.Message, AppResources.alrt_msg_Alert, AppResources.alrt_msg_Ok);
                            return false;
                        }
                    }

                    UserDialogs.Instance.HideLoading();
                    return true;
                }
                else
                {
                    await UserDialogs.Instance.AlertAsync(AppResources.lp_login_WrongAcces, AppResources.lp_Login, AppResources.alrt_msg_Ok);
                    UserDialogs.Instance.HideLoading();
                    return false;
                }
            }
            catch (Exception ex)
            {
                UserDialogs.Instance.HideLoading();
                await UserDialogs.Instance.AlertAsync(ex.Message, AppResources.lp_Login, AppResources.alrt_msg_Ok);
                return false;
            }
        }

        public bool CheckUser(User user)
        {
            if (user.UserName != "") //  && user.PassWord != ""
            {
                return true;
            }
            return false;
        }

        public async Task<Token> Login(User user)
        {
            try
            {
                Token result = await WebServiceClient.Login(App.RestServiceUrl, user.UserName, user.PassWord);
                return result != null ? result : new Token();
            }
            catch (XpertWebException e)
            {
                if (e.Code == XpertWebException.ERROR_XPERT_INCORRECTPASSWORD)
                {
                    await UserDialogs.Instance.AlertAsync(AppResources.err_msg_IncorrectLoginInfos, AppResources.alrt_msg_Alert, AppResources.alrt_msg_Ok);
                }
                else
                {
                    await UserDialogs.Instance.AlertAsync(e.Message, AppResources.alrt_msg_Alert, AppResources.alrt_msg_Ok);
                }
                return null;
            }
        }
    }
}