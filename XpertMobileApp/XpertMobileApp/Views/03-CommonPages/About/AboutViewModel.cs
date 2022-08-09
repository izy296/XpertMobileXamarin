using Acr.UserDialogs;
using System;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;
using XpertMobileApp.Services;

namespace XpertMobileApp.ViewModels
{
    public class AboutViewModel : BaseViewModel
    {
        public string newVersion
        {
            get; set;
        }
        Command getVersionCommand;

        public AboutViewModel()
        {
            Title = AppResources.pn_About;
            OpenWebCommand = new Command(() => Device.OpenUri(new Uri("http://xpertsoft-dz.com/")));
        }

        public ICommand OpenWebCommand { get; }

        /// <summary>
        /// Get the new Version of the mobile Application
        /// </summary>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public async Task<string> GetNewVersion(Label l)
        {
            try
            {
                UserDialogs.Instance.ShowLoading(AppResources.txt_Waiting);
                var NewVersion = await WebServiceClient.GetNewVersion();
                newVersion = NewVersion;
                l.Text = newVersion;
                UserDialogs.Instance.HideLoading();
                return NewVersion;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// Send post request to api to update the web api
        /// </summary>
        /// <exception cref="Exception"></exception>
        public async void UpdateVersion()
        {
            try
            {
                await WebServiceClient.UpdateVersion(VersionTracking.CurrentVersion);
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }
    }
}