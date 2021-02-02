using XpertMobileApp.DataService;
using Xamarin.Forms;
using Xamarin.Forms.Internals;
using Xamarin.Forms.Xaml;
using XpertMobileApp.ViewModels;
using XpertMobileApp.Api;
using System.Collections.Generic;
using Acr.UserDialogs;
using System;

namespace XpertMobileApp.Views
{
    [Preserve(AllMembers = true)]
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ProductHomePage : ContentPage
    {
        ProductHomePageViewModel viewModel;
        public ProductHomePage()
        {

            viewModel = new ProductHomePageViewModel();

            InitializeComponent();

        }

        protected async override void OnAppearing()
        {
            base.OnAppearing();

            try 
            {
                UserDialogs.Instance.ShowLoading(AppResources.txt_Waiting);
                var homeInfos = await BoutiqueManager.GetHomeProducts();
                viewModel.NewArrivalProducts = viewModel.GenerateData(homeInfos.NewProducts);
                viewModel.OfferProducts = viewModel.GenerateData(homeInfos.OfferProduts);
                viewModel.RecommendedProducts = viewModel.GenerateData(homeInfos.RecommendedProduts);
                UserDialogs.Instance.HideLoading();
                this.BindingContext = viewModel;
            }
            catch (Exception ex)
            {
                UserDialogs.Instance.HideLoading();
                await UserDialogs.Instance.AlertAsync(ex.Message, AppResources.alrt_msg_Alert,
                        AppResources.alrt_msg_Ok);
            }
        }
    }
}