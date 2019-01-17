using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using XpertMobileApp.Data;
using XpertMobileApp.Models;

namespace XpertMobileApp.Api.Services
{
    public enum LicState { NotActivated, Valid, Expired, Cracked, InvalidDevice };

    public class LicActivator
    {
        private static string LOCAL_DB_NAME = Constants.LOCAL_DB_NAME;

        private static ClientDatabaseControler clientDatabase = new ClientDatabaseControler(DependencyService.Get<IFileHelper>().GetLocalFilePath(LOCAL_DB_NAME));

        public async static Task<LicState> CheckLicence()
        {
            try
            { 
                Client client = await clientDatabase.GetFirstItemAsync();

                if(client == null || string.IsNullOrEmpty(client.LicenceTxt))
                {
                    return LicState.NotActivated;
                }

                var LicenceInfos = DecryptLicence(client.LicenceTxt);

                if(LicenceInfos.DeviceId != "currentDeviceId")
                {
                    return LicState.InvalidDevice;
                }

                int nbrDays = Convert.ToInt32(DateTime.Now.Subtract(LicenceInfos.EndDate).TotalDays);
                if (nbrDays >= 0)
                {
                    return LicState.Valid;
                }
                else
                {
                    return LicState.Expired;
                }
            }
            catch
            {
                return LicState.Cracked;
            }
        }

        internal static async Task<DateTime?> GetLicenceEndDate(string licenceTxt)
        {
            if(await CheckLicence() == LicState.Valid)
            {
                Client client = await clientDatabase.GetFirstItemAsync();
                var LicenceInfos = DecryptLicence(client.LicenceTxt);
                return LicenceInfos.EndDate;
            }
            else
            {
                return null;
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

        internal Task<Client> ActivateClient(Client client)
        {
            throw new NotImplementedException();
        }

        internal static LicenceInfos DecryptLicence(string licenceTxt)
        {
            throw new NotImplementedException();
        }
    }
}
