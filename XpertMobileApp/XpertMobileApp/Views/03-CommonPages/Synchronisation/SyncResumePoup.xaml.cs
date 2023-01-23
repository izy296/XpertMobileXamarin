using Acr.UserDialogs;
using Plugin.Connectivity;
using Rg.Plugins.Popup.Pages;
using Rg.Plugins.Popup.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using XpertMobileApp;
using XpertMobileApp.Models;
using XpertMobileApp.SQLite_Managment;
using XpertMobileApp.Views;
using XpertMobileAppManafiaa.SQLite_Managment;

namespace XpertMobileAppManafiaa.Views._03_CommonPages.Synchronisation
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SyncResumePoup : PopupPage, INotifyPropertyChanged
    {
        public SyncResumePoup()
        {
            InitializeComponent();
            this.CloseWhenBackgroundIsClicked = false;
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
        }

        public async void CheckSynchronisation()
        {
            try
            {
                bool isConnected = await App.IsConected();
                if (isConnected)
                {
                    var itemSynchronised = false;
                    /* Synchronisation des Tiers */
                    labelTiers.FontAttributes = FontAttributes.Bold;
                    itemSynchronised = await SyncManager.SyncTiersToServer();
                    labelTiers.FontAttributes = FontAttributes.None;
                    if (itemSynchronised)
                    {
                        syncTiers.IsVisible = true;
                        SyncManager.DeleteAllTiersSInQLite();
                    }
                    else
                    {
                        syncTiers.Source = "incorrect.png";
                        syncTiers.IsVisible = true;
                    }

                    /* ---------------------------- */

                    /* Synchronisation des encaiss */
                    labelEncaiss.FontAttributes = FontAttributes.Bold;
                    SyncManager.DeleteAllEncaissInSQlite();

                    itemSynchronised = await SyncManager.SyncEncaissToServer();
                    labelEncaiss.FontAttributes = FontAttributes.None;
                    if (itemSynchronised)
                    {
                        syncEncaiss.IsVisible = true;
                        SyncManager.DeleteAllEncaissInSQlite();
                    }
                    else
                    {
                        syncEncaiss.Source = "incorrect.png";
                        syncEncaiss.IsVisible = true;
                    }
                    /* -------------------------------- */



                    /* Synchronisation des ventes */

                    labelVentes.FontAttributes = FontAttributes.Bold;
                    itemSynchronised = await SyncManager.SyncVenteToServer();
                    labelVentes.FontAttributes = FontAttributes.None;
                    if (itemSynchronised)
                    {
                        syncVentes.IsVisible = true;
                        SyncManager.DeleteAllVentesInSQLite();
                    }
                    else
                    {
                        syncVentes.Source = "incorrect.png";
                        syncVentes.IsVisible = true;
                    }

                    /* ------------------------- */



                    /* Synchronisation des retour stock  */
                    labelCommande.FontAttributes = FontAttributes.Bold;
                    itemSynchronised = await SyncManager.SyncRetourStock();
                    labelCommande.FontAttributes = FontAttributes.None;
                    if (itemSynchronised)
                    {
                        syncRetourStock.IsVisible = true;
                        SyncManager.DeleteAllretourStockInSQLite();
                    }

                    else
                    {
                        syncRetourStock.Source = "incorrect.png";
                        syncRetourStock.IsVisible = true;
                    }
                    /* -------------------------------- */
                    /* Synchronisation des Tournee */
                    labelTournee.FontAttributes = FontAttributes.Bold;
                    itemSynchronised = await SyncManager.SyncTourneesToServer();
                    labelTournee.FontAttributes = FontAttributes.None;

                    if (itemSynchronised)
                    {
                        syncTournees.IsVisible = true;
                        buttonYes.IsEnabled = true;
                        SyncManager.DeleteAllTourneeInSQLite();
                    }
                    else
                    {
                        syncTournees.Source = "incorrect.png";
                        syncTournees.IsVisible = true;
                        buttonYes.IsEnabled = true;
                    }
                    /* ------------------------- */
                }
                else
                {
                    UserDialogs.Instance.HideLoading();
                    CustomPopup AlertPopup = new CustomPopup("Veuillez verifier votre connexion au serveur ! ", trueMessage: AppResources.alrt_msg_Ok);
                    await PopupNavigation.Instance.PushAsync(AlertPopup);
                    buttonYes.IsEnabled = true;
                }
            }
            catch (Exception ex)
            {
                switch (ex.Data["opeartion"])
                {
                    case "TIERS":
                        syncTiers.Source = syncEncaiss.Source = syncVentes.Source = syncTournees.Source = syncRetourStock.Source = "incorrect.png";
                        syncTiers.IsVisible = syncEncaiss.IsVisible = syncVentes.IsVisible = syncTournees.IsVisible = syncRetourStock.IsVisible = true;
                        labelTiers.FontAttributes = FontAttributes.None;
                        buttonYes.IsEnabled = true;
                        break;
                    case "ENCAISS":
                        syncEncaiss.Source = syncVentes.Source = syncTournees.Source = syncRetourStock.Source = "incorrect.png";
                        syncEncaiss.IsVisible = syncVentes.IsVisible = syncTournees.IsVisible = syncRetourStock.IsVisible = true;
                        labelEncaiss.FontAttributes = FontAttributes.None;
                        buttonYes.IsEnabled = true;
                        break;
                    case "VENTES":
                        syncVentes.Source = syncTournees.Source = syncRetourStock.Source = "incorrect.png";
                        syncVentes.IsVisible = syncTournees.IsVisible = syncRetourStock.IsVisible = true;
                        labelVentes.FontAttributes = FontAttributes.None;
                        buttonYes.IsEnabled = true;
                        break;
                    case "TOURNEE":
                        syncTournees.Source = syncRetourStock.Source = "incorrect.png";
                        syncTournees.IsVisible = syncRetourStock.IsVisible = true;
                        labelTournee.FontAttributes = FontAttributes.None;
                        buttonYes.IsEnabled = true;
                        break;
                    case "COMMANDES":
                        syncRetourStock.Source = "incorrect.png";
                        syncRetourStock.IsVisible = true;
                        labelCommande.FontAttributes = FontAttributes.None;
                        buttonYes.IsEnabled = true;
                        break;
                    default:
                        // code blocks
                        break;
                }
            }
        }
        /// <summary>
        /// Fermer la popup page
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void btn_Cancel_Clicked(object sender, EventArgs e)
        {
            await PopupNavigation.Instance.PopAsync();
        }
    }
}