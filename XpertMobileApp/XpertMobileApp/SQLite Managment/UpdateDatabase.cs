using Acr.UserDialogs;
using SQLite;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xpert.Common.WSClient.Helpers;
using Xpert.Common.WSClient.Model;
using Xpert.Common.WSClient.Services;
using XpertMobileApp.Api;
using XpertMobileApp.Api.Managers;
using XpertMobileApp.Api.Services;
using XpertMobileApp.DAL;
using XpertMobileApp.Models;
using XpertMobileApp.Services;
using XpertMobileApp.ViewModels;

namespace XpertMobileApp.SQLite_Managment
{
    class UpdateDatabase
    {
        static SQLiteAsyncConnection db;


        public static SQLiteAsyncConnection getInstance()
        {
            if (db == null)
            {
                // Get an absolute path to the database file
                var databasePath = Path.Combine(FileSystem.AppDataDirectory, "MyData.db");
                db = new SQLiteAsyncConnection(databasePath);
                return db;
            }
            else
            {
                return db;
            }
        }
        public static async Task initialisationDbLocal()
        {
            try
            {
                //await getInstance().DropTableAsync<View_TRS_TIERS>();
                //await getInstance().DeleteAllAsync<View_VTE_VENTE>();

                await getInstance().CreateTableAsync<View_STK_PRODUITS>();
                await getInstance().CreateTableAsync<View_TRS_TIERS>();
                await getInstance().CreateTableAsync<View_LIV_TOURNEE>();
                await getInstance().CreateTableAsync<View_LIV_TOURNEE_DETAIL>();
                await getInstance().CreateTableAsync<View_STK_STOCK>();
                await getInstance().CreateTableAsync<View_VTE_VENTE_LOT>();
                await getInstance().CreateTableAsync<View_VTE_VENTE>();
                await getInstance().CreateTableAsync<SYS_USER>();
                await getInstance().CreateTableAsync<SYS_OBJET_PERMISSION>();
                await getInstance().CreateTableAsync<TRS_JOURNEES>();
                await getInstance().CreateTableAsync<Token>();
                await getInstance().CreateTableAsync<View_BSE_TIERS_FAMILLE>();
                await getInstance().CreateTableAsync<BSE_TABLE_TYPE>();
                await getInstance().CreateTableAsync<BSE_TABLE>();
                await getInstance().CreateTableAsync<SYS_CONFIGURATION_MACHINE>();
                await getInstance().CreateTableAsync<SYS_MOBILE_PARAMETRE>();
                await getInstance().CreateTableAsync<View_TRS_ENCAISS>();
                await getInstance().CreateTableAsync<View_BSE_COMPTE>();
                await getInstance().CreateTableAsync<BSE_ENCAISS_MOTIFS>();
                await getInstance().CreateTableAsync<View_BSE_PRODUIT_PRIX_VENTE_BY_QUANTITY>();
                await getInstance().CreateTableAsync<View_STK_ENTREE>();
                await getInstance().CreateTableAsync<View_STK_ENTREE_DETAIL>();
                await getInstance().CreateTableAsync<LOG_SYNCHRONISATION>();

            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        public static async Task<IEnumerable<TView>> getItemsUnsyncronised<TView, TTabel>(bool _selectAll = true, string param = "", string methodName = "")
        {
            ICurdService<TView> service = new CrudService<TView>(App.RestServiceUrl, typeof(TTabel).Name, App.User.Token);
            if (_selectAll)
            {
                return await service.GetItemsAsync();
            }
            else
            {
                return await service.GetItemsAsyncWithUrl(methodName, param);
            }

        }

        public static async Task SyncData<TView, TTabel>(bool _selectAll = true, string param = "", string methodName = "")
        {
            var ListItems = new List<TView>();

            var items = await getItemsUnsyncronised<TView, TTabel>(_selectAll, param, methodName);
            await getInstance().DeleteAllAsync<TView>();

            try
            {
                if (items != null)//&& items.IsCompleted && items.Status == TaskStatus.RanToCompletion)
                {
                    ListItems.AddRange(items);
                }
            }
            catch (Exception exc)
            {
                Console.WriteLine(exc.Message);
            }
            var id = await getInstance().InsertAllAsync(ListItems);

            ListItems = null;
        }

        internal static async Task<View_TRS_TIERS> SelectScanedTiers(string text)
        {
            var tiers = await getInstance().Table<View_TRS_TIERS>().FirstOrDefaultAsync(e => e.CODE_TIERS.Equals(text));
            return tiers;
        }

        internal static async Task<int> UpdateTourneeItemVisited(View_LIV_TOURNEE_DETAIL selectedItem)
        {
            selectedItem.CODE_ETAT = TourneeStatus.Visited;
            selectedItem.ETAT_COLOR = "#FFA500";
            selectedItem.DESIGNATION_ETAT = "#Visited";
            return await getInstance().UpdateAsync(selectedItem);
        }




        public static async Task<TRS_JOURNEES> getCurrenetSession()
        {
            TRS_JOURNEES session = await getInstance().Table<TRS_JOURNEES>().FirstOrDefaultAsync();
            return session;
        }

        public static async Task SyncFamille()
        {
            try
            {
                var itemsC = await WebServiceClient.getTiersFamilles();
                await getInstance().DeleteAllAsync<View_BSE_TIERS_FAMILLE>();

                View_BSE_TIERS_FAMILLE allElem = new View_BSE_TIERS_FAMILLE();
                allElem.CODE_FAMILLE = string.Empty;
                allElem.DESIGN_FAMILLE = string.Empty;
                var id = await getInstance().InsertAsync(allElem);

                var id2 = await getInstance().InsertAllAsync(itemsC);
            }
            catch (Exception ex)
            {
                await UserDialogs.Instance.AlertAsync(WSApi2.GetExceptionMessage(ex), AppResources.alrt_msg_Alert,
                    AppResources.alrt_msg_Ok);
            }
        }

        public static async Task SyncTypeTiers()
        {
            try
            {
                var itemsC = await WebServiceClient.getTiersTypes();
                await getInstance().DeleteAllAsync<BSE_TABLE_TYPE>();

                BSE_TABLE_TYPE allElem = new BSE_TABLE_TYPE();
                allElem.CODE_TYPE = string.Empty;
                allElem.DESIGNATION_TYPE = string.Empty;
                var id = await getInstance().InsertAsync(allElem);

                var id2 = await getInstance().InsertAllAsync(itemsC);
            }
            catch (Exception ex)
            {
                await UserDialogs.Instance.AlertAsync(WSApi2.GetExceptionMessage(ex), AppResources.alrt_msg_Alert,
                    AppResources.alrt_msg_Ok);
            }
        }

        public static async Task SyncSecteurs()
        {
            try
            {
                var itemsC = await CrudManager.BSE_LIEUX.GetItemsAsync();
                await getInstance().DeleteAllAsync<BSE_TABLE>();

                BSE_TABLE allElem = new BSE_TABLE();
                allElem.CODE = string.Empty;
                allElem.DESIGNATION = string.Empty;
                var id = await getInstance().InsertAsync(allElem);

                var id2 = await getInstance().InsertAllAsync(itemsC);
            }
            catch (Exception ex)
            {
                await UserDialogs.Instance.AlertAsync(WSApi2.GetExceptionMessage(ex), AppResources.alrt_msg_Alert,
                    AppResources.alrt_msg_Ok);
            }
        }

        public static async Task SyncBseCompte()
        {
            try
            {
                var ListComptes = await WebServiceClient.getComptes();
                // si admin get all compte sinon get juste compte user exp : user.CodeCompte

                if (!AppManager.HasAdmin)
                {

                    ListComptes = ListComptes.Where(x => x.CODE_COMPTE == App.User.CODE_COMPTE).ToList();
                }

                ListComptes.Insert(0, new View_BSE_COMPTE()
                {
                    DESIGNATION_TYPE = AppResources.txt_All,
                    DESIGN_COMPTE = AppResources.txt_All,
                    CODE_TYPE = string.Empty
                });
                await getInstance().DeleteAllAsync<View_BSE_COMPTE>();
                var id = await getInstance().InsertAllAsync(ListComptes);

            }
            catch (Exception ex)
            {
                await UserDialogs.Instance.AlertAsync(WSApi2.GetExceptionMessage(ex), AppResources.alrt_msg_Alert,
                    AppResources.alrt_msg_Ok);
            }
        }
        public static async Task<List<View_BSE_COMPTE>> getComptes(string CodeCompte)
        {
            List<View_BSE_COMPTE> comptes = await getInstance().Table<View_BSE_COMPTE>().ToListAsync();
            if (string.IsNullOrEmpty(CodeCompte))
            {
                return comptes;
            }
            else
            {
                List<View_BSE_COMPTE> cpt = comptes.Where(x => x.CODE_COMPTE == CodeCompte).ToList();
                return cpt;
            }
        }
        /// <summary>
        /// Retourne  la liste des comptes ...
        /// </summary>
        /// <returns></returns>
        public static async Task<List<View_BSE_COMPTE>> getComptes()
        {
            List<View_BSE_COMPTE> comptes = await getInstance().Table<View_BSE_COMPTE>().ToListAsync();
            List<SYS_USER> users = await getInstance().Table<SYS_USER>().ToListAsync();
            var code_compte = users.Where(x => x.ID_USER == App.User.UserName.ToUpper()).FirstOrDefault()?.CODE_COMPTE;
            if (string.IsNullOrEmpty(code_compte))
            {
                return comptes;
            }
            else
            {
                List<View_BSE_COMPTE> cpt = comptes.Where(x => x.CODE_COMPTE == code_compte).ToList();
                return cpt;
            }
        }

        public static async Task SyncMotifs()
        {
            try
            {
                var itemsM = await WebServiceClient.GetAllMotifs();
                itemsM.Insert(0, new BSE_ENCAISS_MOTIFS()
                {
                    DESIGN_MOTIF = AppResources.txt_All,
                    CODE_MOTIF = string.Empty
                });
                await getInstance().DeleteAllAsync<BSE_ENCAISS_MOTIFS>();
                var id = await getInstance().InsertAllAsync(itemsM);
            }
            catch (Exception ex)
            {
                await UserDialogs.Instance.AlertAsync(WSApi2.GetExceptionMessage(ex), AppResources.alrt_msg_Alert,
                    AppResources.alrt_msg_Ok);
            }
        }

        public static async Task SyncProductPriceByQuantity()
        {
            try
            {
                var bll = new ProductManager();
                var items = await bll.SelectListePrix();
                await getInstance().DeleteAllAsync<View_BSE_PRODUIT_PRIX_VENTE_BY_QUANTITY>();
                var id = await getInstance().InsertAllAsync(items);
            }
            catch (Exception ex)
            {
                await UserDialogs.Instance.AlertAsync(WSApi2.GetExceptionMessage(ex), AppResources.alrt_msg_Alert,
                    AppResources.alrt_msg_Ok);
            }
        }


        public static decimal getPrixByQuantitySync(string codeProduit, decimal qteVendu)
        {
            Task<List<View_BSE_PRODUIT_PRIX_VENTE_BY_QUANTITY>> AllPrices = getInstance().Table<View_BSE_PRODUIT_PRIX_VENTE_BY_QUANTITY>().ToListAsync();
            var prixProduct = AllPrices.Result.Where(x => x.CODE_PRODUIT == codeProduit).FirstOrDefault();
            if (Math.Abs(qteVendu) >= prixProduct.QTE_VENTE)
            {
                return prixProduct.PRIX_GROS;
            }
            else
            {
                return prixProduct.PRIX_DETAIL;
            }
        }

        public static async Task<decimal> getPrixByQuantity(string codeProduit, decimal qteVendu)
        {
            List<View_BSE_PRODUIT_PRIX_VENTE_BY_QUANTITY> AllPrices = await getInstance().Table<View_BSE_PRODUIT_PRIX_VENTE_BY_QUANTITY>().ToListAsync();
            var prixProduct = AllPrices.Where(x => x.CODE_PRODUIT == codeProduit).FirstOrDefault();
            if (Math.Abs(qteVendu) >= prixProduct.QTE_VENTE)
            {
                return prixProduct.PRIX_GROS;
            }
            else
            {
                return prixProduct.PRIX_DETAIL;
            }
        }
        public static async Task<List<View_TRS_TIERS>> FilterTiers(string search)
        {
            List<View_TRS_TIERS> allTiers = await getInstance().Table<View_TRS_TIERS>().ToListAsync();
            if (search == string.Empty)
            {
                return allTiers;
            }
            else
            {
                var tiersFiltred = allTiers.Where(x => x.FULL_NOM_TIERS.ToUpper().Contains(search.ToUpper())).ToList();
                return tiersFiltred;
            }
        }

        public static async Task<List<View_STK_PRODUITS>> FilterProduits(string search)
        {
            List<View_STK_PRODUITS> allTiers = await getInstance().Table<View_STK_PRODUITS>().ToListAsync();
            if (search == string.Empty)
            {
                return allTiers;
            }
            else
            {
                var tiersFiltred = allTiers.Where(x => x.TRUNCATED_DESIGNATION.ToUpper().Contains(search.ToUpper())).ToList();
                return tiersFiltred;
            }
        }

        public static async Task<List<View_LIV_TOURNEE_DETAIL>> FilterTournee(string search)
        {
            List<View_LIV_TOURNEE_DETAIL> allTiers = await getInstance().Table<View_LIV_TOURNEE_DETAIL>().ToListAsync();
            if (search == string.Empty)
            {
                return allTiers;
            }
            else
            {
                var tiersFiltred = allTiers.Where(x => x.NOM_TIERS.ToUpper().Contains(search.ToUpper())).ToList();
                return tiersFiltred;
            }
        }

        public static async Task<List<BSE_ENCAISS_MOTIFS>> getMotifs(string typeMotif = "ENC")
        {
            List<BSE_ENCAISS_MOTIFS> AllMotifs = await getInstance().Table<BSE_ENCAISS_MOTIFS>().ToListAsync();
            List<BSE_ENCAISS_MOTIFS> Motifs;
            Motifs = AllMotifs.Where(e => e.TYPE_MOTIF == typeMotif).ToList();
            return Motifs;
        }



        public static async Task<List<View_TRS_ENCAISS>> LoadEncDec(string typeMotif = "")
        {
            List<View_TRS_ENCAISS> AllEncDec = await getInstance().Table<View_TRS_ENCAISS>().ToListAsync();
            if (typeMotif == string.Empty || typeMotif == "All")
            {
                return AllEncDec;
            }
            return AllEncDec.Where(e => e.CODE_TYPE == typeMotif).ToList();
        }
        public static async Task<bool> DeleteEncaiss(View_TRS_ENCAISS encaiss)
        {
            await getInstance().DeleteAsync(encaiss);
            return true;
        }

        public static async Task<View_TRS_ENCAISS> getselectedItemEncaiss(View_TRS_ENCAISS encaiss)
        {
            if (encaiss != null)
            {
                List<View_TRS_ENCAISS> AllEncDec = await getInstance().Table<View_TRS_ENCAISS>().ToListAsync();
                var item = AllEncDec.Where(x => x.CODE_ENCAISS == encaiss.CODE_ENCAISS).FirstOrDefault();
                if (item != null)
                {
                    return item;
                }
                else
                {
                    return null;
                }
            }
            else
            {
                return null;
            }
        }

        public static async Task<bool> UpdateEncaiss(View_TRS_ENCAISS encaiss)
        {
            await getInstance().UpdateAsync(encaiss);
            return true;
        }

        public static async Task SyncSysParams()
        {
            var Params = await CrudManager.SysParams.GetParams();
            await getInstance().DeleteAllAsync<SYS_MOBILE_PARAMETRE>();
            var id = await getInstance().InsertAsync(Params);
        }

        public static async Task<SYS_MOBILE_PARAMETRE> getParams()
        {
            SYS_MOBILE_PARAMETRE Params = await getInstance().Table<SYS_MOBILE_PARAMETRE>().FirstOrDefaultAsync();
            return Params;
        }


        public static async Task<string> AjoutVente(View_VTE_VENTE vente)
        {
            if ((vente.TOTAL_PAYE + vente.MBL_MT_VERCEMENT) > vente.TOTAL_TTC)
            {
                await UserDialogs.Instance.AlertAsync("Montant versé suppérieur au montant à payer!", AppResources.alrt_msg_Alert, AppResources.alrt_msg_Ok);
                return vente.MBL_MT_VERCEMENT.ToString();
            }
            else
            {
                vente.TOTAL_PAYE = vente.MT_VERSEMENT = vente.MBL_MT_VERCEMENT;
                vente.TOTAL_RESTE = vente.TOTAL_TTC - vente.TOTAL_PAYE;
                if (!"CXPERTCOMPTOIR".Equals(vente.CODE_TIERS))
                {
                    decimal solde = await getSoldTiers(vente.CODE_TIERS);
                    vente.OLD_SOLDE = solde;
                    vente.NEW_SOLDE = vente.OLD_SOLDE + vente.TOTAL_RESTE;

                }
                vente.CREATED_BY = App.User.UserName;
                var id = await getInstance().InsertAsync(vente);

                vente.CODE_VENTE = await generateCode(vente.TYPE_DOC, vente.ID.ToString());
                vente.NUM_VENTE = await generateNum(vente.TYPE_DOC, vente.ID.ToString());
                vente.DATE_VENTE = DateTime.Now;
                await getInstance().UpdateAsync(vente);
                foreach (var item in vente.Details)
                {
                    item.CODE_VENTE = vente.CODE_VENTE;
                }
                var id2 = await getInstance().InsertAllAsync(vente.Details);
                await UpdateStock(vente);
                if (vente.TOTAL_TTC != vente.MT_VERSEMENT)
                {
                    decimal sold = vente.TOTAL_TTC - vente.MT_VERSEMENT;
                    await UpdateSoldTiers(sold, vente.CODE_TIERS);
                }
                if (!string.IsNullOrEmpty(vente.MBL_CODE_TOURNEE_DETAIL))
                {
                    await UpdateTourneeDetail(vente);
                    await UpdateTournee();
                }

                return vente.CODE_VENTE;
            }
        }

        public static async Task<string> AjoutEntree(View_STK_ENTREE entree)
        {
            await getInstance().CreateTableAsync<View_STK_ENTREE>();
            await getInstance().CreateTableAsync<View_STK_ENTREE_DETAIL>();

            entree.CREATED_BY = App.User.UserName;
            var id = await getInstance().InsertAsync(entree);

            entree.CODE_ENTREE = await generateCode("EN", entree.ID.ToString());
            entree.NUM_ENTREE = await generateNum("EN", entree.ID.ToString());

            entree.DATE_ENTREE = DateTime.Now;

            await getInstance().UpdateAsync(entree);

            foreach (var item in entree.Details)
            {
                item.CODE_ENTREE = entree.CODE_ENTREE;
            }
            var id2 = await getInstance().InsertAllAsync(entree.Details);

            return entree.CODE_ENTREE;
        }


        private static async Task UpdateSoldTiers(decimal sold, string codeTiers)
        {
            //View_LIV_TOURNEE_DETAIL
            View_TRS_TIERS UpdatedTiers = await getInstance().Table<View_TRS_TIERS>().FirstOrDefaultAsync(x => x.CODE_TIERS == codeTiers);
            View_LIV_TOURNEE_DETAIL Tournnee = await getInstance().Table<View_LIV_TOURNEE_DETAIL>().FirstOrDefaultAsync(x => x.CODE_TIERS == codeTiers);
            if (UpdatedTiers.SOLDE_TIERS >= 0)
            {
                UpdatedTiers.SOLDE_TIERS = UpdatedTiers.SOLDE_TIERS + sold;
            }
            else
            {
                UpdatedTiers.SOLDE_TIERS = UpdatedTiers.SOLDE_TIERS - sold;
            }
            await getInstance().UpdateAsync(UpdatedTiers);

            // juste dans offline :: update solde tierss dans tournnee juste dans offline car le champs de solde tiers dans tournnee not calculable dans offline 
            if (Tournnee != null)
            {
                Tournnee.SOLDE_TIERS = UpdatedTiers.SOLDE_TIERS;
                await getInstance().UpdateAsync(Tournnee);
            }
        }

        private static async Task UpdateSoldTiersApresEncaiss(decimal sold, string codeTiers, string type = "ENC")
        {
            View_TRS_TIERS UpdatedTiers = await getInstance().Table<View_TRS_TIERS>().FirstOrDefaultAsync(x => x.CODE_TIERS == codeTiers);
            View_LIV_TOURNEE_DETAIL Tournnee = await getInstance().Table<View_LIV_TOURNEE_DETAIL>().FirstOrDefaultAsync(x => x.CODE_TIERS == codeTiers);

            if (type == "ENC")
            {
                UpdatedTiers.SOLDE_TIERS = UpdatedTiers.SOLDE_TIERS - sold;
            }
            else
            {
                UpdatedTiers.SOLDE_TIERS = UpdatedTiers.SOLDE_TIERS + sold;
            }
            await getInstance().UpdateAsync(UpdatedTiers);

            // juste dans offline :: update solde tierss dans tournnee juste dans offline car le champs de solde tiers dans tournnee not calculable dans offline 
            if (Tournnee != null)
            {
                Tournnee.SOLDE_TIERS = UpdatedTiers.SOLDE_TIERS;
                await getInstance().UpdateAsync(Tournnee);
            }

        }


        private static async Task<decimal> getSoldTiers(string codeTiers)
        {
            View_TRS_TIERS UpdatedTiers = await getInstance().Table<View_TRS_TIERS>().FirstOrDefaultAsync(x => x.CODE_TIERS == codeTiers);
            return UpdatedTiers.SOLDE_TIERS;
        }


        public static async Task<string> generateCode(string TypeDoc, string ID)
        {
            var date = DateTime.Now.Year.ToString();
            var date2 = string.Empty;
            if (date.Length == 2)
            {
                date2 = "/" + date.Substring(0, 2);
            }
            else if (date.Length == 4)
            {
                date2 = "/" + date.Substring(2, 2);
            }
            var code = date + TypeDoc + ID + date2 + "/" + App.PrefixCodification;
            return code;
        }

        public static async Task<string> generateNum(string TypeDoc, string ID)
        {
            var date = DateTime.Now.Year.ToString();
            var date2 = string.Empty;
            if (date.Length == 2)
            {
                date2 = "/" + date.Substring(0, 2);
            }
            else if (date.Length == 4)
            {
                date2 = "/" + date.Substring(2, 2);
            }
            var num = ID + date2 + "/" + App.PrefixCodification;
            return num;
        }

        public static async Task AjoutTiers(View_TRS_TIERS tiers)
        {
            tiers.NOM_TIERS1 = tiers.NOM_TIERS + " " + tiers.PRENOM_TIERS;

            List<BSE_TABLE_TYPE> Types = await getInstance().Table<BSE_TABLE_TYPE>().ToListAsync();
            tiers.DESIGNATION_TYPE = Types.Where(e => e.CODE_TYPE == tiers.CODE_TYPE).FirstOrDefault()?.DESIGNATION_TYPE;

            List<View_BSE_TIERS_FAMILLE> familles = await getInstance().Table<View_BSE_TIERS_FAMILLE>().ToListAsync();
            tiers.DESIGN_FAMILLE = familles.Where(e => e.CODE_FAMILLE == tiers.CODE_FAMILLE).FirstOrDefault()?.DESIGN_FAMILLE;

            tiers.ETAT_TIERS = STAT_TIERS_MOBILE.ADDED;

            var id = await getInstance().InsertAsync(tiers);

            tiers.CODE_TIERS = tiers.ID.ToString() + "/" + App.PrefixCodification + "/MOB";
            tiers.NUM_TIERS = tiers.ID.ToString() + "/" + App.PrefixCodification + "/MOB";
            await getInstance().UpdateAsync(tiers);
        }

        public static async Task<View_STK_STOCK> getProductfromStock(View_STK_PRODUITS product)
        {
            List<View_STK_STOCK> Stocks = await getInstance().Table<View_STK_STOCK>().ToListAsync();
            var productInStock = Stocks.Where(e => e.CODE_PRODUIT == product.CODE_PRODUIT).FirstOrDefault();
            if (productInStock == null)
            {
                View_STK_STOCK newStockProduct = new View_STK_STOCK();
                //newStockProduct.ID_STOCK = null;
                newStockProduct.CODE_PRODUIT = product.CODE_PRODUIT;
                newStockProduct.CODE_MAGASIN = App.CODE_MAGASIN;
                newStockProduct.COUT_ACHAT = product.PRIX_ACHAT_TTC;
                newStockProduct.PPA = product.PPA;
                newStockProduct.SHP = product.SHP;
                //newStockProduct.QUANTITE = product.QTE_STOCK;
                newStockProduct.CODE_BARRE_LOT = product.CODE_BARRE;
                newStockProduct.DESIGNATION_PRODUIT = product.DESIGNATION;
                newStockProduct.PRIX_VENTE = product.PRIX_VENTE_HT;
                newStockProduct.HAS_NEW_ID_STOCK = true;


                var count = await getInstance().InsertAsync(newStockProduct);

                newStockProduct.ID_STOCK = newStockProduct.ID;
                await getInstance().UpdateAsync(newStockProduct);

                return newStockProduct;
            }
            else
            {
                //productInStock.PRIX_VENTE = product.PRIX_VENTE_HT;
                //await getInstance().UpdateAsync(productInStock);
                return productInStock;
            }
        }

        public static async Task<List<View_STK_STOCK>> SelectByCodeBarreLot(string cb_prod, string codeMagasin)
        {
            List<View_STK_STOCK> Products = await getInstance().Table<View_STK_STOCK>().ToListAsync();
            List<View_STK_STOCK> Produit = Products.Where(e => e.CODE_BARRE == cb_prod).Where(e => e.CODE_MAGASIN == codeMagasin).ToList();
            return Produit;
        }

        public static async Task UpdateTiers(View_TRS_TIERS tiers)
        {
            tiers.NOM_TIERS1 = tiers.NOM_TIERS + " " + tiers.PRENOM_TIERS;

            List<BSE_TABLE_TYPE> Types = await getInstance().Table<BSE_TABLE_TYPE>().ToListAsync();
            tiers.DESIGNATION_TYPE = Types.Where(e => e.CODE_TYPE == tiers.CODE_TYPE).FirstOrDefault()?.DESIGNATION_TYPE;

            List<View_BSE_TIERS_FAMILLE> familles = await getInstance().Table<View_BSE_TIERS_FAMILLE>().ToListAsync();
            tiers.DESIGN_FAMILLE = familles.Where(e => e.CODE_FAMILLE == tiers.CODE_FAMILLE).FirstOrDefault()?.DESIGN_FAMILLE;

            tiers.ETAT_TIERS = STAT_TIERS_MOBILE.UPDATED;
            await getInstance().UpdateAsync(tiers);
        }

        public static async Task AjoutToken(Token token)
        {
            await getInstance().DeleteAllAsync<Token>();

            var id = await getInstance().InsertAsync(token);

        }
        public static async Task AjoutEnciassement(View_TRS_ENCAISS item)
        {
            int id = 0;
            View_TRS_TIERS Tiers = await getInstance().Table<View_TRS_TIERS>().FirstOrDefaultAsync(e => e.CODE_TIERS == item.CODE_TIERS);
            item.NOM_TIERS = Tiers?.NOM_TIERS + " " + Tiers?.PRENOM_TIERS;

            View_BSE_COMPTE compte = await getInstance().Table<View_BSE_COMPTE>().FirstOrDefaultAsync(e => e.CODE_COMPTE == item.CODE_COMPTE);
            item.DESIGN_COMPTE = compte?.DESIGN_COMPTE;

            item.CREATED_BY = App.User.UserName;

            BSE_ENCAISS_MOTIFS motif = await getInstance().Table<BSE_ENCAISS_MOTIFS>().FirstOrDefaultAsync(e => e.CODE_MOTIF == item.CODE_MOTIF);
            item.DESIGN_MOTIF = motif?.DESIGN_MOTIF;

            decimal solde = await getSoldTiers(item.CODE_TIERS);
            item.OLD_SOLDE = solde;
            item.NEW_SOLDE = solde - item.TOTAL_ENCAISS;

            if (string.IsNullOrEmpty(item.CODE_ENCAISS))
            {
                id = await getInstance().InsertAsync(item);
                item.CODE_ENCAISS = await generateCode(item.CODE_TYPE, item.ID.ToString());
                item.NUM_ENCAISS = await generateNum(item.CODE_TYPE, item.ID.ToString());
            }

            await getInstance().UpdateAsync(item);

            await UpdateSoldTiersApresEncaiss(item.TOTAL_ENCAISS, item.CODE_TIERS, item.CODE_TYPE);

        }

        public static async Task UpdateStock(View_VTE_VENTE vente)
        {
            var stock = await getInstance().Table<View_STK_STOCK>().ToListAsync();

            foreach (var item in stock)
            {
                foreach (var items in vente.Details)
                {
                    if (item.ID_STOCK == items.ID_STOCK)
                    {
                        if (items.QUANTITE > 0)
                        {
                            item.OLD_QUANTITE = item.OLD_QUANTITE - items.QUANTITE;
                            item.QUANTITE = item.QUANTITE - items.QUANTITE;
                            await getInstance().UpdateAsync(item);
                        }
                    }
                }
            }
        }

        public static async Task UpdateTourneeDetail(View_VTE_VENTE vente)
        {
            string codeTourneeDetail = vente.MBL_CODE_TOURNEE_DETAIL;
            var item = await getInstance().Table<View_LIV_TOURNEE_DETAIL>().FirstOrDefaultAsync(x => x.CODE_DETAIL == codeTourneeDetail);
            if (item != null)
            {
                item.CODE_VENTE = vente.CODE_VENTE;
                item.CODE_ETAT = TourneeStatus.Delevred;
                item.DESIGNATION_ETAT = "Delevred";
                item.ETAT_COLOR = "#008000";
                item.SOLDE_TIERS = await getSoldTiers(item.CODE_TIERS);
                item.GPS_LATITUDE = vente.GPS_LATITUDE;
                item.GPS_LONGITUDE = vente.GPS_LATITUDE;
                await getInstance().UpdateAsync(item);
            }

        }

        public static async Task<bool> saveGPSToTiers(View_TRS_TIERS tiers)
        {
            if (!XpertHelper.IsNullOrEmpty(tiers) && !XpertHelper.IsNullOrEmpty(tiers.CODE_TIERS))
            {
                await getInstance().UpdateAsync(tiers);
                return true;
            }
            return false;
        }

        public static async Task UpdateTournee()
        {
            var tournee = await getInstance().Table<View_LIV_TOURNEE>().FirstOrDefaultAsync();
            if (tournee != null)
            {
                tournee.NBR_EN_DELEVRED = tournee.NBR_EN_DELEVRED + 1;
                await getInstance().UpdateAsync(tournee);
            }
        }

        public static async Task<SYS_USER> AuthUser(User _user)
        {
            bool validInformations = false;
            var password = XpertHelper.GetMD5Hash(_user.PassWord);
            SYS_USER User = await getInstance().Table<SYS_USER>().Where(x => x.PASS_USER == password).FirstOrDefaultAsync();
            if (XpertHelper.IsNotNullAndNotEmpty(User))
            {
                validInformations = true;
                return User;
            }
            return null;
        }

        public static async Task<SYS_USER> getUserInfo(string userid)
        {
            if (userid != null)
            {
                var Users = await getInstance().Table<SYS_USER>().ToListAsync();
                var user = Users.Where(x => x.ID_USER.ToLower() == userid.ToLower()).FirstOrDefault();
                if (user != null)
                {
                    return user;
                }
            }
            return null;
        }


        public static async Task<Token> getToken(User _user)
        {
            Token validToken = new Token();
            var Tokens = await getInstance().Table<Token>().ToListAsync();
            foreach (var item in Tokens)
            {
                if (item.userID.ToLower() == _user.UserName.ToLower())
                {
                    validToken = item;
                    return validToken;
                }
            }
            return null;
        }

        public static async Task<List<SYS_OBJET_PERMISSION>> getPermission()
        {
            var permission = await getInstance().Table<SYS_OBJET_PERMISSION>().ToListAsync();
            return permission;
        }

        public static async Task<List<View_BSE_TIERS_FAMILLE>> getFamille()
        {
            var Famille = await getInstance().Table<View_BSE_TIERS_FAMILLE>().ToListAsync();
            return Famille;
        }

        public static async Task<List<BSE_TABLE_TYPE>> getTypeTiers()
        {
            var TypeTiers = await getInstance().Table<BSE_TABLE_TYPE>().ToListAsync();
            return TypeTiers;
        }

        public static async Task<List<BSE_TABLE>> getSecteurs()
        {
            var Secteurs = await getInstance().Table<BSE_TABLE>().ToListAsync();
            return Secteurs;
        }





        public static async Task<List<View_VTE_VENTE_LOT>> getVenteDetails(string CodeVente)
        {
            List<View_VTE_VENTE_LOT> ventes = await getInstance().Table<View_VTE_VENTE_LOT>().ToListAsync();
            List<View_VTE_VENTE_LOT> VenteDetail = ventes.Where(e => e.CODE_VENTE == CodeVente).ToList();
            return VenteDetail;
        }

        public static async Task<SYS_CONFIGURATION_MACHINE> AjoutPrefix()
        {

            //Nom de la machine
            var userHost = Dns.GetHostName();
            //Adresse IP
            var userIp = Dns.GetHostEntry(userHost).AddressList[0].ToString();

            // Device Model (SMG-950U, iPhone10,6)
            var device = DeviceInfo.Model;

            var model = Environment.MachineName;

            // Device Name (Motz's iPhone)
            var deviceName = DeviceInfo.Name;
            var res = userIp.Split('.');
            string suffix = string.Empty;
            if (XpertHelper.IsNotNullAndNotEmpty(res))
            {
                suffix = res[res.Count() - 1];
            }
            //var deviceNameRandom = XpertHelper.RandomString(2);

            deviceName = deviceName + "/" + suffix;

            SYS_CONFIGURATION_MACHINE prefix = new SYS_CONFIGURATION_MACHINE();
            prefix.IP = userIp;
            prefix.MACHINE = deviceName;

            var bll = new SYS_MACHINE_CONFIG_Manager();
            var resMachine = await bll.AddMachine(prefix);
            if (resMachine)
            {
                return prefix;
            }
            else
            {
                return null;
            }
        }

        public static async Task<SYS_CONFIGURATION_MACHINE> getPrefix()
        {
            var prefix = await getInstance().Table<SYS_CONFIGURATION_MACHINE>().FirstOrDefaultAsync();
            if (prefix != null)
            {
                string deviceName = prefix.MACHINE;
                SYS_MACHINE_CONFIG_Manager bll = new SYS_MACHINE_CONFIG_Manager();
                var res = await bll.GetPrefix(deviceName);
                await getInstance().DeleteAllAsync<SYS_CONFIGURATION_MACHINE>();
                var id = await getInstance().InsertAsync(res);
                return res;
            }
            return null;
        }
        public static async Task AssignPrefix()
        {
            var prefix = await getInstance().Table<SYS_CONFIGURATION_MACHINE>().FirstOrDefaultAsync();
            if (!(string.IsNullOrEmpty(prefix.PREFIX)))
            {
                App.PrefixCodification = prefix.PREFIX;
            }
            else
            {
                //await DisplayAlert(AppResources.alrt_msg_Alert, "Veuillez configurer votre prefixe", AppResources.alrt_msg_Ok);
            }
        }
        public static async Task AssignMagasin()
        {
            var obj = await getInstance().Table<View_LIV_TOURNEE>().ToListAsync();
            if (obj != null && obj.Count > 0)
            {
                App.CODE_MAGASIN = obj[0].CODE_MAGASIN;
            }
        }

        public static bool TableExists(SQLiteAsyncConnection connection)
        {
            const string cmdText = "SELECT name FROM sqlite_master WHERE type='table' AND name=View_LIV_TOURNEE";
            var cmd = connection.ExecuteScalarAsync<string>(cmdText);
            if (cmd.Result.Length > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
            //return string.IsNullOrEmpty(cmd) ? false : true;
        }

     

     

     

      
    }
}
