using Acr.UserDialogs;
using Rg.Plugins.Popup.Services;
using Syncfusion.SfMaps.XForms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using XpertMobileApp.Api.Managers;
using XpertMobileApp.DAL;
using XpertMobileApp.Services;
using XpertMobileApp.ViewModels;

namespace XpertMobileApp.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class TourneeVisitPage : ContentPage
	{
		TourneeVisitViewModel viewModel;
		View_TRS_TIERS tier;
		public TourneeVisitPage ()
		{
			InitializeComponent ();
			BindingContext = viewModel = new TourneeVisitViewModel();
		}

		public TourneeVisitPage(View_LIV_TOURNEE_DETAIL item)
		{
			InitializeComponent();
			BindingContext = viewModel = new TourneeVisitViewModel(item);
			new Command(async () =>
			{
				var result = await WebServiceClient.GetTier(item.CODE_TIERS);
				tier = result.First();
			});
			
		}

		protected override void OnAppearing()
        {
			base.OnAppearing();
			new Command(async () =>
			{
				await viewModel.ExecuteLoadActiviteCommand();
			}).Execute(null);
        }

        private async void Location(object sender, EventArgs e)
        {
			LocationSelector locationSelector = new LocationSelector(tier);
			await PopupNavigation.Instance.PushAsync(locationSelector);
			if (await locationSelector.PopupClosedTask)
            {
				if (locationSelector.Result != null)
				{
					var postition = ((SfMaps) locationSelector.Content.FindByName("MyMap")).Layers[0].GetLatLonFromPoint(locationSelector.Result);
					tier.GPS_LATITUDE = postition.Y;
					tier.GPS_LONGITUDE = postition.X;
					await CrudManager.TiersManager.UpdateItemAsync(tier);
					await UserDialogs.Instance.AlertAsync("Mise a jour avec succee", AppResources.alrt_msg_Alert, AppResources.alrt_msg_Ok);
				}
			}

		}

		private void Appele(object sender, EventArgs e)
        {

        }

        private void Info(object sender, EventArgs e)
        {

        }

        private async void VisitDemarer(object sender, EventArgs e)
        {

				viewModel.Item.CODE_ETAT = TourneeStatus.EnRoute;
				await CrudManager.TourneeDetails.UpdateItemAsync(viewModel.Item);
        }
    }
}