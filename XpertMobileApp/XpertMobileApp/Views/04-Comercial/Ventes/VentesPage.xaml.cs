
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
using XpertMobileApp.Views.Encaissement;

namespace XpertMobileApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class VentesPage : XBasePage
    {
        VentesViewModel viewModel;
        private string typeDoc = VentesTypes.Vente;
        public string CurrentStream = Guid.NewGuid().ToString();
        public VentesPage(string typeVente)
        {
            typeDoc = typeVente;

            MessagingCenter.Subscribe<App, Object>(this, "ExtraData", async (obj, selectedItem) =>
            {
                await Navigation.PushAsync(new VenteDetailPage(new View_VTE_VENTE(), Convert.ToString(selectedItem)));
            });

            MessagingCenter.Subscribe<NotificationPage, Object>(this, "ExtraData", async (obj, selectedItem) =>
            {
                await Navigation.PushAsync(new VenteDetailPage(new View_VTE_VENTE(), Convert.ToString(selectedItem)));
            });


            InitializeComponent();

            if (string.IsNullOrEmpty(typeVente))
                btn_Additem.IsVisible = false;

            itemSelector = new TiersSelector(CurrentStream);

            BindingContext = viewModel = new VentesViewModel(typeVente);
            vteGlobalInfos.IsVisible = typeVente == VentesTypes.Vente && viewModel.HasAdmin;
            viewModel.LoadSummaries = true; // typeVente == VentesTypes.Vente

            MessagingCenter.Subscribe<TiersSelector, View_TRS_TIERS>(this, CurrentStream, async (obj, selectedItem) =>
            {
                viewModel.SelectedTiers = selectedItem;
                ent_SelectedTiers.Text = selectedItem.NOM_TIERS1;
            });

        }

        async void OnItemSelected(object sender, SelectedItemChangedEventArgs args)
        {
            var item = args.SelectedItem as View_VTE_VENTE;
            if (item == null)
                return;

            await Navigation.PushAsync(new VenteDetailPage(item));

            // Manually deselect item.
            ItemsListView.SelectedItem = null;
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
                    if (Constants.AppName == Apps.XCOM_Livraison)
                        await Navigation.PushAsync(new VenteFormLivraisonPage(null, typeDoc));
                    else
                        await Navigation.PushAsync(new VenteFormPage(null, typeDoc));
                }
            }
            else
            {
                var session = await SQLite_Manager.getCurrenetSession();
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
                    if (Constants.AppName == Apps.XCOM_Livraison)
                        await Navigation.PushAsync(new VenteFormLivraisonPage(null, typeDoc));
                    else
                        await Navigation.PushAsync(new VenteFormPage(null, typeDoc));
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

            if (viewModel.TypeVente == VentesTypes.Vente)
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
            FilterPanel.IsVisible = !FilterPanel.IsVisible;
        }

        private async void btn_ApplyFilter_Clicked(object sender, EventArgs e)
        {
            viewModel.LoadItemsCommand.Execute(null);
        }

        private void btn_CancelFilter_Clicked(object sender, EventArgs e)
        {
            viewModel.SelectedType = null;
            viewModel.SelectedTiers = null;
            FilterPanel.IsVisible = false;
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
            await SQLite_Manager.getInstance().DeleteAllAsync<View_VTE_VENTE>();
        }
    }
}