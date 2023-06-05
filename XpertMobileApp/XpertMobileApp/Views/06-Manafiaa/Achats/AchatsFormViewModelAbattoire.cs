using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using XpertMobileApp.Api;
using XpertMobileApp.DAL;
using XpertMobileApp.Models;
using XpertMobileApp.ViewModels;

namespace XpertMobileApp.Views.Achats
{
    public class AchatsViewModelAbattoire : ItemRowsDetailViewModel<View_ACH_DOCUMENT, View_ACH_DOCUMENT_DETAIL>
    {
        private List<String> immatriculationList;
        public List<String> ImmatriculationList
        {
            get { return immatriculationList; }
            set { SetProperty(ref immatriculationList, value); }
        }

        public class AchRecMotifsObject
        {
            public string DESIGNATION { get; set; }
            public string CODE_MOTIF { get; set; }
        }

        private List<AchRecMotifsObject> motifs { get; set; }
        public List<AchRecMotifsObject> Motifs
        {
            get { return motifs; }
            set
            {
                motifs = value;
                OnPropertyChanged("Motifs");
            }
        }

        public bool hasEditDetails
        {
            get
            {
                if (AppManager.HasAdmin && Constants.AppName != Apps.XCOM_Abattoir) return true;
                bool result = false;
                if (AppManager.permissions != null)
                {
                    var obj = AppManager.permissions.Where(x => x.CodeObjet == "ACH_UPDATE_DETAIL").FirstOrDefault();
                    result = obj != null && obj.AcUpdate > 0;
                }
                return result;
            }
        }

        public bool hasInsertDetails
        {
            get
            {
                if (AppManager.HasAdmin && Constants.AppName != Apps.XCOM_Abattoir) return true;
                bool result = false;
                if (AppManager.permissions != null)
                {
                    var obj = AppManager.permissions.Where(x => x.CodeObjet == "ACH_UPDATE_DETAIL").FirstOrDefault();
                    result = obj != null && obj.AcInsert > 0;
                }
                return result;
            }
        }

        public bool hasExaminationVeterinaire
        {
            get
            {
                bool result = false;
                if (AppManager.permissions != null)
                {
                    var obj = AppManager.permissions.Where(x => x.CodeObjet == "ACH_VALIDATE_VETERINAIRE").FirstOrDefault();
                    result = obj != null && obj.AcSelect > 0;
                }
                return result;
            }
        }

        public bool hasEditPrice
        {
            get
            {
                if (AppManager.HasAdmin && Constants.AppName != Apps.XCOM_Abattoir) return true;
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
                if (AppManager.HasAdmin && Constants.AppName != Apps.XCOM_Abattoir) return true;
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

        private AchRecMotifsObject selectedMotif;
        public AchRecMotifsObject SelectedMotif
        {
            get { return selectedMotif; }
            set { SetProperty(ref selectedMotif, value); }
        }

        public AchatsViewModelAbattoire(View_ACH_DOCUMENT obj, string itemId) : base(obj, itemId)
        {
            /*
            ImmatriculationList = new List<string>();
            ImmatriculationList.Add("DDD-23234");
            ImmatriculationList.Add("AAA-09809");
            ImmatriculationList.Add("CCC-45654");
            ImmatriculationList.Add("TTT-09854");
            */
            Motifs = new List<AchRecMotifsObject>() {new AchRecMotifsObject()
            {
                DESIGNATION = "Production",
                CODE_MOTIF = PesageMotifs.PesageForProduction
            },
            new AchRecMotifsObject()
            {
                DESIGNATION = "Prestation",
                CODE_MOTIF = PesageMotifs.PesagePrestation
            }};

            SelectedMotif = Motifs.Where(elm=>elm.CODE_MOTIF == Item.CODE_MOTIF).FirstOrDefault();
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
