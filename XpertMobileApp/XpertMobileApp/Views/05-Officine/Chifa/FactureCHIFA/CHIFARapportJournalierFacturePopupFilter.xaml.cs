using Acr.UserDialogs;
using Rg.Plugins.Popup.Pages;
using Rg.Plugins.Popup.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Xpert.Common.DAO;
using Xpert.Common.WSClient.Helpers;
using Xpert.Common.WSClient.Services;
using XpertMobileApp.Api.Managers;
using XpertMobileApp.Api.Services;
using XpertMobileApp.DAL;
using XpertMobileApp.Helpers;
using XpertMobileApp.Models;
using XpertMobileApp.Services;
using XpertMobileApp.SQLite_Managment;
using XpertMobileApp.ViewModels;
using XpertMobileApp.Views._05_Officine.Chifa.FactureCHIFA;
using XpertMobileApp.Views.Templates;

namespace XpertMobileApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CHIFARapportJournalierFacturePopupFilter : PopupPage, INotifyPropertyChanged
    {
        RapportJournalierFactureCHIFAViewModel viewModel;
        public CHIFARapportJournalierFacturePopupFilter(RapportJournalierFactureCHIFAViewModel viewModel)
        {
            InitializeComponent();
            BindingContext = this.viewModel = viewModel;
        }


        private async void Close_filter_popup(object sender, EventArgs e)
        {
            if (viewModel.SearchedNumFacture == string.Empty)
            {
                viewModel.EtatFiltre = "#2196F3";
            }
            await PopupNavigation.Instance.PopAsync();
        }
        public void ClearFilters()
        {
            viewModel.SelectedDate = DateTime.Now;
            viewModel.SearchedNumFacture = string.Empty;
        }

        /// <summary>
        /// Apply the filter 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void ApplyFilter(object sender, EventArgs e)
        {
            viewModel.EtatFiltre = "#FF9500";
            viewModel.LoadTotauxFactureCommand.Execute(null);
            viewModel.LoadItemsCommand.Execute(null);
            await PopupNavigation.Instance.PopAsync();

        }
        private void btn_CancelFilter_Clicked(object sender, EventArgs e)
        {
            viewModel.EtatFiltre = "#2196F3";
            ClearFilters();
        }

        private void InitializeNumFactureEntry(object sender, EventArgs e)
        {
            viewModel.SearchedNumFacture = string.Empty;
        }
    }
}

