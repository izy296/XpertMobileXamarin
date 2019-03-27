using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using Xamarin.Forms;
using XpertMobileApp.Services;

namespace XpertMobileApp.Api.Services.RFID
{
    public class RfidServices
    {
        bool loopFlag;

        public List<string> ScanedRfield = new List<string>();
 
        public IRfidScaner RFScaner = DependencyService.Get<IRfidScaner>();

        public void StopInventory()
        {
            RFScaner.StopInventory();
            loopFlag = false;
        }
        public void Init() {
            RFScaner.GetInstance();
            RFScaner.Init();
        }

        public void SatrtContenuesInventary(byte anti, byte q) {
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

                        ScanedRfield.Insert(0,sb.ToString());
                    }
                }
            }));
            th.Start();
        }
    }
}
