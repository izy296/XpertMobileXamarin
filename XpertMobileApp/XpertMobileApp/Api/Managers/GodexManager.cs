using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;
using XpertMobileApp.Api.Services;

namespace XpertMobileApp.Api
{
    public class GodexManager
    {

        static private IGodexPlatform Godex = DependencyService.Get<IGodexPlatform>();
        
        
        public string GetMX30HardWareName()
        {
            return Godex.GetMX30HardWareName();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Adress">Type : String
        /// MAC address of the printer Bluetooth or BLE
        /// IP address of the printer
        /// If using USB connect the printer, the parameter is null </param>
        /// <param name="typeConnection">Type : int
        /// 1 = Wi-Fi
        /// 2 = Bluetooth
        /// 3 = USB
        /// 4 = BLE
        /// </param>
        /// <param name="nomTiers"></param>
        /// <param name="codeDocs"></param>
        /// <param name="codeTiers"></param>
        /// <param name="DateDoc"></param>

        public void PrintQrCode_Agrecelteur(string Adress,int typeConnection
            ,string nomTiers,string codeDocs,string codeTiers,DateTime? DateDoc)
        {
            if(Adress!=null && !Adress.Equals(""))
            {
                Godex.Debug(1);
                Godex.getMainContext();
                Godex.Openport(Adress, typeConnection);
                Godex.Setup("45", "8", "2", "0", "2", "0");
                Godex.SendCommand("^L");
                if (nomTiers != null && nomTiers.Length < 21)
                {
                    Godex.SendCommand("AD,5,50,1,1,0,0," + nomTiers);
                }
                else
                {
                    Godex.SendCommand("AD,5,50,1,1,0,0," + nomTiers.Substring(0, 21));
                    Godex.SendCommand("AD,5,80,1,1,0,0," + nomTiers.Substring(21, nomTiers.Length - 21));
                }
                Godex.Bar_QRCode(5, 130, 2, 2, "M", 8, 8, 0, codeDocs
                    + "-" + codeTiers + "-" +
                    DateDoc.Value.ToString("dd/MM/yyyy"));
                Godex.SendCommand("AD,210,160,1,1,0,0," + codeDocs);
                Godex.SendCommand("AD,210,210,1,1,0,0," + codeTiers);
                Godex.SendCommand("AD,210,260,1,1,0,0," + DateDoc.Value.ToString("dd/MM/yyyy"));
                Godex.SendCommand("E");
                Godex.Close();
            }
        }       
            
    }
}
