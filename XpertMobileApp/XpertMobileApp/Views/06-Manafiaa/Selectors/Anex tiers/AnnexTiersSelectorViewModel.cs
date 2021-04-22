using System.Collections.Generic;
using Xpert.Common.DAO;
using XpertMobileApp.Api.ViewModels;
using XpertMobileApp.Models;

namespace XpertMobileApp.ViewModels
{
    public class AnnexTiersSelectorViewModel : CrudBaseViewModel2<ACH_INFO_ANEX, VIEW_ACH_INFO_ANEX>
    {

        private bool iS_ACHAT;
        public bool IS_ACHAT
        {
            get
            {
                return iS_ACHAT;
            }
            set
            {

                iS_ACHAT = value;
                OnPropertyChanged("IS_ACHAT");
            }
        }

        private List<VIEW_ACH_INFO_ANEX> currentAnnex;
        public List<VIEW_ACH_INFO_ANEX> CurrentAnnex
        {
            get
            {
                return currentAnnex;
            }
            set
            {

                currentAnnex = value;

                if (currentAnnex == null) return;
                foreach (var emb in currentAnnex)
                {
                    Items.Add(emb);
                }
            }
        }


        public string SearchedText { get; set; } = "";

        public AnnexTiersSelectorViewModel(string title= "" )
        {
            Title = title;
        }

        protected override void OnAfterLoadItems(IEnumerable<VIEW_ACH_INFO_ANEX> list)
        {
            base.OnAfterLoadItems(list);
        }

        protected override QueryInfos GetFilterParams()
        {
            base.GetFilterParams();

            this.AddCondition<VIEW_ACH_INFO_ANEX, string>(e => e.NOM_TIERS, SearchedText);

            this.AddOrderBy<VIEW_ACH_INFO_ANEX, string>(e => e.NOM_TIERS);

            return qb.QueryInfos;
        }
    }
}
