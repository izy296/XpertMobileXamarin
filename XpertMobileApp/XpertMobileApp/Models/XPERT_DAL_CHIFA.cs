using System;
using System.Collections.Generic;
using System.Text;

namespace XpertMobileApp.DAL
{

    public partial class CFA_BORDEREAU : BASE_CLASS
    {
        public string CODE_BORDEREAU { get; set; } // nvarchar(50)
        public string NUM_BORDEREAU { get; set; } // nvarchar(10)
        public string CODE_ETAT { get; set; } // varchar(5)
        public string ID_CENTRE { get; set; } // varchar(10)

        public string OBSERVATION { get; set; } // nvarchar(250)
        public DateTime? DATE_CLOTURE { get; set; } // datetime(3)
        public DateTime? DATE_DEPOT { get; set; } // datetime(3)
        public DateTime? DATE_PAIEMENT { get; set; } // datetime(3)
        public DateTime? DATE_VALIDATION { get; set; } // datetime(3)
        public DateTime? CREATED_ON { get; set; } // datetime(3)
        public string CREATED_BY { get; set; } // varchar(200)
        public DateTime? MODIFIED_ON { get; set; } // datetime(3)
        public string MODIFIED_BY { get; set; } // varchar(200)
        public bool IS_IMPORTED { get; set; }

        //-------------------------
        public decimal MONT_ASSURE { get; set; }
        public decimal SUM_MAJORATION { get; set; }
        public decimal TOTAL_FACTURES { get; set; }

        public decimal TOTAL_TO_PAY { get; set; }

        public decimal TOTAL_EXCLUDED { get; set; }

        public decimal TOTAL_OFFICINE_PAYE { get; set; }

        public decimal TOTAL_AJUSTEMENT { get; set; }

        public int NBR_FACTS_REFUSED { get; set; }

        public int NBR_FACTS_IGNORED { get; set; }

        public int NBR_FACTS_ACCEPTED { get; set; }

        public int NBR_FACTS_DELETED { get; set; }
        public int NBR_FACTS { get; set; }
    }


    public partial class CFA_ETAT
    {

        public string NUM_ETAT { get; set; } // varchar(2)
        public string CODE_ETAT { get; set; } // varchar(3)
        public string DESIGN_ETAT { get; set; } // varchar(500)
        public string TYPE_ETAT { get; set; } // varchar(10)
    }


    public partial class View_CFA_BORDEREAU : CFA_BORDEREAU
    {
        public string DESIGN_CENTRE { get; set; } // nvarchar(500)
        public decimal MONT_BORD { get; set; } // money(19,4)
        public string DESIGN_ETAT { get; set; } // varchar(500)
        public string Encaissements { get; set; } // nvarchar

        public int? NbrDaysToPaye
        {
            get
            {
                if (DATE_DEPOT != null && DATE_PAIEMENT != null)
                {
                    return Convert.ToInt32((Convert.ToDateTime(DATE_PAIEMENT) - Convert.ToDateTime(DATE_DEPOT)).TotalDays);
                }
                else
                {
                    return null;
                }
            }
        }

        public int? NbrDaysFromState
        {
            get
            {
                if (DATE_VALIDATION != null && CODE_ETAT == BordereauStatus.Validated)
                {
                    return Convert.ToInt32((DateTime.Now - Convert.ToDateTime(DATE_VALIDATION)).TotalDays);
                }
                if (DATE_DEPOT != null && CODE_ETAT == BordereauStatus.Deposed)
                {
                    return Convert.ToInt32((DateTime.Now - Convert.ToDateTime(DATE_DEPOT)).TotalDays);
                }
                else
                {
                    return null;
                }
            }
        }

        public string InfosTime
        {
            get
            {
                if (DATE_VALIDATION != null && CODE_ETAT == BordereauStatus.Validated)
                {
                    int days = Convert.ToInt32((DateTime.Now - Convert.ToDateTime(DATE_VALIDATION)).TotalDays);
                    return string.Format("Validé depuis {0} jours", days);
                }
                if (DATE_DEPOT != null && CODE_ETAT == BordereauStatus.Deposed)
                {
                    int days = Convert.ToInt32((DateTime.Now - Convert.ToDateTime(DATE_DEPOT)).TotalDays);
                    return string.Format("Déposé depuis {0} jours", days);
                }
                if (DATE_CLOTURE != null && CODE_ETAT == BordereauStatus.Clotured)
                {
                    int days = Convert.ToInt32((DateTime.Now - Convert.ToDateTime(DATE_CLOTURE)).TotalDays);
                    return string.Format("Clôturé depuis {0} jours", days);
                }
                else
                {
                    return "";
                }
            }
        }

        public decimal BrdEncours
        {
            get
            {

                return CODE_ETAT == BordereauStatus.EnCours ? TOTAL_TO_PAY : 0;
            }
        }

        public decimal BrdClotured
        {
            get
            {
                return CODE_ETAT == BordereauStatus.Clotured ? TOTAL_TO_PAY : 0;
            }
        }

