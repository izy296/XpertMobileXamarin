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
using XpertMobileApp.Views._05_Officine.Chifa.Consommation;
using XpertMobileApp.Views.Templates;

namespace XpertMobileApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CHIFA_ConsommationPopupPage : PopupPage, INotifyPropertyChanged
    {
        CHIFA_ConsommationViewModel viewModel { get; set; }
        public CHIFA_ConsommationPopupPage(CHIFA_ConsommationViewModel viewModel)
        {
            InitializeComponent();
            this.viewModel = viewModel;
            BindingContext = this.viewModel;
        }

        private async void Close_filter_popup(object sender, EventArgs e)
        {
            await PopupNavigation.Instance.PopAsync();
        }

        private async void ApplyFilter(object sender, EventArgs e)
        {
            viewModel.ListeFactByDci.Clear();
            viewModel.LoadListeFactByDciCommand.Execute(null);
            await PopupNavigation.Instance.PopAsync();
        }

        private void Initialize_ent_SearchedDCI(object sender, EventArgs e)
        {
            try
            {
                viewModel.searchedDCI = String.Empty; 
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void Initialize_ent_searchedNomCommercial(object sender, EventArgs e)
        {
            try
            {
                viewModel.searchedNomCommercial = String.Empty;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}

