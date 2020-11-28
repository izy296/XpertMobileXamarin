using Acr.UserDialogs;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xpert.Common.WSClient.Model;
using Xpert.Key_Activator;
using XpertMobileApp.Data;
using XpertMobileApp.Models;
using XpertMobileApp.Services;

namespace XpertMobileApp.Api.Services
{
    public enum LicState { NotActivated, Valid, Expired, Cracked, InvalidDevice };

    public class LicActivator
    {
        private static string LOCAL_DB_NAME = Constants.LOCAL_DB_NAME;

        public static IDeviceInfos DInfos = DependencyService.Get<IDeviceInfos>();

        public static LicenceInfos GetLicenceInfos()
        {
            try 
            { 
                Client client = App.ClientDatabase.GetFirstItemAsync().Result;
                if (client == null || string.IsNullOrEmpty(client.LicenceTxt))
                {
                    return null;
                }

                var LicenceInfos = DecryptLicence(client.LicenceTxt);

                return LicenceInfos;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async static Task<LicState> CheckLicence(LicenceInfos licenceInfos)
        {
            try
            { 
                if(licenceInfos == null)
                {
                    return LicState.NotActivated;
                }

                //System.Environment.Exit(1);
                var DInfos = DependencyService.Get<IDeviceInfos>();
                string serial = DInfos?.GetSecureOsId();
                if (licenceInfos.DeviceId != serial)
                {
                    return LicState.InvalidDevice;
                }

                int nbrDays = Convert.ToInt32(licenceInfos.ExpirationDate.Subtract(DateTime.Now).TotalDays);
                if (nbrDays >= 0)
                {
                    return LicState.Valid;
                }
                else
                {
                    return LicState.Expired;
                }
            }
            catch(Exception e)
            {
                await UserDialogs.Instance.AlertAsync(e.Message, AppResources.alrt_msg_Alert,
                    AppResources.alrt_msg_Ok);
                return LicState.Cracked;
            }
        }

        internal static async Task<DateTime> GetLicenceEndDate(string licenceTxt)
        {
            LicenceInfos licenceInfos = GetLicenceInfos();
            LicState state = await CheckLicence(licenceInfos);
            if (state == LicState.Valid)
            {
                return licenceInfos.ExpirationDate;
            }
            else
            {
                throw new XpertWebException("Invlide licence", XpertWebException.ERROR_XPERT_INVALIDELICENCE);
            }
        }

        public static string GetLicenceMsgFromState(LicState licState)
        {
            string message = "";
            switch (licState)
            {
                case LicState.Valid:
                    message = "";
                    break;
                case LicState.Expired:
                    message = AppResources.msg_Licence_Expired;
                    break;
                case LicState.Cracked:
                    message = AppResources.msg_Licence_Cracked;
                    break;
                case LicState.NotActivated:
                    message = AppResources.msg_Licence_NotActivated;
                    break;
                case LicState.InvalidDevice:
                    message = AppResources.InvalidDevice;
                    break;
                default:
                    message = "";
                    break;
            }

            return message;
        }

        internal static LicenceInfos DecryptLicence(string licenceTxt)
        {
            StringEncryptMobile decriptor = new StringEncryptMobile("XpEMobrtile@2019", DInfos.GetSecureOsId());
            string result = decriptor.Decrypt(licenceTxt);
            LicenceInfos lInfos = JsonConvert.DeserializeObject<LicenceInfos>(result);
            return lInfos;
        }
    }
}
