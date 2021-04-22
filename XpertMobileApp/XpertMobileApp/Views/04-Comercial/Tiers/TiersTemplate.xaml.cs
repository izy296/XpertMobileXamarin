using Rg.Plugins.Popup.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
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
    }
}