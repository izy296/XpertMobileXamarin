using Acr.UserDialogs;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Internals;
using Xpert.Common.WSClient.Helpers;
using Xpert.Common.WSClient.Services;
using XpertMobileApp.Api;
using XpertMobileApp.Api.Managers;
using XpertMobileApp.DAL;
using XpertMobileApp.Models;
using XpertMobileApp.ViewModels;
using XpertMobileApp.Views.XLogin;

namespace XpertMobileApp.Views
{
    [Preserve(AllMembers = true)]
    public partial class CataloguePage : BaseView
    {
        LoadMoreViewModel viewModel;
        internal CrudService<View_WishList> WishList;
        public CataloguePage()
        {
            viewModel = new LoadMoreViewModel(this);
            this.BindingContext = viewModel;
            InitializeComponent();

            NavigationPage.SetHasNavigationBar(this, false);
        }

        private CrudService<View_WishList> GetWListBll() 
        {
            if(WishList == null) 
            { 
                WishList = new CrudService<View_WishList>(App.RestServiceUrl, "WishList", App.User.Token);
            }

            return WishList;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            if (viewModel.Items.Count == 0)
                viewModel.LoadMoreItemsCommand.Execute(listView);

           
            if(viewModel.Familles.Count == 0)
                viewModel.LoadExtrasDataCommand.Execute(listView);
            /* */
            if (App.User?.Token != null) 
            { 
                viewModel.ExecuteLoadPanierCommand(this);
            }
        }

        #region Actions de l'interface
        private void Filter_Clicked(object sender, EventArgs e)
        {
            FilterPanel.IsVisible = !FilterPanel.IsVisible;
        }

        private void btn_CancelFilter_Clicked(object sender, EventArgs e)
        {

            FilterPanel.IsVisible = false;
        }

        private void btn_ApplyFilter_Clicked(object sender, EventArgs e)
        {
             viewModel.Reload(this);
        }

        private void Sort_Clicked(object sender, EventArgs e)
        {
            sortPicker.IsOpen = true;
        }

        private async void pullToRefresh_Refreshing(object sender, EventArgs e)
        {
            pullToRefresh.IsRefreshing = true;
            await viewModel.Reload(this);
            pullToRefresh.IsRefreshing = false;
        }

        private void brn_Wished_Clicked(object sender, EventArgs e)
        {
            var btn = sender as Button;
            Product p = btn.BindingContext as Product;

            if (App.User?.Token != null) 
            { 
                View_WishList wl = new View_WishList();
                wl.ID_USER = App.User.Token.userID;
                wl.CODE_PRODUIT = p.Id;

                if (p.Wished)
                {
                    p.Wished = false;
                    GetWListBll().DeleteItemAsync(p.Id);
                }
                else
                {
                    p.Wished = true;
                    GetWListBll().AddItemAsync(wl);
                }
            }
            else 
            {
                TabbedForm identificationPage = new TabbedForm();
                Navigation.PushAsync(identificationPage);
            }
        }

        private async void listView_ItemTapped(object sender, Syncfusion.ListView.XForms.ItemTappedEventArgs e)
        {
            try 
            { 
                var item = e.ItemData as Product;
                if (item == null)
                    return;

                Product p = await BoutiqueManager.LoadProdDetails(item.Id);
                await Navigation.PushAsync(new BtqProductDetailPage(p,false,false));

                // Manually deselect item.
                listView.SelectedItem = null;
            }
            catch (Exception ex)
            {
                await UserDialogs.Instance.AlertAsync(WSApi2.GetExceptionMessage(ex), AppResources.alrt_msg_Alert,
                    AppResources.alrt_msg_Ok);
            }
        }

        private void sortPicker_SelectionChanged(object sender, Syncfusion.SfPicker.XForms.SelectionChangedEventArgs e)
        {
            viewModel.Reload(this);
        }
        #endregion


    }
}