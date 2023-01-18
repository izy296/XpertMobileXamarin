using SQLite;
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
    public class LOG_SYNCHRONISATION
    {
        [PrimaryKey, AutoIncrement]
        public int ID { get; set; }
        public string CODE_TOURNEE { get; set; }
        public DateTime SYNC_TIERS { get; set; }
        public DateTime SYNC_ENCAISS { get; set; }
        public DateTime SYNC_VENTE { get; set; }
        public DateTime SYNC_TOURNEE { get; set; }
        public DateTime SYNC_COMMANDE { get; set; }
    }
}
