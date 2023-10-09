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
    public partial class SettingsDisplayPricePopup : PopupPage
    {
        public bool serverChanged = false;
        public SettingsModel viewModel;
        List<string> socketMessages = new List<string>();

        enum radioButtonsClass { radioButtonLocalIp, radioButtonRemoteIp }
        public SettingsDisplayPricePopup()
        {
            InitializeComponent();
            CloseWhenBackgroundIsClicked = false;
            BindingContext = viewModel = new SettingsModel();
            Device.BeginInvokeOnMainThread(async () =>
            {
                var urlService = await viewModel.GetUrlService();
                LocalIp.Text = urlService.DisplayUrlService;
            });
            LocalIp.TextChanged += LocalIp_TextChanged; 
        }

        private void LocalIp_TextChanged(object sender, TextChangedEventArgs e)
        {
            viewModel.UrlServices[0].DisplayUrlService = LocalIp.Text;
        }

        private async void ConfirmBtn_Clicked(object sender, EventArgs e)
        {
            var selected = viewModel.UrlServices.Where(elm => elm.Selected).First();

            if (selected != null)
            {
                // check if url selected is empty
                if (!string.IsNullOrEmpty(selected.DisplayUrlService.Replace("http://", "")))
                {
                    // Serielize new list and save it
                    var list = JsonConvert.SerializeObject(viewModel.UrlServices);
                    viewModel.Settings.ServiceUrl = list;
                    await viewModel.SaveSettings();

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

        private void SyncConfiBtn_Clicked(object sender, EventArgs e)
        {
            MessagingCenter.Send(this, "SyncConfig", "start");
        }

        private void RefreshImages(object sender, EventArgs e)
        {
            MessagingCenter.Send(this, "RefreshImages", "start");
        }
    }
}