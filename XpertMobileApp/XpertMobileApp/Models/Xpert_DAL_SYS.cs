using System;


namespace XpertMobileApp.DAL
{
    public class SYS_OBJET_PERMISSION
    {
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
}
