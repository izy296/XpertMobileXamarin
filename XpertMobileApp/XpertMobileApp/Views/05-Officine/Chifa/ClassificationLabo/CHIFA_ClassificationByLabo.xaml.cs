using Rg.Plugins.Popup.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using XpertMobileApp.DAL;
using XpertMobileApp.Views._05_Officine.Chifa.Consommation;

namespace XpertMobileApp.Views._05_Officine.Chifa.ClassificationLabo
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CHIFA_ClassificationByLabo : ContentPage
    {
        CHIFA_ClassificaitonByLaboViewModel viewModel { get; set; }
        public CHIFA_ClassificationByLabo()
        {
            InitializeComponent();
            viewModel = new CHIFA_ClassificaitonByLaboViewModel();
            BindingContext = viewModel;
        }
        protected override void OnAppearing()
        {
            base.OnAppearing();
            if (viewModel.Items.Count() <= 0)
            {
                viewModel.LoadListLaboratory.Execute(null);
            }
        }

        private async void ShowHideFilter(object sender, EventArgs e)
        {
            CHIFA_ClassificationByLaboPopupFilter filter = new CHIFA_ClassificationByLaboPopupFilter(viewModel);
            //Load data for the first time ...
            await PopupNavigation.Instance.PushAsync(filter);
        }

        private async void ItemsListView_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            try
            {
                View_CFA_MOBILE_DETAIL_FACTURE ClassificationItem = (View_CFA_MOBILE_DETAIL_FACTURE)e.SelectedItem;
                if (ClassificationItem != null)
                {
                    if (viewModel.ClassificationDisplayType == ClassificationDisplayType.LABORATOIRES)
                    {
                        await Navigation.PushAsync(new CHIFA_ConsommationDetail(ClassificationItem, viewModel.SelectedStartDate, viewModel.SelectedEndDate, SenderType.LABO.ToString()));
                    }
                    else
                    {
                        await Navigation.PushAsync(new CHIFA_ConsommationDetail(ClassificationItem, viewModel.SelectedStartDate, viewModel.SelectedEndDate, SenderType.THERAP.ToString()));
                    }

                    ItemsListView.SelectedItem = null;
                    //ItemsListView1.SelectedItem = null;
                }
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
                    viewModel.SearchLaboratory = String.Empty;
                    if (Title.ToUpper() == ClassificationDisplayType.LABORATOIRES.ToString().ToUpper())
                    {
                        viewModel.ClassificationDisplayType = ClassificationDisplayType.LABORATOIRES;
                    }
                    else
                    {
                        viewModel.ClassificationDisplayType = ClassificationDisplayType.THERAP;
                    }
                    viewModel.LoadListLaboratory.Execute(null);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void DatePicker_DateSelected(object sender, DateChangedEventArgs e)
        {
            try
            {
                viewModel.LoadListLaboratory.Execute(null);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
    }
}