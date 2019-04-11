
using Com.Rscja.Deviceapi;
using XpertMobileApp.Api.Services;
using Xamarin.Forms;
using Com.Zebra.Adc.Decoder;
using Android.App;
using Android.Widget;
using XpertMobileApp.Api.Services.CODE_BARRE;
using System;
using Android.OS;

[assembly: Dependency(typeof(XpertMobileApp.Droid.Services.CODE_BARRE_Reder))]
namespace XpertMobileApp.Droid.Services
{

    public class CODE_BARRE_Reder :  ICODE_BARRE_Reder
    {
        static private Barcode2DWithSoft barcode ;

        public static bool isOpen { get; set; } = false;
        public void setIsOpen(bool _isOpen) {
            isOpen = _isOpen;
        }
        public bool GetIsOpen() {
            return isOpen;
        }
        public bool GetInstance()
        {
           
            if (barcode == null)
            {
                try
                {
                    barcode = Barcode2DWithSoft.Instance;
                }
                catch (Exception ex)
                {
                    Toast.MakeText(MainActivity.Instance, ex.Message, ToastLength.Short);
                }
            }
            return true;
        }

        public void Scan() {
            barcode.Scan();
        }
        public void StopScan()
        {
            barcode.StopScan();
        }
        public void Close()
        {
            barcode.Close();
        }

        
        ICallBackReciver reciver;

        public void  Init (ICallBackReciver _reciver)
        {
            reciver = _reciver;
            new InitTask(_reciver).Execute();

        }
        private class InitTask : AsyncTask<Java.Lang.Void, Java.Lang.Void, string[]>
        {
            ICallBackReciver reciver;

            public InitTask(ICallBackReciver _reciver)
            {
                reciver = _reciver;
            }

            ProgressDialog proDialg = null;

            protected override string[] RunInBackground(params Java.Lang.Void[] @params)
            {
                return null;
            }

            protected override Java.Lang.Object DoInBackground(params Java.Lang.Object[] @params)
            {
                string result = string.Empty;
                if (barcode != null)
                {
                    if (barcode.Open(MainActivity.Instance))
                    {
                        barcode.SetParameter(402, 1);
                        result = "OK";
                        isOpen = true;
                        barcode.SetScanCallback(new BarcodeCallback(reciver));
                    }
                }
                return result;
            }
            protected override void OnPostExecute(Java.Lang.Object result)
            {
                proDialg.Cancel();
                if (result.ToString() != "OK")
                    Toast.MakeText(MainActivity.Instance, "Init failure!", ToastLength.Short);
            }

            //开始执行任务
            protected override void OnPreExecute()
            {
                proDialg = new ProgressDialog(MainActivity.Instance);
                proDialg.SetMessage("init.....");
                proDialg.Show();
            }
        }
        public class BarcodeCallback : Java.Lang.Object, Barcode2DWithSoft.IScanCallback
        {
            public static string ScanedData = "";
            ICallBackReciver reciver;

            public BarcodeCallback(ICallBackReciver reciver)
            {
                this.reciver = reciver;
            }


            public void OnScanComplete(int symbology, int length, byte[] data)
            {
                string strData = "";
                if (length < 1)
                {
                    if (length == -1)
                    {
                        strData = "Scan canceled\r\n";
                    }
                    else if (length == 0)
                    {
                        strData = "Scan timeout\r\n";
                    }
                    else
                    {
                        strData = "Scan failure\r\n";
                    }
                }
                else
                {
                    strData = System.Text.ASCIIEncoding.ASCII.GetString(data, 0, length);

                }
                reciver.ReciveData( strData);
                barcode.StopScan();
            }
        }
    }
}