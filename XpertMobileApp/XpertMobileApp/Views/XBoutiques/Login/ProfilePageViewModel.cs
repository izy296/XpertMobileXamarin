using Acr.UserDialogs;
using System;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Internals;
using Xpert.Common.WSClient.Services;
using XpertMobileApp.DAL;

namespace XpertMobileApp.ViewModels.XLogin
{
    [Preserve(AllMembers = true)]
    public class ProfilePageViewModel : LoginViewModel
    {
        internal CrudService<AspNetUsers> bll;

        private AspNetUsers item;
        public AspNetUsers Item
        {
            get { return item; }
            set { SetProperty(ref item, value); }
        }

        public Command SaveCommand { get; set; }

        public ProfilePageViewModel()
        {
            Title = "Profile";
            this.SaveCommand = new Command(async (object obj) => await this.SaveClicked(obj));
        }

        internal CrudService<AspNetUsers> GetBll()
        {
            if (bll == null)
            {
                bll = new CrudService<AspNetUsers>(App.RestServiceUrl, "Users", App.User.Token);
            }

            return bll;
        }

        private async Task SaveClicked(object obj)
        {
            try 
            {
                UserDialogs.Instance.ShowLoading(AppResources.txt_Loading);
                
                Item.Email = Email;
                
                bool res = await GetBll().UpdateItemAsync(Item);
                
                if(res)
                    await UserDialogs.Instance.AlertAsync("Profile mis à jour!", AppResources.alrt_msg_Info, AppResources.alrt_msg_Ok);
                
                UserDialogs.Instance.HideLoading();
            } 
            catch(Exception ex)
            {
                UserDialogs.Instance.HideLoading();
                await UserDialogs.Instance.AlertAsync(ex.Message, AppResources.alrt_msg_Info, AppResources.alrt_msg_Ok);
            }
        }
    }
}