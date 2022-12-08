using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xpert.Common.DAO;
using XpertMobileApp.Api.Services;
using XpertMobileApp.Api.ViewModels;
using XpertMobileApp.DAL;
using XpertMobileApp.Models;
using XpertMobileApp.SQLite_Managment;
using XpertMobileApp.Views;
using XpertMobileApp.Views.Helper;

namespace XpertMobileApp.ViewModels
{
    public class LotSelectorLivraisonViewModel : CrudBaseViewModel2<STK_STOCK, View_STK_STOCK>
    {

        public string SearchedText { get; set; } = "";

        public string CodeTiers { get; set; } = "";

        public string AutoriserReception { get; set; } = "";

        private decimal totalSelected;
        public decimal TotalSelected
        {
            get
            {
                return totalSelected;
            }
            set
            {
                totalSelected = value;
                OnPropertyChanged("TotalSelected");
                OnPropertyChanged("SelectionsInfos");
            }
        }

        private View_TRS_TIERS tier;
        public View_TRS_TIERS Tier
        {
            get { return tier; }
            set { tier = value; }
        }

        public string SelectionsInfos
        {
            get
            {
                return "Total : " + TotalSelected.ToString("N2") + "\r\n" + "Total Qte : " + Items.Sum(e => (e.SelectedQUANTITE + Manager.TotalQuantiteUnite(e.UnitesList)));
            }
        }

        public LotSelectorLivraisonViewModel(string title = "")
        {
            Title = title;
        }

        protected async override void OnAfterLoadItems(IEnumerable<View_STK_STOCK> list)
        {
            base.OnAfterLoadItems(list);

            int i = 0;
            foreach (var item in list)
            {
                i += 1;
                (item as BASE_CLASS).Index = i;
            }
        }

        public override async Task<List<View_STK_STOCK>> SelectByPageFromSqlLite(QueryInfos filter)
        {
            List<View_STK_STOCK> result;
            result = await SQLite_Manager.GetProduitPrixUniteByCodeFamille(Tier.CODE_FAMILLE) as List<View_STK_STOCK>;
            if (!XpertHelper.IsNullOrEmpty(SearchedText))
                result = result.Where(e => e.DESIGNATION_PRODUIT.Contains(SearchedText)).ToList();
            return result;
        }

        protected override QueryInfos GetFilterParams()
        {
            base.GetFilterParams();
            this.AddCondition<View_STK_STOCK, string>(e => e.DESIGNATION_PRODUIT, Operator.LIKE_ANY, SearchedText);

            if (!string.IsNullOrEmpty(App.Settings.DefaultMagasinVente))
            {
                this.AddCondition<View_STK_STOCK, string>(e => e.CODE_MAGASIN, App.Settings.DefaultMagasinVente);
            }
            this.AddCondition<View_STK_STOCK, bool>(e => e.IS_BLOCKED, 0);
            this.AddCondition<View_STK_STOCK, decimal>(e => e.QUANTITE, Operator.GREATER, 0);

            // this.AddCondition<View_STK_STOCK, bool>(e => e.IS_VALID, 1);

            this.AddOrderBy<View_STK_STOCK, string>(e => e.DESIGNATION_PRODUIT);

            qb.QueryInfos.Param1 = CodeTiers;

            return qb.QueryInfos;
        }
    }
}
