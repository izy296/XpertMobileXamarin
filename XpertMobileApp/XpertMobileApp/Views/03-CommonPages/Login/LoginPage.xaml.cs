using Acr.UserDialogs;
using Newtonsoft.Json;
using Rg.Plugins.Popup.Services;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Xpert.Common.WSClient.Model;
using XpertMobileApp.Api;
using XpertMobileApp.Api.Managers;
using XpertMobileApp.Api.Models;
using XpertMobileApp.Interfaces;
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

            // Check if the user if is in the demo mode !
            Client client = App.ClientDatabase.GetFirstItemAsync().Result;
            if (client.DemoMode == true)
            {
                checkBoxDemo.IsVisible = true;
                labelDemo.IsVisible = true;
            }
        }
        private void Init()
        {
            //  App.StatrtCheckIfInternet(Lbl_NoInternet, this);

            Ent_UserName.Text = "";
            Ent_PassWord.Text = "";
            Ent_UserName.Completed += (s, e) => Ent_PassWord.Focus();
            Ent_PassWord.Completed += (s, e) => ConnectUserAsync(s, e);
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();

            Btn_Config.IsVisible = true;

            // if the user want to login with password only and he remembered his credietial
            if (App.Settings.ConnectWithPasswordOnly && App.Settings.IsChecked == "true")
            {
                usernameContainer.IsVisible = false;
                Ent_UserName.IsVisible = false;
                Ent_UserName.Text = "";
                Ent_UserName.Placeholder = AppResources.lp_ph_UserName;
                checkBox.IsChecked = true;
                Ent_PassWord.Text = App.Settings.Password;
            }
            else if ((App.Settings.ConnectWithPasswordOnly && App.Settings.IsChecked == "false") || ((App.Settings.ConnectWithPasswordOnly && App.Settings.IsChecked == null)))
            {
                usernameContainer.IsVisible = false;
                Ent_UserName.IsVisible = false;
                Ent_UserName.Text = "";
                Ent_UserName.Placeholder = AppResources.lp_ph_UserName;
                Ent_PassWord.Placeholder = AppResources.lp_ph_PassWord;
                checkBox.IsChecked = false;
            }
            // if the user remembred his cridentials and connect with user and password 
            else if (App.Settings.IsChecked == "true" && !App.Settings.ConnectWithPasswordOnly)
            {
                Ent_PassWord.Text = App.Settings.Password;
                Ent_UserName.Text = App.Settings.Username;
                checkBox.IsChecked = true;
                Btn_LogIn.Text = AppResources.lp_btn_Login;
            }
            else
            {
                Ent_PassWord.Placeholder = AppResources.lp_ph_PassWord;
                Ent_UserName.Placeholder = AppResources.lp_ph_UserName;
                Btn_LogIn.Text = AppResources.lp_btn_Login;
            }
        }

        async void ConnectUserAsync(object sender, EventArgs e)
        {
            UserDialogs.Instance.ShowLoading(AppResources.txt_Waiting);

            // Check if the WebService is configured
            if (string.IsNullOrEmpty(App.RestServiceUrl))
            {
                CustomPopup AlertPopup = new CustomPopup(AppResources.alrt_msg_MissingServerInfos, trueMessage: AppResources.alrt_msg_Ok);
                await PopupNavigation.Instance.PushAsync(AlertPopup);
                return;
            }

            try
            {


                // if the url in the moment of login is unreachable 
                if (App.ServiceUrlIsRemote)
                    await SettingsModel.GetNewTunnelUrlIfNotConnected();
                bool isconnected = await App.IsConected();

                if (!isconnected)
                    if (Constants.AppName != Apps.X_DISTRIBUTION)
                    {
                        UserDialogs.Instance.HideLoading();
                        CustomPopup AlertPopup = new CustomPopup(AppResources.alrt_msg_NoConnexion, trueMessage: AppResources.alrt_msg_Ok);
                        await PopupNavigation.Instance.PushAsync(AlertPopup);
                        return;
                    }
                if (App.Online)
                {

                    // Create User object And affect to it the username and the password from the textField... 
                    User user;
                    user = new User(Ent_UserName.Text, Ent_PassWord.Text);

                    //Ignore the white space at the end of the username ...
                    if (user.UserName != null)
                    {
                        user.UserName = user.UserName.Trim();
                    }

                    // Authentification via le WebService
                    Token token = await viewModel.Login(user);

                    //Save the username and the password to show it next time

                    //Save the password if the user want to remember its credentials...
                    if (App.Settings.IsChecked == "true")
                    {
                        App.Settings.Password = Ent_PassWord.Text;
                    }

                    //Save the username if the Username anyway...
                    if (!String.IsNullOrEmpty(Ent_UserName.Text))
                    {
                        App.Settings.Username = Ent_UserName.Text;
                        App.Settings.UsernameOnly = Ent_UserName.Text;
                    }

                    // Cas d'un souci avec le web service 
                    if (token == null)
                    {
                        UserDialogs.Instance.HideLoading();

                        //CustomPopup AlertPopup = new CustomPopup("un soucis est survenu lors de la tentative de connexion!", trueMessage: AppResources.alrt_msg_Ok);
                        //await PopupNavigation.Instance.PushAsync(AlertPopup);
                        return;
                    }

                    if (token.access_token != null)
                    {
                        token.userID = token.userName;
                        user.Id = token.userID;
                        user.CODE_TIERS = token.CODE_TIERS;
                        user.UserGroup = token.UserGroup;
                        user.GroupName = token.GroupName;
                        user.ClientId = App.Settings.ClientId;
                        user.UserName = token.userName;

                        //user.CODE_COMPTE = token.CODE_COMPTE;
                        user.Token = token;
                        App.User = user;

                        CrudManager.InitServices();

                        //Création des différents tables de la base de donnée local...
                        await SQLite_Manager.InitialisationDbLocal();

                        //Insertion du token dans la table token ...
                        await SQLite_Manager.AjoutToken(token);

                        //check if Direct_Login if true in order to login with password only next time.
                        var param = await AppManager.GetSysParams();

                        //if the user Connect with password only
                        if (param.DIRECT_LOGIN)
                        {
                            App.Settings.ConnectWithPasswordOnly = true;
                            App.Settings.Username = App.Settings.UsernameOnly = user.UserName;
                        }
                        else
                        {
                            App.Settings.ConnectWithPasswordOnly = false;
                        }

                        //Récuperation prefix
                        if (Constants.AppName == Apps.XCOM_Livraison || Constants.AppName == Apps.X_DISTRIBUTION)
                        {
                            await RecupererPrefix();
                        }

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

                        //Sauvgarder (ServiceUrl, ClientName, ClientId, Mobile_Edition) dans la table Settings dans la db local ....
                        await App.SettingsDatabase.SaveItemAsync(App.Settings);

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
                //
                else
                {
                    UserDialogs.Instance.ShowLoading(AppResources.txt_Waiting);
                    User user = new User(Ent_UserName.Text, Ent_PassWord.Text);

                    if (viewModel.CheckUser(user))
                    {
                        // Vérification si le user avec le meme username et mot de pass existe dans la table ...
                        SYS_USER validInfo = await SQLite_Manager.AuthUser(user);
                        if (validInfo != null)
                        {
                            User tempUser = new User { UserName = validInfo.ID_USER };
                            // obtenir le token de la base local...
                            Token token = await SQLite_Manager.getToken(tempUser);
                            if (token == null)
                            {
                                UserDialogs.Instance.HideLoading();

                                CustomPopup AlertPopup = new CustomPopup("un soucis est survenu lors de la tentative de connexion!", trueMessage: AppResources.alrt_msg_Ok);
                                await PopupNavigation.Instance.PushAsync(AlertPopup);
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

                                // sauvegarde du user et du token en cours dans la bdd local
                                try
                                {
                                    if (Constants.AppName != Apps.X_BOUTIQUE)
                                    {
                                        //Obtenir les permission de la table SYS_OBJET_PERMISSION ...
                                        AppManager.permissions = await SQLite_Manager.getPermission();
                                    }
                                    if (Constants.AppName == Apps.X_DISTRIBUTION)
                                    {
                                        await SQLite_Manager.AssignPrefix();
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

                            CustomPopup AlertPopup = new CustomPopup(AppResources.lp_login_WrongAcces, trueMessage: AppResources.alrt_msg_Ok);
                            await PopupNavigation.Instance.PushAsync(AlertPopup);
                        }
                    }
                    else
                    {
                        UserDialogs.Instance.HideLoading();
                        CustomPopup AlertPopup = new CustomPopup(AppResources.lp_login_WrongAcces, trueMessage: AppResources.alrt_msg_Ok);
                        await PopupNavigation.Instance.PushAsync(AlertPopup);
                        UserDialogs.Instance.HideLoading();
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
                        CustomPopup AlertPopup = new CustomPopup("Veuillez configurer votre prefixe!!", trueMessage: AppResources.alrt_msg_Ok);
                        await PopupNavigation.Instance.PushAsync(AlertPopup);
                    }
                }
            }
            catch (Exception ex)
            {
                UserDialogs.Instance.HideLoading();
                CustomPopup AlertPopup = new CustomPopup(ex.Message, trueMessage: AppResources.alrt_msg_Ok);
                await PopupNavigation.Instance.PushAsync(AlertPopup);
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
        protected async void Settings_Clicked(object sender, EventArgs e)
        {/*
            SettingsPage sp = new SettingsPage(true);
            this.Navigation.PushModalAsync(new NavigationPage(sp));
            */
            SettingsPopup sp = new SettingsPopup();
            await PopupNavigation.Instance.PushAsync(sp);
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
        private void OnDemoCheckBoxCheckedChanged(object sender, Syncfusion.XForms.Buttons.StateChangedEventArgs e)
        {
            try
            {
                if (e.IsChecked.HasValue && e.IsChecked.Value)
                {
                    Ent_UserName.IsEnabled = Ent_PassWord.IsEnabled = false;
                    Btn_LogIn.Text = "Connexion en mode demo";
                    Ent_UserName.Text = "Administrateur";
                    Ent_PassWord.Text = "00";
                }
                else
                {
                    Ent_UserName.IsEnabled = Ent_PassWord.IsEnabled = true;
                    Btn_LogIn.Text = "Connexion";
                    Ent_UserName.Text = "";
                    Ent_PassWord.Text = "";
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }
    }
}