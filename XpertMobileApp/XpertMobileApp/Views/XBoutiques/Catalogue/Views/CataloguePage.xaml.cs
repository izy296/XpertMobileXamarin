#region Copyright Syncfusion Inc. 2001-2019.
// Copyright Syncfusion Inc. 2001-2019. All rights reserved.
// Use of this code is subject to the terms of our license.
// A copy of the current license can be obtained at any time by e-mailing
// licensing@syncfusion.com. Any infringement will be prosecuted under
// applicable laws. 
#endregion

using FFImageLoading.Forms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Internals;
using Xamarin.Forms.Xaml;
using XpertMobileApp;
using XpertMobileApp.Api;
using XpertMobileApp.Api.Managers;
using XpertMobileApp.DAL;
using XpertMobileApp.Models;
using XpertMobileApp.ViewModels;
using XpertMobileApp.Views;

namespace XpertMobileApp.Views
{
    [Preserve(AllMembers = true)]
    public partial class CataloguePage : BaseView
    {
        LoadMoreViewModel viewModel;
        public CataloguePage()
        {
            viewModel = new LoadMoreViewModel();
            this.BindingContext = viewModel;
            InitializeComponent();

            NavigationPage.SetHasNavigationBar(this, false);
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            if (viewModel.Items.Count == 0)
                viewModel.LoadMoreItemsCommand.Execute(listView);

            if(viewModel.Familles.Count == 0)
                viewModel.LoadExtrasDataCommand.Execute(listView);

            viewModel.ExecuteLoadPanierCommand();
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
             viewModel.Reload();
        }

        private async void pullToRefresh_Refreshing(object sender, EventArgs e)
        {
            pullToRefresh.IsRefreshing = true;
            await viewModel.Reload();
            pullToRefresh.IsRefreshing = false;
        }

        private void brn_Wished_Clicked(object sender, EventArgs e)
        {
            var btn = sender as Button;
            Product p = btn.BindingContext as Product;

            View_WishList wl = new View_WishList();
            wl.ID_USER = App.User.Token.userID;
            wl.CODE_PRODUIT = p.Id;

            if (p.Wished)
            {
                p.Wished = false;
                CrudManager.WishList.DeleteItemAsync(p.Id);
            }
            else
            {
                p.Wished = true;
                CrudManager.WishList.AddItemAsync(wl);
            }
        }

        private async void listView_ItemTapped(object sender, Syncfusion.ListView.XForms.ItemTappedEventArgs e)
        {
            var item = e.ItemData as Product;
            if (item == null)
                return;

            await Navigation.PushAsync(new BtqProductDetailPage(item));

            // Manually deselect item.
            listView.SelectedItem = null;
        }
        #endregion


    }
}