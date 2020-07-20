using System;
using Rg.Plugins.Popup.Pages;
using Xamarin.Forms;
using Rg.Plugins.Popup.Services;
using XpertMobileApp.ViewModels;
using XpertMobileApp.DAL;
using XpertMobileApp.Api.Services;
using XpertMobileApp.Models;
using Xpert.Common.WSClient.Helpers;
using Acr.UserDialogs;

namespace XpertMobileApp.Views
{
    public partial class VteValidationPage : PopupPage
    {
        VteValidationViewModel viewModel;
        public string ParentStream { get; set; }
        public View_VTE_VENTE Item 
        { 
            get 
            { 
                return viewModel.Item; 
            } 
            internal set 
            { 
                viewModel.Item = value; 
            } 
        }

        public VteValidationPage(string stream)
        {
            InitializeComponent();
            ParentStream = stream;
            BindingContext = viewModel = new VteValidationViewModel();

            MessagingCenter.Subscribe<TiersSelector, View_TRS_TIERS>(this, viewModel.CurrentStream, async (obj, selectedItem) =>
            {
                viewModel.SelectedTiers = selectedItem;
                viewModel.Item.CODE_TIERS = selectedItem.CODE_TIERS;
                viewModel.Item.NOM_TIERS = selectedItem.NOM_TIERS1;
            });
           
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
        }


        private async void btnCancel_Clicked(object sender, EventArgs e)
        {
            await PopupNavigation.Instance.PopAsync();
        }

        private async void btnValidate_Clicked(object sender, EventArgs e)
        {
            try
            {
                bool res = await viewModel.ValidateVte(viewModel.Item);

                if (res)
                { 
                    await PopupNavigation.Instance.PopAsync(); 
                }
            }
            catch (Exception ex) 
            {
                await UserDialogs.Instance.AlertAsync(WSApi2.GetExceptionMessage(ex), AppResources.alrt_msg_Alert, AppResources.alrt_msg_Ok);
            }
        }

        private TiersSelector itemSelector;
        private async void btn_Select_Clicked(object sender, EventArgs e)
        {
            itemSelector = new TiersSelector(viewModel.CurrentStream);
            await PopupNavigation.Instance.PushAsync(itemSelector);
        }

        private void SfNumericTextBox_ValueChanged(object sender, Syncfusion.SfNumericTextBox.XForms.ValueEventArgs e)
        {

        }

        private void TOTAL_RECU_ValueChanged(object sender, Syncfusion.SfNumericTextBox.XForms.ValueEventArgs e)
        {
            decimal Mt_Recu = Convert.ToDecimal(e.Value);
            if(Mt_Recu >= viewModel.Item.TOTAL_RESTE) 
            { 
                viewModel.Item.MBL_MT_RENDU = Mt_Recu - viewModel.Item.TOTAL_RESTE;
                viewModel.Item.MBL_MT_VERCEMENT = viewModel.Item.TOTAL_RESTE;
            } 
            else 
            {
                viewModel.Item.MBL_MT_RENDU = 0;
                viewModel.Item.MBL_MT_VERCEMENT = Mt_Recu;
            }

        }
    }
}
