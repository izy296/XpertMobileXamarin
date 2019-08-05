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

        private List<View_BSE_EMBALLAGE> currentEmballages;
        public List<View_BSE_EMBALLAGE> CurrentEmballages
        {
            get
            {
                return currentEmballages;
            }
            set
            {
                currentEmballages = value;

                if (currentEmballages == null) return;

                foreach (var emb in currentEmballages)
                {
                    var itm = Items.Where(x => x.CODE_EMBALLAGE == emb.CODE_EMBALLAGE).FirstOrDefault();
                    if (itm != null)
                    {
                        itm.QUANTITE_ENTREE = emb.QUANTITE_ENTREE;
                        itm.QUANTITE_SORTIE = emb.QUANTITE_SORTIE;
                        itm.QUANTITE_VIDE = emb.QUANTITE_VIDE;
                    }
                }
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
