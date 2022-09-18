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

namespace XpertMobileApp.Views._04_Comercial.Echange
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PopupFilter : PopupPage, INotifyPropertyChanged
    {
        private EchangeListViewModel viewModel;
        public PopupFilter(EchangeListViewModel viewModel)
        {
            InitializeComponent();
            this.viewModel = viewModel;
            BindingContext = this.viewModel;
            if (viewModel.Motifs.Count <= 0 && viewModel.ListeTiers.Count <= 0 && viewModel.TypeTiers.Count <= 0)
                viewModel.LoadDataCommand.Execute(null);
        }


        private async void Close_filter_popup(object sender, EventArgs e)
        {
            await PopupNavigation.Instance.PopAsync();
        }

        /// <summary>
        /// Update Liste tiers when selecting item from the picker
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void UpdateTiersList(object sender, EventArgs e)
        {
            string typeTiersSelected = viewModel.TypeTiersSelected.CODE_TYPE;
            viewModel.UpdateTiersList(typeTiersSelected);
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

        private void Initialize_Motifs_picker(object sender, EventArgs e)
        {
            if (viewModel.MotifSelected != null)
            {
                viewModel.MotifSelected = new View_STK_MOTIF_ECHANGE();
                motifPicker.SelectedIndex = 0;
            }
        }

        private void Initialize_family_tiers(object sender, EventArgs e)
        {
            if (viewModel.TypeTiersSelected != null)
            {
                viewModel.TypeTiersSelected = new BSE_TIERS_TYPE();
                typeTiersPicker.SelectedIndex = 0;
            }
        }

        private void Initialize_Tiers_Field(object sender, EventArgs e)
        {
            if (viewModel.TiersSelected != null)
            {
                viewModel.TiersSelected = new View_TRS_TIERS();
                tiersPicker.SelectedIndex = 0;
            }
        }
    }
}

