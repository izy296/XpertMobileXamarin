using System;
using System.Collections.Generic;
using System.Text;
using SQLite;
using Xpert.Common.WSClient.Model;

namespace XpertMobileApp.DAL
{
    public class RegisterBindingModel
    {
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public string USER_NAME { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }
    }

    public class SYS_OBJET_PERMISSION
    {
        [PrimaryKey, AutoIncrement]
        public int ID { get; set; }
        public string CODE_PERMISSION { get; set; }
        public string CodeObjet { get; set; }
        public string CODE_GROUPE { get; set; } 
        public short AcInsert { get; set; }
        public short AcUpdate { get; set; }
        public short AcDelete { get; set; }
        public short AcSelect { get; set; }
        public short AcPrint { get; set; }
        public short AcImport { get; set; }
        public short AcConfigure { get; set; }
        public short AcCloture { get; set; }
        public short AcDecloture { get; set; }
        public string AcCustomData { get; set; }
        public string CONFIG_LIST { get; set; }
        public string CONFIG_VERSION { get; set; }
        public DateTime? CREATED_ON { get; set; }
        public string CREATED_BY { get; set; }
        public DateTime? MODIFIED_ON { get; set; }
        public string MODIFIED_BY { get; set; }
        public string OWNER { get; set; }
        public string EXERCICE { get; set; }
        public string CONFIG_LIST_GRID { get; set; }
    }

    public partial class SYS_CONFIG_TIROIRS
    {
        public string MACHINE_NAME { get; set; }
        public string CODE_COMPTE { get; set; }
        public int TIROIR_LOCAL { get; set; }
        public string TIROIR_PORT { get; set; }
        public string TITROIR_SERVER { get; set; }
        public DateTime? MODIFIED_ON { get; set; } // datetime(3)
        public DateTime? CREATED_ON { get; set; } // datetime(3)
        public string MODIFIED_BY { get; set; } // varchar(32)
        public string CREATED_BY { get; set; } // varchar(32)
        public bool DISPLAY_LOT_STOCK { get; set; } // varchar(32)
        public bool USE_TIROIR_CAISSE { get; set; }
        public string REPORT_FOLDER { get; set; }

        public string DESIGN_COMPTE { get; set; } // varchar(32)
        public bool IMPORT_CONFIG { get; set; } // varchar(32)


    }

    public class CBScanedEventArgs : EventArgs
    {
        public string CodeBarre { get; set; }
    }

    public class LotInfosEventArgs : EventArgs
    {
        public decimal Quantity { get; set; }
        public decimal Price { get; set; }
    }
}
