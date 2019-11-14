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
        public void PrintQrCode_Agrecelteur(string Adress,int typeConnection
            ,string nomTiers,string codeDocs,string codeTiers,DateTime? DateDoc)
        {
            Godex.Debug(1);
            Godex.getMainContext();
            Godex.Openport(Adress, typeConnection);
            Godex.Setup("45", "8", "2", "0", "2", "0");
            Godex.SendCommand("^L");
            Godex.SendCommand("AD,30,80,1,1,0,0," + nomTiers);
            Godex.Bar_QRCode(30, 130, 2, 2, "M", 8, 8, 0, codeDocs
                + "-" + codeTiers + "-" +
                DateDoc.Value.ToString("dd/MM/yyyy"));
            Godex.SendCommand("AD,250,160,1,1,0,0,"+ codeDocs);
            Godex.SendCommand("AD,250,210,1,1,0,0," + codeTiers);
            Godex.SendCommand("AD,250,260,1,1,0,0," + DateDoc.Value.ToString("dd/MM/yyyy"));
            Godex.SendCommand("E");
            Godex.Close();

        }       
            
    }
}
