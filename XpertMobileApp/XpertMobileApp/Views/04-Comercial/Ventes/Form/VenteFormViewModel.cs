using Acr.UserDialogs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xpert.Common.DAO;
using Xpert.Common.WSClient.Helpers;
using XpertMobileApp.Api;
using XpertMobileApp.Api.Managers;
using XpertMobileApp.Api.Services;
using XpertMobileApp.DAL;
using XpertMobileApp.Models;
using XpertMobileApp.SQLite_Managment;
using XpertMobileApp.ViewModels;

namespace XpertMobileApp.Views
{
    public class VenteFormViewModel : ItemRowsDetailViewModel<View_VTE_VENTE, View_VTE_VENTE_LOT>
    {
        private List<String> immatriculationList;
        public List<String> ImmatriculationList
        {
            get { return immatriculationList; }
            set { SetProperty(ref immatriculationList, value); }
        }

        internal string TypeDoc { get; set; }

        public bool hasEditDetails
        {
            get
            {
                if (AppManager.HasAdmin) return true;
                bool result = false;
                if (AppManager.permissions != null)
                {
                    var obj = AppManager.permissions.Where(x => x.CodeObjet == "ACH_UPDATE_DETAIL").FirstOrDefault();
                    result = obj != null && obj.AcUpdate > 0;
                }
                return result;
            }
        }

        public bool hasEditPrice
        {
            get
            {
                if (AppManager.HasAdmin) return true;
                bool result = false;
                if (AppManager.permissions != null)
                {
                    var obj = AppManager.permissions.Where(x => x.CodeObjet == "ACH_UPDATE_PRIX_HT").FirstOrDefault();
                    result = obj != null && obj.AcUpdate > 0;
                }
                return result;
            }
        }

        private bool _isEnabled;

        public bool IsEnabled
        {
            get
            {
                return _isEnabled;
            }
            set
            {
                _isEnabled = value;
            }
        }

        bool isretour;
        public bool isRetour
        {
            get
            {
                    return isretour;
            }

            set
            {
                isretour = value;
            }
        }


        public bool hasEditHeader
        {
            get
            {
                if (AppManager.HasAdmin) return true;
                bool result = false;
                if (AppManager.permissions != null)
                {
                    var obj = AppManager.permissions.Where(x => x.CodeObjet == "ACH_UPDATE_ENTETE").FirstOrDefault();
                    result = obj != null && obj.AcUpdate > 0;
                }
                return result;
            }
        }

        internal void InitNewVentes()
        {
            ItemRows.Clear();
            Title = AppResources.pn_NewVente;

            var vte = new View_VTE_VENTE();
            vte.ID_Random = XpertHelper.RandomString(7);
            vte.TYPE_DOC = TypeDoc;
            vte.TYPE_VENTE = TypeDoc;
            vte.DATE_VENTE = DateTime.Now.Date;
            Item = vte;

            SelectedTiers = new View_TRS_TIERS()
            {
                CODE_TIERS = "CXPERTCOMPTOIR",
                NOM_TIERS1 = "COMPTOIR"
            };

        }

        private BSE_CHAUFFEUR selectedChauffeur;
        public BSE_CHAUFFEUR SelectedChauffeur
        {
            get { return selectedChauffeur; }
            set { SetProperty(ref selectedChauffeur, value); }
        }

        private string selectedImmat;
        public string SelectedImmat
        {
            get { return selectedImmat; }
            set { SetProperty(ref selectedImmat, value); }
        }

        public VenteFormViewModel(View_VTE_VENTE obj, string itemId) : base(obj, itemId)
        {

        }

