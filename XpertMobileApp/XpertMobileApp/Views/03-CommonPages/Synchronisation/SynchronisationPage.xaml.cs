using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
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
            await SQLite_Manager.synchroniseUpload();
        }

        private async void SyncDownload(object sender, EventArgs e)
        {
            await SQLite_Manager.SynchroniseDownload();
        }
    }
}