using Acr.UserDialogs;
using Rg.Plugins.Popup.Services;
using Syncfusion.SfMaps.XForms;
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
            try
            {
                var tr = (sender as SwipeItem)?.Parent?.Parent?.Parent?.BindingContext as View_TRS_TIERS;

                //await Navigation.PushAsync(new LocationSelector());
                LocationSelector popup = new LocationSelector(tr);
                await PopupNavigation.Instance.PushAsync(popup);
                SfMaps MyMap= (SfMaps)popup.Content.FindByName("MyMap");
                if (await popup.PopupClosedTask)
                {
                    if (popup.Result != null)
                    {
                        var postition = MyMap.Layers[0].GetLatLonFromPoint(popup.Result);
                        tr.GPS_LATITUDE = postition.Y;
                        tr.GPS_LONGITUDE = postition.X;
                        await CrudManager.TiersManager.UpdateItemAsync(tr);
                        await UserDialogs.Instance.AlertAsync("Mise a jour avec succee", AppResources.alrt_msg_Alert, AppResources.alrt_msg_Ok);
                    }
                }

            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

    }
}