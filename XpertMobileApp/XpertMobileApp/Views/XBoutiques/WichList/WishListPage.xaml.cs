#region Copyright Syncfusion Inc. 2001-2019.
// Copyright Syncfusion Inc. 2001-2019. All rights reserved.
// Use of this code is subject to the terms of our license.
// A copy of the current license can be obtained at any time by e-mailing
// licensing@syncfusion.com. Any infringement will be prosecuted under
// applicable laws. 
#endregion

using Syncfusion.ListView.XForms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Internals;
using Xamarin.Forms.Xaml;
using XpertMobileApp.Views;

namespace SampleBrowser.SfListView
{
    [Preserve(AllMembers = true)]
    public partial class WishListPage : BaseView
    {
        WishListViewModel viewModel;
        private GridLayout gridLayout;
        public WishListPage()
        {
            viewModel = new WishListViewModel();
            this.BindingContext = viewModel;

            InitializeComponent();
            gridLayout = new GridLayout();

            if (Device.RuntimePlatform == Device.Android || Device.RuntimePlatform == Device.iOS)
                gridLayout.SpanCount = Device.Idiom == TargetIdiom.Phone ? 2 : 4;
            else if (Device.RuntimePlatform == Device.UWP)
            {
                gridLayout.SpanCount = Device.Idiom == TargetIdiom.Desktop || Device.Idiom == TargetIdiom.Tablet ? 4 : 2;
                listView.ItemSize = Device.Idiom == TargetIdiom.Desktop || Device.Idiom == TargetIdiom.Tablet ? 230 : 140;
            }

            listView.LayoutManager = gridLayout;
        }

        protected async override void OnAppearing()
        {
            base.OnAppearing();
            
            if (viewModel.Items.Count == 0)
                await viewModel.AddProducts(1,10);
            
            if (viewModel.Familles.Count == 0)
                viewModel.LoadExtrasDataCommand.Execute(listView);
        }

        private void Filter_Clicked(object sender, EventArgs e)
        {
           // FilterPanel.IsVisible = !FilterPanel.IsVisible;
        }

        private void btn_ApplyFilter_Clicked(object sender, EventArgs e)
        {
            viewModel.LoadItemsCommand.Execute(listView);
        }

        private void btn_CancelFilter_Clicked(object sender, EventArgs e)
        {
          //  FilterPanel.IsVisible = false;
        }
    }
}