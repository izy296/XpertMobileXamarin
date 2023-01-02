using Acr.UserDialogs;
using Rg.Plugins.Popup.Pages;
using Rg.Plugins.Popup.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Xpert.Common.DAO;
using Xpert.Common.WSClient.Helpers;
using Xpert.Common.WSClient.Services;
using XpertMobileApp.Api.Managers;
using XpertMobileApp.Api.Services;
using XpertMobileApp.DAL;
using XpertMobileApp.Helpers;
using XpertMobileApp.Models;
using XpertMobileApp.Services;
using XpertMobileApp.SQLite_Managment;
using XpertMobileApp.ViewModels;
using XpertMobileApp.Views.Templates;

namespace XpertMobileApp.Views._03_CommonPages.Synchronisation
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SyncResumePoup : PopupPage, INotifyPropertyChanged
    {

        public SyncResumePoup()
        {
            InitializeComponent();

        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
        }



        /// <summary>
        /// Fermer la popup page
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void btn_Cancel_Clicked(object sender, EventArgs e)
        {
            await PopupNavigation.Instance.PopAsync();
        }

    }
}

