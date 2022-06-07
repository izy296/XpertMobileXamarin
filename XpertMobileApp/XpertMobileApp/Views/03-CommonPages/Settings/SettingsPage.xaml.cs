using XpertMobileApp.ViewModels;
using Plugin.Connectivity;
using System;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using System.Collections.ObjectModel;
using XpertMobileApp.Model;
using XpertMobileApp.Api.Services;
using XpertMobileApp.Models;
using XpertMobileApp.Helpers;
using System.Text.RegularExpressions;
using XpertWebApi.Models;
using System.Collections.Generic;
using XpertMobileApp.SQLite_Managment;
using Xpert.Common.WSClient.Helpers;
using Acr.UserDialogs;
using XpertMobileApp.DAL;
using XpertMobileApp.Api.Models;
using Newtonsoft.Json;
using XpertMobileApp.Views.Helper;
using Rg.Plugins.Popup.Services;
using XpertMobileApp.Views._04_Comercial.Selectors.Settings;
using TranslateExtension = XpertMobileApp.Helpers.TranslateExtension;

namespace XpertMobileApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SettingsPage : ContentPage
    {

        public ObservableCollection<Language> Languages { get; }
        public ObservableCollection<UrlService> UrlServices { get; set; }

        public SettingsModel viewModel;

        //private SettingsSelector itemSelector;

        public string Greeting { get; set; }
        // public static IPrinterSPRT printerLocal;
        public SettingsPage()
        {
            InitializeComponent();
            BindingContext = viewModel = new SettingsModel();
            // printerLocal = DependencyService.Get<IPrinterSPRT>();
            LanguagesPicker.SelectedItem = viewModel.GetLanguageElem(viewModel.Settings.Language);

            //Set the selected Item from urlService....
            UrlServicePicker.SelectedItem = viewModel.GetUrlService();
        }

        public SettingsPage(bool isModal = false)
        {
            InitializeComponent();
            //CommandeGrid.IsVisible = isModal;
            BindingContext = viewModel = new SettingsModel();
            //itemSelector = new SettingsSelector(CurrentStream);

            LanguagesPicker.SelectedItem = viewModel.GetLanguageElem(viewModel.Settings.Language);

            //Set the selected Item from urlService....
            UrlServicePicker.SelectedItem = viewModel.GetUrlService();

        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            /*
            if (printerLocal == null)
            {
                printerLocal = DependencyService.Get<IPrinterSPRT>();
            }
            else
            {
                updateUI();
            }
            */
            LanguagesPicker.SelectedIndexChanged += LanguagesPicker_SelectedIndexChanged;
            UrlServicePicker.SelectedIndexChanged += UrlServicePicker_SelectedIndexChanged;

            //Set the index of the UrlService in the Picker to display it as default  
            UrlServicePicker.SelectedIndex = GetServiceUrlIndex();

            Client client = App.ClientDatabase.GetFirstItemAsync().Result;
            if (client != null)
            {
                DateTime expirationDate = LicActivator.GetLicenceEndDate(client.LicenceTxt).Result;
                lbl_ExperationDate.Text = string.Format("{0} : {1}", TranslateExtension.GetTranslation("msg_ExpireOn"),
                    expirationDate.ToString("dd/MM/yyyy"));
            }

            if (viewModel.IsConnected)
            {
                await viewModel.BindDeviceList();

                await viewModel.LoadMagasins();
            }

            viewModel.LoadSettings();
        }

        /// <summary>
        /// Search in the local Database the index of the selected UrlService
        /// </summary>
        /// <returns></returns>
        private int GetServiceUrlIndex()
        {
            bool finded = false;
            int i = 0;
            List<UrlService> listeUrlService;
            try
            {
                if (Manager.isJson(viewModel.Settings.ServiceUrl))
                {
                    listeUrlService = JsonConvert.DeserializeObject<List<UrlService>>(viewModel.Settings.ServiceUrl);
                    foreach (var Item in listeUrlService)
                    {
                        if (Item.Selected == true)
                        {
                            finded = true;
                            break;
                        }
                        else
                            i++;
                    }

                    if (finded == true)
                        return i;
                    else
                        return 0;
                }
                else return 0;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        private void LanguagesPicker_SelectedIndexChanged(object sender, EventArgs e)
        {
            var language = viewModel.Languages[LanguagesPicker.SelectedIndex];

            App.SetAppLanguage(language.ShortName);
            lbl_LanguageSelector.Text = AppResources.sp_lbl_SelectLanguage;
            //lbl_ServiceName.Text = AppResources.sp_lbl_ServiceName;
            Btn_CloseSettings.Text = AppResources.btn_Close;
            Btn_SaveSettings.Text = AppResources.sp_btn_SaveSettings;
            //Btn_TestCnx.Text = AppResources.sp_btn_TestConnection;
            //lbl_ConnexionInfos.Text = AppResources.sp_lbl_ConnexionInfos;
            Btn_RemoveLicence.Text = AppResources.sp_btn_RemoveLicence;
            lbl_LicenceInfos.Text = AppResources.sp_lbl_LicenceInfos;
        }
        private void UrlServicePicker_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (UrlServicePicker.SelectedIndex != -1)
            {
                var urlService = viewModel.UrlServices[UrlServicePicker.SelectedIndex];
                App.SetUrlServices(urlService);
            }
            else
            {
                App.SetUrlServices(new UrlService
                { Selected = true, Title = "", DisplayUrlService = "" });
            }


        }

        protected override async void OnDisappearing()
        {
            base.OnDisappearing();

            if (viewModel.Settings.isModified)
            {
                var action = await DisplayAlert(AppResources.alrt_msg_title_Settings, AppResources.alrt_msg_SaveSettings,
                AppResources.alrt_msg_Ok, AppResources.alrt_msg_Cancel);
                if (action)
                {
                    await viewModel.SaveSettings();
                }
            }
        }

        protected void SettingsClose_Clicked(object sender, EventArgs e)
        {
            this.Navigation.PopModalAsync(true);
        }

        protected async void SaveSettings_Clicked(object sender, EventArgs e)
        {
            await viewModel.SaveSettings();
            await DisplayAlert(AppResources.alrt_msg_Info, AppResources.alrt_msg_SettingsSaved, AppResources.alrt_msg_Ok);
        }

        protected async void ConfigurerAppareil_Clicked(object sender, EventArgs e)
        {
            try
            {
                var res = await SQLite_Manager.AjoutPrefix();
                if (res != null)
                {
                    await DisplayAlert("Succes", AppResources.txt_actionsSucces, AppResources.alrt_msg_Ok);
                    await SQLite_Manager.getInstance().CreateTableAsync<SYS_CONFIGURATION_MACHINE>();
                    var id = await SQLite_Manager.getInstance().InsertAsync(res);
                    //RecupererPrefix_Clicked(sender,e);
                }
            }
            catch (Exception ex)
            {
                await UserDialogs.Instance.AlertAsync(WSApi2.GetExceptionMessage(ex), AppResources.alrt_msg_Alert, AppResources.alrt_msg_Ok);
            }
        }

        protected async void RecupererPrefix_Clicked(object sender, EventArgs e)
        {
            try
            {
                var res = await SQLite_Manager.getPrefix();
                if (res != null && !(string.IsNullOrEmpty(res.PREFIX)))
                {
                    App.PrefixCodification = res.PREFIX;
                    await DisplayAlert("Succes", AppResources.txt_actionsSucces, AppResources.alrt_msg_Ok);
                }
                else
                {
                    await UserDialogs.Instance.AlertAsync("Veuillez configurer votre prefixe!!", AppResources.alrt_msg_Alert, AppResources.alrt_msg_Ok);
                }
            }
            catch (Exception ex)
            {
                await UserDialogs.Instance.AlertAsync("Veuillez configurer votre prefixe!!", AppResources.alrt_msg_Alert, AppResources.alrt_msg_Ok);
            }
        }

        protected async void TestConnexion_Clicked(object sender, EventArgs e)
        {
            // Check if the WebService is configured
            if (string.IsNullOrEmpty(App.RestServiceUrl) || UrlServicePicker.SelectedIndex == -1)
            {
                await DisplayAlert(AppResources.alrt_msg_Info, AppResources.alrt_msg_MissingServerInfos, AppResources.alrt_msg_Ok);
                return;
            }

            try
            {
                //Get the Service Url from the picker and check if it is reachable....
                string itemSelected = this.UrlServicePicker.Items[UrlServicePicker.SelectedIndex];
                var listeUrlService = JsonConvert.DeserializeObject<List<UrlService>>(viewModel.Settings.ServiceUrl);
                string url = "";

                for (int i = 0; i < listeUrlService.Count; i++)
                {
                    if (listeUrlService[i].Title == itemSelected.ToString())
                    {
                        url = listeUrlService[i].DisplayUrlService;
                        break;
                    }

                }

                bool reachable = await this.IsBlogReachableAndRunning(url);
                if (reachable)
                {
                    await DisplayAlert(AppResources.alrt_msg_Info, AppResources.alrt_msg_ConnectionSucces, AppResources.alrt_msg_Ok);
                }
                else
                {
                    await DisplayAlert(AppResources.alrt_msg_Info, AppResources.alrt_msg_ConnectionError, AppResources.alrt_msg_Ok);
                }
            }
            catch
            {
                await DisplayAlert(AppResources.alrt_msg_Info, AppResources.alrt_msg_CantTestConnexionSettings, AppResources.alrt_msg_Ok);
            }
        }

        public async Task<bool> IsBlogReachableAndRunning(string url, int msTimeout = 5000)
        {
            try
            {
                var connectivity = CrossConnectivity.Current;
                if (!connectivity.IsConnected)
                    return false;

                // Port
                int port = 80;
                Regex r = new Regex(@"^(?<proto>\w+)://[^/]+?(?<port>:\d+)?/", RegexOptions.None, TimeSpan.FromMilliseconds(150));
                Match m = r.Match(url);

                if (m.Success && !string.IsNullOrEmpty(m.Groups["port"]?.Value))
                {
                    port = Convert.ToInt32(m.Groups["port"].Value.Replace(":", ""));
                }
                var uri = new System.Uri(url);
                TimeSpan ts = new TimeSpan(0, 0, 0, msTimeout);
                string furl = url.Replace(":" + port.ToString(), "");
                bool reachable = await connectivity.IsRemoteReachable(furl, port, msTimeout);

                return reachable;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        private async void Btn_RemoveLicence_Clicked(object sender, EventArgs e)
        {
            var res = await DisplayAlert(AppResources.msg_Confirmation, AppResources.alrt_msg_CondifirmDesactivateLicence, AppResources.alrt_msg_Ok, AppResources.alrt_msg_Cancel);
            if (res)
            {
                bool result = await viewModel.DeactivateClient();
                if (result)
                {
                    await DisplayAlert(AppResources.alrt_msg_Info,
                                String.Format(AppResources.msg_DeactivationSucces), AppResources.alrt_msg_Ok);

                    if (Device.RuntimePlatform == Device.Android)
                    {
                        Application.Current.MainPage = new ActivationPage();
                    }
                    else if (Device.RuntimePlatform == Device.iOS)
                    {
                        await Navigation.PushModalAsync(new ActivationPage(), false);
                    }
                    else
                    {
                        Application.Current.MainPage = new ActivationPage();
                    }
                }
                else
                {

                }
            }
        }

        #region Test de l'imprémante par Sofiane
        /*
        private void updateUI()
        {
            if (printerLocal != null)
            {
                if (printerLocal.isConnected())
                {
                    this.Btn_desconnectPrinter.IsEnabled = true;
                    this.Btn_ConnectPrinter.IsEnabled = false;
                    this.Btn_Print.IsEnabled = true;
                }
                else
                {
                    this.Btn_desconnectPrinter.IsEnabled = false;
                    this.Btn_ConnectPrinter.IsEnabled = true;
                    this.Btn_Print.IsEnabled = false;
                }
            }
            else
            {
                this.Btn_desconnectPrinter.IsEnabled = false;
                this.Btn_ConnectPrinter.IsEnabled = true;
                this.Btn_Print.IsEnabled = false;
            }
        }
        async void eventUpdateUI(object sender, EventArgs e)
        {
            this.updateUI();
        }
        async void ConnectPrinterAsync(object sender, EventArgs e)
        {
            printerLocal.GetPrinterInstance(eventUpdateUI, viewModel.SelectedDevice?.Name);
            printerLocal.openConnection();
        }

        async void DesconnectPrinterAsync(object sender, EventArgs e)
        {
            printerLocal.closeConnection();
            updateUI();
        }
        async void PrintExempleAsync(object sender, EventArgs e)
        {
            List<Get_Print_VTE_TiketCaisse> data = new List<Get_Print_VTE_TiketCaisse>();
            data.Add(new Get_Print_VTE_TiketCaisse
            {
                ADRESSE_PHARM = "ANNABA",
                CREATED_BY = "Administrateur",
                DATE_VENTE = DateTime.Now,
                DESIGNATION_PRODUIT = "JAVEL",
                NOM_PHARM = "LEMLOUM MOURAD",
                NOM_TIERS = "comptoire",
                MT_TTC = 12000.56m,
                DESIGNATION_VTE = "Bon livraison",
                TOTAL_ENCAISS_REAL = 500,
                TOTAL_TTC = 500,
                MT_RECU = 500,
                QUANTITE = 2,
                PRIX_VTE_TTC = 6000

            });
            data.Add(new Get_Print_VTE_TiketCaisse
            {
                ADRESSE_PHARM = "ANNABA",
                CREATED_BY = "Administrateur",
                DATE_VENTE = DateTime.Now,
                DESIGNATION_PRODUIT = "Grizil",
                NOM_PHARM = "LEMLOUM MOURAD",
                NOM_TIERS = "comptoire",
                MT_TTC = 300.25m,
                DESIGNATION_VTE = "Bon livraison",
                TOTAL_ENCAISS_REAL = 500,
                TOTAL_TTC = 12500,
                MT_RECU = 500,
                QUANTITE = 100,
                PRIX_VTE_TTC = 30
            });
            printerLocal.setPrinter(13, 0);
            printerLocal.setPrinter(13, 0);
            printerLocal.setFont(0, 0, 1, 1, 0);
            printerLocal.PrintText(data[0].NOM_PHARM + Environment.NewLine);
            printerLocal.PrintText(data[0].ADRESSE_PHARM + Environment.NewLine);
            printerLocal.PrintText("-----------------------------------------------" + Environment.NewLine);
            printerLocal.setFont(0, 0, 0, 1, 0);
            string date = String.Format($"Date :{data[0].DATE_VENTE:dd/MM/yyyy}") + Environment.NewLine;
            printerLocal.PrintText(date);
            printerLocal.PrintText("Etablie par : " + data[0].CREATED_BY + Environment.NewLine);
            printerLocal.PrintText("-----------------------------------------------" + Environment.NewLine);
            printerLocal.setFont(0, 0, 0, 1, 0);
            printerLocal.PrintText("Designation        Qte   Prix       MT " + Environment.NewLine);
            printerLocal.PrintText("-----------------------------------------------" + Environment.NewLine);
            string datvalue = "";
            foreach (Get_Print_VTE_TiketCaisse item in data)
            {
                datvalue = string.Format($"{item.DESIGNATION_PRODUIT,-18} {item.QUANTITE,-5:N1} {item.PRIX_VTE_TTC,-10:0.00} {item.MT_TTC,-12:0.00}") + Environment.NewLine;
                printerLocal.PrintText(datvalue);
            }
            printerLocal.PrintText("-----------------------------------------------" + Environment.NewLine);
            printerLocal.PrintText(string.Format($"                      Total : {data[0].TOTAL_TTC:0.00}") + Environment.NewLine);
            printerLocal.PrintText("Mt.recue :" + data[0].MT_RECU + Environment.NewLine);
            printerLocal.PrintText("Mt.Rendue :" + (data[0].MT_RECU - data[0].TOTAL_TTC) + Environment.NewLine);

        }
        */
        #endregion


        /// <summary>
        /// Add new Configuration (UrlService)...
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public async void Add_ServiceUrl(object sender, EventArgs e)
        {
            var page = new SettingsSelector();
            page.data += async (popupSender, urlData) =>
            {
                var desirliazedData = JsonConvert.DeserializeObject<SettingsSelectorViewModel>(urlData);

                if (desirliazedData != null)
                {
                    try
                    {
                        //addToCard the new UrlService to the liste of urlServices
                        viewModel.UrlServices.Add(new UrlService
                        {
                            DisplayUrlService = desirliazedData.Url,
                            Title = desirliazedData.Titre,
                            Selected = false,
                        });

                        //render the the new list in the picker ...
                        List<UrlService> listeUrlService;
                        if (!string.IsNullOrEmpty(viewModel.Settings.ServiceUrl))
                        {
                            //deserliaze the list stored in local db 
                            if (Manager.isJson(viewModel.Settings.ServiceUrl))
                            {
                                listeUrlService = JsonConvert.DeserializeObject<List<UrlService>>(viewModel.Settings.ServiceUrl);
                            }
                            else
                            {
                                listeUrlService = new List<UrlService>();
                            }
                        }
                        else
                        {
                            listeUrlService = new List<UrlService>();
                        }
                        //Here we add the new Url to the liste
                        UrlService ObjtoSerialize = new UrlService
                        {
                            DisplayUrlService = desirliazedData.Url,
                            Selected = false,
                            Title = desirliazedData.Titre,
                        };
                        listeUrlService.Add(ObjtoSerialize);
                        for (int i = 0; i < viewModel.UrlServices.Count; i++)
                        {
                            viewModel.Settings.ServiceUrl = JsonConvert.SerializeObject(listeUrlService);
                        }
                    }
                    catch (Exception ex)
                    {
                        await UserDialogs.Instance.AlertAsync(ex.Message);
                    }
                }

            };
            await PopupNavigation.Instance.PushAsync(page);
        }
        /// <summary>
        /// Remove Configuration (Url Service) ....
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void DeleteService(object sender, EventArgs e)
        {
            try
            {
                if (UrlServicePicker.SelectedIndex == -1)
                {
                    await DisplayAlert(AppResources.txt_alert, AppResources.sp_txt_alert_supression, AppResources.alrt_msg_Ok);
                    return;
                }
                var result = await DisplayAlert(AppResources.txt_sp_url, AppResources.txt_suppression_message_url, AppResources.alrt_msg_Ok, AppResources.alrt_msg_Cancel);
                if (result)
                {
                    List<UrlService> listeUrlService;
                    if (viewModel.Settings.ServiceUrl != "")
                    {
                        listeUrlService = JsonConvert.DeserializeObject<List<UrlService>>(viewModel.Settings.ServiceUrl);
                        string itemSelected = UrlServicePicker.Items[UrlServicePicker.SelectedIndex];
                        for (int i = 0; i < listeUrlService.Count; i++)
                        {
                            if (listeUrlService[i].Title == itemSelected)
                            {
                                listeUrlService.RemoveAt(i);
                                await DisplayAlert(AppResources.txt_supp_reussite, AppResources.txt_supp_message_url, AppResources.alrt_msg_Ok);
                                break;
                            }
                        }
                        viewModel.Settings.ServiceUrl = JsonConvert.SerializeObject(listeUrlService);

                        //delete the item from the picker 
                        object url = UrlServicePicker.SelectedItem;
                        UrlService UrlToBeDeleted = url as UrlService;
                        viewModel.UrlServices.Remove(UrlToBeDeleted);

                        //save the new Setting...
                        await viewModel.SaveSettings();
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        /// <summary>
        /// Edit Configuration (Url Service ) ... 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void EditService(object sender, EventArgs e)
        {
            try
            {
                List<UrlService> listeUrlService;
                string titre = "";
                string urlService = "";
                if (UrlServicePicker.SelectedIndex == -1)
                {
                    await DisplayAlert(AppResources.txt_alert, AppResources.sp_txt_alert_modification, AppResources.alrt_msg_Ok);
                    return;
                }

                var page = new EditSettingsSelector(titre, urlService);
                listeUrlService = JsonConvert.DeserializeObject<List<UrlService>>(viewModel.Settings.ServiceUrl);
                foreach (var item in listeUrlService)
                {
                    if (item.Selected == true)
                    {
                        titre = item.Title;
                        urlService = item.DisplayUrlService;
                    }
                }

                if (!string.IsNullOrEmpty(titre) && !string.IsNullOrEmpty(urlService))
                {
                    page = new EditSettingsSelector(titre, urlService);
                }
                page.data += async (popupSender, urlData) =>
                {
                    var desirliazedData = JsonConvert.DeserializeObject<SettingsSelectorViewModel>(urlData);

                    if (desirliazedData != null)
                    {
                        //getting the item selected here from the picker ...
                        string ItemSelected = UrlServicePicker.Items[UrlServicePicker.SelectedIndex];

                        //get the url selected from the picker ..
                        object url = UrlServicePicker.SelectedItem;
                        UrlService urlToBeModified = url as UrlService;

                        if (viewModel.Settings.ServiceUrl != "")
                        {
                            //Search in the deserialized liste the element that will be modified...
                            listeUrlService = JsonConvert.DeserializeObject<List<UrlService>>(viewModel.Settings.ServiceUrl);
                            for (int i = 0; i < listeUrlService.Count; i++)
                            {
                                if (listeUrlService[i].Title == ItemSelected)
                                {
                                    listeUrlService[i].DisplayUrlService = desirliazedData.Url;
                                    listeUrlService[i].Title = desirliazedData.Titre;
                                }
                            }
                            //serialize the result list
                            viewModel.Settings.ServiceUrl = JsonConvert.SerializeObject(listeUrlService);

                            //update the itemSource of the picker to show new result 
                            UrlService newUrl = new UrlService
                            {
                                DisplayUrlService = desirliazedData.Url,
                                Title = desirliazedData.Titre
                            };

                            int indexModified = viewModel.UrlServices.IndexOf(urlToBeModified);
                            listeUrlService[indexModified] = newUrl;
                            viewModel.UrlServices.Remove(urlToBeModified);
                            viewModel.UrlServices.Insert(indexModified, new UrlService
                            {
                                DisplayUrlService = listeUrlService[indexModified].DisplayUrlService,
                                Selected = listeUrlService[indexModified].Selected,
                                Title = listeUrlService[indexModified].Title
                            });

                            //Save all settings
                            await viewModel.SaveSettings();
                            await DisplayAlert(AppResources.txt_modification_succee, AppResources.txt_modification_message, AppResources.alrt_msg_Ok);
                        }
                    };
                };
                await PopupNavigation.Instance.PushAsync(page);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}