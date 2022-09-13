using System;
using System.Collections.Generic;
using System.Text;

namespace XpertMobileApp.Models
{
    public partial class BSE_MAGASINS
    {
        public string CODE { get; set; }
        public string DESIGNATION { get; set; }
        public bool AUTORISER_VENTE { get; set; } // bit
    }
    public partial class View_BSE_MAGASIN : BSE_MAGASINS
    {
        public string CODE_MAGASIN { get; set; } // varchar(10)
        public string DESIGN_MAGASIN { get; set; } // varchar(100)
    }

    public partial class BSE_TIERS_TYPE
    {
        public string CODE_TYPE { get; set; }

        public string DESIGNATION_TYPE { get; set; }
    }
}

