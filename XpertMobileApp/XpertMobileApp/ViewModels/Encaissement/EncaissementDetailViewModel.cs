using System;
using Xpert.Pharm.DAL;
using XpertMobileApp.Models;

namespace XpertMobileApp.ViewModels
{
    public class EncaissementDetailViewModel : BaseViewModel
    {
        public View_TRS_ENCAISS Item { get; set; }
        public EncaissementDetailViewModel(View_TRS_ENCAISS item = null)
        {
            Title = item?.ToString();
            Item = item;
        }
    }
}
