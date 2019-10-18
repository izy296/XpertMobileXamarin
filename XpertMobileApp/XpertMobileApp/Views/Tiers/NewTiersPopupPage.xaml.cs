using Acr.UserDialogs;
using Rg.Plugins.Popup.Pages;
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
using XpertMobileApp.DAL;
using XpertMobileApp.Helpers;
using XpertMobileApp.Services;
using XpertMobileApp.ViewModels;

namespace XpertMobileApp.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class NewTiersPopupPage : PopupPage
    {

        public View_TRS_TIERS SelectedTiers { get; set; }

        public Command LoadFamilleCommand { get; set; }
        public Command LoadTypesCommand { get; set; }

        public View_TRS_TIERS Item { get; set; }

        public NewTiersPopupPage(View_TRS_TIERS item = null)
		{
			InitializeComponent ();

            Familles  = new ObservableCollection<View_BSE_TIERS_FAMILLE>();
            Types = new ObservableCollection<BSE_TABLE_TYPE>();

            LoadFamilleCommand = new Command(async () => await ExecuteLoadFamillesCommand());
            LoadTypesCommand = new Command(async () => await ExecuteLoadTypesCommand());

            if(item != null)
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


        #region filters data

        public string SearchedText { get; set; }

        public ObservableCollection<View_BSE_TIERS_FAMILLE> Familles { get; set; }
        public View_BSE_TIERS_FAMILLE SelectedFamille { get; set; }

        public ObservableCollection<BSE_TABLE_TYPE> Types { get; set; }
        public BSE_TABLE_TYPE SelectedType { get; set; }

        async Task ExecuteLoadExtrasDataCommand()
        {
            try
            {
                await ExecuteLoadFamillesCommand();
                await ExecuteLoadTypesCommand();
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

            try
            {
                Types.Clear();
                var itemsC = await WebServiceClient.getTiersTypes();

                BSE_TABLE_TYPE allElem = new BSE_TABLE_TYPE();
                allElem.CODE_TYPE = "";
                allElem.DESIGNATION_TYPE = AppResources.txt_All;
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

        async Task ExecuteLoadFamillesCommand()
        {

            try
            {
                Familles.Clear();
                var itemsC = await WebServiceClient.getTiersFamilles();

                View_BSE_TIERS_FAMILLE allElem = new View_BSE_TIERS_FAMILLE();
                allElem.CODE_FAMILLE = "";
                allElem.DESIGN_FAMILLE = AppResources.txt_All;
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

        #endregion

        private async void btn_Save_Clicked(object sender, EventArgs e)
        {
            try
            { 
                if (SelectedTiers == null && Constants.AppName == Apps.XPH_Mob)
                {
                    await DisplayAlert(AppResources.alrt_msg_Alert, AppResources.error_ThirdNotEmpty, AppResources.alrt_msg_Ok);
                    return;
                }

                if (SelectedFamille == null)
                {
                    await DisplayAlert(AppResources.alrt_msg_Alert, "Veullez saisir la famille du tiers.", AppResources.alrt_msg_Ok);
                    return;
                }

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

                await PopupNavigation.Instance.PopAsync();
               // await Navigation.PopModalAsync();
            }
            catch(Exception ex)
            {
                await DisplayAlert(AppResources.alrt_msg_Alert, "Erreur lors de traitement de la requête ! " + ex.Message, AppResources.alrt_msg_Ok);
            }

        }

        private async void btn_Cancel_Clicked(object sender, EventArgs e)
        {
            await PopupNavigation.Instance.PopAsync();
        }
    }
}
 