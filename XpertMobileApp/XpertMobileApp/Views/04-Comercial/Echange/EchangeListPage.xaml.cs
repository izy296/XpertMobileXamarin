using Rg.Plugins.Popup.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using XpertMobileApp.Models;

namespace XpertMobileApp.Views._04_Comercial.Echange
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class EchangeListPage : ContentPage
    {
        private EchangeListViewModel viewModel;
        public EchangeListPage()
        {
            InitializeComponent();
            Title = AppResources.pn_Echanges;
            BindingContext = viewModel = new EchangeListViewModel();
        }
        protected override void OnAppearing()
        {
            base.OnAppearing();

            //Load items for the first time ...
            if (viewModel.Items.Count == 0)
                viewModel.LoadItemsCommand.Execute(null);
        }

        /// <summary>
        /// Show hide the filter section when clicking on the floating button
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void ShowHideFilter(object sender, EventArgs e)
        {
            PopupFilter filter = new PopupFilter(viewModel);
            //Load data for the first time ...
            await PopupNavigation.Instance.PushAsync(filter);
        }

        private async void OnItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            var item = e.SelectedItem as View_STK_ECHANGE;
            if (item == null) return;
            await Navigation.PushAsync(new NavigationPage(new EchangeDetailPage(item)));

            // Manually deselect the item.
            ItemsListView.SelectedItem = null;
        }

        /// <summary>
        /// Open the new popup page to add new Echange ...
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void AjoutItem_Clicked(object sender, EventArgs e)
        {
            NewEchangePopupPage echangePopUp = new NewEchangePopupPage();
            await PopupNavigation.Instance.PushAsync(echangePopUp);
        }
    }
}