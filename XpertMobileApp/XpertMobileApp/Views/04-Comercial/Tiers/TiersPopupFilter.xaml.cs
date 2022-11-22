using Acr.UserDialogs;
using Rg.Plugins.Popup.Pages;
using Rg.Plugins.Popup.Services;
using Syncfusion.XForms.Buttons;
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
    public partial class TiersPopupFilter : PopupPage, INotifyPropertyChanged
    {
        private TiersViewModel viewModel;
        private TiersSelector itemSelector;
        public string CurrentStream = Guid.NewGuid().ToString();
        public TiersPopupFilter(TiersViewModel viewModel)
        {
            InitializeComponent();
            this.viewModel = viewModel;
            BindingContext = this.viewModel;
            itemSelector = new TiersSelector(CurrentStream);

            if (viewModel.Items.Count == 0)
                viewModel.LoadItemsCommand.Execute(null);

            if (viewModel.Familles.Count == 0)
                viewModel.LoadExtrasDataCommand.Execute(null);
        }

        private async void Close_filter_popup(object sender, EventArgs e)
        {
            await closeIcon.ScaleTo(0.75, 50, Easing.Linear);
            await closeIcon.ScaleTo(1, 50, Easing.Linear);
            //FilterPanel.IsVisible = false;
            this.soldeG.IsChecked = false;
            soldeE.IsChecked = false;
            soldeL.IsChecked = false;
            viewModel.ClearFilters();
            viewModel.LoadItemsCommand.Execute(null);
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

        private void rg_solde_StateChanged(object sender, Syncfusion.XForms.Buttons.StateChangedEventArgs e)
        {
            if (e.IsChecked.HasValue && e.IsChecked.Value)
            {
                viewModel.SoldOperator = (sender as SfRadioButton).ClassId;
            }
        }

        private async void btn_Select_Clicked(object sender, EventArgs e)
        {
            await PopupNavigation.Instance.PushAsync(itemSelector);
        }

        private async void Initialize_Type_Picker(object sender, EventArgs e)
        {
            await initTypePicker.ScaleTo(0.75, 50, Easing.Linear);
            await initTypePicker.ScaleTo(1, 50, Easing.Linear);
            if (viewModel.SelectedType != null)
            {
                viewModel.SelectedType = new BSE_TABLE_TYPE();
                TypesPicker.SelectedIndex = 0;
            }
        }

        private async void Initialize_Famille_Picker(object sender, EventArgs e)
        {
            await initFamillePicker.ScaleTo(0.75, 50, Easing.Linear);
            await initFamillePicker.ScaleTo(1, 50, Easing.Linear);
            if (viewModel.SelectedFamille != null)
            {
                viewModel.SelectedFamille = new View_BSE_TIERS_FAMILLE();
                FamillesPicker.SelectedIndex = 0;
            }
        }

        private async void initialize_Tiers_Entry(object sender, EventArgs e)
        {
            await initTierEntry.ScaleTo(0.75, 50, Easing.Linear);
            await initTierEntry.ScaleTo(1, 50, Easing.Linear);
            ent_SearchedTiers.Text = "";
        }
    }
}

