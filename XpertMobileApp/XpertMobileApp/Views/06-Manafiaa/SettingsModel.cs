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
using XpertMobileApp.Api.Managers;
using XpertMobileApp.DAL;
using XpertMobileApp.Api;
using XpertMobileApp.Api.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using XpertMobileApp.Views.Helper;
using XpertMobileApp.Interfaces;

namespace XpertMobileApp.ViewModels
{
    public class SettingsModel : BaseViewModel
    {
        private string oldCaisseDedier;
        public Settings Settings
        {
            get => App.Settings;
            set => App.Settings = value;
        }

        public ObservableCollection<Language> Languages { get; }
        public ObservableCollection<UrlService> UrlServices { get; set; }

        public ObservableCollection<Notification> Notifications { get; set; }

        public bool HasVenteConfig
        {
            get
            {
                return IsConnected && Constants.AppName != Apps.XAGRI_Mob;
            }
        }

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
                new Language { DisplayName =  "Français - French", ShortName = "fr" }
               // new Language { DisplayName =  "中文 - Chinese (simplified)", ShortName = "zh-Hans" }
            };
            this.Settings = App.SettingsDatabase.GetFirstItemAsync().Result;
            UrlServices = new ObservableCollection<UrlService>() { };
            Notifications = new ObservableCollection<Notification>() { };

