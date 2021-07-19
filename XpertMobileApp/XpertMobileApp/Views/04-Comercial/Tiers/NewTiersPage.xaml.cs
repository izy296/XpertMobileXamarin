using Acr.UserDialogs;
using Rg.Plugins.Popup.Services;
using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Xpert.Common.WSClient.Helpers;
using XpertMobileApp.Api;
using XpertMobileApp.Api.Managers;
using XpertMobileApp.Api.Services;
using XpertMobileApp.DAL;
using XpertMobileApp.Helpers;
using XpertMobileApp.Services;
using XpertMobileApp.SQLite_Managment;
using XpertMobileApp.ViewModels;
using ZXing.Net.Mobile.Forms;

namespace XpertMobileApp.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class NewTiersPage : ContentPage
	{

        public View_TRS_TIERS SelectedTiers { get; set; }

        public Command LoadFamilleCommand { get; set; }
        public Command LoadSecteursCommand { get; set; }
        
        public Command LoadTypesCommand { get; set; }
        public string CurrentStream = Guid.NewGuid().ToString();
        public View_TRS_TIERS Item { get; set; }

        public NewTiersPage(View_TRS_TIERS item = null)
		{
			InitializeComponent ();

            itemSelector = new TiersSelector(CurrentStream);

            Familles  = new ObservableCollection<View_BSE_TIERS_FAMILLE>();
            Types = new ObservableCollection<BSE_TABLE_TYPE>();
            Secteurs = new ObservableCollection<BSE_TABLE>();

            LoadFamilleCommand = new Command(async () => await ExecuteLoadFamillesCommand());
            LoadTypesCommand = new Command(async () => await ExecuteLoadTypesCommand());
            LoadSecteursCommand = new Command(async () => await ExecuteLoadSecteursCommand());
            if (item != null)
            {
                Item = item;
            } 
            else
            {
                Item = new View_TRS_TIERS
                {
                    NOM_TIERS = "",
                    PRENOM_TIERS = ""
                };
            }

            BindingContext = this;
            /*
            MessagingCenter.Subscribe<TiersSelector, View_TRS_TIERS>(this, MCDico.ITEM_SELECTED, async (obj, selectedItem) =>
            {
                SelectedTiers = selectedItem;

                Item.CODE_TIERS = SelectedTiers.CODE_TIERS;
                TierSolde.Text = string.Format("{0:F2} DA", SelectedTiers.SOLDE_TIERS);
                ent_SelectedTiers.Text = selectedItem.NOM_TIERS1;
            });
            */
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            if (Types.Count == 0)
                LoadTypesCommand.Execute(null);

            if (Familles.Count == 0)
               LoadFamilleCommand.Execute(null);

            if (Secteurs.Count == 0)
                LoadSecteursCommand.Execute(null);
        }

        private void SelectSecteurs(string codeElem)
        {
            for (int i = 0; i < Secteurs.Count; i++)
            {
                if (Secteurs[i].CODE == codeElem)
                {
                    SecteursPicker.SelectedIndex = i;
                    return;
                }
            }
        }

        private void SelectFamilles(string codeElem)
        {
            for (int i = 0; i < Familles.Count; i++)
            {
                if(Familles[i].CODE_FAMILLE == codeElem)
                {
                    FamillesPicker.SelectedIndex = i;
                    return;
                }
            }
        }

        private void SelectType(string codeElem)
        {
            for (int i = 0; i < Types.Count; i++)
            {
                if (Types[i].CODE_TYPE == codeElem)
                {
                    TypesPicker.SelectedIndex = i;
                    return;
                }
            }
        }

        private void TypesPicker_SelectedIndexChanged(object sender, EventArgs e)
        {
            var motif = Types[TypesPicker.SelectedIndex];
            Item.CODE_TYPE = motif.CODE_TYPE;
        }

        private void FamillesPicker_SelectedIndexChanged(object sender, EventArgs e)
        {
            var motif = Familles[FamillesPicker.SelectedIndex];
            Item.CODE_FAMILLE = motif.CODE_FAMILLE;
        }

        private void SecteursPicker_SelectedIndexChanged(object sender, EventArgs e)
        {
            var itm = Secteurs[SecteursPicker.SelectedIndex];
            Item.CODE_LIEUX = itm.CODE;
        }

        private TiersSelector itemSelector;

        private async void btn_Select_Clicked(object sender, EventArgs e)
        {
            await PopupNavigation.Instance.PushAsync(itemSelector);
        }

        #region filters data

        public string SearchedText { get; set; }

        public ObservableCollection<View_BSE_TIERS_FAMILLE> Familles { get; set; }
        public View_BSE_TIERS_FAMILLE SelectedFamille { get; set; }

        public ObservableCollection<BSE_TABLE_TYPE> Types { get; set; }
        public BSE_TABLE_TYPE SelectedType { get; set; }

        public ObservableCollection<BSE_TABLE> Secteurs { get; set; }
        public BSE_TABLE_TYPE SelectedSecteur { get; set; }

        async Task ExecuteLoadExtrasDataCommand()
        {
            try
            {
                await ExecuteLoadFamillesCommand();
                await ExecuteLoadTypesCommand();
                await ExecuteLoadSecteursCommand();
            }
            catch (Exception ex)
            {
                await UserDialogs.Instance.AlertAsync(WSApi2.GetExceptionMessage(ex), AppResources.alrt_msg_Alert,
                    AppResources.alrt_msg_Ok);
            }
            finally
            {
     
            }
        }

        async Task ExecuteLoadTypesCommand()
        {
            if (App.Online)
            {
                try
                {
                    Types.Clear();
                    var itemsC = await WebServiceClient.getTiersTypes();

                    BSE_TABLE_TYPE allElem = new BSE_TABLE_TYPE();
                    allElem.CODE_TYPE = "";
                    allElem.DESIGNATION_TYPE = "";
                    Types.Add(allElem);

                    foreach (var itemC in itemsC)
                    {
                        Types.Add(itemC);
                    }
                }
                catch (Exception ex)
                {
                    await UserDialogs.Instance.AlertAsync(WSApi2.GetExceptionMessage(ex), AppResources.alrt_msg_Alert,
                        AppResources.alrt_msg_Ok);
                }
            }
            else
            {
                Types.Clear();
                var itemsC = await SQLite_Manager.getTypeTiers();
                foreach (var itemC in itemsC)
                {
                    Types.Add(itemC);
                }
            }

        }

        async Task ExecuteLoadFamillesCommand()
        {
            if (App.Online)
            {
                try
                {

                    Familles.Clear();
                    var itemsC = await WebServiceClient.getTiersFamilles();

                    View_BSE_TIERS_FAMILLE allElem = new View_BSE_TIERS_FAMILLE();
                    allElem.CODE_FAMILLE = "";
                    allElem.DESIGN_FAMILLE = "";
                    Familles.Add(allElem);

                    foreach (var itemC in itemsC)
                    {
                        Familles.Add(itemC);
                    }
                }
                catch (Exception ex)
                {
                    await UserDialogs.Instance.AlertAsync(WSApi2.GetExceptionMessage(ex), AppResources.alrt_msg_Alert,
                        AppResources.alrt_msg_Ok);
                }
            }
            else
            {
                Familles.Clear();
                var itemsC = await SQLite_Manager.getFamille();
                foreach (var itemC in itemsC)
                {
                    Familles.Add(itemC);
                }
            }
        }

        async Task ExecuteLoadSecteursCommand()
        {
            if (App.Online)
            {
                try
                {
                    Secteurs.Clear();
                    var itemsC = await CrudManager.BSE_LIEUX.GetItemsAsync();

                    BSE_TABLE allElem = new BSE_TABLE();
                    allElem.CODE = "";
                    allElem.DESIGNATION = "";
                    Secteurs.Add(allElem);

                    foreach (var itemC in itemsC)
                    {
                        Secteurs.Add(itemC);
                    }
                }
                catch (Exception ex)
                {
                    await UserDialogs.Instance.AlertAsync(WSApi2.GetExceptionMessage(ex), AppResources.alrt_msg_Alert,
                        AppResources.alrt_msg_Ok);
                }
            }
            else
            {
                Secteurs.Clear();
                var itemsC = await SQLite_Manager.getSecteurs(); 
                foreach (var itemC in itemsC)
                {
                    Secteurs.Add(itemC);
                }
            }

        }
        #endregion

        private async void btn_Save_Clicked(object sender, EventArgs e)
        {
            try
            { 
                /*
                if (SelectedType == null && Constants.AppName == Apps.XPH_Mob)
                {
                    await DisplayAlert(AppResources.alrt_msg_Alert, AppResources.error_ThirdNotEmpty, AppResources.alrt_msg_Ok);
                    return;
                }
                */
                if (SelectedFamille == null)
                {
                    await DisplayAlert(AppResources.alrt_msg_Alert, "Veullez saisir la famille du tiers.", AppResources.alrt_msg_Ok);
                    return;
                }
                if (Item.TEL1_TIERS == null)
                {
                    await DisplayAlert(AppResources.alrt_msg_Alert, "Veullez saisir le numero de téléphone", AppResources.alrt_msg_Ok);
                    return;
                }
                UserDialogs.Instance.ShowLoading(AppResources.txt_Waiting);

                if (App.Online)
                {
                    if (string.IsNullOrEmpty(Item.CODE_TIERS))
                    {
                        Item.ACTIF_TIERS = 1;
                        string codeTiers = await CrudManager.TiersService.AddItemAsync(Item);
                        Item.CODE_TIERS = codeTiers;
                        Item.NOM_TIERS1 = Item.NOM_TIERS + " " + Item.PRENOM_TIERS;
                        MessagingCenter.Send(App.MsgCenter, MCDico.ITEM_ADDED, Item);
                    }
                    else
                    {
                        await CrudManager.TiersService.UpdateItemAsync(Item);
                        MessagingCenter.Send(App.MsgCenter, MCDico.ITEM_ADDED, Item);
                    }
                }
                else
                {
                    if (Item.ID == 0)
                    {
                        Item.ACTIF_TIERS = 1;
                        await SQLite_Manager.AjoutTiers(Item);
                        Item.NOM_TIERS1 = Item.NOM_TIERS + " " + Item.PRENOM_TIERS;
                        MessagingCenter.Send(App.MsgCenter, MCDico.ITEM_ADDED, Item);
                    }
                    else
                    {
                        await SQLite_Manager.UpdateTiers(Item);
                        Item.NOM_TIERS1 = Item.NOM_TIERS + " " + Item.PRENOM_TIERS;
                        MessagingCenter.Send(App.MsgCenter, MCDico.ITEM_ADDED, Item);
                    }
                }

                UserDialogs.Instance.HideLoading();
                await DisplayAlert(AppResources.alrt_msg_Info, AppResources.txt_actionsSucces, AppResources.alrt_msg_Ok);
                await Navigation.PopModalAsync();
            }
            catch(Exception ex)
            {
                UserDialogs.Instance.HideLoading();
                await DisplayAlert(AppResources.alrt_msg_Alert, "Erreur lors de traitement de la requête ! " + ex.Message, AppResources.alrt_msg_Ok);
            }
        }

        private async void btn_Cancel_Clicked(object sender, EventArgs e)
        {
            await Navigation.PopModalAsync();
        }

        private async void ScnanCB_Clicked(object sender, EventArgs e)
        {
            var scaner = new ZXingScannerPage();
            await Navigation.PushAsync(scaner);
            scaner.OnScanResult += (result) =>
            {
                scaner.IsScanning = false;
                Device.BeginInvokeOnMainThread(async () =>
                {
                    XpertHelper.PeepScan();
                    CBCarte.Text = result.Text;
                    await Navigation.PopAsync();
                });
            };
        }
    }
}
 