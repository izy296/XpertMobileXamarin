using System;
using System.Collections.Generic;
using System.Linq;
using XpertMobileApp.Api.ViewModels;
using XpertMobileApp.Models;

namespace XpertMobileApp.ViewModels
{
    public class AnnexTiersSelectorViewModel : CrudBaseViewModel<ACH_INFO_ANEX, VIEW_ACH_INFO_ANEX>
    {

        private List<VIEW_ACH_INFO_ANEX> currentEmballages;
        public List<VIEW_ACH_INFO_ANEX> CurrentEmballages
        {
            get
            {
                return currentEmballages;
            }
            set
            {
                clearInfos();
                currentEmballages = value;

                if (currentEmballages == null) return;
                foreach (var emb in currentEmballages)
                {
                    var itm = Items.Where(x => x.ID_LIGNE == emb.ID_LIGNE).FirstOrDefault();
                    if (itm != null)
                    {
                        itm.NOM_TIERS = emb.NOM_TIERS;
                        itm.QUANTITE_APPORT = emb.QUANTITE_APPORT;
                        itm.CODE_DOC = emb.CODE_DOC;
                    }
                }
            }
        }

        private void clearInfos()
        {
            CurrentEmballages.Clear();
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

        protected override Dictionary<string, string> GetFilterParams()
        {
            Dictionary<string, string> result = base.GetFilterParams();

            result.Add("searchText", SearchedText);
            return result;
        }
    }
}
