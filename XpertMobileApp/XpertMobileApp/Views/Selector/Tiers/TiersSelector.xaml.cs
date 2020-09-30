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
            SendResponse();

            await PopupNavigation.Instance.PopAsync();
        }

        private void SendResponse() 
        {
            // App.MsgCenter.SendAction(this, CurrentStream, "", MCDico.ITEM_SELECTED, viewModel.SelectedItem);
            if (string.IsNullOrEmpty(CurrentStream))
                MessagingCenter.Send(this, MCDico.ITEM_SELECTED, viewModel.SelectedItem);
            else
            {
               // MessagingCenter.Send(App.MsgCenter, CurrentStream, viewModel.SelectedItem);
                MessagingCenter.Send(this, CurrentStream, viewModel.SelectedItem);
            }
        }

        private async void OnItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            //btnSelect.IsEnabled = true;
            SendResponse();
            await PopupNavigation.Instance.PopAsync();
        }

        async void btn_Search_Clicked(object sender, EventArgs e)
        {
            viewModel.LoadItemsCommand.Execute(null);
        }

        NewTiersPopupPage form;
        private async void btn_Add_Clicked(object sender, EventArgs e)
        {
            form = new NewTiersPopupPage(null);
            await PopupNavigation.Instance.PushAsync(form);
         //   await Navigation.PushModalAsync(new NavigationPage(form));
        }

        private void Ent_Filter_TextChanged(object sender, TextChangedEventArgs e)
        {
            viewModel.LoadItemsCommand.Execute(null);
        }

        private async void OnUpdateSwipeItemInvoked(object sender, EventArgs e)
        {
            var item = viewModel.SelectedItem;
            var tr = (sender as SwipeItem)?.Parent?.Parent?.Parent?.BindingContext as View_TRS_TIERS;

            form = new NewTiersPopupPage(tr);
            await PopupNavigation.Instance.PushAsync(form);
        }

        private void OnDeleteSwipeItemInvoked(object sender, EventArgs e)
        {
            var item = viewModel.SelectedItem;
            var tr = (sender as SwipeItem)?.Parent?.Parent?.Parent?.BindingContext as View_TRS_TIERS;
        }
    }
}