        public async Task<View_VTE_VENTE_LOT> AddNewRow(List<View_STK_STOCK> products, bool retour)
        {
            foreach (var product in products)
            {
                if (retour)
                {
                    var row = ItemRows.Where(e => e.ID_STOCK == product.ID_STOCK && e.QUANTITE < 0).FirstOrDefault();
                    if (row == null)
                    {
                        row = new View_VTE_VENTE_LOT();
                        decimal qte = product.SelectedQUANTITE == 0 ? 1 : product.SelectedQUANTITE;
                        // row.Parent_Doc = Item;
                        row.VenteID = row.ID;
                        row.ID = row.ID + "_" + XpertHelper.RandomString(7);
                        row.CODE_VENTE = Item.CODE_VENTE;
                        row.ID_STOCK = product.ID_STOCK;
                        row.CODE_PRODUIT = product.CODE_PRODUIT;
                        row.CODE_BARRE_LOT = product.CODE_BARRE_LOT;
                        row.CODE_BARRE = product.CODE_BARRE;
                        row.DESIGNATION_PRODUIT = product.DESIGNATION_PRODUIT;
                        row.CODE_DETAIL_ORIGINE = XpertHelper.RandomString(5);
                        row.HAS_NEW_ID_STOCK = product.HAS_NEW_ID_STOCK;

                        // prix ht et ttc
                        // get prix gros ou detail
                        try
                        {
                            var prix = await UpdateDatabase.getPrixByQuantity(product.CODE_PRODUIT, qte);
                            if (prix > 0)
                            {
                                row.PRIX_VTE_HT = prix;
                                row.PRIX_VTE_TTC = prix;
                            }
                            else
                            {
                                row.PRIX_VTE_HT = product.SelectedPrice;
                                row.PRIX_VTE_TTC = product.SelectedPrice;
                            }
                        }
                        catch
                        {
                            row.PRIX_VTE_HT = product.SelectedPrice;
                            row.PRIX_VTE_TTC = product.SelectedPrice;
                        }

                        qte = qte * -1;
                        row.QUANTITE = qte;
                        ItemRows.Add(row);
                        this.Item.Details = ItemRows.ToList();
                    }
                    else
                    {
                        row.PRIX_VTE_HT = product.SelectedPrice;
                        row.PRIX_VTE_TTC = product.SelectedPrice;
                        decimal qt = product.SelectedQUANTITE ;
                        qt = qt * -1;
                        row.QUANTITE  = (row.QUANTITE ) + qt;
                    }


                    row.MT_TTC = row.PRIX_VTE_TTC * row.QUANTITE;
                    row.MT_HT = row.PRIX_VTE_HT * row.QUANTITE;
                    Item.TOTAL_TTC = ItemRows.Sum(e => e.MT_TTC * e.QUANTITE);
                    
                    this.isRetour = true;
                    this.isretour = true;

                    row.Index = ItemRows.Count();
                    
                    UpdateMontants();
                    row.PropertyChanged += Row_PropertyChanged;

                }
                else
                {
                    var row = ItemRows.Where(e => e.ID_STOCK == product.ID_STOCK && e.QUANTITE > 0).FirstOrDefault();
                    if (row == null)
                    {
                        row = new View_VTE_VENTE_LOT();
                        decimal qte = product.SelectedQUANTITE == 0 ? 1 : product.SelectedQUANTITE;
                        // row.Parent_Doc = Item;
                        row.VenteID = row.ID;
                        row.ID = row.ID + "_" + XpertHelper.RandomString(7);
                        row.CODE_VENTE = Item.CODE_VENTE;
                        row.ID_STOCK = product.ID_STOCK;
                        row.CODE_PRODUIT = product.CODE_PRODUIT;
                        row.CODE_BARRE_LOT = product.CODE_BARRE_LOT;
                        row.CODE_BARRE = product.CODE_BARRE;
                        row.DESIGNATION_PRODUIT = product.DESIGNATION_PRODUIT;
                        // get prix gros ou detail
                        try
                        {
                            var prix = await UpdateDatabase.getPrixByQuantity(product.CODE_PRODUIT, qte);
                            if (prix > 0)
                            {
                                row.PRIX_VTE_HT = prix;
                                row.PRIX_VTE_TTC = prix;
                            }
                            else
                            {
                                row.PRIX_VTE_HT = product.SelectedPrice;
                                row.PRIX_VTE_TTC = product.SelectedPrice;
                            }
                        }
                        catch
                        {
                            row.PRIX_VTE_HT = product.SelectedPrice;
                            row.PRIX_VTE_TTC = product.SelectedPrice;
                        }

                        row.QUANTITE = qte;
                        ItemRows.Add(row);
                        this.Item.Details = ItemRows.ToList();
                    }
                    else
                    {
                        row.PRIX_VTE_HT = product.SelectedPrice;
                        row.PRIX_VTE_TTC = product.SelectedPrice;
                        row.QUANTITE += product.SelectedQUANTITE;
                    }
                    row.MT_TTC = row.PRIX_VTE_TTC * row.QUANTITE;
                    row.MT_HT = row.PRIX_VTE_HT * row.QUANTITE;
                    Item.TOTAL_TTC = ItemRows.Sum(e => e.MT_TTC * e.QUANTITE);

                    this.isRetour = false;
                    this.isretour = false;

                    row.Index = ItemRows.Count();
                    UpdateMontants();
                    row.PropertyChanged += Row_PropertyChanged;
                    //return row;
                }
            }
            return null;
        }

