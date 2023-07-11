using Acr.UserDialogs;
using Newtonsoft.Json;
using Plugin.Connectivity;
using Rg.Plugins.Popup.Pages;
using Rg.Plugins.Popup.Services;
using Syncfusion.XForms.Buttons;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using XpertMobileApp.Api;
using XpertMobileApp.Api.Models;
using XpertMobileApp.DAL;
using XpertMobileApp.Helpers;
using XpertMobileApp.Services;
using XpertMobileApp.ViewModels;
using XpertMobileApp.Views.Helper;

namespace XpertMobileApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SettingsPopup : PopupPage
    {
        public bool serverChanged = false;
        public SettingsModel viewModel;

        enum radioButtonsClass { radioButtonLocalIp, radioButtonRemoteIp }
        public SettingsPopup()
        {
            InitializeComponent();
            CloseWhenBackgroundIsClicked = false;
            BindingContext = viewModel = new SettingsModel();
            viewModel.LoadSettings();
            var remoteAvailable = viewModel.UrlServices.Any(elm => elm.TypeUrl == UrlService.TypeService.Remote);
            var localAvailable = viewModel.UrlServices.Any(elm => elm.TypeUrl == UrlService.TypeService.Local);
            if (!remoteAvailable && !localAvailable)
            {
                if (viewModel.UrlServices.Count > 1)
                {
                    var remote = viewModel.UrlServices.First();
                    remote.TypeUrl = UrlService.TypeService.Remote;
                    var local = viewModel.UrlServices.Last();
                    local.TypeUrl = UrlService.TypeService.Local;
                    RemoteIp.BindingContext = remote;
                    LocalIp.BindingContext = local;
                }
                else if (viewModel.UrlServices.Count == 1)
                {
                    viewModel.UrlServices.Add(
                        new UrlService()
                        {
                            DisplayUrlService = "http://192.168.1.1:100/",
                            Selected = false,
                            Title = Constants.AppName == Apps.XPH_Mob ? "Pharmacie" : "Entreprise" + "(Local)",
                            TypeUrl = UrlService.TypeService.Local,
                        });
                    var remote = viewModel.UrlServices.First();
                    remote.TypeUrl = UrlService.TypeService.Remote;
                    RemoteIp.BindingContext = viewModel.UrlServices.First();
                    LocalIp.BindingContext = viewModel.UrlServices.Last();
                }
            }
            else if (remoteAvailable && localAvailable)
            {
                RemoteIp.BindingContext = viewModel.UrlServices.Where(elm => elm.TypeUrl == UrlService.TypeService.Remote).First();
                LocalIp.BindingContext = viewModel.UrlServices.Where(elm => elm.TypeUrl == UrlService.TypeService.Local).First();
            }
            else if (!remoteAvailable)
            {
                viewModel.UrlServices.Add(
                    new UrlService()
                    {
                        DisplayUrlService = "http://",
                        Selected = false,
                        Title = Constants.AppName == Apps.XPH_Mob ? "Pharmacie" : "Entreprise" + "(Remote)",
                        TypeUrl = UrlService.TypeService.Remote
                    });
                RemoteIp.BindingContext = viewModel.UrlServices.Last();
                LocalIp.BindingContext = viewModel.UrlServices.First();
            }
            else if (!localAvailable)
            {
                viewModel.UrlServices.Add(
                    new UrlService()
                    {
                        DisplayUrlService = "http://192.168.1.1:100/",
                        Selected = false,
                        Title = Constants.AppName == Apps.XPH_Mob ? "Pharmacie" : "Entreprise" + "(Local)",
                        TypeUrl = UrlService.TypeService.Local
                    });
                RemoteIp.BindingContext = viewModel.UrlServices.First();
                LocalIp.BindingContext = viewModel.UrlServices.Last();
            }

            if (((UrlService)RemoteIp.BindingContext).Selected)
                radioButtonRemoteIp.IsChecked = true;
            else if (((UrlService)LocalIp.BindingContext).Selected)
                radioButtonLocalIp.IsChecked = true;

            serverChanged = false;
        }

        async Task<IPAddress> ScanWebApi()
        {
            IPAddress ipAddress = null;
            var client = new HttpClient();
            client.Timeout = new TimeSpan(2000);

            //Start with a list of URLs
            var urls = new List<string>();

            for (int i = 1; i < 255; i++)
                urls.Add($"http://192.168.1.{i}:100/api/{ServiceUrlDico.ACCESSBILITY_URL}/{ServiceUrlDico.ACCESSBILITY_URL_TEST}");

            List<string> urls2 = new List<string>();

            foreach (string url in urls)
            {
                if (await CrossConnectivity.Current.IsRemoteReachable(url))
                    urls2.Add(url);
            }

            //Start requests for all of them
            List<Task<HttpResponseMessage>> requests =null;
            try
            {
                requests = urls.Select
                (
                    url => client.GetAsync(url)
                ).ToList();
            }
            catch (Exception ex)
            {
                //
            }

             if (requests!=null)
            {
                //Wait for all the requests to finish
                await Task.WhenAll(requests);

                //Get the responses
                var responses = requests.Select
                    (
                        task => task.Result
                    );

                foreach (var r in responses)
                {
                    // Extract the message body
                    var s = await r.Content.ReadAsStringAsync();
                    if (s == "Accessible")
                        ipAddress = IPAddress.Parse(r.RequestMessage.RequestUri.Host);
                    Console.WriteLine(s);
                }
            }

            client.Dispose();
            return ipAddress;
        }

        private async void radioButton_StateChanged(object sender, StateChangedEventArgs e)
        {
            serverChanged = true;
            if (e.IsChecked.HasValue && e.IsChecked.Value)
            {
                var buttonClassId = (sender as SfRadioButton).ClassId;
                if (buttonClassId == radioButtonsClass.radioButtonLocalIp.ToString())
                {
                    LocalIpLayout.IsEnabled = true;
                    RemoteIpLayout.IsEnabled = false;
                    ((UrlService)LocalIp.BindingContext).Selected = true;
                    ((UrlService)RemoteIp.BindingContext).Selected = false;
                    //await ScanWebApi();
                }
                else if (buttonClassId == radioButtonsClass.radioButtonRemoteIp.ToString())
                {
                    LocalIpLayout.IsEnabled = false;
                    RemoteIpLayout.IsEnabled = true;
                    ((UrlService)LocalIp.BindingContext).Selected = false;
                    ((UrlService)RemoteIp.BindingContext).Selected = true;

                    if (string.IsNullOrEmpty(RemoteIp.Text.Replace("http://", "")))
                    {
                        try
                        {
                            UserDialogs.Instance.ShowLoading("Récupération de lien public");
                            ((UrlService)RemoteIp.BindingContext).DisplayUrlService = await WebServiceClient.GetTunnelUrl();
                            RemoteIp.Text = ((UrlService)RemoteIp.BindingContext).DisplayUrlService;
                            UserDialogs.Instance.HideLoading();
                        }
                        catch (Exception ex)
                        {
                            UserDialogs.Instance.HideLoading();
                        }
                    }
                }

            }
        }

        private void Ping_PingCompleted(object sender, PingCompletedEventArgs e)
        {
            var res = e.Reply;
        }

        private async void CancelBtn_Clicked(object sender, EventArgs e)
        {
            List<UrlService> liste = JsonConvert.DeserializeObject<List<UrlService>>(App.Settings.ServiceUrl);
            var changed = !viewModel.UrlServices.All(elm => liste.Any(elm2 => elm2.Selected == elm.Selected && elm2.DisplayUrlService == elm.DisplayUrlService));
            if (changed)
            {
                var action = await DisplayAlert(AppResources.alrt_msg_title_Settings, AppResources.alrt_msg_SaveSettings,
                        AppResources.alrt_msg_Ok, AppResources.alrt_msg_Cancel);

                if (action)
                {

                    var list = JsonConvert.SerializeObject(viewModel.UrlServices);
                    viewModel.Settings.ServiceUrl = list;
                    await viewModel.SaveSettings();
                    await PopupNavigation.Instance.PopAsync();
                }
                else await PopupNavigation.Instance.PopAsync();
            }
            else
            {
                await PopupNavigation.Instance.PopAsync();
            }
        }

        private async void ConfirmBtn_Clicked(object sender, EventArgs e)
        {
            var selected = viewModel.UrlServices.Where(elm => elm.Selected).First();

            // append to the name of the urls to distiguish between remote/local addresse + fix the format of the address to have a last '/'
            foreach (var serviceUrl in viewModel.UrlServices)
            {
                if (serviceUrl.Title == (Constants.AppName == Apps.XPH_Mob ? "Pharmacie" : "Entreprise"))
                {
                    serviceUrl.Title += $@"({serviceUrl.TypeUrl})";
                    serviceUrl.DisplayUrlService = Manager.UrlServiceFormatter(serviceUrl.DisplayUrlService);
                }
            }

            if (selected != null)
            {
                // check if url selected is empty
                if (!string.IsNullOrEmpty(selected.DisplayUrlService.Replace("http://", "")))
                {
                    // Serielize new list and save it
                    var list = JsonConvert.SerializeObject(viewModel.UrlServices);
                    viewModel.Settings.ServiceUrl = list;
                    await viewModel.SaveSettings();

                    //if (radioButtonRemoteIp.IsChecked == true)
                    //{
                    //    UserDialogs.Instance.ShowLoading(AppResources.txt_Waiting);
                    //    await viewModel.GetNewTunnelUrlIfNotConnected();
                    //    UserDialogs.Instance.HideLoading();
                    //}
                    await PopupNavigation.Instance.PopAsync();
                }
                else
                {
                    CustomPopup AlertPopup = new CustomPopup("l'url est vide", trueMessage: AppResources.alrt_msg_Ok);
                    await PopupNavigation.Instance.PushAsync(AlertPopup);
                }

            }
        }

        private async void CheckBtn_Clicked(object sender, EventArgs e)
        {
            var serviceUrlTemp = App.Settings.ServiceUrl;
            try
            {
                var list = JsonConvert.SerializeObject(viewModel.UrlServices);
                App.Settings.ServiceUrl = list;
                UserDialogs.Instance.ShowLoading();
                if (await App.IsConected())
                {
                    CustomPopup AlertPopup = new CustomPopup(AppResources.alrt_msg_ConnectionSucces, trueMessage: AppResources.alrt_msg_Ok);
                    await PopupNavigation.Instance.PushAsync(AlertPopup);
                }
                else
                {
                    CustomPopup AlertPopup = new CustomPopup(AppResources.alrt_msg_ConnectionError, trueMessage: AppResources.alrt_msg_Ok);
                    await PopupNavigation.Instance.PushAsync(AlertPopup);
                }
                UserDialogs.Instance.HideLoading();
                App.Settings.ServiceUrl = serviceUrlTemp;
            }
            catch (Exception ex)
            {
                UserDialogs.Instance.HideLoading();
                CustomPopup AlertPopup = new CustomPopup(AppResources.alrt_msg_ConnectionError, trueMessage: AppResources.alrt_msg_Ok);
                await PopupNavigation.Instance.PushAsync(AlertPopup);
                App.Settings.ServiceUrl = serviceUrlTemp;
            }

        }
    }
}