using Rg.Plugins.Popup.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using XpertMobileApp.DAL;
using XpertMobileApp.Helpers;
using XpertMobileApp.Models;
using XpertMobileApp.Services;
using XpertMobileApp.ViewModels;

namespace XpertMobileApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class RfidScanInventairePage : ContentPage
    {
        RfidScanInvnetaireViewModel viewModel;

        private RFID_Manager rfid_manager;
        public Command traitRfidCommande { get; set; }
        public RfidScanInventairePage()
        {
            InitializeComponent();
            BindingContext = viewModel = new RfidScanInvnetaireViewModel();
            viewModel.loadInventaireInfo.Execute(null);
            rfid_manager = new RFID_Manager();
            rfid_manager.Init();
            
            traitRfidCommande = new Command(async () => await AddAllEPCToList());
            MessagingCenter.Subscribe<RFID_Manager, string>(this, MCDico.RFID_SCANED, async (obj, item) =>
            {
                if (!string.IsNullOrEmpty(item))
                {
                    string[] strs = item.Split('@');
                    if (strs.Length == 2)
                    {
                        if (!string.IsNullOrEmpty(strs[0]))
                        {
                            viewModel.TotalElementsCount++;
                            int index = checkIsExistRfid(strs[0]);
                            if (index == -1)
                            {
                                viewModel.ElementsCount++;
                                viewModel.Items.Add(strs[0]);
                                new Task(async () => await AddEPCToList(strs[0], strs[0])).Start();
                            }
                        }
                    }
                }
            });
        }
        private async Task AddAllEPCToList()
        {
            viewModel.InventoredStock.Clear();
            foreach (string element in viewModel.Items)
            {
                List<View_STK_STOCK_RFID> lots = await WebServiceClient.getStockFromRFID(element);

                if (!(lots == null) && lots.Count > 0)
                {
                    if (lots.Count == 1)
                    {

                        int index = checkIsExistLot(lots[0].ID_STOCK);

                        if (index == -1)
                        {
                            lots[0].QTE_INVENTAIRE = 1;
                            viewModel.InventoredStock.Add(lots[0]);
                            viewModel.StockCount++;
                        }
                        else
                        {
                            viewModel.InventoredStock[index].QTE_INVENTAIRE++;

                        }

                    }
                    else
                    {
                        await DisplayAlert(AppResources.alrt_msg_Alert, "un vignette RFId appartien a plusieurs lots", AppResources.alrt_msg_Ok);
                    }
                }
            }

        }

        private async Task AddEPCToList(String epc, String rssi)
        {
            
                List<View_STK_STOCK_RFID> lots = await WebServiceClient.getStockFromRFID(epc);
                if (!(lots == null) && lots.Count > 0)
                {
                    if (lots.Count == 1)
                    {
                    
                        int index = checkIsExistLot(lots[0].ID_STOCK);

                        if (index == -1)
                        {
                            lots[0].QTE_INVENTAIRE = 1;
                            viewModel.InventoredStock.Add(lots[0]);
                            viewModel.StockCount++;
                        }
                        else
                        {
                            viewModel.InventoredStock[index].QTE_INVENTAIRE++;

                        }
             
                    }
                    else
                    {
                        await DisplayAlert(AppResources.alrt_msg_Alert, "un vignette RFId appartien a plusieurs lots", AppResources.alrt_msg_Ok);
                    }
            }

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

        public int checkIsExistLot(int? idStock)
        {
            int existFlag = -1;
            if (idStock == null)
            {
                return existFlag;
            }

            for (int i = 0; i < viewModel.InventoredStock.Count; i++)
            {
                if (viewModel.InventoredStock[i].ID_STOCK == idStock)
                {
                    existFlag = i;
                    break;
                }
            }
            return existFlag;
        }

        private void Sound()
        {
            // TODO jouer un son pour indiquee que le lecteur RFID a trouver un Tag Rfid


        }
        async void OnItemSelected(object sender, SelectedItemChangedEventArgs args)
        {

        }
        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            rfid_manager.StopInventory();
        }
        protected override void OnAppearing()
        {
            base.OnAppearing();
            if (rfid_manager.LoopFlag) btn_Scan.Text = "Stop";
        }

        private void Filter_Clicked(object sender, EventArgs e)
        {
            FilterPanel.IsVisible = !FilterPanel.IsVisible;
        }

        private void btn_Clear_Clicked(object sender, EventArgs e)
        {
            viewModel.InventoredStock.Clear();
            viewModel.Items.Clear();
            viewModel.ElementsCount = 0;
            viewModel.StockCount = 0;
            viewModel.NotFondCount = 0;
            viewModel.TotalElementsCount = 0;
        }

        private void btn_Scan_Clicked(object sender, EventArgs e)
        {
            if (btn_Scan.Text == "Stop")
            {
                rfid_manager.StopInventory();
                btn_Scan.Text = "Scan";
                btn_Clear.IsEnabled = true;
                AntiP.IsEnabled = true;
                CScan.IsEnabled = true;
                qvalue.IsEnabled = true;
                return;
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
                btn_Clear.IsEnabled = false;
                AntiP.IsEnabled = false;
                CScan.IsEnabled = false;
                qvalue.IsEnabled = false;
                rfid_manager.StartContenuesInventary(Convert.ToByte(viewModel.Anti), (byte)viewModel.q);

                btn_Scan.Text = "Stop";
            }
        }

        private void SaveRfids_Clicked(object sender, EventArgs e)
        {
            viewModel.SaveInventaireCommand.Execute(null);
        }

        private void TraiteRfids_Clicked(object sender, EventArgs e)
        {
            traitRfidCommande.Execute(null);
        }
    }
}