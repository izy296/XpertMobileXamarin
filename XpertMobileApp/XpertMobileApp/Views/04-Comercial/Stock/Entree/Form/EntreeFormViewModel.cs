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

namespace XpertMobileApp.Views.Entree
{
    public class EntreeFormViewModel : ItemRowsDetailViewModel<View_STK_ENTREE, View_STK_ENTREE_DETAIL>
    {


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


        internal void InitNewVentes()
        {
            ItemRows.Clear();
            Title = AppResources.pn_NewVente;

            var entree = new View_STK_ENTREE();
            entree.TYPE_ENTREE = "ES10";
            entree.DATE_ENTREE = DateTime.Now.Date;
            Item = entree;

        }


        private string selectedImmat;
        public string SelectedImmat
        {
            get { return selectedImmat; }
            set { SetProperty(ref selectedImmat, value); }
        }

        public EntreeFormViewModel(View_STK_ENTREE obj, string itemId) : base(obj, itemId)
        {

        }

        public async Task<View_STK_ENTREE_DETAIL> AddNewRow(List<View_STK_STOCK> products)
        {
            foreach (var product in products)
            {
                var row = ItemRows.Where(e => e.ID_STOCK == product.ID_STOCK && e.QUANTITE < 0).FirstOrDefault();
                if (row == null)
                {
                    row = new View_STK_ENTREE_DETAIL();
                    decimal qte = product.SelectedQUANTITE == 0 ? 1 : product.SelectedQUANTITE;
                    // row.Parent_Doc = Item;
                    row.CODE_ENTREE = Item.CODE_ENTREE;
                    row.ID_STOCK = product.ID_STOCK;
                    row.CODE_PRODUIT = product.CODE_PRODUIT;
                    row.CODE_BARRE_LOT = product.CODE_BARRE_LOT;
                    row.CODE_BARRE = product.CODE_BARRE;
                    row.DESIGNATION_PRODUIT = product.DESIGNATION_PRODUIT;

                    row.PRIX_UNITAIRE = product.SelectedPrice;
                    row.PRIX_TTC = product.SelectedPrice;
                    row.PRIX_VENTE = product.SelectedPrice;
                    row.QUANTITE = qte;
                    row.MT_TTC = row.PRIX_UNITAIRE * row.QUANTITE;
                    row.MT_HT = row.PRIX_UNITAIRE * row.QUANTITE;
                    ItemRows.Add(row);
                    this.Item.Details = ItemRows.ToList();
                }
                else
                {
                    row.PRIX_UNITAIRE = product.SelectedPrice;
                    row.PRIX_TTC = product.SelectedPrice;
                    row.PRIX_VENTE = product.SelectedPrice;
                    row.QUANTITE = (row.QUANTITE) + product.SelectedQUANTITE;
                    row.MT_TTC = row.PRIX_UNITAIRE * row.QUANTITE;
                    row.MT_HT = row.PRIX_UNITAIRE * row.QUANTITE;
                }


              
                Item.TOTAL_TTC = ItemRows.Sum(e => e.MT_TTC * e.QUANTITE);

                row.Index = ItemRows.Count();

                UpdateMontants();
                row.PropertyChanged += Row_PropertyChanged;
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
                                        row.PRIX_UNITAIRE = prix;
                                        row.PRIX_TTC = prix;
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

        public async Task<View_STK_ENTREE_DETAIL> AddScanedProduct(string cb_prod)
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

                var res = await AddNewRow(prods); // false veut dire le type de produit ajouter est une vente (pas retour)
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
