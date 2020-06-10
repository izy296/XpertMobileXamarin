using System;
using Xamarin.Forms;
using XpertMobileApp.Api.Services;



[assembly: Dependency(typeof(XpertMobileApp.Droid.Services.GodexPlatform))]
namespace XpertMobileApp.Droid.Services
{
    public class GodexPlatform : IGodexPlatform
    {

        public void Bar_QRCode(int PosX, int PosY, int Mode, int Type, string ErrLevel, int Mask, int Mul, int Len, int Rotation, string Data)
        {
            throw new NotImplementedException();
        }

        public void Bar_QRCode(int PosX, int PosY, int Mode, int Type, string ErrLevel, int Mask, int Mul, int Rotation, string Data)
        {
            throw new NotImplementedException();
        }

        public void Close()
        {
            throw new NotImplementedException();
        }

        public void Debug(int select)
        {
            throw new NotImplementedException();
        }

        public void getMainContext()
        {
            throw new NotImplementedException();
        }

        public string GetPrinterAdress()
        {
            throw new NotImplementedException();
        }

        public string GetPrinterAdress(string printerName)
        {
            throw new NotImplementedException();
        }

        public void Openport(string address, int type)
        {
            throw new NotImplementedException();
        }

        public bool SendCommand(string commande)
        {
            throw new NotImplementedException();
        }

        public bool SendCommand(string commande, string encoding)
        {
            throw new NotImplementedException();
        }

        public bool Setup(string height, string dark, string speed, string mode, string gap, string top)
        {
            throw new NotImplementedException();
        }
    }
}