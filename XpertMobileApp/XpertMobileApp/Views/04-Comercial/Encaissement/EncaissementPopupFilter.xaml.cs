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
            viewModel.ExecuteLoadExtrasDataCommand();
            BindingContext = this.viewModel;
        }

        private async void Close_filter_popup(object sender, EventArgs e)
        {
            await closeIcon.ScaleTo(0.75, 50, Easing.Linear);
            await closeIcon.ScaleTo(1, 50, Easing.Linear);
            await PopupNavigation.Instance.PopAsync();
        }
        public void ClearFilters()
        {
            viewModel.StartDate = DateTime.Now;
            viewModel.EndDate = DateTime.Now;
            viewModel.SelectedCompte = null;
            viewModel.SelectedMotif = null;
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
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

        private async void Initialize_Compte_Picker(object sender, EventArgs e)
        {
            await initComptePicker.ScaleTo(0.75, 50, Easing.Linear);
            await initComptePicker.ScaleTo(1, 50, Easing.Linear);
            if (viewModel.SelectedCompte != null)
            {
                viewModel.SelectedCompte = new View_BSE_COMPTE();
                ComptesPicker.SelectedIndex = 0;
            }
        }

        private async void Initialize_Motifs_picker(object sender, EventArgs e)
        {
            await initMotifPicker.ScaleTo(0.75, 50, Easing.Linear);
            await initMotifPicker.ScaleTo(1, 50, Easing.Linear);
            if (viewModel.SelectedMotif != null)
            {
                viewModel.SelectedMotif = new BSE_ENCAISS_MOTIFS();
                MotifsPicker.SelectedIndex = 0;
            }
        }

        private void CheckBox_StateChangedTransfertDeFond(object sender, Syncfusion.XForms.Buttons.StateChangedEventArgs e)
        {
            viewModel.CheckBoxTransfertDeFond = (bool)e.IsChecked;
        }
    }
}

