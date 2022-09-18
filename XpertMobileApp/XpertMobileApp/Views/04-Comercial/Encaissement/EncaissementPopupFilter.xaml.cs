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
using XpertMobileApp.Views.Templates;

namespace XpertMobileApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class EncaissementPopupFilter : PopupPage, INotifyPropertyChanged
    {
        private EncaissementsViewModel viewModel;
        public EncaissementPopupFilter(EncaissementsViewModel viewModel)
        {
            InitializeComponent();
            this.viewModel = viewModel;
            BindingContext = this.viewModel;
        }


        private async void Close_filter_popup(object sender, EventArgs e)
        {
            await PopupNavigation.Instance.PopAsync();
        }
        public void ClearFilters()
        {
            viewModel.StartDate = DateTime.Now;
            viewModel.EndDate = DateTime.Now;
            viewModel.SelectedCompte = null;
            viewModel.SelectedMotif = null;
        }

        /// <summary>
        /// Apply the filter 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void ApplyFilter(object sender, EventArgs e)
        {
            viewModel.LoadItemsCommand.Execute(null);
            await PopupNavigation.Instance.PopAsync();
        }
        private void btn_CancelFilter_Clicked(object sender, EventArgs e)
        {
            ClearFilters();
            viewModel.LoadItemsCommand.Execute(null);
        }

        private void Initialize_date(object sender, EventArgs e)
        {

        }

        private void Initialize_Compte_Picker(object sender, EventArgs e)
        {
            if (viewModel.SelectedCompte != null)
            {
                viewModel.SelectedCompte = new View_BSE_COMPTE();
                ComptesPicker.SelectedIndex = 0;
            }
        }

        private void Initialize_Motifs_picker(object sender, EventArgs e)
        {
            if (viewModel.SelectedMotif != null)
            {
                viewModel.SelectedMotif = new BSE_ENCAISS_MOTIFS();
                MotifsPicker.SelectedIndex = 0;
            }
        }
    }
}

