
using Acr.UserDialogs;
using System;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xpert.Common.WSClient.Helpers;
using XpertMobileApp.Api.Managers;
using XpertMobileApp.Api.Services;
using XpertMobileApp.DAL;

namespace XpertMobileApp.ViewModels
{
    public class OpenSessionViewModel : BaseViewModel
    {
        public SESSION_INFO Item { get; set; }
        public OpenSessionViewModel(string title= "" )
        {
            Title = title;
            Item = new SESSION_INFO();
            Item.DATE_JOURNEE = DateTime.Now;
            Item.POSTE_DEBUT = XpertHelper.GetMachineName();
        }

        internal async Task<bool?> OpenSession()
        {
            if (IsBusy)
                return null;

            bool? result = false;
            try
            {
                if (await App.IsConected())
                {
                    IsBusy = true;
                    UserDialogs.Instance.ShowLoading(AppResources.txt_Waiting);
                    result = await CrudManager.Sessions.OpenSession(Item);
                } 
                else 
                {
                    await UserDialogs.Instance.AlertAsync(AppResources.alrt_msg_NoConnexion, AppResources.alrt_msg_Alert,AppResources.alrt_msg_Ok);
                }
                UserDialogs.Instance.HideLoading();
            }
            catch (Exception ex)
            {
                await UserDialogs.Instance.AlertAsync(WSApi2.GetExceptionMessage(ex), AppResources.alrt_msg_Alert,
                    AppResources.alrt_msg_Ok);
            }
            finally
            {
                UserDialogs.Instance.HideLoading();
                IsBusy = false;
            }

            return result;
        }
    }
}
