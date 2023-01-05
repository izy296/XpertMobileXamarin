using Acr.UserDialogs;
using Rg.Plugins.Popup.Services;
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
using XpertMobileApp.Services;
using XpertMobileApp.SQLite_Managment;
using XpertMobileApp.Views;

namespace XpertMobileApp.ViewModels
{

    public class CommandesViewModel : CrudBaseViewModel2<VTE_VENTE, View_VTE_COMMANDE>
    {

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

        public View_TRS_TIERS SelectedTiers { get; set; }

        public EncaissDisplayType EncaissDisplayType { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; } = DateTime.Now;

        public ObservableCollection<View_BSE_COMPTE> Comptes { get; set; }
        public View_BSE_COMPTE SelectedCompte { get; set; }
        public Command LoadComptesCommand { get; set; }

        public ObservableCollection<View_BSE_COMPTE> Client { get; set; }
        public View_BSE_COMPTE SelectedClient { get; set; }
        public Command LoadClientsCommand { get; set; }

        public ObservableCollection<View_BSE_COMPTE> User { get; set; }
        public View_BSE_COMPTE SelectedUser { get; set; }
        public Command LoadUsersCommand { get; set; }

        public ObservableCollection<BSE_DOCUMENT_STATUS> Status { get; set; }
        public BSE_DOCUMENT_STATUS SelectedStatus { get; set; }

        public CommandesViewModel()
        {
            Title = AppResources.pn_Commandes;
            Status = new ObservableCollection<BSE_DOCUMENT_STATUS>();
            LoadItemsCommand = new Command(async () => await ExecuteLoadItemsCommand());
            if (App.Online)
                StartDate = DateTime.Now;
        }

        protected override string ContoleurName
        {
            get
            {
                return Constants.AppName == Apps.XPH_Mob ? "VTE_COMMANDE" : "VTE_COMMANDE_XCOM";
            }
        }
        //public override Task<List<View_VTE_COMMANDE>> SelectByPageFromSqlLite(QueryInfos filter)
        //{
        //    if (true)
        //    {
        //        //asyncTableQuery = UpdateDatabase.GetInstance().Table<View_VTE_COMMANDE>()
        //        //   .Where(e => e.CODE_TIERS=="").ToListAsync();
        //    }
        //    return base.SelectByPageFromSqlLite(filter);

        //}
        protected override QueryInfos GetFilterParams()
        {
            base.GetFilterParams();

            this.AddCondition<View_VTE_COMMANDE, DateTime?>(e => e.DATE_VENTE, Operator.BETWEEN_DATE, StartDate, EndDate);

            if (!string.IsNullOrEmpty(SelectedTiers?.CODE_TIERS))
                this.AddCondition<View_VTE_COMMANDE, string>(e => e.CODE_TIERS, SelectedTiers?.CODE_TIERS);

            if (!string.IsNullOrEmpty(SelectedCompte?.CODE_COMPTE))
                this.AddCondition<View_VTE_COMMANDE, string>(e => e.CREATED_BY, SelectedCompte?.CODE_COMPTE);

            if (!string.IsNullOrEmpty(SelectedStatus?.CODE_STATUS))
                this.AddCondition<View_VTE_COMMANDE, string>(e => e.STATUS_DOC, SelectedStatus?.CODE_STATUS);

            this.AddOrderBy<View_VTE_COMMANDE, DateTime?>(e => e.CREATED_ON, Sort.DESC);

            return qb.QueryInfos;
        }

        public async override Task<List<View_VTE_COMMANDE>> SelectByPageFromSqlLite(QueryInfos filter)
        {
            var res = await base.SelectByPageFromSqlLite(filter);
            
            if (StartDate == null)
            {
                res = res.Where(e => StartDate.Date.CompareTo(((DateTime)e.DATE_VENTE).Date) <= 0 && EndDate.Date.CompareTo(((DateTime)e.DATE_VENTE).Date) >= 0).ToList();
            }
            
            if (!string.IsNullOrEmpty(SelectedTiers?.CODE_TIERS))
                res = res.Where(e => e.CODE_TIERS == SelectedTiers?.CODE_TIERS).ToList();

            if (!string.IsNullOrEmpty(SelectedCompte?.CODE_COMPTE))
                res = res.Where(e => e.CREATED_BY == SelectedCompte?.CODE_COMPTE).ToList();

            if (!string.IsNullOrEmpty(SelectedStatus?.CODE_STATUS))
                res = res.Where(e => e.STATUS_DOC == SelectedStatus?.CODE_STATUS).ToList();

            return res;
        }
        protected override QueryInfos GetSelectParams()
        {
            base.GetSelectParams();

            this.AddSelect<View_VTE_COMMANDE, string>(e => e.TITRE_VENTE);
            this.AddSelect<View_VTE_COMMANDE, string>(e => e.CREATED_BY);
            this.AddSelect<View_VTE_COMMANDE, string>(e => e.NOM_TIERS);
            this.AddSelect<View_VTE_COMMANDE, decimal>(e => e.TOTAL_HT);
            this.AddSelect<View_VTE_COMMANDE, string>(e => e.CODE_VENTE);
            this.AddSelect<View_VTE_COMMANDE, DateTime?>(e => e.DATE_VENTE);

            return qb.QueryInfos;
        }
        protected override void OnAfterLoadItems(IEnumerable<View_VTE_COMMANDE> list)
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
                //Liste des Status commande
                if (Status.Count == 0)
                {
                    if (App.Online)
                    {
                        var itemsS = await WebServiceClient.GetStatusCommande();
                        foreach (var itemS in itemsS)
                        {
                            Status.Add(itemS);
                        }
                    }

                }


            }
            catch (Exception ex)
            {
                CustomPopup AlertPopup = new CustomPopup(WSApi2.GetExceptionMessage(ex), trueMessage: AppResources.alrt_msg_Ok);
                await PopupNavigation.Instance.PushAsync(AlertPopup);
            }
            finally
            {
                IsBusy = false;
            }
        }
    }

}
