using System;
using Rg.Plugins.Popup.Pages;
using Xamarin.Forms;
using Rg.Plugins.Popup.Services;
using XpertMobileApp.ViewModels;
using XpertMobileApp.Helpers;
using System.Linq;
using System.Threading.Tasks;
using XpertMobileApp.Models;
using XpertMobileApp.DAL;
using XpertMobileApp.Api.Services;

namespace XpertMobileApp.Views
{
    public partial class TiersSelector : PopupPage
    {

        public ContentPage pargentPage;

        TiersSelectorViewModel viewModel;
        public string CurrentStream { get; set; }

        public string SearchedType { get; set; } = "";

        public TiersSelector(string stream)
        {
            CurrentStream = stream;
            InitializeComponent();

            BindingContext = viewModel = new TiersSelectorViewModel();

            MessagingCenter.Subscribe<MsgCenter, View_TRS_TIERS>(this, MCDico.ITEM_ADDED, async (obj, item) =>
            {
                if(viewModel.Items.Count>0)
                    viewModel.Items.Insert(1, item);
            });
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            viewModel.SearchedType = SearchedType;

            if (viewModel.Items.Count == 0)
                viewModel.LoadItemsCommand.Execute(null);
        }

        private async void OnClose(object sender, EventArgs e)
        {
            XpertHelper.SendAction(this, CurrentStream, "", MCDico.ITEM_SELECTED, viewModel.SelectedItem);
            await PopupNavigation.Instance.PopAsync();
        }

        private void OnItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            btnSelect.IsEnabled = true;
        }

        async void btn_Search_Clicked(object sender, EventArgs e)
        {
            viewModel.LoadItemsCommand.Execute(null);
        }

        NewTiersPopupPage form;
        private async void btn_Add_Clicked(object sender, EventArgs e)
        {
            if (form == null)
            { 
                form = new NewTiersPopupPage(null);
            }
            await PopupNavigation.Instance.PushAsync(form);
         //   await Navigation.PushModalAsync(new NavigationPage(form));
        }

        private void Ent_Filter_TextChanged(object sender, TextChangedEventArgs e)
        {
            viewModel.LoadItemsCommand.Execute(null);
        }
    }
}
