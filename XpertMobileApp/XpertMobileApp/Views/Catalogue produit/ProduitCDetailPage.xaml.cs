using Acr.UserDialogs;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Xpert.Common.WSClient.Helpers;
using XpertMobileApp.DAL;
using XpertMobileApp.Models;
using XpertMobileApp.Services;
using XpertMobileApp.ViewModels;

namespace XpertMobileApp.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class ProduitCDetailPage : ContentPage
	{
        ItemDetailViewModel<View_STK_PRODUITS> viewModel;

        public ProduitCDetailPage(View_STK_PRODUITS prod)
        {
            InitializeComponent();

            BindingContext = viewModel = new ItemDetailViewModel<View_STK_PRODUITS>(prod);
        }

        async void Cancel_Clicked(object sender, EventArgs e)
        {
            await Navigation.PopAsync();
        }

        private async void RefBtn_Clicked(object sender, EventArgs e)
        {            
            await Navigation.PushAsync(new ProduitRefDetailPage(viewModel.Item.REFERENCE));            
        }
    }
}