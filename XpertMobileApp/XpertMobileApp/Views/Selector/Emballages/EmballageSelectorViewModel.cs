using System;
using System.Collections.Generic;
using System.Linq;
using XpertMobileApp.Api.ViewModels;
using XpertMobileApp.Models;

namespace XpertMobileApp.ViewModels
{
    public class EmballageSelectorViewModel : CrudBaseViewModel<BSE_EMBALLAGE, View_BSE_EMBALLAGE>
    {

        bool iS_PRINCIPAL = false;
        public bool IS_PRINCIPAL
        {
            get { return iS_PRINCIPAL; }
            set
            {
                iS_PRINCIPAL = value;
                OnPropertyChanged("IS_PRINCIPAL");
            }
        }

        bool iS_SALES = false;
        public bool IS_SALES
        {
            get { return iS_SALES; }
            set
            {
                iS_SALES = value;
                OnPropertyChanged("IS_SALES");
            }
        }

        private List<View_BSE_EMBALLAGE> currentEmballages;
        public List<View_BSE_EMBALLAGE> CurrentEmballages
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
                    var itm = Items.Where(x => x.CODE == emb.CODE_EMBALLAGE).FirstOrDefault();
                    if (itm != null)
                    {
                        itm.QUANTITE_ENTREE = emb.QUANTITE_ENTREE;
                        itm.QUANTITE_SORTIE = emb.QUANTITE_SORTIE;
                        itm.QUANTITE_VIDE = emb.QUANTITE_VIDE;
                    }
                }
            }
        }

        private void clearInfos()
        {
            foreach (var itm in Items)
            {
                itm.QUANTITE_ENTREE = 0;
                itm.QUANTITE_SORTIE = 0;
                itm.QUANTITE_VIDE = 0;
            }
        }

        public string SearchedText { get; set; } = "";

        public EmballageSelectorViewModel(string title= "" )
        {
            Title = title;
        }

        protected override void OnAfterLoadItems(IEnumerable<View_BSE_EMBALLAGE> list)
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
