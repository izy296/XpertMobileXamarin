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
        private IRfidScaner RFScaner = null;
        private static bool loopFlag;
        private ISimpleAudioPlayer _simpleAudioPlayer;
        public RfidScanInventairePage()
        {
            InitializeComponent();
            _simpleAudioPlayer = CrossSimpleAudioPlayer.CreateSimpleAudioPlayer();
            Stream beepStream = GetType().Assembly.GetManifestResourceStream("XpertMobileApp.beep.wav"); 
            bool isSuccess = _simpleAudioPlayer.Load(beepStream);
            BindingContext = viewModel = new RfidScanInvnetaireViewModel();
            RFScaner = DependencyService.Get<IRfidScaner>();
            RFScaner.GetInstance();
            loopFlag = false;
            if (!RFScaner.Init()) {
                UserDialogs.Instance.AlertAsync("Echec d'initialisation de lecteur RFID !", AppResources.alrt_msg_Alert,
                   AppResources.alrt_msg_Ok);
                btn_Scan.IsEnabled = false;
                btn_Clear.IsEnabled = false;
                UpdateInventaire.IsEnabled = false;
                TraiteRfids.IsEnabled = false;
            }
            viewModel.loadInventaireInfo.Execute(null);
            UpdateInventaire.IsEnabled = false;
            MessagingCenter.Subscribe<RfidScanInventairePage, string>(this, MCDico.RFID_SCANED, (obj, item) =>
            {
                
                if (!string.IsNullOrEmpty(item))
                {
                    viewModel.TotalElementsCount++;
                    int index = checkIsExistRfid(item);
                    if (index == -1)
                    {
                        Sound();
                        viewModel.Items.Add(item);
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
            if (loopFlag)
            {
                RFScaner.StopInventory();
                loopFlag = false;
            }
        }
        protected override void OnAppearing()
        {
            base.OnAppearing();
            if (loopFlag) btn_Scan.Text = "Stop";
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
            if (btn_Scan.Text == "Stop")
            {
                // rfid_manager.StopInventory();
                if (loopFlag)
                {
                    RFScaner.StopInventory();
                    loopFlag = false;
                    btn_Scan.Text = "Scan";
                    btn_Clear.IsEnabled = true;
                    AntiP.IsEnabled = true;
                    CScan.IsEnabled = true;
                    qvalue.IsEnabled = true;
                    UpdateInventaire.IsEnabled = true;
                    return;
                }

            }
            if (!loopFlag)
            {
                byte q = 0;
                byte anti = 0;
                if (viewModel.Anti)
                {
                    q = Convert.ToByte(viewModel.q);
                    anti = 1;
                }

                if (RFScaner.SatrtContenuesInventary(anti, q))
                {
                    loopFlag = true;
                    btn_Clear.IsEnabled = false;
                    AntiP.IsEnabled = false;
                    CScan.IsEnabled = false;
                    qvalue.IsEnabled = false;
                    UpdateInventaire.IsEnabled = false;
                    btn_Scan.Text = "Stop";
                    ContinuousRead();
                };
            }
        }

        public void ContinuousRead()
        {

            Thread th = new Thread(new ThreadStart(delegate
            {
                while (loopFlag)
                {
                    string[] res = RFScaner.ReadTagFromBuffer();
                    if (res != null)
                    {
                        if (!string.IsNullOrEmpty(res[1]))
                        {
                            string str = "EPC:" + RFScaner.ConvertUiiToEPC(res[1]);
                            MessagingCenter.Send(this, MCDico.RFID_SCANED, str);
                        };
                    }
                }
            }));
            th.Start();
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
            viewModel.SaveInventaireCommand.Execute(null);
            UpdateInventaire.IsEnabled = false;
            Clear();
        }

        private async void TraiteRfids_Clicked(object sender, EventArgs e)
        {
            try
            {
                TraiteRfids.IsEnabled = false;
                UpdateInventaire.IsEnabled = false;
                CScan.IsEnabled = false;
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
                CScan.IsEnabled = true;
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