using Acr.UserDialogs;
using System;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Xpert.Common.WSClient.Model;
using XpertMobileApp.Api;
using XpertMobileApp.Api.Managers;
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
            Lbl_AppFullName.Text = Constants.AppFullName.Replace(" ", "\n");

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
            // if the user want to login with password only and he remembered his credietial
            if (App.Settings.ConnectWithPasswordOnly && App.Settings.IsChecked == "true")
            {
                Ent_UserName.IsVisible = false;
                Lbl_UserName.IsVisible = false;
                Ent_UserName.Text = "";
                Ent_UserName.Placeholder = AppResources.lp_ph_UserName;
                checkBox.IsChecked = true;
                Ent_PassWord.Text = App.Settings.Password;
            }
            else if (App.Settings.ConnectWithPasswordOnly && App.Settings.IsChecked == "false")
            {
                Ent_UserName.IsVisible = false;
                Lbl_UserName.IsVisible = false;
                Ent_UserName.Text = "";
                Ent_UserName.Placeholder = AppResources.lp_ph_UserName;
                Ent_PassWord.Placeholder = AppResources.lp_ph_PassWord;
                checkBox.IsChecked = false;
            }
            // if the user remembred his cridentials and connect with user and password 
            else if (App.Settings.IsChecked == "true" && !App.Settings.ConnectWithPasswordOnly)
            {
                Lbl_UserName.Text = AppResources.lp_lbl_UserName;
                Lbl_PassWord.Text = AppResources.lp_lbl_PassWord;
                Ent_PassWord.Text = App.Settings.Password;
                Ent_UserName.Text = App.Settings.Username;
                checkBox.IsChecked = true;
                Btn_LogIn.Text = AppResources.lp_btn_Login;
            }
            else
            {
                Lbl_UserName.Text = AppResources.lp_lbl_UserName;
                Lbl_PassWord.Text = AppResources.lp_lbl_PassWord;
                Ent_PassWord.Placeholder = AppResources.lp_ph_PassWord;
                Ent_UserName.Placeholder = AppResources.lp_ph_UserName;
                Btn_LogIn.Text = AppResources.lp_btn_Login;
            }
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
                if (!isconnected)
                    return;
                if (App.Online)
                {
                    UserDialogs.Instance.ShowLoading(AppResources.txt_Waiting);
                    //check if login_direct 
                    User user;
                    if (App.Settings.ConnectWithPasswordOnly && String.IsNullOrEmpty(Ent_UserName.Text))
                    {
                        user = new User(App.Settings.UsernameOnly, Ent_PassWord.Text);
                    }
                    else
                    {
                        user = new User(Ent_UserName.Text, Ent_PassWord.Text);
                    }

                    if (user.UserName != null)
                        user.UserName = user.UserName.Trim();
                    //username = (XpertHelper.IsNotNullAndNotEmpty(username)) ? username.Trim() : username;

                    //Save the username and the password to show it next time
                    if (App.Settings.IsChecked == "true")
                    {
                        App.Settings.Password = Ent_PassWord.Text;

                        if (!String.IsNullOrEmpty(Ent_UserName.Text))
                        {
                            App.Settings.Username = Ent_UserName.Text;
                            App.Settings.UsernameOnly = Ent_UserName.Text;
                        }
                        await App.SettingsDatabase.SaveItemAsync(App.Settings);
                    }
                    else
                    {
                        if (!String.IsNullOrEmpty(Ent_UserName.Text))
                        {
                            App.Settings.Username = Ent_UserName.Text;
                            App.Settings.UsernameOnly = Ent_UserName.Text;
                        }
                        await App.SettingsDatabase.SaveItemAsync(App.Settings);
                    }

                    //if (!viewModel.CheckUser(user) || viewModel.CheckUser(user))
                    //{
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
                        user.UserName = token.userName;
                        user.Token = token;
                        App.User = user;

                        CrudManager.InitServices();

                        await SQLite_Manager.initialisationDbLocal();
                        await SQLite_Manager.AjoutToken(token);
                        //check if Direct_Login if true in order to login with password only.
                        var param = await AppManager.GetSysParams();

                        //si l'utilisateur connect with password only
                        if (param.DIRECT_LOGIN)
                        {
                            App.Settings.ConnectWithPasswordOnly = true;
                            if (Ent_UserName.Text != App.Settings.Username && Ent_UserName.Text != "")
                                App.Settings.Username = Ent_UserName.Text;
                        }
                        else
                        {
                            App.Settings.ConnectWithPasswordOnly = false;
                        }
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
                    //}
                    //else
                    //{
                    //    await DisplayAlert(AppResources.lp_Login, AppResources.lp_login_WrongAcces, AppResources.alrt_msg_Ok);
                    //}
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
                                user.UserName = token.userName;
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
                    //await SQLite_Manager.AssignPrefix();
                    //await SQLite_Manager.AssignMagasin();
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
        /// <summary>
        /// Set the value of check box to the settings (true or false)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnCheckBoxCheckedChanged(object sender, CheckedChangedEventArgs e)
        {
            if (sender is CheckBox)
            {
                var checkbox = (CheckBox)sender;
                if (checkbox.IsChecked)
                {
                    App.Settings.IsChecked = "true";
                    App.SettingsDatabase.SaveItemAsync(App.Settings);
                }
                else
                {
                    App.Settings.IsChecked = "false";
                    App.SettingsDatabase.SaveItemAsync(App.Settings);
                }
            }
        }
    }
}