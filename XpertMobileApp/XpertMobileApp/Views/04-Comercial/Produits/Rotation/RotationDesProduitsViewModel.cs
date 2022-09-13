using Acr.UserDialogs;
using Rg.Plugins.Popup.Services;
using Syncfusion.Linq;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xpert.Common.DAO;
using Xpert.Common.WSClient.Helpers;
using XpertMobileApp.Api;
using XpertMobileApp.Api.Managers;
using XpertMobileApp.Api.Services;
using XpertMobileApp.Api.ViewModels;
using XpertMobileApp.DAL;
using XpertMobileApp.Models;
using XpertMobileApp.Services;
using XpertMobileApp.Views;

namespace XpertMobileApp.ViewModels
{

    public class RotationDesProduitsViewModel : CrudBaseViewModel2<RotationDesProduits, RotationDesProduits>
    {


        public RotationDesProduitsViewModel()
        {
            Title = AppResources.pn_RotationDesProduits;

        }

        protected override QueryInfos GetFilterParams()
        {
            base.GetFilterParams();
            string endDATE = EndDate?.ToString("yyyyddMM");
            string startDATE = BeginDate?.ToString("yyyyddMM");
            if (endDATE == null && startDATE == null)
            {
                endDATE = DateTime.Now.ToString("yyyyddMM");
                startDATE = DateTime.Now.ToString("yyyyddMM");
            }
            this.AddCondition($"'DateDebut' = '{startDATE}'");

            this.AddCondition($"'DateFin' = '{endDATE}'");

            if (!XpertHelper.IsNullOrEmpty(SearchedText))
            {
                this.AddCondition($"'Code_Produit' = '{SearchedText}'");
            }
            //
            this.AddOrderBy<RotationDesProduits, string>(e => e.DESIGNATION);
            return qb.QueryInfos;
        }

        protected override string ContoleurName
        {
            get
            {
                return Constants.AppName == Apps.XCOM_Mob ? "RotationDesProduits" : "RotationDesProduits";
            }
        }


        protected override QueryInfos GetSelectParams()
        {
            base.GetSelectParams();
            this.AddSelect<RotationDesProduits, string>(e => e.DESIGNATION);
            this.AddSelect<RotationDesProduits, decimal>(e => e.QTE_ENTREE);
            this.AddSelect<RotationDesProduits, decimal>(e => e.QTE_SORTIE);

            return qb.QueryInfos;

        }

        protected override void OnAfterLoadItems(IEnumerable<RotationDesProduits> list)
        {
            base.OnAfterLoadItems(list);

            int i = 0;
            foreach (var item in list)
            {
                i += 1;
                (item as BASE_CLASS).Index = i;
            }
        }

        /// <summary>
        /// remplacer la valeur par défaut ExecuteLoadItemsCommand pour redéfinir la fonction OnCanLoadMore 
        /// sur l'événement d'actualisation de la page ou en appuyant sur le bouton Appliquer sur le filtre
        /// après avoir montré un produit qui est le résultat d'un code-barres scanné
        /// </summary>
        /// <returns></returns>
        internal override Task ExecuteLoadItemsCommand()
        {
            return base.ExecuteLoadItemsCommand();
        }

        #region filters data
        private string searchedText;
        public string SearchedText
        {
            get { return searchedText; }
            set { SetProperty(ref searchedText, value); }
        }

        private DateTime? beginDate;
        public DateTime? BeginDate
        {
            get { return beginDate; }
            set { SetProperty(ref beginDate, value); }
        }

        private DateTime? endDate;
        public DateTime? EndDate
        {
            get { return endDate; }
            set { SetProperty(ref endDate, value); }
        }

        public override void ClearFilters()
        {
            base.ClearFilters();
            searchedText = "";
        }

        #endregion
    }

}
