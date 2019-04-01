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
        public IRfidScaner RFScaner = DependencyService.Get<IRfidScaner>();
        bool loopFlag;
        public RFID_Manager() {
            loopFlag = false;
            this.Init();
        }
   

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
                MessagingCenter.Send(this, MCDico.RFID_SCANED, str);

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

                        MessagingCenter.Send(this, MCDico.RFID_SCANED, strEPC);
                    }
                }
            }));
            th.Start();
        }

        public void Dispose()
        {
            RFScaner.StopInventory();
        }
    }
}
