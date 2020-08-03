using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace XpertMobileApp.Services
{
    public interface IRfidScaner
    {
        string ConvertUiiToEPC(string val);

        string[] ReadTagFromBuffer();

         void StopInventory();

        string InventorySingleTag();
        bool SatrtContenuesInventary(byte anti, byte q);
        bool GetInstance();
        bool Init();
    }
}
