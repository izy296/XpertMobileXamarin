using Acr.UserDialogs;
using Plugin.SimpleAudioPlayer;
using System;
using System.IO;
using System.Threading;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using XpertMobileApp.Helpers;
using XpertMobileApp.Services;
using XpertMobileApp.ViewModels;

namespace XpertMobileApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class RfidScanInventairePage : ContentPage
    {
        RfidScanInvnetaireViewModel viewModel;
        private ISimpleAudioPlayer _simpleAudioPlayer;
        RFID_Manager rfid_manager;
        public RfidScanInventairePage()
        {
            InitializeComponent();
            _simpleAudioPlayer = CrossSimpleAudioPlayer.CreateSimpleAudioPlayer();
            Stream beepStream = GetType().Assembly.GetManifestResourceStream("XpertMobileApp.beep.wav"); 
            bool isSuccess = _simpleAudioPlayer.Load(beepStream);
            BindingContext = viewModel = new RfidScanInvnetaireViewModel();
            rfid_manager = new RFID_Manager();
            if (!rfid_manager.IsInit) {
                if (!rfid_manager.Init())
                {
                    UserDialogs.Instance.AlertAsync(AppResources.alert_echec_Init_Rfid, AppResources.alrt_msg_Alert,
                       AppResources.alrt_msg_Ok);
                    btn_Scan.IsEnabled = false;
                    btn_Clear.IsEnabled = false;
                    UpdateInventaire.IsEnabled = false;
                    TraiteRfids.IsEnabled = false;
                }
            }
            viewModel.loadInventaireInfo.Execute(null);
            UpdateInventaire.IsEnabled = false;
            MessagingCenter.Subscribe<RFID_Manager, string>(this, MCDico.RFID_SCANED, (obj, item) =>
            {
                
                if (!string.IsNullOrEmpty(item))
                {
                    string[] strs = item.Split('@');
                    int index = checkIsExistRfid(strs[0]);
                    viewModel.TotalElementsCount++;
                    if (index == -1)
                    {
                        Sound();
                        viewModel.Items.Add(strs[0]);
                        viewModel.ElementsCount++;
                    }

                }
            });
        }


        public int checkIsExistRfid(string strEPC)
        {
            int existFlag = -1;
            if (string.IsNullOrEmpty(strEPC))
            {
                return existFlag;
            }

            String tempStr = "";
            for (int i = 0; i < viewModel.Items.Count; i++)
            {
                tempStr = viewModel.Items[i];
                if (strEPC == tempStr)
                {
                    existFlag = i;
                    break;
                }
            }
            return existFlag;
        }



        private void Sound()
        {
            _simpleAudioPlayer.Play();
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            if (rfid_manager.LoopFlag)
            {
                rfid_manager.StopInventory();
                rfid_manager.LoopFlag = false;
            }
        }
        protected override void OnAppearing()
        {
            base.OnAppearing();
            if (rfid_manager.LoopFlag) btn_Scan.Text = AppResources.btn_Stop_Scan_Rfid;
        }

        private void Filter_Clicked(object sender, EventArgs e)
        {
            FilterPanel.IsVisible = !FilterPanel.IsVisible;
        }

        private void btn_Clear_Clicked(object sender, EventArgs e)
        {
            Clear();
        }

        private void btn_Scan_Clicked(object sender, EventArgs e)
        {
            if (btn_Scan.Text == AppResources.btn_Stop_Scan_Rfid)
            {
                // rfid_manager.StopInventory();
                if (rfid_manager.LoopFlag)
                {
                    rfid_manager.StopInventory();
                    rfid_manager.LoopFlag = false;
                    btn_Scan.Text = AppResources.btn_scan_Rfid_Text;
                    btn_Clear.IsEnabled = true;
                    AntiP.IsEnabled = true;
                    qvalue.IsEnabled = true;
                    UpdateInventaire.IsEnabled = true;
                    return;
                }

            }
            if (!rfid_manager.LoopFlag)
            {
                byte q = 0;
                byte anti = 0;
                if (viewModel.Anti)
                {
                    q = Convert.ToByte(viewModel.q);
                    anti = 1;
                }
 
                if (rfid_manager.StartContenuesInventary(anti, q))
                {
                    rfid_manager.LoopFlag = true;
                    btn_Clear.IsEnabled = false;
                    AntiP.IsEnabled = false;
                    qvalue.IsEnabled = false;
                    UpdateInventaire.IsEnabled = false;
                    btn_Scan.Text = AppResources.btn_Stop_Scan_Rfid;
                };
            }
        }

 
        private void Clear() {
            viewModel.InventoredStock.Clear();
            viewModel.Items.Clear();
            viewModel.ElementsCount = 0;
            viewModel.StockCount = 0;
            viewModel.TotalElementsCount = 0;
        }
        private void UpdateInventaire_Clicked(object sender, EventArgs e)
        {
            if (viewModel.InventoredStock != null
                && viewModel.InventoredStock.Count > 2) {
                viewModel.SaveInventaireCommand.Execute(null);
                UpdateInventaire.IsEnabled = false;
                Clear();
            }
            
        }

        private async void TraiteRfids_Clicked(object sender, EventArgs e)
        {
            try
            {
                TraiteRfids.IsEnabled = false;
                UpdateInventaire.IsEnabled = false;
                var elements = await WebServiceClient.getStockFromRFIDs(viewModel.Items);
                viewModel.InventoredStock.Clear();
                viewModel.StockCount = 0;
                foreach (var item in elements)
                {
                    viewModel.StockCount++;
                    viewModel.InventoredStock.Add(item);
                }
                viewModel.StockCount -= 2;
                UpdateInventaire.IsEnabled = true;
                TraiteRfids.IsEnabled = true;
            }
            catch (Exception ex)
            {
                await UserDialogs.Instance.AlertAsync(ex.Message, AppResources.alrt_msg_Alert,
                    AppResources.alrt_msg_Ok);
            }

        }

        async private void OnItemSelected(object sender, SelectedItemChangedEventArgs e)
        {

        }
    }
}