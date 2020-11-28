using Acr.UserDialogs;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using XpertMobileApp.Api.Managers;
using XpertMobileApp.DAL;
using XpertMobileApp.Models;

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
                var result = await CrudManager.SysParams.GetParams();
                sysParams = result;
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
                    permissions = await CrudManager.Permissions.GetPermissions(App.User.UserGroup);
                }
                return permissions;
            }
            catch (Exception e)
            {
                await UserDialogs.Instance.AlertAsync(e.Message, AppResources.alrt_msg_Alert, AppResources.alrt_msg_Ok);
                return null;
            }
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
