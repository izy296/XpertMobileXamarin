using Newtonsoft.Json;
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
        public SettingsModel SettingsviewModel;

        public NotificationPage()
        {
            InitializeComponent();

            BindingContext = viewModel = new NotificationViewModel();
            SettingsviewModel = new SettingsModel();
            MessagingCenter.Subscribe<App, string>(this, "RELOAD_NOTIF", async (obj, str) =>
            {
                viewModel.Items = SettingsviewModel.getNotificationAsync();
            });
        }

        private async void Button_Clicked(object sender, EventArgs e)
        {
            try
            {

                SettingsviewModel.deleteteAllNotification();
                viewModel.Items = SettingsviewModel.getNotificationAsync();
                await DisplayAlert(AppResources.np_Alert_Delete, AppResources.np_txt_alert_delete, AppResources.alrt_msg_Ok);
                MessagingCenter.Send(this, "RELOAD_MENU", "");
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        /// <summary>
        /// Delete one notification from sqlite
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void Supprimer(object sender, EventArgs e)
        {
            try
            {
                SwipeItem item = sender as SwipeItem;
                var myCurrentNotif = item.BindingContext as Notification;

                List<Notification> liste = JsonConvert.DeserializeObject<List<Notification>>(App.Settings.Notifiaction);

                for (int i = 0; i < liste.Count; i++)
                {
                    if (liste[i].Index == myCurrentNotif.Index)
                    {
                        liste.RemoveAt(i);
                        break;
                    }
                }

                App.Settings.Notifiaction = JsonConvert.SerializeObject(liste);

                //to update the badge with the number of rest parameter
                MessagingCenter.Send(this, "RELOAD_MENU", "");

                await SettingsviewModel.SaveSettings();
                viewModel.Items = SettingsviewModel.getNotificationAsync();
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        // fonction responsable a afficher les détailles d'un evenement (notification)
        private async void NotifcationClicked(object sender, SelectedItemChangedEventArgs e)
        {
            try
            {
                if (e.SelectedItem!=null)
                {
                    Notification notification = e.SelectedItem as Notification;
                    MainPage RootPage = App.Current.MainPage as MainPage;
                    if (notification.ModuleId != Convert.ToInt32(MenuItemType.None))
                    {
                        await RootPage.NavigateFromMenu(notification.ModuleId);
                        if (notification.Extras != null)
                            MessagingCenter.Send(this, "ExtraData", notification.Extras);
                    }
                    else await App.Current.MainPage.DisplayAlert(AppResources.txt_alert, AppResources.np_txt_alert_Notification, AppResources.alrt_msg_Ok);

                    ItemsListView.SelectedItem = null;
                }
            }
            catch
            {

            }


        }
    }
}