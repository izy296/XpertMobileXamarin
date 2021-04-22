using System;
using Rg.Plugins.Popup.Pages;
using Xamarin.Forms;
using Rg.Plugins.Popup.Services;
using XpertMobileApp.ViewModels;
using XpertMobileApp.Helpers;
using System.Linq;
using System.Collections.Generic;
using XpertMobileApp.Models;
using Acr.UserDialogs;
using Xpert.Common.WSClient.Helpers;

namespace XpertMobileApp.Views
{
    public partial class EmballageSelector : PopupPage
    {
        public string CurrentStream;

        EmballageSelectorViewModel viewModel;

        private List<View_BSE_EMBALLAGE> currentEmballages;
        public List<View_BSE_EMBALLAGE> CurrentEmballages
        {
            get
            {
                return viewModel.CurrentEmballages;
            }
            set
            {
                viewModel.CurrentEmballages = value;
            }
        }

        public bool IS_PRINCIPAL
        {
            get
            {
                return viewModel.IS_PRINCIPAL;
            }
            set
            {
                viewModel.IS_PRINCIPAL = value;

            }
        }

        public bool IS_SALES
        {
            get
            {
                return viewModel.IS_SALES;
            }
            set
            {
                viewModel.IS_SALES = value;

            }
        }

        public EmballageSelector()
        {
            InitializeComponent();

            BindingContext = viewModel = new EmballageSelectorViewModel();

            viewModel.LoadItemsCommand.Execute(null);
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            if(viewModel.Items.Count == 0)
            {
               // viewModel.LoadItemsCommand.Execute(null);
            }
        }

        private async void OnClose(object sender, EventArgs e)
        {
            try
            { 
                ent_Filter.Focus();

                string msg = MCDico.ITEM_SELECTED;
                if (!string.IsNullOrEmpty(CurrentStream))
                {
                    msg = CurrentStream;
                }

                MessagingCenter.Send(this, msg, viewModel.Items.ToList());
            
                await PopupNavigation.Instance.PopAsync();
            }
            catch (Exception ex)
            {
                await UserDialogs.Instance.AlertAsync(WSApi2.GetExceptionMessage(ex), AppResources.alrt_msg_Alert,
AppResources.alrt_msg_Ok);
            }
        }

        async void btn_Search_Clicked(object sender, EventArgs e)
        {
            viewModel.LoadItemsCommand.Execute(null);
        }

        private void ItemsListView_SelectionChanged(object sender, Syncfusion.ListView.XForms.ItemSelectionChangedEventArgs e)
        {
            btnSelect.IsEnabled = true;
        }
    }
}
