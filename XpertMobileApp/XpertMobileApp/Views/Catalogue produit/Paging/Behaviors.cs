#region Copyright Syncfusion Inc. 2001-2019.
// Copyright Syncfusion Inc. 2001-2019. All rights reserved.
// Use of this code is subject to the terms of our license.
// A copy of the current license can be obtained at any time by e-mailing
// licensing@syncfusion.com. Any infringement will be prosecuted under
// applicable laws. 
#endregion

using Syncfusion.SfDataGrid.XForms.DataPager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Internals;
using XpertMobileApp;
using XpertMobileApp.Api.Managers;
using XpertMobileApp.DAL;
using XpertMobileApp.Views;

namespace SampleBrowser.SfListView
{
    [Preserve(AllMembers = true)]
    public class SfListViewPagingBehavior : Behavior<BaseView>
    {
        #region Fields

        private Syncfusion.ListView.XForms.SfListView listView;
        private PagingViewModel PagingViewModel;
        private SfDataPager dataPager;

        #endregion

        #region Methods
        protected override void OnAttachedTo(BaseView bindable)
        {
            listView = bindable.FindByName<Syncfusion.ListView.XForms.SfListView>("listView");
            dataPager = bindable.FindByName<SfDataPager>("dataPager");
            PagingViewModel = new PagingViewModel();
            listView.BindingContext = PagingViewModel;
            dataPager.Source = PagingViewModel.pagingProducts;

            dataPager.OnDemandLoading += DataPager_OnDemandLoading;
            base.OnAttachedTo(bindable);
        }

        private async void DataPager_OnDemandLoading(object sender, OnDemandLoadingEventArgs e)
        {
            /*
            var source = PagingViewModel.pagingProducts.Skip(e.StartIndex).Take(e.PageSize);
            listView.ItemsSource = source.ToList<View_STK_PRODUITS>().ToList();
            */

            int itemCount = await CrudManager.Products.ItemsCount(GetFilterParams());

            dataPager.PageCount = itemCount / e.PageSize;

            var page = (PagingViewModel.pagingProducts.Count / e.PageSize) + 1;

            var currentPage = (e.StartIndex / e.PageSize) + 1;

            IEnumerable<View_STK_PRODUITS> source;
            if (currentPage < page)
            {
                source = PagingViewModel.pagingProducts.Skip(e.StartIndex).Take(e.PageSize);
            }
            else { 
               source = await CrudManager.Products.SelectByPage(GetFilterParams(), page, e.PageSize);
               int i = PagingViewModel.pagingProducts.Count;
               foreach (var item in source)
               {
                   i += 1;
                   PagingViewModel.pagingProducts.Add(item);
                   item.IMAGE_URL = App.RestServiceUrl.Replace("api/", "") + string.Format("Images/GetImage?codeProduit={0}", item.CODE_PRODUIT);
                   (item as BASE_CLASS).Index = i;
               }
            }

            listView.ItemsSource = source;

        }

        protected override void OnDetachingFrom(BaseView bindable)
        {
            listView = null;
            PagingViewModel = null;
            dataPager = null;
            base.OnDetachingFrom(bindable);
        }

        #endregion

        protected Dictionary<string, string> GetFilterParams()
        {
            Dictionary<string, string> result = new Dictionary<string, string>();
            /*
            result.Add("searchText", SearchedText);

            if (!string.IsNullOrEmpty(SelectedFamille?.CODE))
                result.Add("famille", SelectedFamille?.CODE);

            if (!string.IsNullOrEmpty(SelectedType?.CODE_TYPE))
                result.Add("type", SelectedType?.CODE_TYPE);
                */
            return result;
        }
    }
}
