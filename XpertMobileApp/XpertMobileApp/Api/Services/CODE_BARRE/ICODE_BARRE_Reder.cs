using System;
using System.Collections.Generic;
using System.Text;
using XpertMobileApp.Api.Services.CODE_BARRE;

namespace XpertMobileApp.Api.Services
{
    public interface ICODE_BARRE_Reder
    {
        void Init(ICallBackReciver reciver);

        void Scan();
        void StopScan();
        void Close();
        bool GetInstance();
        void setIsOpen(bool _isOpen);
        bool GetIsOpen();
       
    }
}
