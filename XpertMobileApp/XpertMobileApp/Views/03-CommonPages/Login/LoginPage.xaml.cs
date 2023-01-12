using Acr.UserDialogs;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Xpert.Common.WSClient.Model;
using XpertMobileApp.Api;
using XpertMobileApp.Api.Managers;
using XpertMobileApp.Api.Services;
using XpertMobileApp.DAL;
using XpertMobileApp.Models;
using XpertMobileApp.Services;
using XpertMobileApp.SQLite_Managment;
using XpertMobileApp.ViewModels;

namespace XpertMobileApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LoginPage : ContentPage
    {
        LoginViewModel viewModel;

        public LoginPage()
        {
            InitializeComponent();

            BindingContext = viewModel = new LoginViewModel();

            XpertVersion.Text = Mobile_Edition.GetEditionTitle(Constants.AppName) + VersionTracking.CurrentVersion;

            NavigationPage.SetHasNavigationBar(this, false);
            Init();
        }

        private void Init()
        {
            //  App.StatrtCheckIfInternet(Lbl_NoInternet, this);

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

        async void ConnectUserAsync(object sender, EventArgs e)
        {
            // Check if the WebService is configured
            if (string.IsNullOrEmpty(App.RestServiceUrl))
            {
                await DisplayAlert(AppResources.alrt_msg_Alert, AppResources.alrt_msg_MissingServerInfos, AppResources.alrt_msg_Ok);
                return;
            }
            try
            {
                User user;
                bool isconnected = await App.IsConected();
                if (App.Online)
                {
                    UserDialogs.Instance.ShowLoading(AppResources.txt_Waiting);
                    user = new User(Ent_UserName.Text, Ent_PassWord.Text);

                    //if (viewModel.CheckUser(user))
                    {
                        // Authentification via le WebService
                        Token token = await viewModel.Login(user);



                        // Cas d'un souci avec le web service 
                        if (token == null)
                        {
                            UserDialogs.Instance.HideLoading();
                            await DisplayAlert(AppResources.lp_Login, "un soucis est survenu lors de la tentative de connexion!", AppResources.alrt_msg_Ok);
                            return;
                        }

                        if (token.access_token != null)
                        {
                            user.Id = token.userID;
                            user.CODE_TIERS = token.CODE_TIERS;
                            user.CODE_COMPTE = token.CODE_COMPTE;
                            user.UserGroup = token.UserGroup;
                            user.GroupName = token.GroupName;
                            user.UserName = token.userName;
                            user.PREFIX_USER_MOBILE = token.PREFIX_USER_MOBILE;
                            user.ClientId = App.Settings.ClientId;
                            user.Token = token;
                            App.User = user;

                            CrudManager.InitServices();

                            await UpdateDatabase.initialisationDbLocal();
                            await UpdateDatabase.AjoutToken(token);
                            // Alerte apres la connexion
                            // DependencyService.Get<ITextToSpeech>().Speak(AppResources.app_speech_Hello + " " + user.UserName + "!");

                            // suavegrade du user et du token en cours dans la bdd local

                            if (Constants.AppName == Apps.XCOM_Livraison)
                            {
                                //Récuperation prefix
                                App.PrefixCodification = token.PREFIX_USER_MOBILE;

                                //await RecupererPrefix();
                            }


                            try
                            {
                                {
                                    await AppManager.GetPermissions();
                                }
                                // await App.UserDatabase.SaveItemAsync(user);
                                await App.TokenDatabase.SaveItemAsync(token);
                            }
                            catch (Exception ex)
                            {
                                await DisplayAlert(AppResources.lp_Login, ex.Message, AppResources.alrt_msg_Ok);
                            }

                            UserDialogs.Instance.HideLoading();

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
                }
                else
                {
                    UserDialogs.Instance.ShowLoading(AppResources.txt_Waiting);
                    user = new User(Ent_UserName.Text, Ent_PassWord.Text);

                    {
                        // Authentification via SQLite
                        SYS_USER validUser = await UpdateDatabase.AuthUser(user);
                        if (XpertHelper.IsNotNullAndNotEmpty(validUser))
                        {
                            user.UserName = validUser.ID_USER;
                            Token token = await UpdateDatabase.getToken(user);
                            if (token == null)
                            {
                                UserDialogs.Instance.HideLoading();
                                await DisplayAlert(AppResources.lp_Login, "un soucis est survenu lors de la tentative de connexion!", AppResources.alrt_msg_Ok);
                                return;
                            }
                            if (token.access_token != null)
                            {
                                user.CODE_COMPTE = validUser.CODE_COMPTE;

                                user.Id = token.userID;
                                user.CODE_TIERS = token.CODE_TIERS;
                                user.UserGroup = token.UserGroup;
                                user.GroupName = token.GroupName;
                                user.PREFIX_USER_MOBILE = token.PREFIX_USER_MOBILE;
                                user.ClientId = App.Settings.ClientId;
                                user.Token = token;
                                App.User = user;

                                CrudManager.InitServices();
                                if (App.PrefixCodification == null)
                                {
                                    App.PrefixCodification = token.PREFIX_USER_MOBILE;
                                }
                                else if (App.PrefixCodification != null && !App.PrefixCodification.Equals(token.PREFIX_USER_MOBILE))
                                {
                                    App.PrefixCodification = token.PREFIX_USER_MOBILE;
                                }

                              

                                // suavegrade du user et du token en cours dans la bdd local
                                try
                                {
                                    {
                                        AppManager.permissions = await UpdateDatabase.getPermission();
                                    }
                                    // await App.UserDatabase.SaveItemAsync(user);
                                    await App.TokenDatabase.SaveItemAsync(token);
                                }
                                catch (Exception ex)
                                {
                                    await DisplayAlert(AppResources.lp_Login, ex.Message, AppResources.alrt_msg_Ok);
                                }

                                UserDialogs.Instance.HideLoading();

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
                            UserDialogs.Instance.HideLoading();
                            await DisplayAlert(AppResources.lp_Login, AppResources.lp_login_WrongAcces, AppResources.alrt_msg_Ok);
                        }
                    }
                    //else
                    //{
                    //    await DisplayAlert(AppResources.lp_Login, AppResources.lp_login_WrongAcces, AppResources.alrt_msg_Ok);
                    //}
                }

                //
                if (Constants.AppName == Apps.XCOM_Livraison)
                {
                    try
                    {
                        await UpdateDatabase.AssignPrefix();
                        await UpdateDatabase.AssignMagasin();
                    }
                    catch
                    {
                    }
                    if (string.IsNullOrEmpty(App.PrefixCodification))
                    {
                        await UserDialogs.Instance.AlertAsync("Veuillez configurer votre prefixe!!", AppResources.alrt_msg_Alert, AppResources.alrt_msg_Ok);
                    }
                }

                // Alerte apres la connexion
                //DependencyService.Get<ITextToSpeech>().Speak(AppResources.app_speech_Hello + " " + user.UserName + "!");
                await UserDialogs.Instance.AlertAsync("مرحبا "+ user.UserName + " نتمنى لكم يوما سعيدا", AppResources.alrt_msg_Info, AppResources.alrt_msg_Ok);


            }
            catch (Exception ex)
            {
                UserDialogs.Instance.HideLoading();
                await DisplayAlert(AppResources.lp_Login, ex.Message, AppResources.alrt_msg_Ok);
            }
        }

        protected async Task RecupererPrefix()
        {
            var res = await UpdateDatabase.getPrefix();
            if (res != null && !(string.IsNullOrEmpty(res.PREFIX)))
            {
                App.PrefixCodification = res.PREFIX;
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