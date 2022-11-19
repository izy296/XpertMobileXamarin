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
using XpertMobileApp.Api;
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
    public partial class VentesPopupFilter : PopupPage, INotifyPropertyChanged
    {
        private VentesViewModel viewModel;
        public string CurrentStream = Guid.NewGuid().ToString();
        private TiersSelector itemSelector;
        public VentesPopupFilter(VentesViewModel viewModel)
        {
            InitializeComponent();
            itemSelector = new TiersSelector(CurrentStream);
            this.viewModel = viewModel;
            BindingContext = this.viewModel;

            if (viewModel.Types.Count <= 0 && viewModel.User.Count <= 0)
                viewModel.ExecuteLoadExtrasDataCommand();

            MessagingCenter.Subscribe<TiersSelector, View_TRS_TIERS>(this, CurrentStream, async (obj, selectedItem) =>
            {
                viewModel.SelectedTiers = selectedItem;
                ent_SelectedTiers.Text = selectedItem.NOM_TIERS1;
            });
            if (viewModel.TypeVente == VentesTypes.VenteComptoir)
            {
                status_layout.IsVisible = false;
            }

            if (Constants.AppName == Apps.X_DISTRIBUTION)
            {
                status_layout.IsVisible = false;
            }
        }


        private async void Close_filter_popup(object sender, EventArgs e)
        {
            await PopupNavigation.Instance.PopAsync();
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

        private async void btn_Select_Clicked(object sender, EventArgs e)
        {
            itemSelector.SearchedType = "";

            await PopupNavigation.Instance.PushAsync(itemSelector);

        }

        private void Initialize_Type_Picker(object sender, EventArgs e)
        {
            if (viewModel.SelectedType != null)
            {
                viewModel.SelectedType = new BSE_DOCUMENTS_TYPE();
                motifPicker.SelectedIndex = 0;
            }
        }

        private void Initialize_Tiers_Picker(object sender, EventArgs e)
        {
            if (viewModel.SelectedTiers != null)
            {
                viewModel.SelectedTiers = new View_TRS_TIERS();
                ent_SelectedTiers.Text = "";
            }
        }
    }
}

