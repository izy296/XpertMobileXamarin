using Acr.UserDialogs;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Extended;
using Xpert.Common.DAO;
using Xpert.Common.WSClient.Helpers;
using XpertMobileApp.Api;
using XpertMobileApp.Api.ViewModels;
using XpertMobileApp.DAL;
using XpertMobileApp.Models;
using XpertMobileApp.Services;
using XpertMobileApp.SQLite_Managment;

namespace XpertMobileApp.ViewModels
{

    public class SortieListViewModel : CrudBaseViewModel2<STK_SORTIE, View_STK_SORTIE>
    {


        public string TypeDoc { get; set; }

        public string RefDocum { get; set; } // Reference du document original (fournisseur)

        public string CODE_SORTIE { get; set; }

        decimal totalTurnover;
        public decimal TotalTurnover
        {
            get { return totalTurnover; }
            set { SetProperty(ref totalTurnover, value); }
        }

        decimal totalMargin;
        public decimal TotalMargin
        {
            get { return totalMargin; }
            set { SetProperty(ref totalMargin, value); }
        }

        public bool hasEditHeader
        {
            get
            {
                if (AppManager.HasAdmin) return true;

                bool result = false;
                if (AppManager.permissions != null)
                {
                    var obj = AppManager.permissions.Where(x => x.CodeObjet == "STK_SORTIE").FirstOrDefault();
                    result = obj != null && obj.AcUpdate > 0;
                }
                return result;
            }
        }

        public View_SYS_USER SelectedMotifs { get; set; }

        public EncaissDisplayType EncaissDisplayType { get; set; }
        public DateTime StartDate { get; set; } = DateTime.ParseExact("2020-03-21", "yyyy-MM-dd", System.Globalization.CultureInfo.InvariantCulture);
        public DateTime EndDate { get; set; } = DateTime.Now;


        public ObservableCollection<BSE_DOCUMENT_STATUS> Status { get; set; }
        public ObservableCollection<BSE_DOCUMENT_STATUS> Motif { get; set; }
        public BSE_DOCUMENT_STATUS SelectedStatus { get; set; }
        public BSE_DOCUMENT_STATUS SelectedMotif { get; set; }


        public ObservableCollection<View_BSE_COMPTE> Client { get; set; }
        public View_BSE_COMPTE SelectedClient { get; set; }
        public Command LoadClientsCommand { get; set; }


        public ObservableCollection<View_BSE_COMPTE> User { get; set; }
        public View_BSE_COMPTE SelectedUser { get; set; }
        public Command LoadUsersCommand { get; set; }



        public SortieListViewModel(string typeDoc)
        {
            Title = AppResources.pn_Sortie;
            TypeDoc = typeDoc;
            IsLoadExtrasBusy = false;

            Status = new ObservableCollection<BSE_DOCUMENT_STATUS>();
            Motif = new ObservableCollection<BSE_DOCUMENT_STATUS>();
            base.InitConstructor();
            LoadExtrasDataCommand = new Command(async () => await ExecuteLoadExtrasDataCommand());
        }

        async Task ExecuteLoadExtrasDataCommand()
        {
            if (IsLoadExtrasBusy)
                return;

            try
            {
                IsLoadExtrasBusy = true;
                //await autres filtres
                await ExecuteLoadSortieDeStock();
            }
            catch (Exception ex)
            {
                await UserDialogs.Instance.AlertAsync(WSApi2.GetExceptionMessage(ex), AppResources.alrt_msg_Alert,
                    AppResources.alrt_msg_Ok);
            }
            finally
            {
                IsLoadExtrasBusy = false;
            }
        }

