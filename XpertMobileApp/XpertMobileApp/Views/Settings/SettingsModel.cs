using XpertMobileApp.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Collections.ObjectModel;
using XpertMobileApp.Model;
using System.Linq;
using System.Threading.Tasks;
using XpertMobileApp.Services;
using Acr.UserDialogs;
using XpertMobileApp.Helpers;
using Xpert.Common.WSClient.Model;
using Xpert.Common.WSClient.Helpers;
using Xamarin.Forms;
using XpertMobileApp.Api.Services;
using System.Windows.Input;
using XpertWebApi.Models;

namespace XpertMobileApp.ViewModels
{
    public class SettingsModel : BaseViewModel
    {

        public Settings Settings { get => App.Settings; set => App.Settings = value; }

        public ObservableCollection<Language> Languages { get; }

        public bool IsConnected
        {
            get
            {
                return App.User != null;
            }
        }

        public bool IsAdminUser
        {
            get
            {
                return App.User != null;
            }
        }

        public SettingsModel()
        {
            Title = AppResources.pn_Settings;

            Languages = new ObservableCollection<Language>()
            {
                new Language { DisplayName =  "Device language", ShortName = "" },
                new Language { DisplayName =  "عربي", ShortName = "ar" },
                new Language { DisplayName =  "English", ShortName = "en" },
                new Language { DisplayName =  "Français - French", ShortName = "fr" },
                new Language { DisplayName =  "中文 - Chinese (simplified)", ShortName = "zh-Hans" }
            };

            _blueToothService = DependencyService.Get<IBlueToothService>();

        }

        public Language GetLanguageElem(string language)
        {
            Language result = Languages[0];
            if (Languages.Where( e => e.ShortName == language).Count() > 0)
            {
                result = Languages.Where(e => e.ShortName == language).ToList()[0];
            }
            return result;
        }

        

        public void LoadSettings()
        {
            this.Settings = App.SettingsDatabase.GetFirstItemAsync().Result;
            if (this.Settings == null)
            {
                this.Settings = new Settings();
                // App.Settings = this.Settings;
            }
            this.Settings.isModified = false;           
        }

        public void SaveSettings()
        {
            if (Settings.SubscribedToFBNotifications)
            {
                FireBaseHelper.RegisterUserForDefaultTopics(App.User, App.User.ClientId);
            }
            else
            {
                FireBaseHelper.UnsubscribeFromAllTopics();
            }

            App.SettingsDatabase.SaveItemAsync(Settings);
            this.Settings.isModified = false;
        }

        internal async Task<bool> DeactivateClient()
        {
            try
            {
                if (IsBusy)
                    return false;

                IsBusy = true;

                Client client = App.ClientDatabase.GetFirstItemAsync().Result;

                if (client == null) return false;

                bool result = await WebServiceClient.DeactivateClient(client);
                if (result)
                { 
                    await App.ClientDatabase.DeleteItemAsync(client);
                }

                IsBusy = false;

                return true;
            }
            catch (XpertWebException ex)
            {
                if (ex.Code == XpertWebException.ERROR_XPERT_UNKNOWN)
                {
                    await UserDialogs.Instance.AlertAsync(WSApi2.GetExceptionMessage(ex), AppResources.alrt_msg_Alert,
                        AppResources.alrt_msg_Ok);
                }
                else
                {
                    string msgKey = string.Format("Exception_errMsg_{0}", ex.Code);
                    await UserDialogs.Instance.AlertAsync(TranslateExtension.GetTranslation(msgKey), AppResources.alrt_msg_Alert,
                    AppResources.alrt_msg_Ok);
                }
                return false;
            }
            finally
            {
                IsBusy = false;
            }
        }


        #region Impression BlueTooth

        private readonly IBlueToothService _blueToothService;

        private IList<XPrinter> _deviceList;
        public IList<XPrinter> DeviceList
        {
            get
            {
                if (_deviceList == null)
                    _deviceList = new ObservableCollection<XPrinter>();
                return _deviceList;
            }
            set
            {
                _deviceList = value;
            }
        }

        private XPrinter _selectedDevice;
        public XPrinter SelectedDevice
        {
            get
            {
                return _selectedDevice;
            }
            set
            {
                _selectedDevice = value;
                this.Settings.PrinterName = value != null ? value.Name : "";
                this.Settings.PrinterType = value != null ? value.Type : "";
                OnPropertyChanged("SelectedDevice");
            }
        }

        /// <summary>
        /// Print text-message
        /// </summary>
        public ICommand PrintCommand => new Command(async () =>
        {
            try
            {
                if (SelectedDevice != null) 
                { 
                    string printMessage = " Lorem Ipsum is simply dummy text of the printing and typesetting industry. Lorem Ipsum has been the industry's standard dummy text ever since the 1500s, when an unknown printer took a galley of type and scrambled it to make a type specimen book. It has survived not only five centuries, but also the leap into electronic typesetting, remaining essentially unchanged. It was popularised in the 1960s with the release of Letraset sheets containing Lorem Ipsum passages, and more recently with desktop publishing software like Aldus PageMaker including versions of Lorem Ipsum.";
                    
                    if(SelectedDevice.Type == Printer_Type.Bluetooth) 
                    { 
                        await _blueToothService.Print(SelectedDevice.Name, printMessage);
                    } 
                    else if(SelectedDevice.Type == Printer_Type.Wifi)
                    {
                        await UserDialogs.Instance.AlertAsync("L'impression pour les impémantes wifi n'est pas encor imlémenté", AppResources.alrt_msg_Alert,
AppResources.alrt_msg_Ok);
                    }
                    else 
                    { 
                    
                    }
                }
                else 
                {
                    await UserDialogs.Instance.AlertAsync(AppResources.alrt_msg_NoPrinterSelected, AppResources.alrt_msg_Alert,
                    AppResources.alrt_msg_Ok);
                }
            }
            catch (Exception e)
            {
                await UserDialogs.Instance.AlertAsync(e.Message, AppResources.alrt_msg_Alert,
                        AppResources.alrt_msg_Ok);
            }
        });



        /// <summary>
        /// Get printers list
        /// </summary>
        public async Task BindDeviceList()
        {
            try 
            { 
                DeviceList.Clear();

                // Blue tooth printer
                var list = _blueToothService.GetDeviceList();
                foreach (var item in list) 
                {
                    XPrinter itm = new XPrinter()
                    {
                        Name = item,
                        Type = Printer_Type.Bluetooth
                    };
                    DeviceList.Add(itm);

                    if (item == Settings.PrinterName && Printer_Type.Bluetooth == Settings.PrinterType)
                    {
                        SelectedDevice = itm;
                    }
                }

                // Netwirk Printer
                var ntworkProinters = await WebServiceClient.GetPrintersList();
                foreach (var item in ntworkProinters)
                {
                    DeviceList.Add(item);
                    if(item.Name == Settings.PrinterName && item.Type == Settings.PrinterType) 
                    {
                        SelectedDevice = item;
                    }
                }


            }
            catch (Exception e)
            {
                await UserDialogs.Instance.AlertAsync(e.Message, AppResources.alrt_msg_Alert,
AppResources.alrt_msg_Ok);
            }
        }
        #endregion
    }
}

