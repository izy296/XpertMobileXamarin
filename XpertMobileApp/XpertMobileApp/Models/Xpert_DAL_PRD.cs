using System;

namespace XpertMobileApp.DAL
{
    public partial class INFO_PROMOTION_DISPLAY_PRICE
    {
        public string DESIGNATION_PRODUIT { get; set; }
        public decimal PRODUIT_RPIX { get; set; }
        public bool IS_PROMOTION { get; set; }
        public decimal? PROMO_RPIX { get; set; }
        public DateTime? PROMO_DATE_FIN { get; set; }
        public int? PROMO_QTE_RESTANTE { get; set; }
        public int? PROMO_QTE_MIN { get; set; }
    }
}
