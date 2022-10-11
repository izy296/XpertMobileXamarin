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
    public partial class TransferStockPopupFilter : PopupPage, INotifyPropertyChanged
    {
        private TransfertStockViewModel viewModel;
        public string CurrentStream = Guid.NewGuid().ToString();
        public TransferStockPopupFilter(TransfertStockViewModel viewModel)
        {
            InitializeComponent();
            this.viewModel = viewModel;
            BindingContext = this.viewModel;

            //if (Constants.AppName == Apps.XCOM_Mob)
            //{
            //    Labo_layout.IsVisible = false;
            //    tags_layout.IsVisible = false;
            //    checkBoxR.IsVisible = false;
            //}
            //else
            //{
            //    fideleteRadioBtns.IsVisible = false;
            //}


            MessagingCenter.Subscribe<ProductTagSelector, List<BSE_PRODUIT_TAG>>(this, MCDico.ITEM_SELECTED, async (obj, items) =>
            {

            });
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

        /// <summary>
        /// Btn to cancel the filter 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_CancelFilter_Clicked(object sender, EventArgs e)
        {
            //quantiteG.IsChecked = false;
            //quantiteE.IsChecked = false;
            //quantiteL.IsChecked = false;
            //etatAll.IsChecked = false;
            //etatActive.IsChecked = false;
            //etatNonActive.IsChecked = false;
            //TagsPicker.Text = "";
            viewModel.ClearFilters();
            viewModel.LoadItemsCommand.Execute(null);
        }

        private async void buttonClick(object sender, EventArgs e)
        {
            ProductTagSelector productTagSelector = new ProductTagSelector();
            await PopupNavigation.Instance.PushAsync(productTagSelector);
        }

    }
}

