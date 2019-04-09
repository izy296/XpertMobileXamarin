using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using Xamarin.Forms;
using XpertMobileApp.Helpers;
using XpertMobileApp.Services;

namespace XpertMobileApp.ViewModels
{
    public class RFID_Manager :IDisposable
    {
        private static IRfidScaner RFScaner = null;
        private static bool loopFlag;
        private static bool isInit=false;
        public bool IsInit
        {
            get { return isInit; }
            set { isInit = value; }
        }
        public bool LoopFlag {
            get { return loopFlag; }
            set { loopFlag = value; }
        }
        public RFID_Manager() {
            if(RFScaner == null)
            {
                RFScaner= DependencyService.Get<IRfidScaner>();
                RFScaner.GetInstance();
            }
            loopFlag = false;
        }
   

        public void StopInventory()
        {
            if (loopFlag)
            {
                RFScaner.StopInventory();
                loopFlag = false;
            }
        }

        public bool Init()
        {
            if (!isInit) {
                loopFlag = false;
                isInit = RFScaner.Init();
            }
            return isInit;
        }

        public void StartContenuesInventary(byte anti, byte q)
        {
            if (RFScaner.SatrtContenuesInventary(anti, q))
            {
                loopFlag = true;
                ContinuousRead();
            };

        }

        public void StartInventorySingl()
        {
            string element = RFScaner.InventorySingleTag();
            string str = RFScaner.ConvertUiiToEPC(element);
            if (!string.IsNullOrEmpty(str))
            {
                MessagingCenter.Send(this, MCDico.RFID_SCANED, str+"@");

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
                        strEPC = "EPC:" + RFScaner.ConvertUiiToEPC(res[1]) + "@";
                        sb.Append(strTid);
                        sb.Append(strEPC);
                        sb.Append(res[2]);
                        MessagingCenter.Send(this, MCDico.RFID_SCANED, sb.ToString());
                    }
                }
            }));
            th.Start();
        }

        public void Dispose()
        {
            if (RFScaner != null)
            {
                RFScaner.StopInventory();
            }
            loopFlag = false;
        }
    }
}
