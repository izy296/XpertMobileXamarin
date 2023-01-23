using Acr.UserDialogs;
using Rg.Plugins.Popup.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using XpertMobileApp;
using XpertMobileApp.SQLite_Managment;
using XpertMobileApp.Views;
using XpertMobileAppManafiaa.Api.Models.Interfaces;
using XpertMobileAppManafiaa.SQLite_Managment;

namespace XpertMobileAppManafiaa.Views._03_CommonPages.Synchronisation
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SynchronisationPage : ContentPage
    {
        public SynchronisationPage()
        {
            InitializeComponent();
        }
        private async void SyncUpload(object sender, EventArgs e)
        {
            try
            {
               
                if (await App.IsConected())
                {
                    SyncResumePoup popup = new SyncResumePoup();
                    Btn_Upload.IsEnabled = false;
                    Btn_Download.IsEnabled = false;

                    //syncImage.RotateTo(350 * -360, 10 * 60 * 1000);
                    await PopupNavigation.Instance.PushAsync(popup);
                    popup.CheckSynchronisation();

                    //syncImage.CancelAnimations();
                    Btn_Upload.IsEnabled = true;
                    Btn_Download.IsEnabled = true;
                }

                else
                {

                    CustomPopup AlertPopup = new CustomPopup("Connexion introuvable, voulez-vous vous connecter  ? ", falseMessage: AppResources.exit_Button_No, trueMessage: "Oui");
                    await PopupNavigation.Instance.PushAsync(AlertPopup);
                    if (await AlertPopup.PopupClosedTask)
                    {
                        if (AlertPopup.Result)
                        {
                            App.canCheckInternet = true;
                            DependencyService.Get<ISettingsStart>().StartSettings();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                await UserDialogs.Instance.AlertAsync(ex.Message, AppResources.alrt_msg_Alert, AppResources.alrt_msg_Ok);
                Btn_Upload.IsEnabled = true;
                Btn_Download.IsEnabled = true;
            }

        }

        private async void SyncDownload(object sender, EventArgs e)
        {
            try
            {
                bool isconnected = await App.IsConected();

                if (isconnected)
                {
                    Btn_Upload.IsEnabled = false;
                    Btn_Download.IsEnabled = false;

                    syncImage.RotateTo(350 * 360, 10 * 60 * 1000);
                    await SyncManager.synchroniseDownload();
                    //syncImage.CancelAnimations();
                    Btn_Upload.IsEnabled = true;
                    Btn_Download.IsEnabled = true;
                }
                else
                {
                    CustomPopup AlertPopup = new CustomPopup("Veuillez verifier votre connexion au serveur ! ", trueMessage: AppResources.alrt_msg_Ok);
                    await PopupNavigation.Instance.PushAsync(AlertPopup);
                }

            }
            catch (Exception ex)
            {
                await UserDialogs.Instance.AlertAsync(ex.Message, AppResources.alrt_msg_Alert, AppResources.alrt_msg_Ok);
                Btn_Upload.IsEnabled = true;
                Btn_Download.IsEnabled = true;
            }
        }
    }
}