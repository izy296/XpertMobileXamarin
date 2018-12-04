using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using Xamarin.Forms;
using Xpert.Pharm.DAL;

namespace XpertMobileApp.ViewModels
{


    public class HomeViewModel : BaseViewModel
    {

        public Command LoadItemsCommand { get; set; }

        public HomeViewModel()
        {
            Title = AppResources.pn_home;


        }
    }
}
