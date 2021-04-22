using System.Collections.Generic;
using Xpert.Common.DAO;
using XpertMobileApp.Api.ViewModels;
using XpertMobileApp.Models;

namespace XpertMobileApp.ViewModels
{
    public class ChauffeurSelectorViewModel : CrudBaseViewModel2<BSE_CHAUFFEUR, BSE_CHAUFFEUR>
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

        protected override QueryInfos GetFilterParams()
        {
            base.GetFilterParams();

            this.AddCondition<BSE_CHAUFFEUR, string>(e => e.NOM_CHAUFFEUR, SearchedText);

            this.AddOrderBy<BSE_CHAUFFEUR, string>(e => e.NOM_CHAUFFEUR);

            return qb.QueryInfos;
        }
    }
}
