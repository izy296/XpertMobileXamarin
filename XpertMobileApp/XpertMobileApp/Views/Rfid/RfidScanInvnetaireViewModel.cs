using Acr.UserDialogs;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Extended;
using Xpert.Common.WSClient.Model;
using XpertMobileApp.DAL;
using XpertMobileApp.Helpers;
using XpertMobileApp.Models;
using XpertMobileApp.Services;

namespace XpertMobileApp.ViewModels
{
   public class RfidScanInvnetaireViewModel : BaseViewModel
   {
        public Command loadInventaireInfo { get; set; }
        public Command SaveInventaireCommand { get; set; }
        private View_STK_INVENTAIRE currentInv;
        public View_STK_INVENTAIRE CurrentInv
        {
            get { return currentInv; }
            set { SetProperty(ref currentInv, value); }
        }
        private int totalElementsCount;
        public int TotalElementsCount
        {
            get { return totalElementsCount; }
            set { SetProperty(ref totalElementsCount, value); }
        }
      
        public List<string> Items { get; set; }
        public ObservableCollection<View_STK_STOCK_RFID> InventoredStock { get; set; }

        private int elementsCount;
        public int ElementsCount
        {
            get { return elementsCount; }
            set { SetProperty(ref elementsCount, value); }
        }
        private int stockCount;
        public int StockCount
        {
            get { return stockCount; }
            set { SetProperty(ref stockCount, value); }
        }

        public bool ContinuesScan { get; set; } = true;

        public bool Anti { get; set; } = false;

        public int q { get; set; } = 3;

        public RfidScanInvnetaireViewModel()
        {
            Title = AppResources.pn_rfid_inventaire;
            SaveInventaireCommand = new Command(async () => await UpdateCurentInventaire());
            
            loadInventaireInfo = new Command(async () => await ExecuteLoadInvntaireInfoCommand());
            Items = new List<string>();
            InventoredStock = new ObservableCollection<View_STK_STOCK_RFID>();
           
        }
        async Task ExecuteLoadInvntaireInfoCommand()
        {
            try
            {
                if (await App.IsConected())
                {
                
                    var CurentInventaire = await WebServiceClient.getCurentInventaire();
                    if (!(CurentInventaire == null))
                    {
                        this.CurrentInv = CurentInventaire;
                    }
                    else {
                        await UserDialogs.Instance.AlertAsync("pas d'inventaire ouvert !", AppResources.alrt_msg_Alert,
                      AppResources.alrt_msg_Ok);
                    }
                }
            }
            catch (Exception ex)
            {
                await UserDialogs.Instance.AlertAsync(ex.Message, AppResources.alrt_msg_Alert,
                                   AppResources.alrt_msg_Ok);
            }

        }
        public async Task UpdateCurentInventaire()
        {
            try
            {
                if (await App.IsConected())
                {
                    if (CurrentInv == null) {
                         await UserDialogs.Instance.AlertAsync(AppResources.alrt_notOpenInventary_Text, AppResources.alrt_msg_Alert,
                                    AppResources.alrt_msg_Ok);
                        
                    } else {
                        List<View_STK_STOCK_RFID> Lots = new List<View_STK_STOCK_RFID>();
                        foreach (var element in InventoredStock)
                        {
                            element.NUM_ENTREE = CurrentInv.NUM_INVENT;
                            Lots.Add(element as View_STK_STOCK_RFID);
                        }
                        bool result = await WebServiceClient.UpdateCurentInventaire(Lots, "ECR", CurrentInv.NUM_INVENT, CurrentInv.IS_OUVERT);
                    }


                }
            }
            catch (XpertWebException e)
            {
                await UserDialogs.Instance.AlertAsync(e.Message, AppResources.alrt_msg_Alert,
                                    AppResources.alrt_msg_Ok);
                
            }
        }

    }
}
