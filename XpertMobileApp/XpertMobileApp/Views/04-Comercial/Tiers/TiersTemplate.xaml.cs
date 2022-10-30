using Acr.UserDialogs;
using Rg.Plugins.Popup.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using XpertMobileApp.Api.Managers;
using XpertMobileApp.Api.Services;
using XpertMobileApp.DAL;

namespace XpertMobileApp.Views.Templates
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class TiersTemplate : ContentView
    {
        public TiersTemplate()
        {
            InitializeComponent();
        }

        NewTiersPopupPage form;
        private async void OnUpdateSwipeItemInvoked(object sender, EventArgs e)
        {
            var tr = (sender as SwipeItem)?.Parent?.Parent?.Parent?.BindingContext as View_TRS_TIERS;
            // form = new NewTiersPage(tr);
            // await Navigation.PushModalAsync(new NavigationPage(form));

            form = new NewTiersPopupPage(tr);
            await PopupNavigation.Instance.PushAsync(form);
        }

        private async void OnDeleteSwipeItemInvoked(object sender, EventArgs e)
        {
            var tr = (sender as SwipeItem)?.Parent?.Parent?.Parent?.BindingContext as View_TRS_TIERS;
        }

        private void OnCallSwipeItemInvoked(object sender, EventArgs e)
        {
            var tr = (sender as SwipeItem)?.Parent?.Parent?.Parent?.BindingContext as View_TRS_TIERS;
            if (!XpertHelper.IsNullOrEmpty(tr.TEL1_TIERS))
                PhoneDialer.Open(tr.TEL1_TIERS);
            else UserDialogs.Instance.AlertAsync("Numero Telephone est vide", AppResources.alrt_msg_Alert,
    AppResources.alrt_msg_Ok);
        }

        private async void OnLocateSwipeItemInvoked(object sender, EventArgs e)
        {
            //        var tr = (sender as SwipeItem)?.Parent?.Parent?.Parent?.BindingContext as View_TRS_TIERS;

            //        //await Navigation.PushAsync(new LocationSelector());
            //        LocationSelector popup = new LocationSelector(tr);
            //        await PopupNavigation.Instance.PushAsync(popup);
            //        if (await popup.PopupClosedTask)
            //        {
            //            if (popup.Result!=null)
            //            {
            //                tr.GPS_LATITUDE = popup.Result.Latitude;
            //                tr.GPS_LONGITUDE = popup.Result.Longitude;
            //                await CrudManager.TiersManager.UpdateItemAsync(tr);
            //                await UserDialogs.Instance.AlertAsync("Mise a jour avec succee", AppResources.alrt_msg_Alert,
            //AppResources.alrt_msg_Ok);
            //            }
            //        }

            await Navigation.PushModalAsync(new TierDetailPage());

        }

        private async void TapGestureRecognizer_Tapped(object sender, EventArgs e)
        {
            //UserDialogs.Instance.AlertAsync();
            await PopupNavigation.Instance.PushAsync(new TierDetailPage());
        }
    }
}