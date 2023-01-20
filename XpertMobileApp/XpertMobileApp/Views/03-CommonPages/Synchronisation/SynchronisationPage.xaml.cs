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
                if (App.Online)
                {
                    Btn_Upload.IsEnabled = false;
                    Btn_Download.IsEnabled = false;
                    //syncImage.RotateTo(350 * -360, 10 * 60 * 1000);

                    SyncResumePoup popup = new SyncResumePoup();
                    await PopupNavigation.Instance.PushAsync(popup);
                    popup.CheckSynchronisation();

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
                //syncImage.CancelAnimations();
                Btn_Upload.IsEnabled = true;
            }

        }

        private async void SyncDownload(object sender, EventArgs e)
        {
            try
            {
                if (App.Online)
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
                //syncImage.CancelAnimations();
                Btn_Upload.IsEnabled = true;
            }
        }
    }
}