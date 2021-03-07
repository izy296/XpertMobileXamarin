using Acr.UserDialogs;
using Rg.Plugins.Popup.Services;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Xpert.Common.DAO;
using Xpert.Common.WSClient.Helpers;
using XpertMobileApp.Api.Managers;
using XpertMobileApp.Api.Services;
using XpertMobileApp.DAL;
using XpertMobileApp.ViewModels;
using ZXing.Net.Mobile.Forms;

namespace XpertMobileApp.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class TourneesDetailsPage : ContentPage
	{
        TourneesDetailsViewModel viewModel;

        public TourneesDetailsPage(string codeTournee)
		{
			InitializeComponent();

            BindingContext = viewModel = new TourneesDetailsViewModel(codeTournee);
        }

        async void OnItemSelected(object sender, SelectedItemChangedEventArgs args)
        {
            var item = args.SelectedItem as View_LIV_TOURNEE_DETAIL;
            if (item == null)
                return;

            //await Navigation.PushAsync(new TourneesDetailsPage(item.CODE_TOURNEE));

            // Manually deselect item.
            listView.SelectedItem = null;
        }

        async void AddItem_Clicked(object sender, EventArgs e)
        {
           // await Navigation.PushModalAsync(new NavigationPage(new NewEncaissementPage(null, viewModel.EncaissDisplayType)));
        }
         
        protected override void OnAppearing()
        {
            base.OnAppearing();

           // if (viewModel.Items.Count == 0)
                LoadData();

         //   if (viewModel.Familles.Count == 0)
         //       viewModel.LoadExtrasDataCommand.Execute(null);
        }

        private void TypeFilter_Clicked(object sender, EventArgs e)
        {
            LoadData();
        }

        private void LoadData()
        {
            viewModel.LoadItemsCommand.Execute(null);
        }

        private void Filter_Clicked(object sender, EventArgs e)
        {
            FilterPanel.IsVisible = !FilterPanel.IsVisible;
        }

        private void btn_ApplyFilter_Clicked(object sender, EventArgs e)
        {         
            viewModel.LoadItemsCommand.Execute(null);
        }

        private void btn_CancelFilter_Clicked(object sender, EventArgs e)
        {
            FilterPanel.IsVisible = false;
            viewModel.ClearFilters();
            viewModel.LoadItemsCommand.Execute(null);
        }

        private void ComptePicker_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private async void OnVisiteSwipeItemInvoked(object sender, EventArgs e)
        {
            var item = viewModel.SelectedItem;
            var tr = (sender as SwipeItem).Parent.Parent.Parent.BindingContext as View_LIV_TOURNEE_DETAIL;

            var scaner = new ZXingScannerPage();
            Navigation.PushAsync(scaner);
            scaner.OnScanResult += (result) =>
            {
                scaner.IsScanning = false;
                Device.BeginInvokeOnMainThread(async () =>
                {
                    await Navigation.PopAsync();

                    var tiers = await SelectScanedTiers(result.Text);
                    if(tiers != null && tiers.CODE_TIERS == tr.CODE_TIERS) 
                    {
                        viewModel.SelectedItem.CODE_ETAT = TourneeStatus.Visited;
                        var res = await viewModel.UpdateItem(viewModel.SelectedItem);
                    }
                    /*
                    ClassId = string.Format("pb_{0}", vteLot.ID);
                    var pbruteElem2 = ItemsListView.Children.Where(x => x.ClassId == ClassId).FirstOrDefault() as SfNumericTextBox;
                    pbruteElem2.Focus();
                    */
                });
            };
        }

        internal async Task<View_TRS_TIERS> SelectScanedTiers(string cb_tiers)
        {
            try
            {
                // Récupérer le lot depuis le serveur
                XpertSqlBuilder qb = new XpertSqlBuilder();
                qb.AddCondition<View_TRS_TIERS, string>(x => x.NUM_CARTE_FIDELITE, cb_tiers);
                qb.AddOrderBy<View_TRS_TIERS, string>(x => x.CODE_TIERS);
                var tiers = await CrudManager.TiersManager.SelectByPage(qb.QueryInfos, 1, 2);
                if (tiers == null)
                    return null;

                XpertHelper.PeepScan();

                if (tiers.Count() > 1)
                {
                    await UserDialogs.Instance.AlertAsync("Plusieurs tiers pour ce code barre!", AppResources.alrt_msg_Alert, AppResources.alrt_msg_Ok);
                    return null;
                }
                else if (tiers.Count() == 0)
                {
                    await UserDialogs.Instance.AlertAsync("Aucun tiers pour ce code barre!", AppResources.alrt_msg_Alert, AppResources.alrt_msg_Ok);
                    return null;
                }
                else
                {
                    return tiers.FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                await UserDialogs.Instance.AlertAsync(WSApi2.GetExceptionMessage(ex), AppResources.alrt_msg_Alert,
                    AppResources.alrt_msg_Ok); 
                return null;
            }
        }

        private async void OnDelevrySwipeItemInvoked(object sender, EventArgs e)
        {
            var item = viewModel.SelectedItem;
            var tr = (sender as SwipeItem).Parent.Parent.Parent.BindingContext as View_LIV_TOURNEE_DETAIL;

            var session = await CrudManager.Sessions.GetCurrentSession();
            if (session == null)
            {
                var res = await DisplayAlert(AppResources.msg_Confirmation, AppResources.msg_ShouldPrepaireSession, AppResources.alrt_msg_Ok, AppResources.alrt_msg_Cancel);
                if (res)
                {
                    OpenSessionPage openPage = new OpenSessionPage(viewModel.CurrentStream);
                    await PopupNavigation.Instance.PushAsync(openPage);
                }
            }
            else
            {
                var trs = new View_TRS_TIERS();
                trs.CODE_TIERS = tr.CODE_TIERS;
                trs.NOM_TIERS1 = tr.NOM_TIERS;
                VenteFormPage form = new VenteFormPage(null, VentesTypes.Livraison, trs, tr.CODE_DETAIL);
                
                await Navigation.PushAsync(form);
            }
        }
    }
}