        public decimal BrdDeposed
        {
            get
            {
                return CODE_ETAT == BordereauStatus.Deposed ? TOTAL_TO_PAY : 0;
            }
        }

        public decimal BrdValidted
        {
            get
            {
                return CODE_ETAT == BordereauStatus.Validated ? TOTAL_TO_PAY : 0;
            }
        }

        public decimal BrdPayed
        {
            get
            {
                return CODE_ETAT == BordereauStatus.Payed || CODE_ETAT == BordereauStatus.Archived ? TOTAL_TO_PAY : 0;
            }
        }

        public decimal ECART
        {
            get
            {

                return TOTAL_AJUSTEMENT + TOTAL_EXCLUDED;
            }
        }

        #region mobile
        public bool BrdEncoursPassed
        {
            get
            {
                return Convert.ToInt32(CODE_ETAT) >= Convert.ToInt32(BordereauStatus.EnCours);
            }
        }

        public bool BrdCloturedPassed
        {
            get
            {
                return Convert.ToInt32(CODE_ETAT) >= Convert.ToInt32(BordereauStatus.Clotured);
            }
        }

        public bool BrdDeposedPassed
        {
            get
            {
                return Convert.ToInt32(CODE_ETAT) >= Convert.ToInt32(BordereauStatus.Deposed);
            }
        }

        public bool BrdValidtedPassed
        {
            get
            {
                return Convert.ToInt32(CODE_ETAT) >= Convert.ToInt32(BordereauStatus.Validated);
            }
        }

        public bool BrdPayedPassed
        {
            get
            {
                return Convert.ToInt32(CODE_ETAT) >= Convert.ToInt32(BordereauStatus.Payed);
            }
        }
        #endregion
    }

    public partial class View_CFA_BORDEREAUX_CHIFA : CFA_BORDEREAU
    {
        public decimal MONT_MAJORATION { get; set; }
        public decimal MONT_OFFICINE { get; set; }
        public decimal MONT_FACTURE { get; set; }
        public decimal MONT_ACHAT { get; set; }
        public decimal TOTAL { get; set; }
        public decimal TOTAL_CHIFA { get; set; }
        public decimal TOTAL_CASNOS { get; set; }
        public decimal TOTAL_PHARMNOS { get; set; }
    }

    public partial class CFA_CENTRES
    {
        public string CODE { get; set; } // varchar(10)
        public string DESIGNATION { get; set; } // nvarchar(500)
        public int TYPE { get; set; } // tinyint(3)
    }

    public static class BordereauStatus
    {
        public static string EnCours { get { return "01"; } }
        public static string Clotured { get { return "02"; } }
        public static string Deposed { get { return "03"; } }
        public static string Validated { get { return "04"; } }
        public static string Payed { get { return "05"; } }
        public static string Archived { get { return "06"; } }
    }
    public partial class CFA_BENEFICIAIRE : BASE_CLASS
    {
        public string NO_ASSURE { get; set; } // char(12)
        public string RANG_AD { get; set; } // char(2)
        public string NOM { get; set; } // varchar(20)        
        public string PRENOM { get; set; } // varchar(20)        
        public string NOM_ASSURE { get; set; } // varchar(20)        
        public string PRENOM_ASSURE { get; set; } // varchar(20)        
    }

    public partial class View_CFA_BENEFICIAIRE : CFA_BENEFICIAIRE
    {

    }

    public partial class View_CFA_MOBILE_DETAIL_FACTURE : BASE_CLASS
    {
        public double SHP { get; set; }
        public double PPA { get; set; }
        public int DUREE_TRAIT { get; set; }
        public int QUANTITE { get; set; }
        public double PRIX_VENTE { get; set; }
        public string NUM_BOURDEREAU { get; set; }
        public DateTime DATE_FACTURE { get; set; }
        public int PSYCHOTHROPE { get; set; }
        public string NUM_FACTURE { get; set; }
        public double MONT_MAJORATION { get; set; }
        public string DESIGNATION_PRODUIT { get; set; }
        public string TYPE_PRODUIT { get; set; }
        public double TARIF { get; set; }
        public string DOSAGE { get; set; }
        public string DESIGN_FORME { get; set; }
        public double MONT_ASSURE { get; set; }
        public string NUM_ASSURE { get; set; }
        public string NOMC_TIERS { get; set; }
        public string LOT { get; set; }
        public string RAND_AD { get; set; }
        public string CODE_TIERS { get; set; }
        public string CODE_CLIENT { get; set; }
        public decimal MONT_FACTURE { get; set; }
        public decimal TOTAL_FACTURES { get; set; }
        public decimal MONTANT_FACTURES { get; set; }
        public bool isPsychotrope
        {
            get
            {
                if (this.PSYCHOTHROPE == 0)
                {
                    return false;
                }
                return true;
            }
            set
            {
                isPsychotrope = value;
            }
        }

    }

    public partial class CFA_MOBILE_DETAIL_FACTURE : View_CFA_MOBILE_DETAIL_FACTURE
    {

    }

}

