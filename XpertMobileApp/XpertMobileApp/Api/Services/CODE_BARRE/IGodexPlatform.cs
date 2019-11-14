using System;
using System.Collections.Generic;
using System.Text;
using XpertMobileApp.Api.Services.CODE_BARRE;

namespace XpertMobileApp.Api.Services
{
    public interface IGodexPlatform
    {
        void getMainContext();
        void Debug(int select);
        void Openport(string address, int type);
        bool Setup(string height, string dark, string speed, string mode, string gap, string top);
        bool SendCommand(string commande);
        bool SendCommand(string commande, string encoding);
        void Bar_QRCode(int PosX, int PosY, int Mode, int Type, string ErrLevel, int Mask,int  Mul,  int Rotation, string Data );
        void Close();
    }
}
