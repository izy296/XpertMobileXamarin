using Acr.UserDialogs;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Extended;
using XpertMobileApp.DAL;
using XpertMobileApp.Helpers;
using XpertMobileApp.Models;
using XpertMobileApp.Services;

namespace XpertMobileApp.ViewModels
{
   public class RfidScanInvnetaireViewModel : BaseViewModel
   {
        public Command loadInventaireInfo { get; set; }

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
        
        public ObservableCollection<string> Items { get; set; }
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
        private int notFondCount;
        public int NotFondCount
        {
            get { return notFondCount; }
            set { SetProperty(ref notFondCount, value); }
        }
        public bool ContinuesScan { get; set; }

        public bool Anti { get; set; } = false;

        public int q { get; set; } = 3;
        public Command SaveInventaireCommand { get; set; }
        public RfidScanInvnetaireViewModel()
        {
            Title = AppResources.pn_RfidScan;
            SaveInventaireCommand = new Command(async () => await UpdateCurentInventaire());
            loadInventaireInfo = new Command(async () => await ExecuteLoadInvntaireInfoCommand());
            Items = new ObservableCollection<string>();
            InventoredStock = new ObservableCollection<View_STK_STOCK_RFID>();
           
        }
        async Task ExecuteLoadInvntaireInfoCommand()
        {
            try
            {
                if (App.IsConected)
                {
                
                    var CurentInventaire = await WebServiceClient.getCurentInventaire();
                    if (!(CurentInventaire == null))
                    {
                        this.CurrentInv = CurentInventaire;
                    }
                }
            }
            catch (Exception ex)
            {

            }

        }
        public async Task<bool> UpdateCurentInventaire()
        {
            try
            {
                if (App.IsConected)
                {
                    List<View_STK_STOCK_RFID> Lots = new List<View_STK_STOCK_RFID>();
                    foreach (var element in InventoredStock)
                    {
                        element.NUM_ENTREE = CurrentInv.NUM_INVENT;
                        Lots.Add(element as View_STK_STOCK_RFID);
                    }
                    bool result = await WebServiceClient.UpdateCurentInventaire(Lots, "ECR", CurrentInv.NUM_INVENT, CurrentInv.IS_OUVERT);
                    return result;

                }
                else return false;
                
            }
            catch (XpertException e)
            {

               return false;
            }
        }

    }
}
