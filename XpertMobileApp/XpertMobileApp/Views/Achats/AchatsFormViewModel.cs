using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using XpertMobileApp.DAL;
using XpertMobileApp.Models;
using XpertMobileApp.ViewModels;

namespace XpertMobileApp.Views.Achats
{
    public class AchatsFormViewModel : ItemRowsDetailViewModel<View_ACH_DOCUMENT, View_ACH_DOCUMENT_DETAIL>
    {

        public bool hasEditDetails
        {
            get
            {
                bool result = false;
                if (App.permissions != null)
                {
                    var obj = App.permissions.Where(x => x.CodeObjet == "ACH_UPDATE_DETAIL").FirstOrDefault();
                    result = obj != null && obj.AcUpdate > 0;
                }
                return result;
            }            
        }

        public bool hasEditPrice
        {
            get
            {
                bool result = false;
                if (App.permissions != null)
                {
                    var obj = App.permissions.Where(x => x.CodeObjet == "ACH_UPDATE_PRIX_HT").FirstOrDefault();
                    result = obj != null && obj.AcUpdate > 0;
                }
                return result;
            }
        }

        public bool hasEditHeader
        {
            get
            {
                bool result = false;
                if (App.permissions != null)
                {
                    var obj = App.permissions.Where(x => x.CodeObjet == "ACH_UPDATE_ENTETE").FirstOrDefault();
                    result = obj != null && obj.AcUpdate > 0;
                }
                return result;
            }
        }

        private BSE_CHAUFFEUR selectedChauffeur;
        public BSE_CHAUFFEUR SelectedChauffeur
        {
            get { return selectedChauffeur; }
            set { SetProperty(ref selectedChauffeur, value); }
        }

        private string selectedImmat;
        public string SelectedImmat
        {
            get { return selectedImmat; }
            set { SetProperty(ref selectedImmat, value); }
        }

        public AchatsFormViewModel(View_ACH_DOCUMENT obj, string itemId) : base(obj, itemId)
        {

        }

        public decimal CalculatePeseeNet(decimal peseeBrute, List<View_BSE_EMBALLAGE> emballages)
        {
            decimal result = peseeBrute;
            foreach (var emballage in emballages)
            {
                result = result - (emballage.QUANTITE_ENTREE * emballage.QUANTITE_UNITE);
            }

            return result;
        }

        public decimal GetPeseeBrute(decimal pEntree, decimal pSortie, List<View_BSE_EMBALLAGE> emballages)
        {
            decimal result = 0;
            decimal qteBruteInitial = pEntree - pSortie;

            foreach (var emballage in emballages)
            {
                qteBruteInitial = qteBruteInitial + (emballage.QTE_DEFF * emballage.QUANTITE_UNITE);
            }

            result = qteBruteInitial;

            return result;
        }

    }
}
