using System;
using System.Collections.Generic;
using System.Text;
using XpertMobileApp.ViewModels;
using XpertMobileApp.DAL;
using System.Collections.ObjectModel;
using Xamarin.Forms;

namespace XpertMobileApp.Api.ViewModels
{
    public class VerifTiersViewModel : BaseViewModel
    {
        private View_TRS_TIERS tiers;
        public View_TRS_TIERS Tiers
        {
            get { return tiers; }
            set
            {
                tiers = value;
                OnPropertyChanged("Tiers");
            }
        }
    
        public ObservableCollection<View_PRD_AGRICULTURE_DETAIL> ProductionsDetails { get; set; }

        public Command LoadTiersCommand { get; set; }

        public Command LoadProductionsCommand { get; set; }

        public VerifTiersViewModel()
        {
            Title = "Vérification";
        }
    }
}
