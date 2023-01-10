using Acr.UserDialogs;
using Rg.Plugins.Popup.Services;
using System.Threading.Tasks;
using Xpert.Common.WSClient.Model;
using XpertMobileApp.Models;
using XpertMobileApp.Services;
using XpertMobileApp.Views;

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
            catch (XpertWebException e)
            {
                if (e.Code == XpertWebException.ERROR_XPERT_INCORRECTPASSWORD)
                {
                    CustomPopup AlertPopup = new CustomPopup(AppResources.err_msg_IncorrectLoginInfos, trueMessage: AppResources.alrt_msg_Ok);
                    await PopupNavigation.Instance.PushAsync(AlertPopup);
                }
                else
                {
                    CustomPopup AlertPopup = new CustomPopup(e.Message, trueMessage: AppResources.alrt_msg_Ok);
                    await PopupNavigation.Instance.PushAsync(AlertPopup);
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
            return true; //a revoire 
        }
    }
}