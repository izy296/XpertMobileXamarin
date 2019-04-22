using Acr.UserDialogs;
using System.Threading.Tasks;
using Xpert.Common.WSClient.Model;
using XpertMobileApp.Models;
using XpertMobileApp.Services;

namespace XpertMobileApp.ViewModels
{
    public class LoginViewModel : BaseViewModel
    {
        public LoginViewModel()
        {
            Title = AppResources.pn_Login;
        }

        public async Task<Token> Login(User user)
        {
            try
            {
                Token result = await WebServiceClient.Login(App.RestServiceUrl, user.UserName, user.PassWord);
                return result != null ? result : new Token();
            }
            catch (XpertException e)
            {
                if (e.Code == XpertException.ERROR_XPERT_INCORRECTPASSWORD)
                {
                    await UserDialogs.Instance.AlertAsync(AppResources.err_msg_IncorrectLoginInfos, AppResources.alrt_msg_Alert, AppResources.alrt_msg_Ok);
                }
                else
                {
                    await UserDialogs.Instance.AlertAsync(e.Message, AppResources.alrt_msg_Alert, AppResources.alrt_msg_Ok);
                }
                return null;
            }
        }

        public bool CheckUser(User user)
        {
            if (user.UserName != "") //  && user.PassWord != ""
            {
                return true;
            }
            return false;
        }
    }
}