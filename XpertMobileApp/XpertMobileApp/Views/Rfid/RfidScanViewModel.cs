using Acr.UserDialogs;
using System;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Extended;
using XpertMobileApp.DAL;
using XpertMobileApp.Services;

namespace XpertMobileApp.ViewModels
{
   public class RfidScanViewModel : BaseViewModel
   {
        public IRfidScaner RFScaner = DependencyService.Get<IRfidScaner>();

        public InfiniteScrollCollection<string> Items { get; set; }

        bool loopFlag;

        public int elementsCount { get; set; }

        public bool ContinuesScan { get; set; }

        public bool Anti { get; set; } = false;

        public int q { get; set; } = 3;

        public RfidScanViewModel()
        {
            Title = AppResources.pn_RfidScan;

            Items = new InfiniteScrollCollection<string>();
            Init();
        }

        #region Rfid scaner tool

        public void StopInventory()
        {
            RFScaner.StopInventory();
            loopFlag = false;
        }

        public void Init()
        {
            RFScaner.GetInstance();
            RFScaner.Init();
        }

        public void SatrtContenuesInventary(byte anti, byte q)
        {
            RFScaner.SatrtContenuesInventary(anti, q);
            loopFlag = true;
            ContinuousRead();
        }

        public void StartInventorySingl()
        {
            string element = RFScaner.InventorySingleTag();
            if (!string.IsNullOrEmpty(element)) {
                Items.Add(element);
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
                        strEPC = "EPC:" + RFScaner.ConvertUiiToEPC(res[1])+ "@";
                        sb.Append(strTid);
                        sb.Append(strEPC);
                        sb.Append(res[2]);
                    
                        if(!Items.Contains(sb.ToString()))
                            Items.Add(sb.ToString());
                    }
                }
            }));
            th.Start();
        }
        #endregion
    }
}
