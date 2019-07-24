using System.Collections.Generic;
using XpertMobileApp.Api.ViewModels;
using XpertMobileApp.Models;

namespace XpertMobileApp.ViewModels
{
    public class ChauffeurSelectorViewModel : CrudBaseViewModel<BSE_CHAUFFEUR, BSE_CHAUFFEUR>
    {
        public string SearchedText { get; set; } = "";

        public ChauffeurSelectorViewModel(string title= "" )
        {
            Title = title;
        }

        protected override void OnAfterLoadItems(IEnumerable<BSE_CHAUFFEUR> list)
        {
            base.OnAfterLoadItems(list);

            BSE_CHAUFFEUR fElem = new BSE_CHAUFFEUR();
            fElem.NOM_CHAUFFEUR = "";
            fElem.CODE_CHAUFFEUR = "";
            Items.Insert(0, fElem);
        }

        protected override Dictionary<string, string> GetFilterParams()
        {
            Dictionary<string, string> result = base.GetFilterParams();

            result.Add("searchText", SearchedText);
            return result;
        }
    }
}
