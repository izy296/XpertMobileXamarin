using Rg.Plugins.Popup.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using XpertMobileApp.DAL;

namespace XpertMobileApp.Views._05_Officine.Chifa.Consommation
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CHIFA_Consommation : TabbedPage
    {
        CHIFA_ConsommationViewModel viewModel;
        public CHIFA_Consommation()
        {
            InitializeComponent();
            viewModel = new CHIFA_ConsommationViewModel();
            BindingContext = viewModel;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            if (viewModel.ListeFactByDci.Count() <= 0)
            {
                viewModel.LoadListeFactByDciCommand.Execute(null);
            }
        }
        private async void OnItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            try
            {
                View_CFA_MOBILE_DETAIL_FACTURE ConsommationItem = (View_CFA_MOBILE_DETAIL_FACTURE)e.SelectedItem;
                if (ConsommationItem != null)
                {
                    await Navigation.PushAsync(new CHIFA_ConsommationDetail(viewModel, ConsommationItem, viewModel.StartDate, viewModel.EndDate));
                    ItemsListView1.SelectedItem = null;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private async void ShowHideFilter(object sender, EventArgs e)
        {
            try
            {
                CHIFA_ConsommationPopupPage filter = new CHIFA_ConsommationPopupPage(viewModel);
                await PopupNavigation.Instance.PushAsync(filter);
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        private void TabbedPage_CurrentPageChanged(object sender, EventArgs e)
        {
            try
            {
                var tabbedPage = (TabbedPage)sender;
                Title = tabbedPage.CurrentPage.Title;
                if (viewModel != null)
                {
                    viewModel.searchedDCI = viewModel.searchedNomCommercial = String.Empty;
                    if (Title == ConsommationDisplayType.DCI.ToString())
                    {
                        viewModel.ConsommationDisplayType = ConsommationDisplayType.DCI;
                    }
                    else
                    {
                        viewModel.ConsommationDisplayType = ConsommationDisplayType.NOMC;
                    }

                    viewModel.InitAndReloadItemList();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void MenuItem1_Clicked(object sender, EventArgs e)
        {
            try
            {
                viewModel.orderByAlphabetic = true;
                viewModel.orderByQuatity = viewModel.orderByPrice = false;
                viewModel.InitAndReloadItemList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void MenuItem2_Clicked(object sender, EventArgs e)
        {
            try
            {
                viewModel.orderByPrice = true;
                viewModel.orderByQuatity = viewModel.orderByAlphabetic = false;
                viewModel.InitAndReloadItemList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void MenuItem3_Clicked(object sender, EventArgs e)
        {
            try
            {
                viewModel.orderByQuatity = true;
                viewModel.orderByPrice = viewModel.orderByAlphabetic = false;
                viewModel.InitAndReloadItemList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}