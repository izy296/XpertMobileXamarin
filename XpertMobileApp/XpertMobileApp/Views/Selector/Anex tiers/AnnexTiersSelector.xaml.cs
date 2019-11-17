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
    public partial class AnnexTiersSelector : PopupPage
    {

        AnnexTiersSelectorViewModel viewModel;

        private List<VIEW_ACH_INFO_ANEX> currentEmballages;
        public List<VIEW_ACH_INFO_ANEX> CurrentEmballages
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

        public AnnexTiersSelector()
        {
            InitializeComponent();

            BindingContext = viewModel = new AnnexTiersSelectorViewModel();

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

            MessagingCenter.Send(this, MCDico.ITEM_SELECTED, viewModel.Items.ToList());
            
            await PopupNavigation.Instance.PopAsync();
        }

        private void ItemsListView_SelectionChanged(object sender, Syncfusion.ListView.XForms.ItemSelectionChangedEventArgs e)
        {
            btnSelect.IsEnabled = true;
        }

        private void btn_ApplyFilter_Clicked(object sender, EventArgs e)
        {
            viewModel.Items.Add(new VIEW_ACH_INFO_ANEX()
            {
                NOM_TIERS = TiersName.Text,
                QUANTITE_APPORT = Convert.ToDecimal(TiersQte.Text.Trim())
                
            });
        }

        private void btn_CancelFilter_Clicked(object sender, EventArgs e)
        {
            var elem = (sender as Button).BindingContext as VIEW_ACH_INFO_ANEX;
            viewModel.Items.Remove(elem);
        }

        private void Btn_Delete_Clicked(object sender, EventArgs e)
        {

        }
    }
}
