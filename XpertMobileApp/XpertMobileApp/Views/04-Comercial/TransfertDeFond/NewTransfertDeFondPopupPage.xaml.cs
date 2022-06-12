using Acr.UserDialogs;
using Rg.Plugins.Popup.Pages;
using Rg.Plugins.Popup.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Xpert.Common.DAO;
using Xpert.Common.WSClient.Helpers;
using XpertMobileApp.Api.Managers;
using XpertMobileApp.Api.Services;
using XpertMobileApp.DAL;
using XpertMobileApp.Helpers;
using XpertMobileApp.Models;
using XpertMobileApp.Services;
using XpertMobileApp.SQLite_Managment;
using XpertMobileApp.ViewModels;
using XpertMobileApp.Views.Templates;

namespace XpertMobileApp.Views._04_Comercial.TransfertDeFond
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class NewTransfertDeFondPopupPage : PopupPage, INotifyPropertyChanged
    {
        public string CurrentStream = Guid.NewGuid().ToString();
        public Command LoadComptesAndMotifsCommand { get; set; }
        public Command LoadIMotifsCommand { get; set; }
        public View_TRS_VIREMENT Item { get; set; }
        public View_TRS_ENCAISS ItemEnc { get; set; }
        public View_TRS_ENCAISS ItemDec { get; set; }
        public ObservableCollection<BSE_ENCAISS_MOTIFS> Motifs { get; set; }
        private BSE_ENCAISS_MOTIFS selectedMotif;
        public BSE_ENCAISS_MOTIFS SelectedMotif
        {
            get
            {
                return selectedMotif;
            }
            set
            {
                selectedMotif = value;
            }
        }
        public ObservableCollection<BSE_MODE_REG> ModeReg { get; set; }
        private BSE_MODE_REG modeReglement;
        public BSE_MODE_REG ModeReglement
        {
            get
            {
                return modeReglement;
            }
            set
            {
                modeReglement = value;
            }
        }
        public ObservableCollection<View_BSE_COMPTE> Comptes
        {
            get; set;
        }
        public View_BSE_COMPTE SelectedCompteSrc { get; set; }
        public View_BSE_COMPTE SelectedCompteDst { get; set; }

        internal XpertSqlBuilder querry = new XpertSqlBuilder();
        public NewTransfertDeFondPopupPage(View_TRS_VIREMENT item = null, View_TRS_ENCAISS itemEnc = null, View_TRS_ENCAISS itemDec = null)
        {
            InitializeComponent();
            Comptes = new ObservableCollection<View_BSE_COMPTE>();
            Motifs = new ObservableCollection<BSE_ENCAISS_MOTIFS>();
            ModeReg = new ObservableCollection<BSE_MODE_REG>();
            LoadComptesAndMotifsCommand = new Command(async () => await ExecuteLoadComptesAndMotifs());

            BindingContext = this;

            if (itemEnc != null)
                ItemEnc = itemEnc;
            else
            {
                ItemEnc = new View_TRS_ENCAISS
                {
                    DATE_ENCAISS = DateTime.Now,
                    CODE_TYPE = "ENC"
                };
            }
            if (itemDec != null)
                ItemDec = itemDec;
            else
            {
                ItemDec = new View_TRS_ENCAISS
                {
                    DATE_ENCAISS = DateTime.Now,
                    CODE_TYPE = "DEC"
                };
            }
        }
        protected override void OnAppearing()
        {
            base.OnAppearing();
            if (Comptes.Count == 0 && Motifs.Count == 0 && ModeReg.Count == 0)
                LoadComptesAndMotifsCommand.Execute(null);
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


        /// <summary>
        /// Avoir les comptes disponible et les ajouter dans le picker
        /// </summary>
        /// <returns></returns>
        async Task ExecuteLoadComptesAndMotifs()
        {
            if (App.Online)
            {
                try
                {
                    Comptes.Clear();
                    Motifs.Clear();
                    ModeReg.Clear();
                    var itemsC = await WebServiceClient.getComptes();
                    var itemsM = await WebServiceClient.GetMotifs("MTF");
                    var itemsR = await WebServiceClient.GetMODE_REGs();

                    itemsC.Insert(0, new View_BSE_COMPTE()
                    {
                        DESIGNATION_TYPE = "",
                        DESIGN_COMPTE = "",
                        CODE_TYPE = ""
                    });

                    itemsM.Insert(0, new BSE_ENCAISS_MOTIFS()
                    {
                        DESIGN_MOTIF = AppResources.txt_All,
                        CODE_MOTIF = ""
                    });

                    itemsR.Insert(0, new BSE_MODE_REG()
                    {
                        DESIGN_MODE = "",
                        CODE_MODE = "",
                    });


                    foreach (var itemC in itemsC)
                        Comptes.Add(itemC);

                    foreach (var itemM in itemsM)
                        Motifs.Add(itemM);
                    foreach (var itemR in itemsR)
                        ModeReg.Add(itemR);

                }
                catch (Exception ex)
                {
                    await UserDialogs.Instance.AlertAsync(WSApi2.GetExceptionMessage(ex), AppResources.alrt_msg_Alert,
                        AppResources.alrt_msg_Ok);
                }
            }
            else
            {
                try
                {
                    Comptes.Clear();
                    Motifs.Clear();
                    ModeReg.Clear();
                    var itemsC = await WebServiceClient.getComptes();
                    itemsC.Insert(0, new View_BSE_COMPTE()
                    {
                        DESIGNATION_TYPE = AppResources.txt_All,
                        DESIGN_COMPTE = AppResources.txt_All,
                        CODE_TYPE = ""
                    });

                    var itemsM = await WebServiceClient.GetMotifs();
                    itemsM.Insert(0, new BSE_ENCAISS_MOTIFS()
                    {
                        DESIGN_MOTIF = AppResources.txt_All,
                        CODE_MOTIF = ""
                    });

                    foreach (var itemC in itemsC)
                        Comptes.Add(itemC);

                    foreach (var itemM in itemsM)
                        Motifs.Add(itemM);

                }
                catch (Exception ex)
                {
                    await UserDialogs.Instance.AlertAsync(WSApi2.GetExceptionMessage(ex), AppResources.alrt_msg_Alert,
                        AppResources.alrt_msg_Ok);
                }
            }
        }

        private void SetDataEncDec()
        {
            if (compteSourcePicker.SelectedIndex != -1)
                this.SelectedCompteSrc = compteSourcePicker.ItemsSource[compteSourcePicker.SelectedIndex] as View_BSE_COMPTE;

            if (compteDestPicker.SelectedIndex != -1)
                this.SelectedCompteDst = compteDestPicker.ItemsSource[compteDestPicker.SelectedIndex] as View_BSE_COMPTE;

            if (string.IsNullOrEmpty(this.ItemDec.CODE_MODE))
                this.ItemDec.CODE_MODE = this.ItemEnc.CODE_MODE = ModeReglement.CODE_MODE;

            if (this.ItemDec.REF_REG != null)
                this.ItemDec.REF_REG = this.ItemEnc.REF_REG = refEntry.Text.ToString();

            this.ItemDec.TOTAL_ENCAISS = ItemEnc.TOTAL_ENCAISS;
            this.ItemDec.DATE_ENCAISS = ItemEnc.DATE_ENCAISS;
        }
        private async Task<bool> CheckFields(View_TRS_ENCAISS itemEncaiss, View_TRS_ENCAISS itemDecaiss)
        {
            if (string.IsNullOrEmpty(itemDecaiss.CODE_COMPTE))
            {
                await DisplayAlert(AppResources.alrt_msg_Alert, AppResources.txt_alert_compte_source, AppResources.alrt_msg_Ok);
                return false;
            }
            else
            if (string.IsNullOrEmpty(itemEncaiss.CODE_COMPTE))
            {
                await DisplayAlert(AppResources.alrt_msg_Alert, AppResources.txt_alert_comptedestination, AppResources.alrt_msg_Ok);
                return false;
            }
            else if (itemEncaiss.TOTAL_ENCAISS == 0 && itemDecaiss.TOTAL_ENCAISS == 0)
            {
                await DisplayAlert(AppResources.alrt_msg_Alert, AppResources.txt_alert_montant, AppResources.alrt_msg_Ok);
                return false;
            }
            else
            if (string.IsNullOrEmpty(itemEncaiss.CODE_MOTIF) && string.IsNullOrEmpty(itemDecaiss.CODE_MOTIF))
            {
                await DisplayAlert(AppResources.alrt_msg_Alert, AppResources.txt_alert_code_motif, AppResources.alrt_msg_Ok);
                return false;
            }
            else
            {
                return true;
            }
        }

        //Add new Transfert de Fond 
        async void AddNewTransfertDeFond(object sender, EventArgs e)
        {
            /*------ Set the necessary data ------*/
            SetDataEncDec();

            /*------ Check if field are empty before insert ------*/
            bool check;
            check = await CheckFields(ItemEnc, ItemDec);

            /*------ Begin insertion ------*/
            var codeEnc = "";
            var codeDec = "";
            if (App.Online)
            {
                if (string.IsNullOrEmpty(ItemEnc.CODE_ENCAISS))
                {
                    if (check)
                    {
                        UserDialogs.Instance.ShowLoading(AppResources.txt_Waiting);

                        /*------ Decaiss and return CodeDecaiss ------*/
                        codeDec = await CrudManager.Encaiss.AddItemAsync(ItemDec);

                        /*------ Encaiss and return CodeEncaiss ------*/
                        codeEnc = await CrudManager.Encaiss.AddItemAsync(ItemEnc);

                        /*------ update CODE_VIR ------*/
                        Update_Code_Vir(codeEnc, codeDec);

                        UserDialogs.Instance.HideLoading();
                        await DisplayAlert(AppResources.alrt_msg_Alert, AppResources.txt_comfirmation_ajout_virement, AppResources.alrt_msg_Ok);
                    }
                    else
                    {
                        return;
                    }

                }
                await PopupNavigation.Instance.PopAsync();
            }
        }

        private async void Update_Code_Vir(string codeEnc, string codeDec)
        {
            string codeVir = XpertHelper.GetMD5Hash(codeEnc + codeDec);
            ItemDec.CODE_VIR = ItemEnc.CODE_VIR = codeVir;
            ItemEnc.CODE_ENCAISS = codeEnc;
            ItemDec.CODE_ENCAISS = codeDec;
            await CrudManager.Encaiss.UpdateItemAsync(ItemEnc);
            await CrudManager.Encaiss.UpdateItemAsync(ItemDec);
        }
        private void CompteSrcPicker_SelectedIndexChanged(object sender, EventArgs e)
        {
            var compte = Comptes[compteSourcePicker.SelectedIndex];
            ItemDec.CODE_COMPTE = compte.CODE_COMPTE;
        }

        private void CompteDesPicker_SelectedIndexChanged(object sender, EventArgs e)
        {
            var compte = Comptes[compteDestPicker.SelectedIndex];
            ItemEnc.CODE_COMPTE = compte.CODE_COMPTE;
        }
        private void MotifPicker_SelectedIndexChanged(object sender, EventArgs e)
        {
            var motif = Motifs[motifPicker.SelectedIndex];
            ItemEnc.CODE_MOTIF = motif.CODE_MOTIF;
            ItemDec.CODE_MOTIF = motif.CODE_MOTIF;
        }
    }
}

