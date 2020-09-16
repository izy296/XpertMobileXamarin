using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;

using Android.App;
using Android.Bluetooth;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Com.Printer.Sdk;
using Java.Lang;
using XpertMobileApp.Api.Services;

[assembly: Xamarin.Forms.Dependency(typeof(XpertMobileApp.Droid.Services.PrinterSPRT))]
namespace XpertMobileApp.Droid.Services
{
	public class MyHandler : Handler
	{
        public bool isConnected { get; set; } = false;
        public event EventHandler<EventArgs> eventHandelsConnected;
        public override void HandleMessage(Message msg)
		{
			switch (msg.What)
			{
				case 101:
                    isConnected = true;
                    eventHandelsConnected.Invoke(this, null);
                    break;
				case 102:
					isConnected = false;

					break;
				case 103:
					isConnected = false;
				
					break;
				case 104:
					isConnected = false;
				
					break;
				
				default:
					break;
			}
		}
    }
	
	public class PrinterSPRT : IPrinterSPRT
    {
		public Handler mHandler = new MyHandler();
        public static PrinterInstance myPrinter;

        public void closeConnection()
        {
            if(myPrinter!=null)
            myPrinter.CloseConnection();
        }

        public int getCurrentStatus()
        {
            if (myPrinter != null)
                return myPrinter.GetPrintingStatus(100);
            else return 0;
        }
        BluetoothDevice btd;
        public bool GetPrinterInstance(EventHandler<EventArgs> ev,string printerName)
        {
           ((MyHandler) mHandler).eventHandelsConnected += ev;
            btd = GetPrinterBluetoothByName(printerName);
            if (btd == null) return false;
            myPrinter = PrinterInstance.GetPrinterInstance(btd, mHandler);
            return myPrinter != null;
        }
        
        public BluetoothDevice GetPrinterBluetoothByName(string printerName)
        {
            BluetoothAdapter btAdapter = BluetoothAdapter.DefaultAdapter;
            if (btAdapter != null)
            {
                var pairedDevices = btAdapter.BondedDevices;

                if (pairedDevices != null && pairedDevices.Count > 0)
                {
                    foreach (BluetoothDevice device in pairedDevices)
                    {
                        string deviceName = device.Name;
                        if (deviceName.Equals(printerName))
                        {
                            return device;
                        }
                    }
                }
            }
            return null;
        }
        public bool openConnection()
        {
            if (myPrinter != null)
                return myPrinter.OpenConnection();
            else return false;
        }


        public int sendBytesData(byte[] srcData)
        {
            if(myPrinter!=null)
            return myPrinter.SendBytesData(srcData);
            else return 0;
        }

        public void setFont(int mCharacterType, int mWidth, int mHeight, int mBold, int mUnderline)
        {
            if(myPrinter!=null)
            myPrinter.SetFont(mCharacterType,  mWidth,  mHeight,  mBold,  mUnderline);
        }

        public void setPrinter(int comand, int value)
        {
            if (myPrinter != null)
                myPrinter.SetPrinter(comand,value);
        }

        public void InitPrinter()
        {
            if (myPrinter != null)
                myPrinter.InitPrinter(); 
        }

        public int PrintText(string content)
        {
            if (myPrinter != null)
                return myPrinter.PrintText(content);
            else return 0;
        }

        public bool isConnected()
        {
            if (mHandler != null)
                return (mHandler as MyHandler).isConnected;
            else return false;
        }
    }
}