using Acr.UserDialogs;
using Rg.Plugins.Popup.Pages;
using Rg.Plugins.Popup.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Xamarin.RisePlugin.Floatingactionbutton;
using Xamarin.RisePlugin.Floatingactionbutton.Enums;
using Xpert.Common.WSClient.Helpers;
using XpertMobileApp.Api.Managers;
using XpertMobileApp.DAL;
using XpertMobileApp.Helpers;
using XpertMobileApp.Services;
using XpertMobileApp.SQLite_Managment;
using ZXing.Net.Mobile.Forms;

namespace XpertMobileApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class TierDetailPage : PopupPage
    {
        private bool isEnable = false;
        private bool isOpen = false;
        public Command LoadFamilleCommand { get; set; }
        public Command LoadTypesCommand { get; set; }
        public Command LoadSecteursCommand { get; set; }
        public string SearchedText { get; set; }
        public BSE_TABLE SelectedSecteur { get; set; }
        public ObservableCollection<View_BSE_TIERS_FAMILLE> Familles { get; set; }
        public View_BSE_TIERS_FAMILLE SelectedFamille { get; set; }

        public ObservableCollection<BSE_TABLE_TYPE> Types { get; set; }
        public BSE_TABLE_TYPE SelectedType { get; set; }

        public ObservableCollection<BSE_TABLE> Secteurs { get; set; }
        public View_TRS_TIERS Item { get; set; }

        ZXingBarcodeImageView barcode;
        public TierDetailPage(View_TRS_TIERS tiers)
        {
            InitializeComponent();

            /* Disable all the input to be non editable */
            inputPrenom.IsEnabled =
            inputName.IsEnabled =
            inputTelephone.IsEnabled =
            inputEmail.IsEnabled =
            inputNaissance.IsEnabled =
            inputCodeBar.IsEnabled =
            inputSecteur.IsEnabled =
            inputType.IsEnabled =
            inputFamille.IsEnabled = false;

            Familles = new ObservableCollection<View_BSE_TIERS_FAMILLE>();
            Types = new ObservableCollection<BSE_TABLE_TYPE>();
            Secteurs = new ObservableCollection<BSE_TABLE>();

            LoadFamilleCommand = new Command(async () => await ExecuteLoadFamillesCommand());
            LoadTypesCommand = new Command(async () => await ExecuteLoadTypesCommand());
            LoadSecteursCommand = new Command(async () => await ExecuteLoadSecteursCommand());

            if (tiers != null)
            {
                Item = tiers;
            }
            else
            {
                Item = new View_TRS_TIERS
                {
                    NOM_TIERS = "",
                    PRENOM_TIERS = ""
                };
            }


            /* Concerning the barcode */
            barcode = new ZXingBarcodeImageView
            {
                HorizontalOptions = LayoutOptions.FillAndExpand,
                VerticalOptions = LayoutOptions.FillAndExpand,
                AutomationId = "zxingBarcodeImageView"
            };

            barcode.BarcodeFormat = ZXing.BarcodeFormat.QR_CODE;

            barcode.BarcodeOptions.Width = 400;

            barcode.BarcodeOptions.Height = 400;

            barcode.BarcodeOptions.Margin = 10;

            barcode.BarcodeValue = Item.CODE_TIERS;
            qrCodeViewer.Children.Insert(1, barcode);
            LocationSelector locationSelector = new LocationSelector(tiers);

            Map.Content = locationSelector.Content;



            BindingContext = this;
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
        private void DisableEnableEntries(object sender, EventArgs e)
        {
            try
            {
                isEnable = !isEnable;
                if (isEnable)
                {
                    /*Disable all the input to be non editable */
                    inputPrenom.IsEnabled =
                    inputName.IsEnabled =
                    inputTelephone.IsEnabled =
                    inputEmail.IsEnabled =
                    inputNaissance.IsEnabled =
                    inputCodeBar.IsEnabled =
                    inputSecteur.IsEnabled =
                    inputType.IsEnabled =
                    inputFamille.IsEnabled = true;
                }
                else
                {
                    /* Enable all the input to be editable */
                    inputPrenom.IsEnabled =
                    inputName.IsEnabled =
                    inputTelephone.IsEnabled =
                    inputEmail.IsEnabled =
                    inputNaissance.IsEnabled =
                    inputCodeBar.IsEnabled =
                    inputSecteur.IsEnabled =
                    inputType.IsEnabled =
                    inputFamille.IsEnabled = false;
                }
            }
            catch (Exception ex)
            {

                throw ex;
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
                    allElem.DESIGN_FAMILLE = AppResources.txt_All;
                    Familles.Add(allElem);

                    foreach (var itemC in itemsC)
                    {
                        Familles.Add(itemC);
                        if (itemC.CODE_FAMILLE == Item?.CODE_FAMILLE)
                        {
                            SelectedFamille = itemC;
                            FamillesPicker.SelectedIndex = Familles.IndexOf(itemC);
                        }
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
                    if (itemC.CODE_FAMILLE == Item?.CODE_FAMILLE)
                    {
                        SelectedFamille = itemC;
                        FamillesPicker.SelectedIndex = Familles.IndexOf(itemC);
                    }
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
                        if (itemC.CODE == Item?.CODE_LIEUX)
                        {
                            SelectedSecteur = itemC;
                            SecteursPicker.SelectedIndex = Secteurs.IndexOf(itemC);
                        }
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
                    if (itemC.CODE == Item?.CODE_LIEUX)
                    {
                        SelectedSecteur = itemC;
                        SecteursPicker.SelectedIndex = Secteurs.IndexOf(itemC);
                    }
                }
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
                    allElem.DESIGNATION_TYPE = AppResources.txt_All;
                    Types.Add(allElem);

                    foreach (var itemC in itemsC)
                    {
                        Types.Add(itemC);
                        if (itemC.CODE_TYPE == Item?.CODE_TYPE)
                        {
                            SelectedType = itemC;
                            TypesPicker.SelectedIndex = Types.IndexOf(itemC);
                        }
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
                    if (itemC.CODE_TYPE == Item?.CODE_TYPE)
                    {
                        SelectedType = itemC;
                        TypesPicker.SelectedIndex = Types.IndexOf(itemC);
                    }
                }
            }

        }

        private void SecteursPicker_SelectedIndexChanged(object sender, EventArgs e)
        {
            var itm = Secteurs[SecteursPicker.SelectedIndex];
            Item.CODE_LIEUX = itm.CODE;
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

        private async void ValidateEditTiers(object sender, EventArgs e)
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
                    try
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
                    catch (Exception ex)
                    {
                        await UserDialogs.Instance.AlertAsync(WSApi2.GetExceptionMessage(ex), AppResources.alrt_msg_Alert, AppResources.alrt_msg_Ok);
                    }
                }

                UserDialogs.Instance.HideLoading();
                await DisplayAlert(AppResources.alrt_msg_Info, AppResources.txt_actionsSucces, AppResources.alrt_msg_Ok);
                await PopupNavigation.Instance.PopAsync();
            }
            catch (Exception ex)
            {
                UserDialogs.Instance.HideLoading();
                await DisplayAlert(AppResources.alrt_msg_Alert, "Erreur lors de traitement de la requête ! " + ex.Message, AppResources.alrt_msg_Ok);
            }
        }

        private async void ClosePopup(object sender, EventArgs e)
        {
            try
            {
                await PopupNavigation.Instance.PopAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private async void AddNewCommand(object sender, EventArgs e)
        {
            await Navigation.PushModalAsync(new NavigationPage(new NewCommandePage(null, Item)));
            await PopupNavigation.Instance.PopAsync();
        }


        private async void OpenCloseButtons(object sender, EventArgs e)
        {
            try
            {
                await ((Frame)sender).ScaleTo(0, 50, Easing.Linear);
                await ((Frame)sender).ScaleTo(1, 50, Easing.Linear);
                
                if (isOpen)
                {
                    isOpen = !isOpen;
                    FloatMenuItem1.IsVisible = false;
                    await Task.Delay(100);
                    FloatMenuItem2.IsVisible = false;
                    await Task.Delay(100);
                    FloatMenuItem3.IsVisible = false;

                }
                else
                {
                    isOpen = !isOpen;
                    FloatMenuItem1.IsVisible = true;
                    await Task.Delay(100);
                    FloatMenuItem2.IsVisible = true;
                    await Task.Delay(100);
                    FloatMenuItem3.IsVisible = true;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}