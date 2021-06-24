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

namespace XpertMobileApp.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class SettingsPage : ContentPage
	{

        public ObservableCollection<Language> Languages { get; }

        public SettingsModel viewModel;

        // public static IPrinterSPRT printerLocal;
        public SettingsPage()
        {
            InitializeComponent();
            BindingContext = viewModel = new SettingsModel();
            // printerLocal = DependencyService.Get<IPrinterSPRT>();
            LanguagesPicker.SelectedItem = viewModel.GetLanguageElem(viewModel.Settings.Language);
        }


        public SettingsPage (bool isModal = false)
		{
			InitializeComponent();
            //CommandeGrid.IsVisible = isModal;
            BindingContext = viewModel = new SettingsModel();

            LanguagesPicker.SelectedItem = viewModel.GetLanguageElem(viewModel.Settings.Language);
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

            Client client = App.ClientDatabase.GetFirstItemAsync().Result;
            if(client != null)
            {
                DateTime expirationDate = LicActivator.GetLicenceEndDate(client.LicenceTxt).Result;
                lbl_ExperationDate.Text = string.Format("{0} : {1}", TranslateExtension.GetTranslation("msg_ExpireOn"),
                    expirationDate.ToString("dd/MM/yyyy"));
            }

            if(viewModel.IsConnected) 
            {
                await viewModel.BindDeviceList();

                await viewModel.LoadMagasins();
            }

            viewModel.LoadSettings();
        }

        private void LanguagesPicker_SelectedIndexChanged(object sender, EventArgs e)
        {
            var language = viewModel.Languages[LanguagesPicker.SelectedIndex];

            App.SetAppLanguage(language.ShortName);

            lbl_LanguageSelector.Text = AppResources.sp_lbl_SelectLanguage;
            lbl_ServiceName.Text      = AppResources.sp_lbl_ServiceName;
            Btn_CloseSettings.Text    = AppResources.btn_Close;
            Btn_SaveSettings.Text     = AppResources.sp_btn_SaveSettings;
            Btn_TestCnx.Text          = AppResources.sp_btn_TestConnection;
            lbl_ConnexionInfos.Text   = AppResources.sp_lbl_ConnexionInfos;
            Btn_RemoveLicence.Text    = AppResources.sp_btn_RemoveLicence;
            lbl_LicenceInfos.Text     = AppResources.sp_lbl_LicenceInfos;
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
                    viewModel.SaveSettings();
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
            if (string.IsNullOrEmpty(App.RestServiceUrl))
            {
                await DisplayAlert(AppResources.alrt_msg_Info, AppResources.alrt_msg_MissingServerInfos, AppResources.alrt_msg_Ok);
                return;
            }

            try
            {
                string serviceUrl = et_ServiceName.Text?.Trim();
                bool reachable = await this.IsBlogReachableAndRunning(serviceUrl);
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
                Regex r = new Regex(@"^(?<proto>\w+)://[^/]+?(?<port>:\d+)?/",RegexOptions.None, TimeSpan.FromMilliseconds(150));
                Match m = r.Match(url);
                /*
                foreach (var item in m.Groups)
                {
                    var l = item.get;
                }
                */
                if (m.Success && ! string.IsNullOrEmpty(m.Groups["port"]?.Value))
                {
                    port = Convert.ToInt32(m.Groups["port"].Value.Replace(":",""));
                    // Console.WriteLine(m.Result("${proto}${port}"));
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
    }
}