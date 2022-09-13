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
    public partial class SortiePopupFilter : PopupPage, INotifyPropertyChanged
    {
        private SortieListViewModel viewModel;
        private UserSelector itemSelector;
        public string CurrentStream = Guid.NewGuid().ToString();
        public SortiePopupFilter(SortieListViewModel viewModel)
        {
            InitializeComponent();
            this.viewModel = viewModel;
            BindingContext = this.viewModel;
            itemSelector = new UserSelector(CurrentStream);

            //if (viewModel.Status.Count <= 0)
            //    viewModel.LoadExtrasDataCommand.Execute(null);

            MessagingCenter.Subscribe<UserSelector, View_SYS_USER>(this, CurrentStream, async (obj, selectedItem) =>
            {
                viewModel.SelectedUsers = selectedItem;
                ent_SelectedUsers.Text = selectedItem.ID_USER;
            });

            if (Constants.AppName == Apps.XCOM_Mob)
            {
                status_layout.IsVisible = false;
            }

            if (StatusPicker.ItemsSource != null && StatusPicker.ItemsSource.Count > 0)
            {
                StatusPicker.SelectedItem = StatusPicker.ItemsSource[1];
            }
        }


        private async void Close_filter_popup(object sender, EventArgs e)
        {
            await PopupNavigation.Instance.PopAsync();
        }

        private void ClearFilters()
        {
            ent_SelectedUsers.Text = "";
            MotifPicker.SelectedItem = MotifPicker.ItemsSource[0];
            StatusPicker.SelectedItem = StatusPicker.ItemsSource[0];
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
        private void btn_CancelFilter_Clicked(object sender, EventArgs e)
        {
            ClearFilters();
            viewModel.LoadItemsCommand.Execute(null);
        }
    }
}

