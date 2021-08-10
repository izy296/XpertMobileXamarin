using Acr.UserDialogs;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Xpert.Common.WSClient.Model;
using XpertMobileApp.Api;
using XpertMobileApp.Api.Managers;
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

            XpertVersion.Text = Mobile_Edition.GetEditionTitle(App.Settings.Mobile_Edition) + VersionTracking.CurrentVersion;

            NavigationPage.SetHasNavigationBar(this, false);
            Init();
        }

        const uint AnimationSpeed = 300;



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

            //Lbl_UserName.Text = AppResources.lp_lbl_UserName;
            //Lbl_PassWord.Text = AppResources.lp_lbl_PassWord;
            Ent_PassWord.Placeholder = AppResources.lp_ph_PassWord;
            Ent_UserName.Placeholder = AppResources.lp_ph_UserName;
            Btn_LogIn.Text = AppResources.lp_btn_Login;
            //lbl_Welcome.Text = AppResources.txt_welcome;
            if (Constants.AppName == Apps.XCOM_Mob)
            {
                lbl_AppName.Text = "XpertCom";
            }
            else if (Constants.AppName == Apps.XAGRI_Mob)
            {
                lbl_AppName.Text = "XpertAgriculture";
            }
            else if (Constants.AppName == Apps.X_BOUTIQUE)
            {
                lbl_AppName.Text = "XpertBoutique";
            }
            else if (Constants.AppName == Apps.XCOM_Livraison)
            {
                lbl_AppName.Text = "XpertLivraison";
            }
            else if (Constants.AppName == Apps.XPH_Mob)
            {
                lbl_AppName.Text = "XpertPharm";
            }
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();
        }

        


        private async void SettingButton_Clicked(object sender, EventArgs e)
        {
            SettingsPage settingsPage = new SettingsPage(true);
            await Navigation.PushModalAsync(new NavigationPage(settingsPage));

            Btn_Config.Clicked += async delegate
            {
                this.IsEnabled = false;
                await Task.Yield();
                await Task.Delay(500);
                this.IsEnabled = true;
            };
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
                bool isconnected = await App.IsConected();
                if (App.Online)
                {
                    UserDialogs.Instance.ShowLoading(AppResources.txt_Waiting);
                    User user = new User(Ent_UserName.Text, Ent_PassWord.Text);

                    if (viewModel.CheckUser(user))
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
                            user.UserGroup = token.UserGroup;
                            user.GroupName = token.GroupName;
                            user.ClientId = App.Settings.ClientId;
                            user.Token = token;
                            App.User = user;

                            CrudManager.InitServices();

                            await SQLite_Manager.initialisationDbLocal();
                            await SQLite_Manager.AjoutToken(token);
                            // Alerte apres la connexion
                            // DependencyService.Get<ITextToSpeech>().Speak(AppResources.app_speech_Hello + " " + user.UserName + "!");

                            // suavegrade du user et du token en cours dans la bdd local

                            //Récuperation prefix
                            if (Constants.AppName == Apps.XCOM_Livraison)                            
                                await RecupererPrefix();

                            try
                            {
                                if (Constants.AppName != Apps.X_BOUTIQUE)
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
                    else
                    {
                        await DisplayAlert(AppResources.lp_Login, AppResources.lp_login_WrongAcces, AppResources.alrt_msg_Ok);
                    }
                }
                else
                {
                    UserDialogs.Instance.ShowLoading(AppResources.txt_Waiting);
                    User user = new User(Ent_UserName.Text, Ent_PassWord.Text);

                    if (viewModel.CheckUser(user))
                    {
                        // Authentification via SQLite
                        bool validInfo = await SQLite_Manager.AuthUser(user);
                        if (validInfo)
                        {
                            Token token = await SQLite_Manager.getToken(user);
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
                                        AppManager.permissions = await SQLite_Manager.getPermission();
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
                    else
                    {
                        await DisplayAlert(AppResources.lp_Login, AppResources.lp_login_WrongAcces, AppResources.alrt_msg_Ok);
                    }
                }
                try
                {
                    await SQLite_Manager.AssignPrefix();
                    await SQLite_Manager.AssignMagasin();
                }
                catch
                {
                }

                if (Constants.AppName == Apps.XCOM_Livraison)
                {
                    if (string.IsNullOrEmpty(App.PrefixCodification))
                    {
                        await UserDialogs.Instance.AlertAsync("Veuillez configurer votre prefixe!!", AppResources.alrt_msg_Alert, AppResources.alrt_msg_Ok);
                    }
                }
            }
            catch (Exception ex)
            {
                UserDialogs.Instance.HideLoading();
                await DisplayAlert(AppResources.lp_Login, ex.Message, AppResources.alrt_msg_Ok);
            }
        }

        protected async Task RecupererPrefix()
        {
            var res = await SQLite_Manager.getPrefix();
            if (res != null && !(string.IsNullOrEmpty(res.PREFIX)))
            {
                App.PrefixCodification = res.PREFIX;
            }
        }
        private void Exit_Clicked(object sender, EventArgs e)
        {
            //System.Environment.Exit(1);
            var closer = DependencyService.Get<ICloseApplication>();
            closer?.closeApplication();
        }
    }
}