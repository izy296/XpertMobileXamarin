using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using XpertMobileApp.Api.Models;
using XpertMobileApp.ViewModels;

namespace XpertMobileApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class NotificationPage : ContentPage
    {

        NotificationViewModel viewModel;

        public NotificationPage()
        {
            InitializeComponent();

            BindingContext = viewModel = new NotificationViewModel();

        }

        private void Button_Clicked(object sender, EventArgs e)
        {
            new SettingsModel().deleteteAllNotification();
            viewModel.Items = new SettingsModel().getNotificationAsync();
            DisplayAlert(AppResources.np_Alert_Delete, AppResources.np_txt_alert_delete, AppResources.alrt_msg_Ok);
        }
    }
}