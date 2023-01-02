using Syncfusion.Data.Extensions;
using Syncfusion.DataSource.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using XpertMobileApp;
using XpertMobileApp.DAL;
using XpertMobileApp.Models;
using XpertMobileApp.SQLite_Managment;
using XpertMobileApp.ViewModels;
using XpertMobileAppManafiaa.SQLite_Managment;

namespace XpertMobileAppManafiaa.Views._06_Manafiaa.DaytimeDelivery
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class DaytimeDelivery_DashBord : ContentPage
    {
        ItemDetailViewModel<View_LIV_TOURNEE> viewModel;

        public DaytimeDelivery_DashBord()
        {
            InitializeComponent();



        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            View_LIV_TOURNEE Tournne = await SyncManager.ClotureTournee();
            if (Tournne != null)
            {
                BindingContext = this.viewModel = new ItemDetailViewModel<View_LIV_TOURNEE>(Tournne);

            }
        }
    }
}