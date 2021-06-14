using System;
using System.Collections.Generic;
using System.Text;
using SQLite;
using Xpert.Common.WSClient.Model;

namespace XpertMobileApp.Models
{
    public partial class SYS_USER
    {
        [PrimaryKey, AutoIncrement]
        public int ID { get; set; }
        public string ID_USER { get; set; } // varchar(32)
        public string PASS_USER { get; set; } // varchar(32)
        public string CODE_GROUPE { get; set; } // varchar(32)
        public bool ETAT_USER { get; set; } // varchar(5)                        
        public string CB_USER { get; set; } // varchar(32)
        public string CODE_COMPTE { get; set; } // varchar(32)
        public string OWNER { get; set; } // varchar(32)
        public DateTime? MODIFIED_ON { get; set; } // datetime(3)
        public DateTime? CREATED_ON { get; set; } // datetime(3)
        public string MODIFIED_BY { get; set; } // varchar(32)
        public string CREATED_BY { get; set; } // varchar(32)
        public int ORDRE_USER { get; set; } // int 
        public string TEL_USER { get; set; } // varchar(32)
        public string CODE_TIERS { get; set; }
    }
}