        /// <summary>
        /// une function 
        /// </summary>
        /// <returns></returns>
        async Task<List<BSE_DOCUMENT_STATUS>> FillStatus()
        {
            List<BSE_DOCUMENT_STATUS> listAllElem = new List<BSE_DOCUMENT_STATUS>();
            BSE_DOCUMENT_STATUS allElem = new BSE_DOCUMENT_STATUS();
            allElem.CODE_STATUS = "";
            allElem.NAME = "";
            listAllElem.Add(allElem);

            allElem = new BSE_DOCUMENT_STATUS();
            allElem.CODE_STATUS = "";
            allElem.NAME = AppResources.txt_cloturee;
            listAllElem.Add(allElem);
            allElem = new BSE_DOCUMENT_STATUS();
            allElem.CODE_STATUS = "";
            allElem.NAME = AppResources.txt_Non_Cloturee;
            listAllElem.Add(allElem);

            return listAllElem;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        async Task<List<BSE_DOCUMENT_STATUS>> GetFilteredMotifs()
        {
            List<View_STK_SORTIE> listMotifs= await getMotifs();
            List<BSE_DOCUMENT_STATUS> listAllElem = new List<BSE_DOCUMENT_STATUS>();
            BSE_DOCUMENT_STATUS allElem;

            foreach (View_STK_SORTIE item in listMotifs)
            {
                allElem = new BSE_DOCUMENT_STATUS();
                allElem.CODE_STATUS = "";
                allElem.NAME = item.DESIGNATION_TYPE;
                listAllElem.Add(allElem);
            }

            return listAllElem;
        }

        async Task ExecuteLoadSortieDeStock()
        {
            // la premier fois le controler initialiser cette condition est n'est pas satissfait alors on pass a la section else
            if (App.Online)
            {
                //cette section execute le deuxiem fois la page rentrer
                try
                {
                    Status.Clear();
                    Motif.Clear();
                    var itemsC = await FillStatus();
                    var itemsB = await GetFilteredMotifs();


                    foreach (var itemC in itemsC)
                    {
                        Status.Add(itemC);
                    }

                    foreach (var itemC in itemsB)
                    {
                        Motif.Add(itemC);
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
                Status.Clear();
                Motif.Clear();
                var itemsC = await FillStatus();
                var itemsB = await GetFilteredMotifs();


                foreach (var itemC in itemsC)
                {
                    Status.Add(itemC);
                }

                foreach (var itemC in itemsB)
                {
                    Motif.Add(itemC);
                }
            }

        }

        protected override string ContoleurName
        {
            get
            {
                return "STK_SORTIE";
            }
        }

        /// <summary>
        /// function responsable a la recuperation des filtres pour un recherche avance
        /// </summary>
        /// <returns></returns>
        protected override QueryInfos GetFilterParams()
        {
            base.GetFilterParams();

            this.AddCondition<View_STK_SORTIE, DateTime?>(e => e.DATE_SORTIE, Operator.BETWEEN_DATE, StartDate, EndDate);
            
            if (!string.IsNullOrEmpty(SelectedMotifs?.ID_USER))
                this.AddCondition<View_STK_SORTIE, string>(e => e.CREATED_BY, SelectedMotifs?.ID_USER);

            if (!string.IsNullOrEmpty(SelectedMotif?.NAME))
                this.AddCondition<View_STK_SORTIE, string>(e => e.DESIGNATION_TYPE, SelectedMotif?.NAME);

            if (!string.IsNullOrEmpty(SelectedStatus?.NAME))
            {
                string value =  SelectedStatus?.NAME;
               
                if (value==AppResources.txt_cloturee)
                    this.AddCondition<View_STK_SORTIE,string> (e => e.STATUS_DOC, "42");
                else 
                    this.AddCondition<View_STK_SORTIE, string>(e => e.STATUS_DOC, "41");
            }

            //this.AddCondition<View_STK_SORTIE, string>(e => e.CODE_MOTIF, "ES10");

            this.AddOrderBy<View_STK_SORTIE, DateTime?>(e => e.DATE_SORTIE, Sort.DESC);

            return qb.QueryInfos;
        }

        /// <summary>
        /// function pour la recupuration de touts les champs DESIGNATION_TYPE sans repetition
        /// pour la remplir de list de motifs dans les filtres
        /// </summary>
        /// <returns></returns>
        async Task<List<View_STK_SORTIE>> getMotifs()
        {
            List<View_STK_SORTIE> Motifs = await WebServiceClient.getSortieMotifs();
            return Motifs;
        }

        /// <summary>
        /// function pour la selection de params reauis pour la requet SQL necissaire
        /// </summary>
        /// <returns></returns>
        protected override QueryInfos GetSelectParams()
        {
            base.GetSelectParams();

            this.AddSelect<View_STK_SORTIE, string>(e => e.CODE_SORTIE);
            this.AddSelect<View_STK_SORTIE, string>(e => e.NUM_SORTIE);
            this.AddSelect<View_STK_SORTIE, DateTime?>(e => e.DATE_SORTIE);
            this.AddSelect<View_STK_SORTIE, string>(e => e.CODE_TIERS);
            this.AddSelect<View_STK_SORTIE, string>(e => e.TYPE_SORTIE);
            this.AddSelect<View_STK_SORTIE, string>(e => e.NOTE_SORTIE);
            this.AddSelect<View_STK_SORTIE, DateTime?>(e => e.CREATED_ON);
            this.AddSelect<View_STK_SORTIE, string>(e => e.CREATED_BY);
            this.AddSelect<View_STK_SORTIE, DateTime?>(e => e.MODIFIED_ON);
            this.AddSelect<View_STK_SORTIE, string>(e => e.MODIFIED_BY);
            this.AddSelect<View_STK_SORTIE, int>(e => e.SENS_DOC);
            this.AddSelect<View_STK_SORTIE, string>(e => e.EXERCICE);
            this.AddSelect<View_STK_SORTIE, decimal>(e => e.TOTAL_ACHAT);
            this.AddSelect<View_STK_SORTIE, decimal>(e => e.TOTAL_PPA);
            this.AddSelect<View_STK_SORTIE, decimal>(e => e.TOTAL_SHP);
            this.AddSelect<View_STK_SORTIE, string>(e => e.TYPE_PAIEMENT);
            this.AddSelect<View_STK_SORTIE, bool>(e => e.SOLVABLE);
            this.AddSelect<View_STK_SORTIE, decimal>(e => e.TOTAL_PAYE);
            this.AddSelect<View_STK_SORTIE, decimal>(e => e.TOTAL_VENTE);
            this.AddSelect<View_STK_SORTIE, decimal>(e => e.TOTAL_SORTIE);
            this.AddSelect<View_STK_SORTIE, string>(e => e.STATUS_DOC);
            this.AddSelect<View_STK_SORTIE, DateTime?>(e => e.CLOTURE_ON);
            this.AddSelect<View_STK_SORTIE, string>(e => e.CLOTURED_BY);
            this.AddSelect<View_STK_SORTIE, string>(e => e.TIERS_NomC);
            this.AddSelect<View_STK_SORTIE, string>(e => e.DESIGNATION_TYPE);

            return qb.QueryInfos;
        }

        protected override void OnAfterLoadItems(IEnumerable<View_STK_SORTIE> list)
        {
            base.OnAfterLoadItems(list);

            int i = 0;
            foreach (var item in list)
            {
                i += 1;
                (item as BASE_CLASS).Index = i;
            }
        }

        async Task ExecuteLoadItemsCommand()
        {
            if (IsBusy)
                return;

            try
            {
                Items.Clear();

                // liste des ventes
                await Items.LoadMoreAsync();
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
    }

}
