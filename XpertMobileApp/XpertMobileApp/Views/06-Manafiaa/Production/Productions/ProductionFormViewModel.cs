using System;
using System.Collections.Generic;
using System.Linq;
using XpertMobileApp.Api;
using XpertMobileApp.DAL;
using XpertMobileApp.Models;
using XpertMobileApp.ViewModels;

namespace XpertMobileApp.ViewModels
{
    public class ProductionFormViewModel : ItemRowsDetailViewModel<View_PRD_AGRICULTURE, View_PRD_AGRICULTURE_DETAIL>
    {
        private List<String> immatriculationList;        
        public List<String> ImmatriculationList {
            get { return immatriculationList; }
            set { SetProperty(ref immatriculationList, value); }
        }

        public bool hasEditDetails
        {
            get
            {
                if (AppManager.HasAdmin) return true;
                bool result = false;
                if (AppManager.permissions != null)
                {
                    var obj = AppManager.permissions.Where(x => x.CodeObjet == "ACH_UPDATE_DETAIL").FirstOrDefault();
                    result = obj != null && obj.AcUpdate > 0;
                }
                return result;
            }            
        }

        public bool hasEditPrice
        {
            get
            {
                if (AppManager.HasAdmin) return true;
                bool result = false;
                if (AppManager.permissions != null)
                {
                    var obj = AppManager.permissions.Where(x => x.CodeObjet == "ACH_UPDATE_PRIX_HT").FirstOrDefault();
                    result = obj != null && obj.AcUpdate > 0;
                }
                return result;
            }
        }

        public bool hasEditHeader
        {
            get
            {
                if (AppManager.HasAdmin) return true;
                bool result = false;
                if (AppManager.permissions != null)
                {
                    var obj = AppManager.permissions.Where(x => x.CodeObjet == "ACH_UPDATE_ENTETE").FirstOrDefault();
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

        public ProductionFormViewModel(View_PRD_AGRICULTURE obj, string itemId) : base(obj, itemId)
        {
            /*
            ImmatriculationList = new List<string>();
            ImmatriculationList.Add("DDD-23234");
            ImmatriculationList.Add("AAA-09809");
            ImmatriculationList.Add("CCC-45654");
            ImmatriculationList.Add("TTT-09854");
            */
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
