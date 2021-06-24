using Acr.UserDialogs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xpert;
using XpertMobileApp.Api.Managers;
using XpertMobileApp.Api.Services;
using XpertMobileApp.DAL;
using XpertMobileApp.Models;
using XpertMobileApp.SQLite_Managment;

namespace XpertMobileApp.Api
{
    public static class AppManager
    {
        private static SYS_MOBILE_PARAMETRE sysParams;
        private static TRS_JOURNEES session;
        internal static List<SYS_OBJET_PERMISSION> permissions;

        /// <summary>
        ///  Paramètres de la table SYS_PARAMS utilisé par l'application mobile
        /// </summary>
        /// <returns></returns>
        public static async Task<SYS_MOBILE_PARAMETRE> GetSysParams()
        {
            if (sysParams == null)
            {
                if (App.Online)
                {
                    sysParams = await CrudManager.SysParams.GetParams();
                }
                else
                {
                    sysParams = await SQLite_Manager.getParams();
                }
                return sysParams;
            }
            return sysParams;
        }

        /// <summary>
        ///  Session en cours de l'utilisateur connecté
        /// </summary>
        /// <returns></returns>
        public static async Task<TRS_JOURNEES> GetCurrentSession()
        {
            try
            {
                if (session == null)
                {
                    session = await CrudManager.Sessions.GetCurrentSession();
                }
                return session;
            }
            catch (Exception e)
            {
                await UserDialogs.Instance.AlertAsync(e.Message, AppResources.alrt_msg_Alert, AppResources.alrt_msg_Ok);
                return null;
            }
        }

        /// <summary>
        ///  Permission de l'utilisateur connecté
        /// </summary>
        /// <returns></returns>
        public static async Task<List<SYS_OBJET_PERMISSION>> GetPermissions()
        {
            try
            {
                if (permissions == null)
                {
                    if (App.Online)
                    {
                        permissions = await CrudManager.Permissions.GetPermissions(App.User.UserGroup);
                    }
                    else
                    {
                        permissions = await SQLite_Manager.getPermission();
                    }
                }
                return permissions;
            }
            catch (Exception e)
            {
                await UserDialogs.Instance.AlertAsync(e.Message, AppResources.alrt_msg_Alert, AppResources.alrt_msg_Ok);
                return null;
            }
        }

        public static bool HasPermission(XpertObjets codeObject, XpertActions action)
        {
            if (AppManager.HasAdmin) return true;
            if (codeObject == XpertObjets.None || action == XpertActions.None) return true;

            bool result = false;
            if (AppManager.permissions != null)
            {
                var obj = AppManager.permissions.Where(x => x.CodeObjet == codeObject.ToString()).FirstOrDefault();
                var res = XpertHelper.GetValue(obj, action.ToString());
                result = obj != null && Convert.ToInt16(res) > 0;
            }
            return result;
        }

        public static bool HasAdmin
        {
            get
            {
                return App.User.UserGroup == "AD";
            }
        }
    }
}