            MagasinsList = new ObservableCollection<View_BSE_MAGASIN>();
            ComptesList = new ObservableCollection<View_BSE_COMPTE>();
            _blueToothService = DependencyService.Get<IBlueToothService>();
        }

        public Language GetLanguageElem(string language)
        {
            Language result = Languages[0];
            if (Languages.Where(e => e.ShortName == language).Count() > 0)
            {
                result = Languages.Where(e => e.ShortName == language).ToList()[0];
            }
            return result;
        }

        /// <summary>
        /// Save all notification in the settings
        /// </summary>
        /// <param name="notif"></param>
        public async void setNotificationAsync(Notification notif)
        {
            if (Settings.Notifiaction == "null" || Settings.Notifiaction == null)
            {
                List<Notification> liste = new List<Notification>();
                liste.Add(notif);
                string jsonNotification = JsonConvert.SerializeObject(liste);
                Settings.Notifiaction = jsonNotification;
            }
            else
            {
                List<Notification> Liste;
                Liste = JsonConvert.DeserializeObject<List<Notification>>(Settings.Notifiaction);
                var found = false;
                foreach (var l in Liste)
                {
                    if (l.Message == notif.Message && l.Title == notif.Title &&
                        l.User == notif.User && l.TimeNotification == notif.TimeNotification &&
                        notif.ModuleId == notif.ModuleId)
                        found = true;
                }
                if (!found)
                    Liste.Add(notif);
                string jsonNotification = JsonConvert.SerializeObject(Liste);
                Settings.Notifiaction = jsonNotification;
            }
            await SaveSettings();
        }


        public static async Task GetNewTunnelUrlIfNotConnected()
        {
            try
            {
                // Check if the current Url is reachable 
                List<UrlService> liste = JsonConvert.DeserializeObject<List<UrlService>>(App.Settings.ServiceUrl);

                if (!await App.IsConected())
                {
                    // If false means that the current Url is not working
                    // we have to get new Url ...

                    /*  Get the new Url */
                    await App.GetTunnelAddress();
                    await App.SettingsDatabase.SaveItemAsync(App.Settings);
                    //OnPropertyChanged("DisplayUrlService");
                    DependencyService.Get<IMessage>().ShortAlert("Service Url a été Mise A jour avec le lien publique");
                }


            }
            catch (Exception ex)
            {
                //await DisplayAlert(AppResources.lp_Login, ex.Message, AppResources.alrt_msg_Ok);
            }
        }

        /// <summary>
        /// Get all notifications in settings
        /// </summary>
        /// <param name="notif"></param>
        public ObservableCollection<Notification> getNotificationAsync()
        {
            ObservableCollection<Notification> liste = new ObservableCollection<Notification>();
            if (Settings.Notifiaction != null)
            {
                if (Settings.Notifiaction != "null" && Manager.isJson(Settings.Notifiaction))
                {
                    liste = JsonConvert.DeserializeObject<ObservableCollection<Notification>>(Settings.Notifiaction);
                    liste = new ObservableCollection<Notification>(liste.Reverse());
                }
            }
            return liste;
        }
        /// <summary>
        /// Delete all Notifications
        /// </summary>
        public async void deleteteAllNotification()
        {
            Settings.Notifiaction = null;
            App.Settings.Notifiaction = null;
            await SaveSettings();
        }

        /// <summary>
        /// Save all Printers in the settings
        /// </summary>
        /// <param name="printer"></param>
        public async void SetMultiPrintersAsync(List<XPrinter> printers)
        {
            List<XPrinter> liste = new List<XPrinter>();
            foreach (XPrinter printer in printers)
            {
                liste.Add(printer);
            }
            string jsonNotification = JsonConvert.SerializeObject(liste);
            Settings.MultiPrinterList = jsonNotification;

        }

        /// <summary>
        /// Get all Printers in settings
        /// </summary>
        public List<XPrinter> GetMultiPrintersAsync()
        {
            List<XPrinter> liste = new List<XPrinter>();
            if (Settings.MultiPrinterList != null && Settings.MultiPrinterList != "null")
            {
                if (Settings.MultiPrinterList != "null" && Manager.isJson(Settings.MultiPrinterList))
                {
                    liste = JsonConvert.DeserializeObject<List<XPrinter>>(Settings.MultiPrinterList);
                }
            }
            return liste;
        }


        //Set Urls Item once the page is loaded...
        public async Task<UrlService> GetUrlService()
        {
            try
            {
                UrlService Result = new UrlService();
                if (Settings.ServiceUrl != "" && Settings.ServiceUrl != null)
                {
                    bool checkIfJson = Manager.isJson(Settings.ServiceUrl);
                    if (checkIfJson)
                    {

                        List<UrlService> Liste;
                        Liste = JsonConvert.DeserializeObject<List<UrlService>>(Settings.ServiceUrl);

                        foreach (var Item in Liste)
                        {
                            if (Item.Selected)
                            {
                                Result = Item;
                            }
                        }
                    }
                    else Result = UrlServices[0];
                }
                else
                {
                    Result = UrlServices[0];
                }
                LoadSettings();
                return Result;
            }
            catch (Exception ex)
            {
                await UserDialogs.Instance.AlertAsync(AppResources.txt_reinstall_app, AppResources.alrt_msg_Alert, AppResources.alrt_msg_Ok);
                return UrlServices[0];
            }
        }
        public async void LoadSettings()
        {

            this.Settings = App.SettingsDatabase.GetFirstItemAsync().Result;
            if (this.Settings.ServiceUrl != "" && this.Settings.ServiceUrl != null)
            {
                if (Manager.isJson(Settings.ServiceUrl))
                {
                    List<UrlService> liste = JsonConvert.DeserializeObject<List<UrlService>>(Settings.ServiceUrl);

                    for (int i = 0; i < liste.Count; i++)
                    {
                        if (liste[i].Title == null)
                        {
                            liste[i].Title = liste[i].DisplayUrlService;
                        }
                    }
                    //Settings.ServiceUrl = JsonConvert.SerializeObject(liste);
                    //await SaveSettings();
                    foreach (var Item in liste)
                    {
                        if (UrlServices.Where(e => e.DisplayUrlService == Item.DisplayUrlService).Count() == 0)
                        {
                            UrlServices.Add(Item);
                        }
                    }
                }
                else
                {
                    //execute when migrating to the new version of (xpret mobile )
                    UrlService ObjtoSerialize = new UrlService
                    {
                        DisplayUrlService = Manager.UrlServiceFormatter(Settings.ServiceUrl),
                        Selected = true,
                        Title = Settings.ServiceUrl
                    };
                    List<UrlService> Liste = new List<UrlService>();
                    Liste.Add(ObjtoSerialize);
                    Settings.ServiceUrl = JsonConvert.SerializeObject(Liste);
                    await SaveSettings();
                }
                //end affectation
                oldCaisseDedier = this.Settings.CaisseDedier;
                if (this.Settings == null)
                {
                    this.Settings = new Settings();
                    // App.Settings = this.Settings;
                }

                foreach (var Item in DeviceList)
                {
                    if (Item.Name == Settings.PrinterName && Item.Type == Settings.PrinterType)
                    {
                        SelectedDevice = Item;
                    }
                }
            }

            this.Settings.isModified = false;
        }

        public async Task SaveSettings()
        {
            if (App.User?.ClientId != null)
            {
                if (Settings.SubscribedToFBNotifications)
                {
                    FireBaseHelper.RegisterUserForDefaultTopics(App.User, App.User.ClientId);
                }
                else
                {
                    FireBaseHelper.UnsubscribeFromAllTopics();
                }
            }

            if (this.Settings.CaisseDedier != oldCaisseDedier)
            {
                var res = await SaveSettingsToServer();

            }
            await App.SettingsDatabase.SaveItemAsync(Settings);

            this.Settings.isModified = false;
        }

        internal async Task<bool?> SaveSettingsToServer()
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
                    this.Settings.MachineName = XpertHelper.GetMachineName();
                    await CrudManager.MobileSettings.AddItemAsync(this.Settings);
                    result = true;
                    UserDialogs.Instance.HideLoading();
                }
                else
                {
                    await UserDialogs.Instance.AlertAsync(AppResources.alrt_msg_NoConnexion, AppResources.alrt_msg_Alert, AppResources.alrt_msg_Ok);
                }

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

        #region Comptes 
        public ObservableCollection<View_BSE_COMPTE> ComptesList { get; }
        private BSE_COMPTE _SelectedCompte;
        public BSE_COMPTE SelectedCompte
        {
            get
            {
                return _SelectedCompte;
            }
            set
            {
                _SelectedCompte = value;
                this.Settings.CaisseDedier = _SelectedCompte.CODE_COMPTE;
                OnPropertyChanged("SelectedCompte");
            }
        }
        #endregion

        #region Magasins
        public ObservableCollection<View_BSE_MAGASIN> MagasinsList { get; }

        private View_BSE_MAGASIN _SelectedMagasin;
        public View_BSE_MAGASIN SelectedMagasin
        {
            get
            {
                return _SelectedMagasin;
            }
            set
            {
                _SelectedMagasin = value;
                this.Settings.DefaultMagasinVente = _SelectedMagasin.CODE;
                OnPropertyChanged("SelectedMagasin");
            }
        }
        public async Task LoadMagasins()
        {
            if (App.Online)
            {
                try
                {
                    // Load Magasins
                    MagasinsList.Clear();
                    var itemsC = await CrudManager.BSE_MAGASINS.GetItemsAsync();

                    View_BSE_MAGASIN allElem = new View_BSE_MAGASIN();
                    allElem.CODE = "";
                    allElem.DESIGNATION = "Aucun";
                    MagasinsList.Add(allElem);

                    foreach (var itemC in itemsC)
                    {
                        MagasinsList.Add(itemC);
                        if (itemC.CODE == Settings.DefaultMagasinVente)
                        {
                            SelectedMagasin = itemC;
                        }
                    }

                    // Load Comptes
                    ComptesList.Clear();
                    var itemsCmt = await CrudManager.BSE_COMPTE.GetItemsAsync();

                    View_BSE_COMPTE allcomptes = new View_BSE_COMPTE();
                    allcomptes.CODE_COMPTE = "";
                    allcomptes.DESIGN_COMPTE = "Aucun";
                    ComptesList.Add(allcomptes);

                    foreach (var itemC in itemsCmt)
                    {
                        ComptesList.Add(itemC);
                        if (itemC.CODE_COMPTE == Settings.CaisseDedier)
                        {
                            SelectedCompte = itemC;
                        }
                    }
                }
                catch (Exception ex)
                {
                    await UserDialogs.Instance.AlertAsync(WSApi2.GetExceptionMessage(ex), AppResources.alrt_msg_Alert,
                        AppResources.alrt_msg_Ok);
                }
            }

        }
        #endregion

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

                    if (SelectedDevice.Type == Printer_Type.Bluetooth)
                    {
                        await _blueToothService.Print(SelectedDevice.Name, printMessage);
                    }
                    else if (SelectedDevice.Type == Printer_Type.Wifi)
                    {
                        await UserDialogs.Instance.AlertAsync("L'impression pour les imprimantes wifi n'est pas encore imlémenté", AppResources.alrt_msg_Alert,
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
                DeviceList.Add(new XPrinter()
                {
                    Name = "",
                    Type = ""
                });
                // Bluetooth printer
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
                if (await App.IsConected())
                {
                    var ntworkProinters = await WebServiceClient.GetPrintersList();
                    foreach (var item in ntworkProinters)
                    {
                        DeviceList.Add(item);
                        if (item.Name == Settings.PrinterName && item.Type == Settings.PrinterType)
                        {
                            SelectedDevice = item;
                        }
                    }
                }
            }
            catch (Exception e)
            {
                await UserDialogs.Instance.AlertAsync(e.Message, AppResources.alrt_msg_Alert, AppResources.alrt_msg_Ok);
            }
        }
        #endregion
    }
}

