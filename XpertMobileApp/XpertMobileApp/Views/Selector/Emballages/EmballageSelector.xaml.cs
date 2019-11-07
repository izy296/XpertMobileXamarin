using System;
using Rg.Plugins.Popup.Pages;
using Xamarin.Forms;
using Rg.Plugins.Popup.Services;
using XpertMobileApp.ViewModels;
using XpertMobileApp.Helpers;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using XpertMobileApp.Models;

namespace XpertMobileApp.Views
{
    public partial class EmballageSelector : PopupPage
    {

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
                return viewModel.IS_PRINCIPAL;
            }
            set
            {
                viewModel.IS_PRINCIPAL = value;

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
            ent_Filter.Focus();

            MessagingCenter.Send(this, MCDico.ITEM_SELECTED, viewModel.Items.ToList());
            
            await PopupNavigation.Instance.PopAsync();
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
