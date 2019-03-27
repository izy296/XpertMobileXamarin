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

        public int Anti { get; set; }

        public RfidScanViewModel()
        {
           Title = AppResources.pn_RfidScan;

            Items = new InfiniteScrollCollection<string>();
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

        public string StartInventorySingl(bool Continuous)
        {
            return RFScaner.InventorySingleTag();
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
                        strEPC = "EPC:" + RFScaner.ConvertUiiToEPC(res[1]) + "@";
                        sb.Append(strTid);
                        sb.Append(strEPC);
                        sb.Append(res[2]);

                        Items.Insert(0, sb.ToString());
                    }
                }
            }));
            th.Start();
        }
        #endregion
    }
}
