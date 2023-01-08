using Acr.UserDialogs;
using Rg.Plugins.Popup.Services;
using Syncfusion.SfNumericTextBox.XForms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Xpert.Common.WSClient.Helpers;
using XpertMobileApp.Api;
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
    public partial class AchatFormPageAbattoire : ContentPage
    {
        AchatsViewModelAbattoire viewModel;
        public string CurrentStream = Guid.NewGuid().ToString();
        SYS_MOBILE_PARAMETRE parames;
        List<SYS_OBJET_PERMISSION> permissions;
        string veterinaryNote;
        public int veterinaryValidation = -1;
        List<PRESTATION_REJECTED> ListPrestationRejected = null;


        public Command AddItemCommand { get; set; }

        public AchatFormPageAbattoire(View_ACH_DOCUMENT vente, string typeDoc, string motif)
        {
            InitializeComponent();

            // NavigationPage.SetHasNavigationBar(this, false);

            lbl_IS_Individuel.IsVisible = motif == AchRecMotifs.PesageForProduction;
            tgl_IS_Individuel.IsVisible = motif == AchRecMotifs.PesageForProduction;
            stk_lbl_NOTE.IsVisible = motif == AchRecMotifs.PesageForProduction;
            lbl_NOTE.IsVisible = motif == AchRecMotifs.PesageForProduction;

            itemSelector = new ProductSelector();
            TiersSelector = new TiersSelector(CurrentStream);
            EmballageSelector = new EmballageSelector();

            var ach = vente == null ? new View_ACH_DOCUMENT() : vente;
            if (vente == null)
            {
                ach.TYPE_DOC = typeDoc;
                ach.CODE_MOTIF = motif;
                ach.DATE_DOC = DateTime.Now.Date;

                ne_PESEE_ENTREE_Label.IsVisible = false;
                ne_PESEE_ENTREE_Layout.IsVisible = false;

                //ne_PESEE_SORTIE_Label.IsVisible = false;
                //ne_PESEE_SORTIE_Layout.IsVisible = false;
                lbl_NOTE.IsVisible = false;
                stk_lbl_NOTE.IsVisible = false;
                lbl_VETERINARY.IsVisible = false;
                cmd_VETERINARY.IsVisible = false;
            }

            if (Apps.XCOM_Abattoir == Constants.AppName)
            {
                lbl_IS_Individuel.IsVisible = false;
                tgl_IS_Individuel.IsVisible = false;
                //var dataTemplate = ItemsListView.ItemTemplate;
                var items = ItemsListView.ItemTemplate.Values;
            }

            BindingContext = this.viewModel = new AchatsViewModelAbattoire(ach, ach?.CODE_DOC);

            // jobFieldAutoComplete.BindingContext = viewModel;

            this.viewModel.Title = string.IsNullOrEmpty(ach.CODE_DOC) ? AppResources.pn_NewPurchase : ach?.ToString();

            this.viewModel.LoadRowsCommand = new Command(async () => await ExecuteLoadRowsCommand());

            // viewModel.ItemRows.CollectionChanged += ItemsRowsChanged;

            MessagingCenter.Subscribe<ProductSelector, View_STK_PRODUITS>(this, MCDico.ITEM_SELECTED, async (obj, selectedItem) =>
            {
                Device.BeginInvokeOnMainThread(() =>
                {
                    this.AddNewRow(selectedItem);
                });
            });

            MessagingCenter.Subscribe<TiersSelector, View_TRS_TIERS>(this, CurrentStream, async (obj, selectedItem) =>
            {
                Device.BeginInvokeOnMainThread(() =>
                {
                    viewModel.SelectedTiers = selectedItem;
                    viewModel.Item.CODE_TIERS = selectedItem.CODE_TIERS;
                    viewModel.Item.TIERS_NomC = selectedItem.NOM_TIERS1;
                });
            });

            MessagingCenter.Subscribe<AnnexTiersSelector, List<VIEW_ACH_INFO_ANEX>>(this, MCDico.ITEM_SELECTED, async (obj, items) =>
            {
                Device.BeginInvokeOnMainThread(() =>
                {
                    try
                    {
                        if (items != null && items.Count > 0)
                        {
                            currentRow.ANNEX_USERS = items;
                            currentRow.QUANTITE = currentRow.QTE_RECUE = items.Sum(x => x.QUANTITE_APPORT);
                        }
                    }
                    catch (Exception ex)
                    {
                        UserDialogs.Instance.AlertAsync(WSApi2.GetExceptionMessage(ex), AppResources.alrt_msg_Alert,
    AppResources.alrt_msg_Ok);
                    }
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
                        var embs = items.Where(e => e.QUANTITE_ENTREE != 0 || e.QUANTITE_SORTIE != 0).ToList();
                        var embalages = new List<View_BSE_EMBALLAGE>();
                        if (embs != null && embs.Count > 0)
                        {
                            foreach (var item in embs)
                            {
                                embalages.Add(XpertHelper.CloneObject<View_BSE_EMBALLAGE>(item));
                            }
                        }
                        currentRow.Embalages = embalages;

                        UpdatePeseeInfos();
                    }
                    catch (Exception ex)
                    {
                        UserDialogs.Instance.AlertAsync(WSApi2.GetExceptionMessage(ex), AppResources.alrt_msg_Alert,
    AppResources.alrt_msg_Ok);
                    }
                });
            });

            MessagingCenter.Subscribe<ProductSelector, View_STK_PRODUITS>(this, MCDico.REMOVE_ITEM, async (obj, selectedItem) =>
            {
                Device.BeginInvokeOnMainThread(() =>
                {
                    this.RemoveNewRow(selectedItem);
                });
            });

            MessagingCenter.Subscribe<VeterinaryPopup, string>(this, "VeterinaryPopup", async (obj, selectedItem) =>
            {
                if (XpertHelper.IsNotNullAndNotEmpty(selectedItem))
                {
                    veterinaryNote = selectedItem;
                    viewModel.Item.NOTE_DOC = veterinaryNote;
                }

            });
            MessagingCenter.Subscribe<VeterinaryAddRejectedPopup, string>(this, "VeterinaryPopup", async (obj, selectedItem) =>
            {
                if (XpertHelper.IsNotNullAndNotEmpty(selectedItem))
                {
                    veterinaryNote = selectedItem;
                    viewModel.Item.NOTE_DOC = veterinaryNote;
                }

            });

            if (viewModel.Item != null && !string.IsNullOrEmpty(this.viewModel.Item.CODE_DOC))
            {
                viewModel.LoadRowsCommand.Execute(null);
            }
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();

            parames = await AppManager.GetSysParams();
            permissions = await AppManager.GetPermissions();

            viewModel.ImmatriculationList = await GetImmatriculations("");

            if (!AppManager.HasAdmin)
            {
                ApplyVisibility();
            }
            else
            {
                if ((viewModel.Item.STATUS_DOC == DocStatus.EnCours || viewModel.Item.STATUS_DOC == DocStatus.EnAttente) && viewModel.Item.PESEE_ENTREE == 0)
                {
                    cmd_Terminate.IsVisible = true;
                }
            }

            // ne_PESEE_ENTREE.IsEnabled = string.IsNullOrEmpty(this.viewModel.Item.CODE_DOC);
            // ne_PESEE_SORTIE.IsEnabled = !string.IsNullOrEmpty(this.viewModel.Item.CODE_DOC);
        }

        private void ApplyVisibility()
        {
            //btn_Immatriculation.IsEnabled = false;
            cmd_Terminate.IsVisible = false;
            string userGroup = App.User.GroupName;

            if (Constants.AppName == Apps.XCOM_Abattoir)
            {
                ne_PESEE_SORTIE_Label.IsVisible = false;
                ne_PESEE_SORTIE_Layout.IsVisible = false;
            }

            // Par défaut le header est caché s'il n'a pas le droit d'éditer le header
            pnl_Header.IsVisible = viewModel.hasEditHeader;

            // Commandes pour la selection de produit
            btn_RowSelect.IsEnabled = viewModel.hasEditDetails;
            btn_RowScan.IsEnabled = viewModel.hasEditDetails;

            // Champs d'edition du header
            dp_EcheanceDate.IsEnabled = viewModel.hasEditHeader;
            btn_TeirsSearch.IsEnabled = viewModel.hasEditHeader;
            jobFieldAutoComplete.IsEnabled = viewModel.hasEditHeader;
            ne_PESEE_ENTREE.IsEnabled = viewModel.hasEditHeader;
            ne_PESEE_SORTIE.IsEnabled = viewModel.hasEditHeader;

            btn_Get_PESEE_ENTREE.IsEnabled = viewModel.hasEditHeader;
            btn_Get_PESEE_SORTIE.IsEnabled = viewModel.hasEditHeader;

            if (string.IsNullOrEmpty(viewModel.Item.STATUS_DOC))
            {
                ne_PESEE_SORTIE.IsEnabled = false;
                btn_Get_PESEE_SORTIE.IsEnabled = false;

                btn_RowSelect.IsEnabled = false;
                btn_RowScan.IsEnabled = false;
            }
            else if (viewModel.Item.STATUS_DOC == DocStatus.EnAttente)
            {
                dp_EcheanceDate.IsEnabled = false;
                btn_TeirsSearch.IsEnabled = false;
                jobFieldAutoComplete.IsEnabled = false;
                ne_PESEE_ENTREE.IsEnabled = false;
                //ne_PESEE_SORTIE.IsEnabled = false;
                btn_Get_PESEE_ENTREE.IsEnabled = false;
                btn_Get_PESEE_SORTIE.IsEnabled = false;
                if (viewModel.Item.PESEE_ENTREE == 0 && viewModel.hasEditDetails)
                {
                    cmd_Terminate.IsVisible = true;
                }
            }
            else if (viewModel.Item.STATUS_DOC == DocStatus.EnCours)
            {
                dp_EcheanceDate.IsEnabled = false;
                btn_TeirsSearch.IsEnabled = false;
                jobFieldAutoComplete.IsEnabled = false;
                ne_PESEE_ENTREE.IsEnabled = false;
                btn_Get_PESEE_ENTREE.IsEnabled = false;

                if (Constants.AppName == Apps.XCOM_Abattoir)
                {
                    ne_PESEE_ENTREE.IsEnabled = true;
                    btn_Get_PESEE_ENTREE.IsEnabled = true;
                }

                if (viewModel.Item.PESEE_ENTREE == 0 && viewModel.hasEditDetails)
                {
                    cmd_Terminate.IsVisible = true;
                }
            }
            else if (viewModel.Item.STATUS_DOC == DocStatus.Terminer || viewModel.Item.STATUS_DOC == DocStatus.Cloturee)
            {
                dp_EcheanceDate.IsEnabled = false;
                btn_TeirsSearch.IsEnabled = false;
                jobFieldAutoComplete.IsEnabled = false;
                ne_PESEE_ENTREE.IsEnabled = false;
                ne_PESEE_SORTIE.IsEnabled = false;

                btn_Get_PESEE_ENTREE.IsEnabled = false;
                btn_Get_PESEE_SORTIE.IsEnabled = false;

                btn_RowSelect.IsEnabled = false;
                btn_RowScan.IsEnabled = false;
            }
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
                var itemsC = await WebServiceClient.GetAchatsDetails(this.viewModel.Item.CODE_DOC);

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

        private void RemoveNewRow(View_STK_PRODUITS product)
        {
            var row = viewModel.ItemRows.Where(e => e.CODE_PRODUIT == product.CODE_PRODUIT).FirstOrDefault();
            if (row == null) return;

            if (row.QUANTITE > 0)
            {
                row.QUANTITE -= 1;
            }
            else
            {
                viewModel.ItemRows.Remove(row);
            }
        }

        private void AddNewRow(View_STK_PRODUITS product)
        {
            var row = viewModel.ItemRows.Where(e => e.CODE_PRODUIT == product.CODE_PRODUIT).FirstOrDefault();

            if (row == null)
            {
                row = new View_ACH_DOCUMENT_DETAIL();
                row.Parent_Doc = viewModel.Item;
                row.CODE_DOC = viewModel.Item.CODE_DOC;

                row.CODE_PRODUIT = product.CODE_PRODUIT;
                row.CODE_BARRE = product.CODE_BARRE;
                row.DESIGNATION_PRODUIT = product.DESIGNATION_PRODUIT;
                // row.UNITE
                row.TAUX_DECHET = product.TAUX_DECHET;
                row.CODE_MOTIF = viewModel.Item.CODE_MOTIF;
                if (viewModel.Item.CODE_MOTIF == AchRecMotifs.PesageForProduction)
                {
                    row.PRIX_UNITAIRE = 0;
                }
                else
                {
                    row.PRIX_UNITAIRE = product.PRIX_ACHAT_HT;
                }

                row.PRIX_VENTE = product.PRIX_VENTE_HT;

                row.CODE_MAGASIN = parames?.DEFAULT_ACHATS_MAGASIN;
                row.LOT = parames?.DEFAULT_COMPAGNE_LOT;
                row.UNITE = viewModel.Item.CODE_UNITE;
                row.CODE_UNITE_ENTETE = viewModel.Item.CODE_UNITE;

                if (viewModel.ItemRows.Count == 0 && viewModel.Item.PESEE_ENTREE > 0)
                {
                    row.IS_PRINCIPAL = true;
                    row.SetPeseeBrute(viewModel.Item.PESEE_BRUTE);
                }

                if (viewModel.ItemRows.Count == 0)
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
            viewModel.Item.TOTAL_HT = viewModel.Item.TOTAL_TTC;
            row.Index = viewModel.ItemRows.Count();

            UpdatePeseeInfos();
        }

        private void ne_PeseeTTCChanged(object sender, ValueEventArgs e)
        {
            foreach (var item in viewModel.ItemRows.ToList())
            {
                item.MT_TTC = item.QUANTITE * item.PRIX_UNITAIRE;
                item.MT_HT = item.MT_TTC;
            }
            viewModel.Item.TOTAL_TTC = viewModel.ItemRows.Sum(x => x.MT_TTC);
            viewModel.Item.TOTAL_HT = viewModel.Item.TOTAL_TTC;
        }

        async Task<bool> AddScanedProduct(string cb_prod)
        {
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
        }

        private void UpdatePeseeInfos()
        {
            try
            {
                var principalItem = viewModel.ItemRows.ToList().Find(x => x.IS_PRINCIPAL == true);
                decimal qteNetPrimaireInitial = 0;

                // Calcul des infos de l'item principal
                if (principalItem != null)
                {
                    // 0 - Calcul du brute initial => celui du camions 
                    qteNetPrimaireInitial = viewModel.Item.PESEE_ENTREE - viewModel.Item.PESEE_SORTIE;
                    if (principalItem.Embalages != null)
                    {
                        qteNetPrimaireInitial = qteNetPrimaireInitial - principalItem.Embalages.Sum(x => x.QTE_DEFF * x.QUANTITE_UNITE);
                    }

                    // 1 - Calcul de la quantité net primaire du produit principal
                    principalItem.SetPeseeBrute(qteNetPrimaireInitial);
                    principalItem.QUANTITE = principalItem.QTE_RECUE = principalItem.QUANTITE_NET_PRIMAIRE = qteNetPrimaireInitial;

                    // 2 - calcul des emballages du l'item principal
                    if (principalItem.Embalages != null)
                    {
                        foreach (var emb in principalItem.Embalages)
                        {
                            emb.QUANTITE_ENTREE_REEL = emb.QUANTITE_ENTREE;
                            foreach (var item in viewModel.ItemRows.ToList())
                            {
                                if (!item.IS_PRINCIPAL && item.Embalages != null)
                                {
                                    var curEmb = item.Embalages.Where(x => x.CODE_EMBALLAGE == emb.CODE_EMBALLAGE).FirstOrDefault();
                                    if (curEmb != null)
                                    {
                                        emb.QUANTITE_ENTREE_REEL = emb.QUANTITE_ENTREE_REEL - curEmb.QUANTITE_ENTREE;
                                    }
                                }
                            }
                        }

                        principalItem.QUANTITE_NET_PRIMAIRE = qteNetPrimaireInitial;
                        principalItem.QUANTITE = principalItem.QTE_RECUE = principalItem.QUANTITE_NET_PRIMAIRE;
                    }
                }

                // Calcul de la quantité net des produits secondaires
                decimal totalQteExeption = 0;
                foreach (var item in viewModel.ItemRows.ToList())
                {
                    if (!item.IS_PRINCIPAL)
                    {

                        decimal totalPoidsEmballage = 0;
                        decimal totalPoidsEmballageVide = 0;
                        decimal totalPoidsEmballagePlein = 0;
                        if (item.Embalages != null)
                        {
                            totalPoidsEmballage = item.Embalages.Sum(e => e.QUANTITE_ENTREE * e.QUANTITE_UNITE);
                            totalPoidsEmballageVide = item.Embalages.Sum(e => e.QUANTITE_VIDE * e.QUANTITE_UNITE);
                            totalPoidsEmballagePlein = totalPoidsEmballage - totalPoidsEmballageVide;
                        }
                        if (item.Edited_BY_QteNet != true)
                        {
                            item.QUANTITE_NET_PRIMAIRE = item.QTE_BRUTE - totalPoidsEmballagePlein;
                            item.QUANTITE_DECHETS = ((item.QUANTITE_NET_PRIMAIRE * item.TAUX_DECHET) / 100);
                            item.QUANTITE = item.QUANTITE_NET_PRIMAIRE - item.QUANTITE_DECHETS;
                            // item.SetQteNet(item.QUANTITE_NET_PRIMAIRE - item.QUANTITE_DECHETS);
                            item.QTE_RECUE = item.QUANTITE;
                            item.MT_TTC = item.QUANTITE * item.PRIX_UNITAIRE;
                            item.MT_HT = item.MT_TTC;
                        }
                        else
                        {
                            item.QTE_RECUE = item.QUANTITE;
                            item.QUANTITE_NET_PRIMAIRE = (100 * item.QUANTITE) / (100 - item.TAUX_DECHET);
                            item.QUANTITE_DECHETS = (item.QUANTITE_NET_PRIMAIRE * item.TAUX_DECHET) / 100;
                            item.QTE_BRUTE = item.QUANTITE_NET_PRIMAIRE + totalPoidsEmballagePlein;
                            //item.SetPeseeBrute(item.QUANTITE_NET_PRIMAIRE + totalPoidsEmballage);
                            item.MT_TTC = item.QUANTITE * item.PRIX_UNITAIRE;
                            item.MT_HT = item.MT_TTC;
                        }

                        totalQteExeption += item.QUANTITE_NET_PRIMAIRE;
                    }
                }

                // calcul des infos de l'item principalt
                if (principalItem != null)
                {
                    // Calcul de la quantité net finale du produit principal

                    decimal NetPrimaireExceptions = viewModel.ItemRows.Where(x => x.IS_PRINCIPAL == false).Sum(x => x.QUANTITE_NET_PRIMAIRE);
                    principalItem.QUANTITE_NET_PRIMAIRE = qteNetPrimaireInitial - NetPrimaireExceptions;
                    principalItem.QUANTITE_DECHETS = ((principalItem.QUANTITE_NET_PRIMAIRE * principalItem.TAUX_DECHET) / 100);
                    principalItem.QUANTITE = principalItem.QUANTITE_NET_PRIMAIRE - principalItem.QUANTITE_DECHETS;
                    principalItem.QTE_RECUE = principalItem.QUANTITE;
                    principalItem.MT_TTC = principalItem.QUANTITE * principalItem.PRIX_UNITAIRE;

                    /* Total poids emballages
                     * ==> les caisse pour le produit principal sont déja calculé dans par la pesée d'entrée et de sortie  */
                    decimal totalPoidsEmballage = 0;
                    if (principalItem.Embalages != null)
                        totalPoidsEmballage = principalItem.Embalages.Sum(e => e.QUANTITE_ENTREE_REEL * e.QUANTITE_UNITE);

                    principalItem.SetPeseeBrute(principalItem.QUANTITE_NET_PRIMAIRE + totalPoidsEmballage);

                }

                // calcul du total du document
                viewModel.Item.TOTAL_TTC = viewModel.ItemRows.Sum(x => x.MT_TTC);
                viewModel.Item.TOTAL_HT = viewModel.Item.TOTAL_TTC;
            }
            catch (Exception ex)
            {
                UserDialogs.Instance.Alert(WSApi2.GetExceptionMessage(ex), AppResources.alrt_msg_Alert,
                     AppResources.alrt_msg_Ok);
            }
        }

        #endregion

        #region Selectors

        private ProductSelector itemSelector;
        private async void RowSelect_Clicked(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(viewModel?.Item?.CODE_DOC))
            {
                await UserDialogs.Instance.AlertAsync("Vous devez valider l'en-têtes avant de pouvoir ajouter des produits !", AppResources.alrt_msg_Alert, AppResources.alrt_msg_Ok);

                return;
            }
            itemSelector.CodeTiers = viewModel?.Item?.CODE_TIERS;
            itemSelector.AutoriserReception = true;
            await PopupNavigation.Instance.PushAsync(itemSelector);
        }

        private TiersSelector TiersSelector;
        private async void btn_Select_Clicked(object sender, EventArgs e)
        {
            TiersSelector.SearchedType = "CF";
            await PopupNavigation.Instance.PushAsync(TiersSelector);
        }

        private EmballageSelector EmballageSelector;
        public static View_ACH_DOCUMENT_DETAIL currentRow;

        private async void Btn_SelectCaiss_Clicked(object sender, EventArgs e)
        {
            currentRow = (sender as Button).BindingContext as View_ACH_DOCUMENT_DETAIL;
            EmballageSelector.CurrentStream = CurrentStream;
            EmballageSelector.IS_PRINCIPAL = currentRow.IS_PRINCIPAL || viewModel.Item.PESEE_ENTREE == 0;
            EmballageSelector.IS_SALES = true;

            await PopupNavigation.Instance.PushAsync(EmballageSelector);

            EmballageSelector.CurrentEmballages = currentRow.Embalages;
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

        private void ne_Pesee_entree_Changed(object sender, ValueEventArgs e)
        {
            UpdatePeseeInfos();
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
            View_ACH_DOCUMENT_DETAIL vteD = (sender as Button).BindingContext as View_ACH_DOCUMENT_DETAIL;

            if (vteD.IS_PRINCIPAL && viewModel.ItemRows.Count > 1)
            {
                await UserDialogs.Instance.AlertAsync("Vous ne pouvez pas supprimer le produit principal tant qu'il y a des produits d'exceptions!", AppResources.alrt_msg_Alert, AppResources.alrt_msg_Ok);
                return;
            }

            if (vteD != null)
            {
                if (await UserDialogs.Instance.ConfirmAsync(AppResources.txt_ConfimDelProductCmd, AppResources.msg_Confirmation, AppResources.alrt_msg_Ok, AppResources.alrt_msg_Cancel))
                {
                    int index = viewModel.ItemRows.IndexOf(vteD);
                    viewModel.ItemRows.Remove(vteD);
                    if (viewModel.Item?.Details?.Count - 1 >= index)
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

            if (viewModel.ItemRows.Count == 0 && (viewModel.Item.STATUS_DOC == DocStatus.EnAttente || viewModel.Item.STATUS_DOC == DocStatus.EnCours))
            {
                await UserDialogs.Instance.AlertAsync("Veuillez sélectionner un produit!", AppResources.alrt_msg_Alert, AppResources.alrt_msg_Ok);
                return;
            }

            try
            {
                UserDialogs.Instance.ShowLoading("Traitement en cours ...", MaskType.Black);

                this.viewModel.Item.Details = viewModel.ItemRows.ToList();
                // this.viewModel.Item.CODE_MOTIF = "ES10";
                this.viewModel.Item.CODE_MAGASIN = parames?.DEFAULT_ACHATS_MAGASIN;
                this.viewModel.Item.CODE_UNITE = parames?.DEFAULT_UNITE_ACHATS;

                viewModel.IsBusy = true;
                viewModel.Item.STATUS_DOC = DocStatus.Terminer;
                await CrudManager.Achats.UpdateItemAsync(viewModel.Item);

                await UserDialogs.Instance.AlertAsync(AppResources.txt_Cat_CommandesUpdated, AppResources.alrt_msg_Alert, AppResources.alrt_msg_Ok);

                await Navigation.PopAsync();
            }
            catch (Exception ex)
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
                if (viewModel.Item.STATUS_DOC == DocStatus.EnCourDeProduction || viewModel.Item.STATUS_DOC == DocStatus.Accepter)
                {
                    await UserDialogs.Instance.AlertAsync("Document " + viewModel.Item.DESIGNATION_STATUS + ", Vous ne pouvez pas le modifier!", AppResources.alrt_msg_Alert, AppResources.alrt_msg_Ok);
                    return;
                }

                if (string.IsNullOrEmpty(viewModel.Item.CODE_TIERS))
                {
                    await UserDialogs.Instance.AlertAsync("Veuillez sélectionner un tiers!", AppResources.alrt_msg_Alert, AppResources.alrt_msg_Ok);
                    return;
                }

                if (string.IsNullOrEmpty(viewModel.Item.IMMATRICULATION))
                {
                    await UserDialogs.Instance.AlertAsync("Veuillez saisir l'immatriculation!", AppResources.alrt_msg_Alert, AppResources.alrt_msg_Ok);
                    return;
                }

                /*
                if (viewModel.Item.PESEE_ENTREE <= 0)
                {
                    await UserDialogs.Instance.AlertAsync("Veuillez saisir la pesée d'entrée!", AppResources.alrt_msg_Alert, AppResources.alrt_msg_Ok);
                    return;
                }
                */
                if (viewModel.Item.PESEE_ENTREE < viewModel.Item.PESEE_SORTIE)
                {
                    await UserDialogs.Instance.AlertAsync("La pesée d'entrée doit être supérieure à la pesée de sortie!", AppResources.alrt_msg_Alert, AppResources.alrt_msg_Ok);
                    return;
                }

                if (viewModel.Item.STATUS_DOC != null)
                    if (veterinaryValidation == -1)
                    {
                        await UserDialogs.Instance.AlertAsync("Veuillez saisir la Validation Véterinaire", AppResources.alrt_msg_Alert, AppResources.alrt_msg_Ok);
                        return;

                    }
                    else if (veterinaryValidation == 2)
                    {
                        viewModel.Item.STATUS_DOC = DocStatus.Accepter;
                        viewModel.Item.List_PRESTATION_REJECTED = ListPrestationRejected;
                    }
                    else
                    {
                        if (veterinaryValidation == 1)
                        {
                            viewModel.Item.STATUS_DOC = DocStatus.Accepter;
                        }
                        else
                        {
                            viewModel.Item.STATUS_DOC = DocStatus.Rejeter;
                        }
                        viewModel.Item.List_PRESTATION_REJECTED = null;
                    }

                this.viewModel.Item.Details = viewModel.ItemRows.ToList();
                // this.viewModel.Item.CODE_MOTIF = "ES10";
                this.viewModel.Item.CODE_MAGASIN = parames?.DEFAULT_ACHATS_MAGASIN;
                this.viewModel.Item.CODE_UNITE = parames?.DEFAULT_UNITE_ACHATS;

                if (string.IsNullOrEmpty(viewModel.Item.CODE_DOC)) // Ajout d'une reception
                {
                    try
                    {
                        UserDialogs.Instance.ShowLoading("Traitement en cours ...", MaskType.Black);

                        viewModel.IsBusy = true;
                        await CrudManager.Achats.AddItemAsync(viewModel.Item);
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
                        await CrudManager.Achats.UpdateItemAsync(viewModel.Item);

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

        private async Task<List<string>> GetImmatriculations(string str)
        {
            List<string> result = null;

            //   if (string.IsNullOrEmpty(str)) return result;

            if (IsBusy)
                return result;

            IsBusy = true;

            try
            {
                result = await WebServiceClient.GetImmatriculations(str);
                return result;
            }
            catch (Exception ex)
            {
                await UserDialogs.Instance.AlertAsync(WSApi2.GetExceptionMessage(ex), AppResources.alrt_msg_Alert,
                    AppResources.alrt_msg_Ok);
                return result;
            }
            finally
            {
                IsBusy = false;
            }
        }

        private async void btn_Pentree_Clicked(object sender, EventArgs e)
        {

            decimal result = 0;

            if (IsBusy)
                return;

            IsBusy = true;

            try
            {
                UserDialogs.Instance.ShowLoading(AppResources.txt_Loading);
                result = await WebServiceClient.GetPesse();
                ne_PESEE_ENTREE.Value = result;
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

        private async void btn_PSortie_Clicked(object sender, EventArgs e)
        {
            decimal result = 0;

            if (IsBusy)
                return;

            IsBusy = true;

            try
            {
                UserDialogs.Instance.ShowLoading(AppResources.txt_Loading);
                result = await WebServiceClient.GetPesse();
                ne_PESEE_SORTIE.Value = result;
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

        private async void AchImpr_Clicked(object sender, EventArgs e)
        {
            bool result = false;

            if (string.IsNullOrEmpty(viewModel.Item.CODE_DOC))
            {
                await UserDialogs.Instance.AlertAsync("Vous devez enregistrer le document avant de pouvoir imprimer l'étiquette", AppResources.alrt_msg_Alert, AppResources.alrt_msg_Ok);
                return;
            }

            string codeDocRecept = viewModel.Item.CODE_DOC;

            if (IsBusy)
                return;

            IsBusy = true;

            try
            {
                UserDialogs.Instance.ShowLoading(AppResources.txt_Loading);

                //result = await WebServiceClient.PrintQRProduit(codeDocRecept, 1, "Godex DT2x");
                result = await XpertHelper.PrintQrCode(viewModel.Item, 1);
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

        private AnnexTiersSelector AnnexTiersSelector;
        private async void Btn_SelectExtraUser_Clicked(object sender, EventArgs e)
        {
            AnnexTiersSelector = new AnnexTiersSelector();
            currentRow = (sender as Button).BindingContext as View_ACH_DOCUMENT_DETAIL;
            AnnexTiersSelector.CODE_DOC = viewModel.Item.CODE_DOC;
            AnnexTiersSelector.CurrentAnnex = currentRow.ANNEX_USERS;
            AnnexTiersSelector.PrixPrestation = currentRow.PRIX_VENTE;
            AnnexTiersSelector.IS_ACHAT = true;

            await PopupNavigation.Instance.PushAsync(AnnexTiersSelector);
        }

        private async void cmd_VETERINARY_Clicked(object sender, EventArgs e)
        {
            if (viewModel.Item.STATUS_DOC == DocStatus.EnCours)
            {
                var bll = CrudManager.Achats;
                VeterinarySecondVerificationPopup rejectPopup = new VeterinarySecondVerificationPopup("Voulez-vous Validé cet article ?", "Recjecté", "Rejecté Partiale", "Valider");
                VeterinaryAddRejectedPopup rejectListPopup = new VeterinaryAddRejectedPopup("Ajouté a la liste de Rejecté", "Annuler", "Valider");
                CustomPopup confirmationPopup = new CustomPopup("etes-vous sûr ?", "Annuller", "Ok");
                await PopupNavigation.Instance.PushAsync(rejectPopup);
                if (await rejectPopup.PopupClosedTask)
                {
                    if (rejectPopup.Result == 1)
                    {
                        Device.BeginInvokeOnMainThread(async () =>
                        {
                            try
                            {
                                VeterinaryPopup popup = new VeterinaryPopup("Donneé un note pour la Validation", "Annuler", "Ok");
                                await PopupNavigation.Instance.PushAsync(popup);
                                if (await popup.PopupClosedTask)
                                {
                                    if (popup.Result)
                                    {
                                        viewModel.Item.NOTE_DOC = veterinaryNote;
                                        //viewModel.Item.STATUS_DOC = DocStatus.Termine;
                                        veterinaryValidation = 1;

                                    }
                                }

                            }
                            catch (Exception ex)
                            {
                                UserDialogs.Instance.HideLoading();
                            }
                        });
                    }
                    else if (rejectPopup.Result == 0)
                    {
                        Device.BeginInvokeOnMainThread(async () =>
                        {
                            try
                            {
                                VeterinaryPopup popup = new VeterinaryPopup("Donneé un note pour la Réjection", "Annuler", "Ok");
                                await PopupNavigation.Instance.PushAsync(popup);
                                if (await popup.PopupClosedTask)
                                {
                                    if (popup.Result)
                                    {
                                        viewModel.Item.NOTE_DOC = veterinaryNote;
                                        //viewModel.Item.STATUS_DOC = DocStatus.Cloture;
                                        veterinaryValidation = 0;
                                    }
                                }
                            }
                            catch (Exception ex)
                            {
                                UserDialogs.Instance.HideLoading();
                            }

                        });
                    }
                    else if (rejectPopup.Result == 2)
                    {
                        Device.BeginInvokeOnMainThread(async () =>
                        {
                            try
                            {
                                await PopupNavigation.Instance.PushAsync(rejectListPopup);
                                MessagingCenter.Send(this, "RejectedList", ListPrestationRejected);
                                if (await rejectListPopup.PopupClosedTask)
                                {
                                    if (rejectListPopup.Result.Count > 0)
                                    {
                                        viewModel.Item.NOTE_DOC = veterinaryNote;
                                        //viewModel.Item.STATUS_DOC = DocStatus.Cloture;

                                        ListPrestationRejected = rejectListPopup.Result;
                                        foreach (var rejected in ListPrestationRejected)
                                        {
                                            rejected.CODE_PRESTATION = viewModel.Item.CODE_DOC;
                                        }

                                        veterinaryValidation = 2;
                                    }
                                }
                            }
                            catch (Exception ex)
                            {
                                UserDialogs.Instance.HideLoading();
                            }

                        });
                    }
                }
            }

        }
    }
}