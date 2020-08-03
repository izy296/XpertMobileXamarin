using System;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

using XpertMobileApp.Models;
using XpertMobileApp.ViewModels;

namespace XpertMobileApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ItemDetailPage : ContentPage
    {
        ItemDetailViewModel<Item> viewModel;

        public ItemDetailPage(ItemDetailViewModel<Item> viewModel)
        {
            InitializeComponent();

            BindingContext = this.viewModel = viewModel;
        }

        public ItemDetailPage()
        {
            InitializeComponent();

            var item = new Item
            {
                Text = "Item 1",
                Description = "This is an item description."
            };

            viewModel = new ItemDetailViewModel<Item>(item);
            BindingContext = viewModel;
        }
    }
}