        private async void Row_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (App.Online)
            {
                if (e.PropertyName == "QUANTITE")
                {
                    UpdateMontants();
                }
            }
            else
            {
                if (e.PropertyName == "QUANTITE")
                {
                    var stock = await UpdateDatabase.getInstance().Table<View_STK_STOCK>().ToListAsync();
                    foreach (var item in stock)
                    {
                        var row = ItemRows.Where(x => x.ID_STOCK == item.ID_STOCK).FirstOrDefault();
                        if (row != null)
                        {
                            if (row.QUANTITE <= item.OLD_QUANTITE)
                            {
                                try
                                {
                                    var prix = await UpdateDatabase.getPrixByQuantity(row.CODE_PRODUIT, row.QUANTITE);
                                    if (prix > 0)
                                    {
                                        row.PRIX_VTE_TTC = prix;
                                        row.PRIX_VTE_HT = prix;
                                    }
                                }
                                catch
                                {
                                }
                                UpdateMontants();
                            }
                            else
                            {
                                row.QUANTITE = item.OLD_QUANTITE;
                                await UserDialogs.Instance.AlertAsync(" Quantité stock insuffisante ! \n La quantité stock = " + item.OLD_QUANTITE, AppResources.alrt_msg_Alert, AppResources.alrt_msg_Ok);
                            }
                        }
                    }
                }
            }
        }

        public void UpdateMontants()
        {
            try
            {
                Item.TOTAL_TTC = ItemRows.Sum(e => e.MT_TTC);
                Item.TOTAL_RESTE = Item.TOTAL_TTC - Item.TOTAL_PAYE;
            }
            catch (Exception ex)
            {
                UserDialogs.Instance.Alert(WSApi2.GetExceptionMessage(ex), AppResources.alrt_msg_Alert,
                     AppResources.alrt_msg_Ok);
            }
        }

        public void RemoveNewRow(View_STK_PRODUITS product)
        {
            var row = ItemRows.Where(e => e.CODE_PRODUIT == product.CODE_PRODUIT).FirstOrDefault();
            if (row == null) return;

            if (row.QUANTITE > 0)
            {
                row.QUANTITE -= 1;
            }
            else
            {
                ItemRows.Remove(row);
            }
        }

        public async Task<View_VTE_VENTE_LOT> AddScanedProduct(string cb_prod)
        {
            try
            {
                // Cas lot déjà ajouté
                var row = ItemRows.Where(e => e.CODE_BARRE == cb_prod).FirstOrDefault();
                if (row != null)
                {
                    row.QUANTITE += 1;
                    XpertHelper.PeepScan();
                    return row;
                }

                // Récupérer le lot depuis le serveur
                string codeTiers = SelectedTiers != null ? SelectedTiers.CODE_TIERS : "";
                List<View_STK_STOCK> prods = new List<View_STK_STOCK>();
                if (App.Online)
                {
                    prods = await CrudManager.Stock.SelectByCodeBarreLot(cb_prod, codeTiers, App.CODE_MAGASIN);
                }
                else
                {
                    prods = await UpdateDatabase.SelectByCodeBarreLot(cb_prod, App.CODE_MAGASIN);
                }
                XpertHelper.PeepScan();

                if (prods.Count > 1)
                {
                    await UserDialogs.Instance.AlertAsync("Plusieurs produits pour ce code barre!", AppResources.alrt_msg_Alert, AppResources.alrt_msg_Ok);
                    return null;
                }
                else if (prods.Count == 0)
                {
                    await UserDialogs.Instance.AlertAsync("Aucun produit pour ce code barre!", AppResources.alrt_msg_Alert, AppResources.alrt_msg_Ok);
                    return null;
                }

                var res = await AddNewRow(prods, false); // false veut dire le type de produit ajouter est une vente (pas retour)
                return res;
            }
            catch (Exception ex)
            {
                await UserDialogs.Instance.AlertAsync(WSApi2.GetExceptionMessage(ex), AppResources.alrt_msg_Alert,
                    AppResources.alrt_msg_Ok);
                return null;
            }
        }

        internal async Task SelectScanedTiers(string cb_tiers)
        {
            try
            {
                // Récupérer le lot depuis le serveur
                XpertSqlBuilder qb = new XpertSqlBuilder();
                qb.AddCondition<View_TRS_TIERS, string>(x => x.NUM_CARTE_FIDELITE, cb_tiers);
                qb.AddOrderBy<View_TRS_TIERS, string>(x => x.CODE_TIERS);
                var tiers = await CrudManager.TiersManager.SelectByPage(qb.QueryInfos, 1, 1);
                if (tiers == null)
                    return;

                XpertHelper.PeepScan();

                if (tiers.Count() > 1)
                {
                    await UserDialogs.Instance.AlertAsync("Plusieurs tiers pour ce code barre!", AppResources.alrt_msg_Alert, AppResources.alrt_msg_Ok);
                }
                else if (tiers.Count() == 0)
                {
                    await UserDialogs.Instance.AlertAsync("Aucun tiers pour ce code barre!", AppResources.alrt_msg_Alert, AppResources.alrt_msg_Ok);
                }
                else
                {
                    SelectedTiers = tiers.FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                await UserDialogs.Instance.AlertAsync(WSApi2.GetExceptionMessage(ex), AppResources.alrt_msg_Alert,
                    AppResources.alrt_msg_Ok);
            }
        }
    }
}
