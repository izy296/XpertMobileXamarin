using Acr.UserDialogs;
using System;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Extended;
using XpertMobileApp.DAL;
using XpertMobileApp.Helpers;
using XpertMobileApp.Models;
using XpertMobileApp.Services;

namespace XpertMobileApp.ViewModels
{
   public class RfidScanViewModel : BaseViewModel
   {
        public IRfidScaner RFScaner = DependencyService.Get<IRfidScaner>();
         
        private View_STK_STOCK currentLot;
        public View_STK_STOCK CurrentLot
        {
            get { return currentLot; }
            set { SetProperty(ref currentLot, value); }
        }

        private int idStock = 15897;
        public int IdStock
        {
            get { return idStock; }
            set { SetProperty(ref idStock, value); }
        }

        public Command loadLotsInfo { get; set; }
        public ObservableCollection<string> Items { get; set; }

        bool loopFlag;

        private int elementsCount;
        public int ElementsCount
        {
            get { return elementsCount; }
            set { SetProperty(ref elementsCount, value); }
        }

        public bool ContinuesScan { get; set; }

        public bool Anti { get; set; } = false;

        public int q { get; set; } = 3;

        public RfidScanViewModel()
        {
            Title = AppResources.pn_RfidScan;

            Items = new ObservableCollection<string>();
            Init();
            
        }

        #region Rfid scaner tool

        public void StopInventory()
        {
            if (loopFlag)
            {
                RFScaner.StopInventory();
                loopFlag = false;
                
            }
        }

        public void Init()
        {
            RFScaner.GetInstance();
            RFScaner.Init();
        }

        public void SatrtContenuesInventary(byte anti, byte q)
        {
            if (RFScaner.SatrtContenuesInventary(anti, q)) {
                loopFlag = true;
                ContinuousRead();
            };
            
        }

        public void StartInventorySingl()
        {
            string element = RFScaner.InventorySingleTag();
            string str = RFScaner.ConvertUiiToEPC(element);
            if (!string.IsNullOrEmpty(str)) {
                if (!Items.Contains(str))
                {
                    MessagingCenter.Send(this, MCDico.RFID_SCANED, str);
                }
                    
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
                        string strEPC;
                        string strTid = "";
                        StringBuilder sb = new StringBuilder();
                        if (res[0].Length != 0 && res[0] != "0000000000000000" && res[0] != "000000000000000000000000")
                        {
                            strTid = "TID:" + res[0] + "\r\n";
                        }
                        strEPC = RFScaner.ConvertUiiToEPC(res[1]);
                      
                        if (!Items.Contains(strEPC)) {
                           MessagingCenter.Send(this, MCDico.RFID_SCANED, strEPC);
                        }
                            
                    }
                }
            }));
            th.Start();
        }
        #endregion
    }
}
