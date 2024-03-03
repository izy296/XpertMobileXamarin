using System;
using XpertMobileApp.DAL;

namespace XpertMobileApp.Models
{
    public class XpertBaseFilterModel 
    {
        public XpertBaseFilterModel() : base()
        {
            // This constructor is used when an object is loaded from a persistent storage.
            // Do not place any code here.
        }

        public View_TRS_TIERS Filter_SelectedTiers { get; set; } 
        public string Filter_GlobalSearch { get; set; } 
        public DateTime Filter_Start_Date { get; set; } = DateTime.Now; // varchar(50)
        public DateTime Filter_End_Date { get; set; } = DateTime.Now; // varchar(50)

        public string CurrentStreamContext { get; set; }
    }

}