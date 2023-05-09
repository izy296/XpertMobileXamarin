
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
using XpertMobileApp.Api.Models;
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
        private bool tapped { get; set; } = false;

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
            //vteGlobalInfos.IsVisible = typeVente == VentesTypes.Vente && viewModel.HasAdmin;
            viewModel.LoadSummaries = true; // typeVente == VentesTypes.Vente

            if (Constants.AppName == Apps.X_DISTRIBUTION)
            {
                Device.BeginInvokeOnMainThread(async () =>
                {
                    if (viewModel.SelectedTournee == null)
                    {
                        await viewModel.LoadTourneeOpen();
                        if (viewModel.TourneeOpen.Count > 1)
                        {
                            TourneeOpenSelector AlertPopup = new TourneeOpenSelector("There is Multiple Tournee Open", falseMessage: AppResources.alrt_msg_Cancel, trueMessage: AppResources.alrt_msg_Ok);
                            AlertPopup.Items = viewModel.TourneeOpen;
                            await PopupNavigation.Instance.PushAsync(AlertPopup);
                            await AlertPopup.PopupClosedTask;
                            if (AlertPopup.Result != null)
                                viewModel.SelectedTournee = AlertPopup.Result;
                            else
                            {
                                await UserDialogs.Instance.AlertAsync("Aucun Tournee est seléctioné ,s'il vous plait essayez une autre fois", AppResources.alrt_msg_Alert,
                                    AppResources.alrt_msg_Ok);
                                await Navigation.PopAsync();
                            }
                        }
                        else if (viewModel.TourneeOpen.Count == 0)
                        {
                            await UserDialogs.Instance.AlertAsync("Aucun Tournee est assigner a vous ,s'il vous plait synchroniser les tournes", AppResources.alrt_msg_Alert,
                                AppResources.alrt_msg_Ok);
                            await ((MainPage)App.Current.MainPage).NavigateFromMenu((int)MenuItemType.Synchronisation);
                        }
                        else
                        {
                            viewModel.SelectedTournee = viewModel.TourneeOpen.FirstOrDefault();
                        }
                        if (viewModel.SelectedTournee.TYPE_TOURNEE != TourneeType.Open)
                        {
                            viewModel.IsAddPermited = false;
                        }
                    }

                });
            }
            //MessagingCenter.Subscribe<TiersSelector, View_TRS_TIERS>(this, CurrentStream, async (obj, selectedItem) =>
            //{
            //    viewModel.SelectedTiers = selectedItem;
            //    ent_SelectedTiers.Text = selectedItem.NOM_TIERS1;
            //});
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
                    if (Constants.AppName == Apps.X_DISTRIBUTION)
                    {
                        await Navigation.PushAsync(new VenteFormLivraisonPage(null, typeDoc));
                        return;
                    }
                    var res = await DisplayAlert(AppResources.msg_Confirmation, AppResources.msg_ShouldPrepaireSession, AppResources.alrt_msg_Ok, AppResources.alrt_msg_Cancel);
                    if (res)
                    {
                        OpenSessionPage openPage = new OpenSessionPage(viewModel.CurrentStream);
                        await PopupNavigation.Instance.PushAsync(openPage);
                    }
                }
                else
                {
                    if (Constants.AppName == Apps.XCOM_Livraison || Constants.AppName == Apps.X_DISTRIBUTION)
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
                    if (Constants.AppName == Apps.X_DISTRIBUTION)
                    {
                        await Navigation.PushAsync(new VenteFormLivraisonPage(null, typeDoc, codeTournee:viewModel.SelectedTournee.CODE_TOURNEE));
                        return;
                    }
                    var res = await DisplayAlert(AppResources.msg_Confirmation, AppResources.msg_ShouldPrepaireSession, AppResources.alrt_msg_Ok, AppResources.alrt_msg_Cancel);
                    if (res)
                    {
                        OpenSessionPage openPage = new OpenSessionPage(viewModel.CurrentStream);
                        await PopupNavigation.Instance.PushAsync(openPage);
                    }
                }
                else
                {
                    if (Constants.AppName == Apps.XCOM_Livraison || Constants.AppName == Apps.X_DISTRIBUTION)
                        await Navigation.PushAsync(new VenteFormLivraisonPage(null, typeDoc, codeTournee: viewModel.SelectedTournee.CODE_TOURNEE));
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

            if (viewModel.TypeVente == VentesTypes.Livraison)
                ArrowUp.IsVisible = false;


        }

        private async void LoadStats()
        {
            viewModel.LoadItemsCommand.Execute(null);
        }

        private async void Filter_Clicked(object sender, EventArgs e)
        {
            //FilterPanel.IsVisible = !FilterPanel.IsVisible;
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
            await SQLite_Manager.GetInstance().DeleteAllAsync<View_VTE_VENTE>();
        }

        private void ShowHide_Clicked(object sender, EventArgs e)
        {
            if (tapped)
            {
                floatingButton.Opacity = 1;
                SummariesInfos.HeightRequest = SummariesListView.HeightRequest = 30;
                tapped = !tapped;
                arrow_img.RotateTo(0, 400, Easing.Linear);
            }
            else
            {
                floatingButton.Opacity = 0.05;
                SummariesInfos.HeightRequest = SummariesListView.HeightRequest = gridSamuary.HeightRequest = 180;
                tapped = !tapped;
                arrow_img.RotateTo(180, 400, Easing.Linear);
            }
        }

        private async void ShowHideFilter(object sender, EventArgs e)
        {
            VentesPopupFilter filter = new VentesPopupFilter(viewModel);
            //Load data for the first time ...
            await PopupNavigation.Instance.PushAsync(filter);
        }
    }
}