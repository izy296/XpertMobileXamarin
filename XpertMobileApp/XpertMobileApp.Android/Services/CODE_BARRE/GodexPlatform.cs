using Android.Bluetooth;
using Com.Godex;
using System;
using System.Collections.Generic;
using Xamarin.Forms;
using XpertMobileApp.Api.Services;



[assembly: Dependency(typeof(XpertMobileApp.Droid.Services.GodexPlatform))]
namespace XpertMobileApp.Droid.Services
{
    public class GodexPlatform : IGodexPlatform
    {
        public string GetMX30HardWareName() {
            BluetoothAdapter btAdapter = BluetoothAdapter.DefaultAdapter;
            if (btAdapter != null)
            {
                var pairedDevices = btAdapter.BondedDevices ;

                if (pairedDevices!=null && pairedDevices.Count> 0)
                {
                    foreach (BluetoothDevice device in pairedDevices)
                    {
                        String deviceName = device.Name;
                        String deviceHardwareAddress = device.Address;
                        if (deviceName.Equals("MX30"))
                        {
                            return deviceHardwareAddress;
                        }
                    }
                }
            }
            return "";
        }

        public void Bar_QRCode(int PosX, int PosY, int Mode, int Type, string ErrLevel, int Mask, int Mul,  int Rotation, string Data)
        {
            Godex.Bar_QRCode(PosX, PosY, Mode, Type,  ErrLevel, Mask, Mul,   Rotation,  Data);
        }

        public void Close()
        {
            Godex.Close();
        }

        public void Debug(int select)
        {
            Godex.Debug(select);
        }

        public void getMainContext()
        {
            Godex.GetMainContext( MainActivity.Instance);
        }

        public void Openport(string address, int type)
        {
            Godex.Openport(address, type);
        }

        public bool SendCommand(string commande)
        {
            return Godex.SendCommand(commande);
        }

        public bool SendCommand(string commande, string encoding)
        {
            return Godex.SendCommand(commande, encoding);
        }

        public bool Setup(string height, string dark, string speed, string mode, string gap, string top)
        {
            return Godex.Setup(height, dark,  speed, mode, gap, top);
        }
    }
}