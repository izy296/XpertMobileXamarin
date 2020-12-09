using System;
using System.Collections.Generic;
using System.Text;
using XpertMobileApp.DAL;

namespace XpertMobileApp.Api.LocalStorage.Ventes
{
    public class SL_VENTE_BLL : RLMDataManager<View_VTE_VENTE>
    {
        public SL_VENTE_BLL(string dbPath) : base(dbPath)
        {
        }
    }
}
