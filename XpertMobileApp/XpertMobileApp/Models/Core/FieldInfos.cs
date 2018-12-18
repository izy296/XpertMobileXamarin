using System;
using System.Collections.Generic;
using System.Text;

namespace XpertMobileApp.Models
{
    [System.AttributeUsage(System.AttributeTargets.Field | System.AttributeTargets.Property)]
    public class FieldInfos : System.Attribute
    {
        public bool VisibleInForm;
        public bool VisibleInFich;
        public string Designation;

        public FieldInfos()
        {

        }
    }
}
