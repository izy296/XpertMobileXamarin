using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using XpertMobileApp.Api;
using XpertMobileApp.DAL;
using XpertMobileApp.Models;
using XpertMobileApp.ViewModels;

namespace XpertMobileApp.ViewModels
{
    public class EncaissementsDetailViewModel : ItemDetailViewModel<View_TRS_ENCAISS>
    {

        public EncaissementsDetailViewModel(View_TRS_ENCAISS itemObj) : base(itemObj)
        {

        }

        public bool HasCancelEncaiss
        {
            get
            {
                return HasPrivilege(Xpert.XpertObjets.TRS_ENCAISS, Xpert.XpertActions.None);
            }
        }

        public bool HasUpdateEncaiss
        {
            get
            {
                return HasPrivilege(Xpert.XpertObjets.TRS_ENCAISS, Xpert.XpertActions.AcUpdate);
            }
        }

        public bool HasDeleteEncaiss
        {
            get
            {
                return HasPrivilege(Xpert.XpertObjets.TRS_ENCAISS, Xpert.XpertActions.AcDelete);
            }
        }
    }
}