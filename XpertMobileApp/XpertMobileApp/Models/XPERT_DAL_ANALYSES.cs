using System;
using System.Collections.Generic;
using System.Text;

namespace XpertMobileApp.Models
{
    public partial class TDB_SIMPLE_INDICATORS
    {
        public string CODE_ANALYSE { get; set; } // varchar(15)
        public string Title { get; set; } // varchar(50)
        public string Query { get; set; } // char(3000)
        public string Profils { get; set; } // char(15)
        public string ID_DetailsForm { get; set; } // char(50)
        public string Color { get; set; } // char(50)
        public byte[] Icon { get; set; } // varchar(50)
        public int ORDRE { get; set; }


        public bool IsCustom { get; set; }

        public decimal CustomValue { get; set; }
    }

    public class SAMMUARY
    {
        public string key { get; set; } // varchar(50)
        public string Title { get; set; } // varchar(50)
        public decimal Value { get; set; } // char(3000)
    }
}
