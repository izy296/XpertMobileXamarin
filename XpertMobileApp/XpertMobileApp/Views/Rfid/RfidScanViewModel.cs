using Acr.UserDialogs;
using System;
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
   public class RfidScanViewModel : BaseViewModel
   {
        private View_STK_STOCK currentLot;
        public View_STK_STOCK CurrentLot
        {
            get { return currentLot; }
            set { SetProperty(ref currentLot, value); }
        }

        private int idStock = 15897;
        public int IdStock
        {
            get { return idStock; }
            set { SetProperty(ref idStock, value); }
        }


        public ObservableCollection<string> Items { get; set; }


        private int elementsCount;
        public int ElementsCount
        {
            get { return elementsCount; }
            set { SetProperty(ref elementsCount, value); }
        }

        public bool ContinuesScan { get; set; }

        public bool Anti { get; set; } = false;

        public int q { get; set; } = 3;

        public RfidScanViewModel()
        {
            Title = AppResources.pn_RfidScan;

            Items = new ObservableCollection<string>();
            
        }

    
    }
}
