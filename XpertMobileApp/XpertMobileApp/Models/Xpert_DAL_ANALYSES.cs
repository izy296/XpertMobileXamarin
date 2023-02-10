using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using Xpert;
using XpertMobileApp.Api;

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
        public string AppNames { get; set; }
        public string SubTitle { get; set; }
        public string Query2 { get; set; }
        public bool IsCustom { get; set; }
        public decimal CustomValue { get; set; }
        public string IconImage { get; set; }
        public XpertObjets CodeObjet { get; set; } = XpertObjets.None;

        public XpertActions Action { get; set; } = XpertActions.None;

        public bool HasPermission
        {
            get
            {
                return AppManager.HasPermission(CodeObjet, Action);
            }
        }
    }

    public class SAMMUARY
    {
        public string key { get; set; } // varchar(50)
        public string Title
        {
            get
            {
                return key == "Taux Marge" ? Value + " " + "%" : Value + " " + Unit;
            }

        } // varchar(50)
        public string Value
        {
            get; set;
        }
        public string Unit { get; set; } = "DA";
        public bool NewBloc { get; set; } = false;
        public Color ValueColor { get; set; } = Color.FromArgb(167, 116, 108);
    }
}
