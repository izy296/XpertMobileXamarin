using Rg.Plugins.Popup.Pages;
using Rg.Plugins.Popup.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using XpertMobileApp.DAL;

namespace XpertMobileApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class BordereauxSelector : PopupPage
    {
        BordereauxSelectorViewModel viewModel;
        public BordereauxSelector()
        {
            InitializeComponent();
            BindingContext = viewModel = new BordereauxSelectorViewModel();

            viewModel.ExecuteLoadItemsCommand();
        }

        private async void btn_Search_Clicked(object sender, EventArgs e)
        {
            viewModel.Items.Clear();
            await viewModel.ExecuteLoadItemsCommand();
        }

        private async void ItemsListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            await PopupNavigation.Instance.PopAsync();
            MessagingCenter.Send(this, "Search", viewModel.SelectedItem);
        }
    }
}