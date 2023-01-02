using Acr.UserDialogs;
using Rg.Plugins.Popup.Services;
using System;
using System.Collections.Generic;
using System.Linq;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Xpert.Common.WSClient.Helpers;
using XpertMobileApp.Api;
using XpertMobileApp.Api.Managers;
using XpertMobileApp.Base;
using XpertMobileApp.DAL;
using XpertMobileApp.Helpers;
using XpertMobileApp.Models;
using XpertMobileApp.SQLite_Managment;
using XpertMobileApp.ViewModels;
using XpertMobileApp.ViewModels.Entree;
using XpertMobileApp.Views.Encaissement;

namespace XpertMobileApp.Views.Entree
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class EntreesPage : XBasePage
    {
        EntreeViewModel viewModel;
        private string typeEntree = EntreeTypes.RetourMobile;
        public string CurrentStream = Guid.NewGuid().ToString();
        public EntreesPage()
		{
            
            InitializeComponent ();

            if (string.IsNullOrEmpty(typeEntree))
                //btn_Additem.IsVisible = false;

            itemSelector = new TiersSelector(CurrentStream);

            BindingContext = viewModel = new EntreeViewModel(typeEntree);
            //vteGlobalInfos.IsVisible = typeEntree == EntreeTypes.RetourMobile && viewModel.HasAdmin;
            viewModel.LoadSummaries = true; // typeVente == VentesTypes.Vente

            MessagingCenter.Subscribe<TiersSelector, View_TRS_TIERS>(this, CurrentStream, async (obj, selectedItem) =>
            {
                viewModel.SelectedTiers = selectedItem;
                //ent_SelectedTiers.Text = selectedItem.NOM_TIERS1;
            });

        }

        async void OnItemSelected(object sender, SelectedItemChangedEventArgs args)
        {
            var item = args.SelectedItem as View_STK_ENTREE;
            if (item == null)
                return;

            //await Navigation.PushAsync(new VenteDetailPage(item));

            // Manually deselect item.
            //ItemsListView.SelectedItem = null;                        
        }

        async void AddItem_Clicked(object sender, EventArgs e)
        {
            if (App.Online)
            {
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
                    await Navigation.PushAsync(new RSFormPage(null, null));
                }
            }
            else
            {
                var session = await UpdateDatabase.getCurrenetSession();
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
                    await Navigation.PushAsync(new RSFormPage(null,null));
                }
            }
        }

        SYS_MOBILE_PARAMETRE parames;
        List<SYS_OBJET_PERMISSION> permissions;
        protected async override void OnAppearing()
        {
            base.OnAppearing();

            parames = await AppManager.GetSysParams();
            permissions = await AppManager.GetPermissions();

            if (viewModel.Items.Count == 0)
                LoadStats();

              if ( viewModel.TypeEntree == EntreeTypes.RetourMobile)
              {                 
                    viewModel.LoadExtrasDataCommand.Execute(null);
              }
        }

        private async void LoadStats()
        {
            viewModel.LoadItemsCommand.Execute(null);
        }

        private async void Filter_Clicked(object sender, EventArgs e)
        {
            //FilterPanel.IsVisible = !FilterPanel.IsVisible;
        }

        private async void btn_ApplyFilter_Clicked(object sender, EventArgs e)
        {        
            viewModel.LoadItemsCommand.Execute(null);
        }

        private void btn_CancelFilter_Clicked(object sender, EventArgs e)
        {
            viewModel.SelectedType = null;
            viewModel.SelectedTiers = null;
           // FilterPanel.IsVisible = false;
            viewModel.LoadItemsCommand.Execute(null);            
        }

        private void ComptePicker_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private TiersSelector itemSelector;
        private async void btn_Select_Clicked(object sender, EventArgs e)
        {
            await PopupNavigation.Instance.PushAsync(itemSelector);
        }

        private async void deleteAllVente(object sender, EventArgs e)
        {
            await UpdateDatabase.getInstance().DeleteAllAsync<View_STK_ENTREE>();
        }
    }
}