using Com.Rscja.Deviceapi;
using System.Collections.Generic;
using Xamarin.Forms;
using XpertMobileApp.Services;

[assembly: Dependency(typeof(XpertMobileApp.Droid.Services.RfidScaner))]
namespace XpertMobileApp.Droid.Services
{
    public class RfidScaner : XpertMobileApp.Services.IRfidScaner
    {
        private RFIDWithUHF uhfAPI;

        public string[] ReadTagFromBuffer()
        {
            return uhfAPI.ReadTagFromBuffer();
        }

        public void StopInventory()
        {
            
            uhfAPI.StopInventory();
                    
        }

        public string InventorySingleTag() {
            return uhfAPI.InventorySingleTag();
        }
           
        public string ConvertUiiToEPC(string val)
        {
            return uhfAPI.ConvertUiiToEPC(val);
        }

        public bool GetInstance()
        {
            if (uhfAPI == null)
            {
                try
                {
                    uhfAPI = RFIDWithUHF.Instance;
                    uhfAPI.StopInventory();
                }
                catch
                {
                    return false;
                }
            }
            return true;
        }

        public bool Init()
        {
           return uhfAPI.Init();
        }

        public bool SatrtContenuesInventary(byte anti, byte q)
        {
            return uhfAPI.StartInventoryTag(anti, q);
        }
    }
}