using Rg.Plugins.Popup.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
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
                Btn_Upload.IsEnabled = false;
                syncImage.RotateTo(350 * -360, 10 * 60 * 1000);

                SyncResumePoup popup = new SyncResumePoup();
                await PopupNavigation.Instance.PushAsync(popup);

                await SQLite_Manager.synchroniseUpload();
                syncImage.CancelAnimations();
                Btn_Upload.IsEnabled = true;
            }
            catch (Exception ex)
            {

            }

        }

        private async void SyncDownload(object sender, EventArgs e)
        {
            try
            {
                Btn_Download.IsEnabled = false;
                syncImage.RotateTo(350 * 360, 10 * 60 * 1000);
                await SQLite_Manager.SynchroniseDownload();
                syncImage.CancelAnimations();
                Btn_Download.IsEnabled = true;
            }
            catch (Exception ex)
            {

            }

        }
    }
}