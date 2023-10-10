using System;

namespace XpertMobileApp.DAL
{
    public partial class INFO_PROMOTION_DISPLAY_PRICE
    {
        public string DESIGNATION_PRODUIT { get; set; }
        public bool IS_PROMOTION { get; set; }
        public decimal PRODUIT_PRIX { get; set; }
        public DateTime? PROMO_DATE_FIN { get; set; }
        public decimal? PROMO_QTE_MIN { get; set; }
        public decimal? PROMO_QTE_RESTANTE { get; set; }
        public decimal? PROMO_PRIX { get; set; }
    }
}
