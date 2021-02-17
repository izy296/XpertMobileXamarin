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
using XpertMobileApp.Views;

namespace XpertMobileApp.ViewModels.XLogin
{
    /// <summary>
    /// ViewModel for sign-up page.
    /// </summary>
    [Preserve(AllMembers = true)]
    public class SignUpPageViewModel : LoginViewModel
    {
        #region Fields

        private string name;
        
        private string phone;

        private string password;

        private string confirmPassword;

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance for the <see cref="SignUpPageViewModel" /> class.
        /// </summary>
        public SignUpPageViewModel()
        {
            // this.LoginCommand = new  Command(this.LoginClicked);
            // this.SignUpCommand = new Command(this.SignUpClicked);
            this.LoginCommand = new Command(async (object obj) => await this.LoginClicked(obj));
            this.SignUpCommand = new Command(async (object obj) => await this.SignUpClicked(obj));
        }

        #endregion

        #region Property

        /// <summary>
        /// Gets or sets the property that bounds with an entry that gets the name from user in the Sign Up page.
        /// </summary>
        public string Name
        {
            get
            {
                return this.name;
            }

            set
            {
                if (this.name == value)
                {
                    return;
                }

                SetProperty(ref name, value);
            }
        }

        /// <summary>
        /// Gets or sets the property that bounds with an entry that gets the name from user in the Sign Up page.
        /// </summary>
        public string Phone
        {
            get
            {
                return this.phone;
            }

            set
            {
                if (this.phone == value)
                {
                    return;
                }

                SetProperty(ref phone, value);
            }
        }

        /// <summary>
        /// Gets or sets the property that bounds with an entry that gets the password from users in the Sign Up page.
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

        /// <summary>
        /// Gets or sets the property that bounds with an entry that gets the password confirmation from users in the Sign Up page.
        /// </summary>
        public string ConfirmPassword
        {
            get
            {
                return this.confirmPassword;
            }

            set
            {
                if (this.confirmPassword == value)
                {
                    return;
                }

                SetProperty(ref confirmPassword, value);
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

        #endregion

        #region Methods

        /// <summary>
        /// Invoked when the Log in button is clicked.
        /// </summary>
        /// <param name="obj">The Object</param>
        private async Task LoginClicked(object obj)
        {
            try
            {
                bool connected = await ConnectUserAsync(Name, Password);
                MessagingCenter.Send(this, "RELOAD_MENU", "");
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
        private async Task SignUpClicked(object obj)
        {
            try 
            { 
                RegisterBindingModel l = new RegisterBindingModel()
                {
                    USER_NAME = Name,
                    PhoneNumber = Phone,
                    Password = Password,
                    ConfirmPassword = ConfirmPassword,
                    Email = Email,
                };

                var res = await BoutiqueManager.Subscribe(l);
                bool connected = await ConnectUserAsync(Email, Password);
                MessagingCenter.Send(this, "RELOAD_MENU", "");

                await UserDialogs.Instance.AlertAsync("Vous êtes maintenant inscrit(e), Vous pouvez désormais passer vos commandes!", AppResources.alrt_msg_Info, AppResources.alrt_msg_Ok);

                popup(obj);
            }
            catch(Exception ex) 
            {
                await UserDialogs.Instance.AlertAsync(ex.Message, AppResources.alrt_msg_Info, AppResources.alrt_msg_Ok);
            }
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