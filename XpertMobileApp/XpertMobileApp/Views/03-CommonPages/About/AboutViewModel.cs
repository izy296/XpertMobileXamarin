using Acr.UserDialogs;
using System;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Xml.Linq;
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
                var xml = await WebServiceClient.GetNewVersion();
                XDocument docWebApiXml = XDocument.Parse(xml);
                XElement itemWebApiXml = docWebApiXml.Element("item");

                var NewVersion = itemWebApiXml.Element("version").Value;

                newVersion = NewVersion;
                l.Text = newVersion;
                UserDialogs.Instance.HideLoading();
                return NewVersion;
            }
            catch (Exception ex)
            {
                UserDialogs.Instance.HideLoading();
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// Get the new Version of the WebApi Version
        /// </summary>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public async Task<bool> GetNewWebApiVersion(Label l)
        {
            try
            {
                UserDialogs.Instance.ShowLoading(AppResources.txt_Waiting);
                var res = await WebServiceClient.GetWebApiVersion();
                Version webVersion = new Version(res);

                var resNewVersion = await WebServiceClient.GetNewWebApiVersion();
                var xml = await WebServiceClient.GetNewVersion();
                XDocument docWebApiXml = XDocument.Parse(xml);
                XElement itemWebApiXml = docWebApiXml.Element("item");

                Version newVersion = new Version (itemWebApiXml.Element("version").Value);

                UserDialogs.Instance.HideLoading();
                if (newVersion > webVersion)
                    return true;
                else return false;
            }
            catch (Exception ex)
            {
                UserDialogs.Instance.HideLoading();
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// Send post request to api to update the web api
        /// </summary>
        /// <exception cref="Exception"></exception>
        public async Task<string> UpdateVersion()
        {
            try
            {
                UserDialogs.Instance.ShowLoading(AppResources.ap_updating_txt);
                string res = await WebServiceClient.UpdateVersion(VersionTracking.CurrentVersion);
                UserDialogs.Instance.HideLoading();
                return res;
            }
            catch (Exception ex)
            {
                UserDialogs.Instance.HideLoading();
                throw new Exception(ex.Message);
            }
        }
    }
}