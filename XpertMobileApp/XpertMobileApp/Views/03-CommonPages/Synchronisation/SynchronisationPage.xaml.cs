using Acr.UserDialogs;
using Rg.Plugins.Popup.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using XpertMobileApp.Api.Models.Interfaces;
using XpertMobileApp.Models;
using XpertMobileApp.SQLite_Managment;

namespace XpertMobileApp.Views._03_CommonPages.Synchronisation
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
                    Btn_Upload.IsEnabled = false;
                    Btn_Download.IsEnabled = false;
                    syncImage.RotateTo(350 * -360, 10 * 60 * 1000);

                    SyncResumeUploadPopup popup = new SyncResumeUploadPopup();
                    await PopupNavigation.Instance.PushAsync(popup);
                    popup.CheckSynchronisation();

                    syncImage.CancelAnimations();
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
                            DependencyService.Get<ISettingsStart>().StartSettings();
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                syncImage.CancelAnimations();
                Btn_Upload.IsEnabled = true;
            }

        }

        private async void SyncDownload(object sender, EventArgs e)
        {
            try
            {
                var check = await UserDialogs.Instance.ConfirmAsync("Telechargement des donnes, voulez-vous vous continuer  ? ", "Alert", "Oui", "Non");
                if (!check)
                    return;
                if (await App.IsConected())
                {
                    Btn_Upload.IsEnabled = false;
                    Btn_Download.IsEnabled = false;
                    syncImage.RotateTo(350 * 360, 10 * 60 * 1000);

                    //SyncResumeDownloadPopup popup = new SyncResumeDownloadPopup();
                    //await PopupNavigation.Instance.PushAsync(popup);

                    await SQLite_Manager.SynchroniseDownload();
                    syncImage.CancelAnimations();
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
                            DependencyService.Get<ISettingsStart>().StartSettings();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                syncImage.CancelAnimations();
                Btn_Upload.IsEnabled = true;
            }
        }
    }
}