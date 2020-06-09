using Acr.UserDialogs;
using Plugin.SimpleAudioPlayer;
using Rg.Plugins.Popup.Services;
using Syncfusion.SfNumericTextBox.XForms;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Xpert.Common.WSClient.Helpers;
using XpertMobileApp.Api.Managers;
using XpertMobileApp.Api.Services;
using XpertMobileApp.DAL;
using XpertMobileApp.Helpers;
using XpertMobileApp.Models;
using XpertMobileApp.Services;
using XpertMobileApp.Views.Achats;
using ZXing.Net.Mobile.Forms;

namespace XpertMobileApp.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class ProductionFormPage : ContentPage
	{
        ProductionFormViewModel viewModel;

        SYS_MOBILE_PARAMETRE parames;
        List<SYS_OBJET_PERMISSION> permissions;

        public Command AddItemCommand { get; set; }

        public ProductionFormPage(View_PRD_AGRICULTURE vente, string typeDoc, string motif)
        {
            InitializeComponent();

            // NavigationPage.SetHasNavigationBar(this, false);

            if (vente.STATUS_DOC == DocProdStatus.PrdEnAttente)
                cmd_Terminate.Text = "En cours";
            else if (vente.STATUS_DOC == DocProdStatus.PrdEnCours)
                cmd_Terminate.Text = "Terminer";
            /*
            else if (vente.STATUS_DOC == DocProdStatus.PrdTermine)
                cmd_Terminate.Text = "Livrer";
             */
            else
                cmd_Terminate.IsVisible = false;

            itemSelector = new ProductSelector();
            TiersSelector = new TiersSelector();
            EmballageSelector = new EmballageSelector();
            AnnexTiersSelector = new AnnexTiersSelector();

            var ach = vente == null ? new View_PRD_AGRICULTURE() : vente;
            if(vente == null)
            { 
                // ach.TYPE_DOC = typeDoc;
                // ach.CODE_MOTIF = motif;
                ach.DATE_DOC = DateTime.Now.Date;
            }

            BindingContext = this.viewModel = new ProductionFormViewModel(ach, ach?.CODE_DOC);

            // jobFieldAutoComplete.BindingContext = viewModel;

            this.viewModel.Title = string.IsNullOrEmpty(ach.CODE_DOC) ? AppResources.pn_NewPurchase : ach?.ToString();

            this.viewModel.LoadRowsCommand = new Command(async () => await ExecuteLoadRowsCommand());

           // viewModel.ItemRows.CollectionChanged += ItemsRowsChanged;

            MessagingCenter.Subscribe<ProductSelector, View_ACH_DOCUMENT>(this, MCDico.ITEM_SELECTED, async (obj, selectedItem) =>
            {
                Device.BeginInvokeOnMainThread(() =>
                {
                    this.AddNewRow(selectedItem);
                });
            });

            MessagingCenter.Subscribe<TiersSelector, View_TRS_TIERS>(this, MCDico.ITEM_SELECTED, async (obj, selectedItem) =>
            {
                Device.BeginInvokeOnMainThread(() =>
                {
                    viewModel.SelectedTiers = selectedItem;
                    viewModel.Item.CODE_TIERS = selectedItem.CODE_TIERS;
                    viewModel.Item.NOM_TIERS = selectedItem.NOM_TIERS1;
                });
            });

            MessagingCenter.Subscribe<EmballageSelector, List<View_BSE_EMBALLAGE>>(this, CurrentStream, async (obj, items) =>
            {
                Device.BeginInvokeOnMainThread(() =>
                {
                    try
                    {
                        foreach (var item in items)
                        {
                            item.CODE_EMBALLAGE = item.CODE;
                        }
                        var embs = items.Where(e=> e.QUANTITE_ENTREE != 0 || e.QUANTITE_SORTIE != 0).ToList();
                        var embalages = new List<View_BSE_EMBALLAGE>();
                        if (embs != null && embs.Count > 0)
                        { 
                            foreach (var item in embs)
                            {
                                embalages.Add(XpertHelper.CloneObject<View_BSE_EMBALLAGE>(item));
                            }
                        }
                        currentRow.Embalages = embalages;

                        SaveEmballages(currentRow, embalages);
                        UpdatePeseeInfos();
                    }
                    catch (Exception ex)
                    {
                        UserDialogs.Instance.AlertAsync(WSApi2.GetExceptionMessage(ex), AppResources.alrt_msg_Alert,
    AppResources.alrt_msg_Ok);
                    }
                });
            });

            MessagingCenter.Subscribe<ProductSelector, View_ACH_DOCUMENT>(this, MCDico.REMOVE_ITEM, async (obj, selectedItem) =>
            {
                Device.BeginInvokeOnMainThread(() =>
                {
                    this.RemoveNewRow(selectedItem);
                });
            });

            if (viewModel.Item != null && !string.IsNullOrEmpty(this.viewModel.Item.CODE_DOC))
            {
                viewModel.LoadRowsCommand.Execute(null);
            }
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();

            parames = await App.GetSysParams();
            permissions = await App.GetPermissions();

            if (!App.HasAdmin)
            { 
                ApplyVisibility();
            }
            else
            {
                if ((viewModel.Item.STATUS_DOC == DocStatus.EnCours || viewModel.Item.STATUS_DOC == DocStatus.EnAttente))
                {
                     cmd_Terminate.IsVisible = true;
                }
            }

            // ne_PESEE_ENTREE.IsEnabled = string.IsNullOrEmpty(this.viewModel.Item.CODE_DOC);
            // ne_PESEE_SORTIE.IsEnabled = !string.IsNullOrEmpty(this.viewModel.Item.CODE_DOC);
        }

        private void ApplyVisibility()
        {
            /*
            //btn_Immatriculation.IsEnabled = false;
            cmd_Terminate.IsVisible = false;
            string userGroup = App.User.GroupName;

            // Par défaut le header est caché s'il n'a pas le droit d'éditer le header
            pnl_Header.IsVisible = viewModel.hasEditHeader;

            // Commandes pour la selection de produit
            btn_RowSelect.IsEnabled = viewModel.hasEditDetails;
            btn_RowScan.IsEnabled = viewModel.hasEditDetails;

            // Champs d'edition du header
            dp_EcheanceDate.IsEnabled = viewModel.hasEditHeader;
            btn_TeirsSearch.IsEnabled = viewModel.hasEditHeader;


            if (string.IsNullOrEmpty(viewModel.Item.STATUS_DOC))
            {


                btn_RowSelect.IsEnabled = false;
                btn_RowScan.IsEnabled = false;
            }
            else if (viewModel.Item.STATUS_DOC == DocStatus.EnAttente)
            {
                dp_EcheanceDate.IsEnabled = false;
                btn_TeirsSearch.IsEnabled = false;

                if (viewModel.hasEditDetails)
                {
                    cmd_Terminate.IsVisible = true;
                }
            }
            else if (viewModel.Item.STATUS_DOC == DocStatus.EnCours)
            {
                dp_EcheanceDate.IsEnabled = false;
                btn_TeirsSearch.IsEnabled = false;


                if (viewModel.hasEditDetails)
                {
                    cmd_Terminate.IsVisible = true;
                }
            }
            else if (viewModel.Item.STATUS_DOC == DocStatus.Termine || viewModel.Item.STATUS_DOC == DocStatus.Cloture)
            {
                dp_EcheanceDate.IsEnabled = false;
                btn_TeirsSearch.IsEnabled = false;


                btn_RowSelect.IsEnabled = false;
                btn_RowScan.IsEnabled = false;
            }
            */
        }

        #region Méthodes

        async Task ExecuteLoadRowsCommand()
        {
            if (string.IsNullOrEmpty(this.viewModel.Item?.CODE_DOC)) return;

            if (IsBusy)
                return;

            IsBusy = true;

            try
            {
                viewModel.ItemRows.Clear();
                var itemsC = await WebServiceClient.GetProductionDetails(this.viewModel.Item.CODE_DOC);

                UpdateItemIndex(itemsC);

                foreach (var itemC in itemsC)
                {
                    itemC.Parent_Doc = viewModel.Item;
                    viewModel.ItemRows.Add(itemC);
                }

                UpdatePeseeInfos();
            }
            catch (Exception ex)
            {
                await UserDialogs.Instance.AlertAsync(WSApi2.GetExceptionMessage(ex), AppResources.alrt_msg_Alert,
                    AppResources.alrt_msg_Ok);
            }
            finally
            {
                IsBusy = false;
            }
        }

        private void UpdateItemIndex<T>(List<T> items)
        {
            int i = 0;
            foreach (var item in items)
            {
                i += 1;
                (item as BASE_CLASS).Index = i;
            }
        }

        private void RemoveNewRow(View_ACH_DOCUMENT det)
        {
            var row = viewModel.ItemRows.Where(e => e.CODE_DOC_RECEPTION == det.CODE_DOC).FirstOrDefault();
            if (row == null) return;
            viewModel.ItemRows.Remove(row);
        }

        private void AddNewRow(View_ACH_DOCUMENT det)
        {
            /*
            var row = viewModel.ItemRows.Where(e => e.CODE_DOC_RECEPTION == det.CODE_DOC).FirstOrDefault();

            if (row == null)
            {
                row = new View_PRD_AGRICULTURE_DETAIL();
                row.Parent_Doc = viewModel.Item;
                row.CODE_DOC = viewModel.Item.CODE_DOC;

                row.CODE_PRODUIT = product.CODE_PRODUIT;
                row.CODE_BARRE = product.CODE_BARRE;
                row.DESIGNATION_PRODUIT = product.DESIGNATION_PRODUIT;
                // row.UNITE
                row.TAUX_DECHET   = product.TAUX_DECHET;
                row.PRIX_UNITAIRE = product.PRIX_ACHAT_HT; 
                row.PRIX_VENTE    = product.PRIX_VENTE_HT;

                row.CODE_MAGASIN = parames.DEFAULT_ACHATS_MAGASIN;
                row.LOT = parames.DEFAULT_COMPAGNE_LOT;
                row.UNITE = viewModel.Item.CODE_UNITE;
                row.CODE_UNITE_ENTETE = viewModel.Item.CODE_UNITE;

                if (viewModel.ItemRows.Count == 0 && viewModel.Item.PESEE_ENTREE > 0)
                {
                    row.IS_PRINCIPAL = true;
                    row.SetPeseeBrute(viewModel.Item.PESEE_BRUTE);
                }

                if(viewModel.ItemRows.Count == 0)
                {
                    viewModel.Item.STATUS_DOC = DocStatus.EnCours;
                }                

                viewModel.ItemRows.Add(row);
                this.viewModel.Item.Details = viewModel.ItemRows.ToList();
            }
            else
            {
              //  row.QUANTITE += 1;
            }

            viewModel.Item.TOTAL_TTC = viewModel.ItemRows.Sum(e => e.MT_TTC * e.QUANTITE);
            row.Index = viewModel.ItemRows.Count();

            UpdatePeseeInfos();
            */
        }



        async Task<bool> AddScanedProduct(string cb_prod)
        {
            return false;
            /*
            // Cas prdouit déjà ajouté
            var row = viewModel.ItemRows.Where(e => e.CODE_BARRE == cb_prod).FirstOrDefault();
            if (row != null)
            {
                row.QUANTITE += 1;
                return true;
            }

            // Cas prdouit pas déjà ajouté
            List<View_STK_PRODUITS> prods = await CrudManager.Products.SelectByCodeBarre(cb_prod);

            if (prods.Count > 0)
            {
                await UserDialogs.Instance.AlertAsync("Plusieurs produits pour ce code barre!", AppResources.alrt_msg_Alert, AppResources.alrt_msg_Ok);
                return false;
            }
            else if (prods.Count == 0)
            {
                await UserDialogs.Instance.AlertAsync("Aucun produit pour ce code barre!", AppResources.alrt_msg_Alert, AppResources.alrt_msg_Ok);
                return false;
            }

            AddNewRow(prods[0]);
            return true;
            */
        }

        private void UpdatePeseeInfos()
        {

        }

        #endregion

        #region Selectors

        private ProductSelector itemSelector;
        private async void RowSelect_Clicked(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(viewModel?.Item?.CODE_DOC))
            {
                await UserDialogs.Instance.AlertAsync("Vous devez valider l'en-têtes avant de pouvoir ajouter des produits !", AppResources.alrt_msg_Alert,
    AppResources.alrt_msg_Ok);

                return;
            }
            itemSelector.CodeTiers = viewModel?.Item?.CODE_TIERS;
            await PopupNavigation.Instance.PushAsync(itemSelector);

        }

        private TiersSelector TiersSelector;
        private async void btn_Select_Clicked(object sender, EventArgs e)
        {
            TiersSelector.SearchedType = "CF";
            await PopupNavigation.Instance.PushAsync(TiersSelector);
        }

        private EmballageSelector EmballageSelector;
        public string CurrentStream = Guid.NewGuid().ToString();
        public static View_PRD_AGRICULTURE_DETAIL currentRow;
        private async void Btn_SelectCaiss_Clicked(object sender, EventArgs e)
        {
            currentRow = (sender as Button).BindingContext as View_PRD_AGRICULTURE_DETAIL;
            EmballageSelector.CurrentStream = CurrentStream;
            EmballageSelector.IS_PRINCIPAL = true;
            EmballageSelector.IS_SALES = false;
            await PopupNavigation.Instance.PushAsync(EmballageSelector);

            EmballageSelector.CurrentEmballages = currentRow.Embalages;
        }

        private AnnexTiersSelector AnnexTiersSelector;
        private async void Btn_SelectExtraUser_Clicked(object sender, EventArgs e)
        {
            AnnexTiersSelector = new AnnexTiersSelector();
            currentRow = (sender as Button).BindingContext as View_PRD_AGRICULTURE_DETAIL;
            AnnexTiersSelector.CODE_DOC = currentRow.CODE_DOC_RECEPTION;
            AnnexTiersSelector.IS_ACHAT = false;
            AnnexTiersSelector.TotalQteProduite = currentRow.QTE_DETAIL_PRODUITE;
            AnnexTiersSelector.CurrentAnnex = currentRow.ANNEX_USERS;

            await PopupNavigation.Instance.PushAsync(AnnexTiersSelector);
        }

        private void btn_SelectImmat_Clicked(object sender, EventArgs e)
        {

        }
        #endregion 

        #region Events
        
        private void ne_QteNetChanged(object sender, ValueEventArgs e)
        {
            if (!viewModel.hasEditDetails)
                return;

            decimal QNet = Convert.ToDecimal(e.Value);

            // Recupération de de l'objet en cours
            View_ACH_DOCUMENT_DETAIL currentItem = (sender as SfNumericTextBox).BindingContext as View_ACH_DOCUMENT_DETAIL;

            // Recupération de l'element qte brute de la ligne en cours
            string ClassId = string.Format("pb_{0}", currentItem.CLEANED_CODE_DOC);
            SfNumericTextBox pbruteElem = ((sender as SfNumericTextBox).Parent as Grid).Children.Where(x => x.ClassId == ClassId).FirstOrDefault() as SfNumericTextBox;

            if (QNet > 0 && currentItem.QTE_BRUTE == 0)
            {
                currentItem.Edited_BY_QteNet = true;

                if (pbruteElem != null)
                {
                    pbruteElem.IsEnabled = false;
                }
            }
            else
            {
                if (pbruteElem != null && QNet == 0)
                {
                    pbruteElem.IsEnabled = true;
                    currentItem.Edited_BY_QteNet = null;
                }
            }

            if (Math.Round(currentItem.QUANTITE, 2) != Math.Round(QNet, 2) || currentItem.Edited_BY_QteNet == true)
            {
                currentItem.QUANTITE = QNet;
                UpdatePeseeInfos();
            }

            /*
            // Recupération de l'element qte brute de la ligne en cours
            string ClassId = string.Format("pb_{0}", currentItem.CLEANED_CODE_DOC);
            SfNumericTextBox pbruteElem = ((sender as SfNumericTextBox).Parent as Grid).Children.Where(x => x.ClassId == ClassId).FirstOrDefault() as SfNumericTextBox;

            if (QNet > 0  && currentItem.QTE_BRUTE == 0)
            {
                currentItem.Edited_BY_QteNet = true;

                if(pbruteElem != null)
                {
                    pbruteElem.IsEnabled = false;
                }
            }
            else
            {
                if (pbruteElem != null && QNet == 0)
                {
                    pbruteElem.IsEnabled = true;
                    currentItem.Edited_BY_QteNet = null;
                }
            }

            if (currentItem.QUANTITE != QNet)
            {
                UpdatePeseeInfos();
            }
            currentItem.QUANTITE = QNet;
            */

            // (sender as SfNumericTextBox).ValueChanged -= ne_QteNetChanged;
            // pbruteElem.ValueChanged -= ne_PeseeBruteChanged;

            // pbruteElem.ValueChanged += ne_PeseeBruteChanged;
            // (sender as SfNumericTextBox).ValueChanged += ne_QteNetChanged;

        }

        private void ne_PeseeBruteChanged(object sender, ValueEventArgs e)
        {

            if (!viewModel.hasEditDetails)
                return;

            decimal QBrute = Convert.ToDecimal(e.Value);

            View_ACH_DOCUMENT_DETAIL currentItem = (sender as SfNumericTextBox).BindingContext as View_ACH_DOCUMENT_DETAIL;

            // Recupération de l'element qte brute de la ligne en cours
            string ClassId = string.Format("pn_{0}", currentItem.CLEANED_CODE_DOC);
            SfNumericTextBox pnetElem = ((sender as SfNumericTextBox).Parent as Grid).Children.Where(x => x.ClassId == ClassId).FirstOrDefault() as SfNumericTextBox;

            if (QBrute > 0 && currentItem.QUANTITE == 0)
            {
                currentItem.Edited_BY_QteNet = false;

                if (pnetElem != null)
                {
                    pnetElem.IsEnabled = false;
                }
            }
            else
            {
                if (pnetElem != null && QBrute == 0)
                {
                    pnetElem.IsEnabled = true;
                    currentItem.Edited_BY_QteNet = null;
                }
            }

            if (Math.Round(currentItem.QTE_BRUTE, 2) != Math.Round(QBrute, 2) || currentItem.Edited_BY_QteNet == false)
            {
                currentItem.QTE_BRUTE = QBrute;
                UpdatePeseeInfos();
            }

            /*
            if (currentItem.Edited_BY_QteNet == true)
            {
                currentItem.QTE_BRUTE = QBrute;
                return;
            }

            // Recupération de l'element qte brute de la ligne en cours
            string ClassId = string.Format("pn_{0}", currentItem.CLEANED_CODE_DOC);
            SfNumericTextBox pnetElem = ((sender as SfNumericTextBox).Parent as Grid).Children.Where(x => x.ClassId == ClassId).FirstOrDefault() as SfNumericTextBox;

            if (QBrute > 0 && currentItem.QTE_BRUTE == 0)
            {
                currentItem.Edited_BY_QteNet = false;

                if (pnetElem != null)
                {
                    pnetElem.IsEnabled = false;
                }
            }
            else
            {
                if (pnetElem != null && QBrute == 0)
                {
                    pnetElem.IsEnabled = true;
                    currentItem.Edited_BY_QteNet = null;
                }
            }

            if (currentItem.QTE_BRUTE != QBrute)
            {
                 UpdatePeseeInfos();
            }
            currentItem.QTE_BRUTE = QBrute;
            */

            /*
            (sender as SfNumericTextBox).ValueChanged -= ne_PeseeBruteChanged;
            pnetElem.ValueChanged -= ne_QteNetChanged;



            pnetElem.ValueChanged += ne_QteNetChanged;
            (sender as SfNumericTextBox).ValueChanged += ne_PeseeBruteChanged;
            */
        }

        private void ne_tauxDechetsChanged(object sender, ValueEventArgs e)
        {
            UpdatePeseeInfos();
        }

        private void ne_QTE_PRODUITE_Changed(object sender, ValueEventArgs e)
        {
            foreach (var item in viewModel.ItemRows)
            {
                if(viewModel.Item.TOTAL_QTE_APPORT > 0)
                { 
                    var qtePercent = (item.QTE_APPORT * 100) / viewModel.Item.TOTAL_QTE_APPORT;
                    decimal qteProd = Convert.ToDecimal(e.Value);
                    item.QTE_DETAIL_PRODUITE = (qtePercent * qteProd) / 100;
                }
            }
            // UpdatePeseeInfos();
        }

        private void ne_Pesee_sortie_Changed(object sender, ValueEventArgs e)
        {
            UpdatePeseeInfos();
        }

        private void RowScan_Clicked(object sender, EventArgs e)
        {
            var scaner = new ZXingScannerPage();
            Navigation.PushAsync(scaner);
            scaner.OnScanResult += (result) =>
            {
                scaner.IsScanning = false;
                Device.BeginInvokeOnMainThread(async () =>
                {
                    await Navigation.PopAsync();

                    await AddScanedProduct(result.Text);

                });
            };
        }

        private void HeaderSettings_Clicked(object sender, EventArgs e)
        {
            pnl_Header.IsVisible = !pnl_Header.IsVisible;
        }

        private async void Btn_Delete_Clicked(object sender, EventArgs e)
        {
            View_PRD_AGRICULTURE_DETAIL vteD = (sender as Button).BindingContext as View_PRD_AGRICULTURE_DETAIL;

            if (vteD != null)
            {
                if (await UserDialogs.Instance.ConfirmAsync(AppResources.txt_ConfimDelProductCmd, AppResources.msg_Confirmation, AppResources.alrt_msg_Ok, AppResources.alrt_msg_Cancel))
                {
                    int index = viewModel.ItemRows.IndexOf(vteD);
                    viewModel.ItemRows.Remove(vteD);
                    if(viewModel.Item?.Details?.Count - 1 >= index)
                    { 
                        viewModel.Item.Details.RemoveAt(index);
                    }
                }
            }
        }

        private async void cmd_Terminate_Clicked(object sender, EventArgs e)
        {
            if (viewModel.IsBusy == true)
            {
                return;
            }

            if (viewModel.ItemRows.Count == 0)
            {
                await UserDialogs.Instance.AlertAsync("Veuillez saisir les détails de la production!", AppResources.alrt_msg_Alert, AppResources.alrt_msg_Ok);
                return;
            }

            try
             {
                 UserDialogs.Instance.ShowLoading("Traitement en cours ...", MaskType.Black);

                 viewModel.IsBusy = true;
                string nextSatus = "";
                if (viewModel.Item.STATUS_DOC == DocProdStatus.PrdEnAttente)
                    nextSatus = DocProdStatus.PrdEnCours;
                else if (viewModel.Item.STATUS_DOC == DocProdStatus.PrdEnCours)
                    nextSatus = DocProdStatus.PrdTermine;
                /*
                else if (viewModel.Item.STATUS_DOC == DocProdStatus.PrdTermine)
                    nextSatus = DocProdStatus.PrdLivre;
                */
                bool result = await WebServiceClient.UpdateStatus(viewModel.Item.CODE_DOC, nextSatus);

                if (result)
                {
                    await UserDialogs.Instance.AlertAsync("L'état de la production a bien été modifié!", AppResources.alrt_msg_Alert, AppResources.alrt_msg_Ok);
                }

                await Navigation.PopAsync();
            }
            catch(Exception ex)
            {
                await UserDialogs.Instance.AlertAsync(WSApi2.GetExceptionMessage(ex), AppResources.alrt_msg_Alert,
                    AppResources.alrt_msg_Ok);
            }
            finally
            {
                 viewModel.IsBusy = false;
                 UserDialogs.Instance.HideLoading();
            }
        }

        private async void cmd_Buy_Clicked(object sender, EventArgs e)
        {
            btn_RowSelect.Focus();

            if (viewModel.IsBusy == true)
            {
                return;
            }

            try
            {
                if(string.IsNullOrEmpty(viewModel.Item.CODE_TIERS))
                { 
                    await UserDialogs.Instance.AlertAsync("Veuillez sélectionner un tiers!", AppResources.alrt_msg_Alert, AppResources.alrt_msg_Ok);
                    return;
                }
                this.viewModel.Item.Details = viewModel.ItemRows.ToList();
                // this.viewModel.Item.CODE_MOTIF = "ES10";
                // this.viewModel.Item.CODE_MAGASIN = parames.DEFAULT_ACHATS_MAGASIN;
                // this.viewModel.Item.CODE_UNITE = parames.DEFAULT_UNITE_ACHATS;

                if (string.IsNullOrEmpty(viewModel.Item.CODE_DOC)) // Ajout d'une reception
                {
                    try
                    { 
                        UserDialogs.Instance.ShowLoading("Traitement en cours ...", MaskType.Black);

                        viewModel.IsBusy = true;
                        await CrudManager.Productions.AddItemAsync(viewModel.Item);
                           /*
                        UserDialogs.Instance.ActionSheet(new ActionSheetConfig()
                            .SetTitle("Choose Type")
                            .Add("Default", null, "Icon.png")
                            .Add("E-Mail", null, "addToCart24.png")
                        );
                           */ 
                        await UserDialogs.Instance.AlertAsync(AppResources.txt_Cat_DocumentSaved, AppResources.alrt_msg_Alert, AppResources.alrt_msg_Ok);
                     
                    }
                    finally
                    {
                        App.CurrentSales = null;
                        viewModel.IsBusy = false;
                        UserDialogs.Instance.HideLoading();
                    }
                }
                else // Modification d'une reception
                {
                    try
                    {
                        UserDialogs.Instance.ShowLoading("Traitement en cours ...", MaskType.Black);

                        viewModel.IsBusy = true;
                        await CrudManager.Productions.UpdateItemAsync(viewModel.Item);

                        await UserDialogs.Instance.AlertAsync(AppResources.txt_Cat_DocumentUpdated, AppResources.alrt_msg_Alert, AppResources.alrt_msg_Ok);
                    }
                    finally
                    {
                        viewModel.IsBusy = false;
                        UserDialogs.Instance.HideLoading();
                    }
                }

                await Navigation.PopAsync();
                /*
                int pageCount = Navigation.NavigationStack.Count;
                if(pageCount > 0)
                { 
                   // await (Application.Current.MainPage as XpertMobileApp.Views.MainPage).NavigateFromMenu(3);
                }
                */
            }
            catch (Exception ex)
            {
                viewModel.IsBusy = false;
                await UserDialogs.Instance.AlertAsync(WSApi2.GetExceptionMessage(ex), AppResources.alrt_msg_Alert,
                    AppResources.alrt_msg_Ok);
            }
        }


        #endregion

        private async void btn_SaveQteProduite_Clicked(object sender, EventArgs e)
        {
            bool result = false;

            if (string.IsNullOrEmpty(viewModel.Item.CODE_DOC))
            {
                await UserDialogs.Instance.AlertAsync("Veuillez enregistrer le document avant d'introduire la quantité produite!", AppResources.alrt_msg_Alert, AppResources.alrt_msg_Ok);
                return;
            }

            if (IsBusy)
                return;

            IsBusy = true;

            try
            {
                UserDialogs.Instance.ShowLoading(AppResources.txt_Loading);

                result = await WebServiceClient.SaveQteProduite(viewModel.Item.CODE_DOC, viewModel.Item.QTE_PRODUITE);
                if (result)
                {
                    await UserDialogs.Instance.AlertAsync("La quantité produite a bien été enregistré!", AppResources.alrt_msg_Alert, AppResources.alrt_msg_Ok);
                }
            }
            catch (Exception ex)
            {
                await UserDialogs.Instance.AlertAsync(WSApi2.GetExceptionMessage(ex), AppResources.alrt_msg_Alert,
                    AppResources.alrt_msg_Ok);
            }
            finally
            {
                UserDialogs.Instance.HideLoading();
                IsBusy = false;
            }
        }

        private async void SaveEmballages(View_PRD_AGRICULTURE_DETAIL detail, List<View_BSE_EMBALLAGE> Embalages)
        {

            bool result = false;

            if (string.IsNullOrEmpty(viewModel.Item.CODE_DOC))
            {
                await UserDialogs.Instance.AlertAsync("Veuillez enregistrer le document avant d'introduire la quantité produite!", AppResources.alrt_msg_Alert, AppResources.alrt_msg_Ok);
                return;
            }

            if (IsBusy)
                return;

            IsBusy = true;

            try
            {
                UserDialogs.Instance.ShowLoading(AppResources.txt_Loading);

                result = await WebServiceClient.SaveProdEmballages(Embalages, detail.CODE_DOC_DETAIL);
                if (result)
                {
                    await UserDialogs.Instance.AlertAsync("Les emballages ont bien été enregistrés!", AppResources.alrt_msg_Alert, AppResources.alrt_msg_Ok);
                }
            }
            catch (Exception ex)
            {
                await UserDialogs.Instance.AlertAsync(WSApi2.GetExceptionMessage(ex), AppResources.alrt_msg_Alert,
                    AppResources.alrt_msg_Ok);
            }
            finally
            {
                UserDialogs.Instance.HideLoading();
                IsBusy = false;
            }
        }

        private async void Btn_PrintRecept_Clicked(object sender, EventArgs e)
        {
            bool result = false;

            string codeDocRecept = ((sender as Button).BindingContext as View_PRD_AGRICULTURE_DETAIL).CODE_DOC_RECEPTION; 

            if (IsBusy)
                return;

            IsBusy = true;

            try
            {
                UserDialogs.Instance.ShowLoading(AppResources.txt_Loading);

                //result = await WebServiceClient.PrintQRProduit(codeDocRecept, 1, "Godex DT2x");
                result = await XpertHelper.PrintQrCode(codeDocRecept, 1);
            }
            catch (Exception ex)
            {
                await UserDialogs.Instance.AlertAsync(WSApi2.GetExceptionMessage(ex), AppResources.alrt_msg_Alert,
                    AppResources.alrt_msg_Ok);
            }
            finally
            {
                UserDialogs.Instance.HideLoading();
                IsBusy = false;
            }
        }

        private async void btn_Livrer_Clicked(object sender, EventArgs e)
        {
            bool result = false;

            Button btn = (sender as Button);

            string codeDocRecept = (btn.BindingContext as View_PRD_AGRICULTURE_DETAIL).CODE_DOC_RECEPTION;
            string codeDocDetail = (btn.BindingContext as View_PRD_AGRICULTURE_DETAIL).CODE_DOC_DETAIL;

            if (IsBusy)
                return;

            IsBusy = true;

            try
            {
                UserDialogs.Instance.ShowLoading(AppResources.txt_Loading);

                result = await WebServiceClient.LivrerProduction(codeDocRecept, codeDocDetail);

                if (result)
                {
                    btn.IsVisible = false;
                   // await UserDialogs.Instance.AlertAsync("La quantité produite a bien été enregistré!", AppResources.alrt_msg_Alert, AppResources.alrt_msg_Ok);
                }
            }
            catch (Exception ex)
            {
                await UserDialogs.Instance.AlertAsync(WSApi2.GetExceptionMessage(ex), AppResources.alrt_msg_Alert,
                    AppResources.alrt_msg_Ok);
            }
            finally
            {
                UserDialogs.Instance.HideLoading();
                IsBusy = false;
            }
        }

        Stream GetStreamFromFile(string filename)
        {
            var assembly = typeof(App).GetTypeInfo().Assembly;
            var stream = assembly.GetManifestResourceStream("XpertMobileApp." + filename);
            return stream;
        }

        private async Task VerifiProd(string scanedText, string refCodeDoc)
        {
            try
            {
                string[] str = scanedText.Split('-');
                if (str.Count() >= 2)
                {
                    string CodeDoc = str[0];
                    if (!string.IsNullOrEmpty(refCodeDoc) && refCodeDoc.Equals(CodeDoc))
                    {

                        try
                        {
                            ISimpleAudioPlayer player = Plugin.SimpleAudioPlayer.CrossSimpleAudioPlayer.Current;
                            player.Load(GetStreamFromFile("beep07.mp3"));
                            player.Play();
                        }
                        catch { }

                        await UserDialogs.Instance.AlertAsync("L'emballage est correct!", AppResources.alrt_msg_Alert,
                                 AppResources.alrt_msg_Ok);
                    }
                    else
                    {
                        try
                        {
                            ISimpleAudioPlayer player = Plugin.SimpleAudioPlayer.CrossSimpleAudioPlayer.Current;
                            player.Load(GetStreamFromFile("beep03.mp3"));
                            player.Play();
                        }
                        catch { }

                        await UserDialogs.Instance.AlertAsync("L'emballage n'appartient pas a cet ordre de production!", AppResources.alrt_msg_Alert, AppResources.alrt_msg_Ok);
                    }
                }
            }
            catch (Exception ex)
            {
                await UserDialogs.Instance.AlertAsync(WSApi2.GetExceptionMessage(ex), AppResources.alrt_msg_Alert,
    AppResources.alrt_msg_Ok);
            }
        }

        private void Btn_Btn_VerifProd_Clicked(object sender, EventArgs e)
        {
            Button btn = (sender as Button);

            var scaner = new ZXingScannerPage();
            scaner.Title = "Vérification emballage";
            
            Navigation.PushAsync(scaner);
            scaner.OnScanResult += (result) =>
            {
                scaner.IsScanning = false;
                Device.BeginInvokeOnMainThread(async () =>
                {
                    string codeDocRecept = (btn.BindingContext as View_PRD_AGRICULTURE_DETAIL).CODE_DOC_RECEPTION;

                    VerifiProd(result.Text, codeDocRecept);

                  //  await Navigation.PopAsync();
                });
            };
        }
    }
}