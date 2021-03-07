using Xamarin.Forms.Internals;
using Xamarin.Forms.Xaml;
using XpertMobileApp.ViewModels;
using XpertMobileApp.Api;
using Acr.UserDialogs;
using System;
using XpertMobileApp.Models;
using Syncfusion.ListView.XForms;
using Xpert.Common.WSClient.Helpers;

namespace XpertMobileApp.Views
{
    [Preserve(AllMembers = true)]
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ProductHomePage : BaseView
    {
        ProductHomePageViewModel viewModel;
        public ProductHomePage()
        {

            InitializeComponent();

            Title = "Accueil";
            viewModel = new ProductHomePageViewModel(this);
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            LoadData();
        }

        private void Refresh_Clicked(object sender, EventArgs e)
        {
            LoadData();
        }

        private async void LoadData()
        {
            try
            {
                UserDialogs.Instance.ShowLoading(AppResources.txt_Waiting);
                
                var homeInfos                 = await BoutiqueManager.GetHomeProducts();
                viewModel.RecommendedProducts = viewModel.GenerateData(homeInfos.RecommendedProduts);
                viewModel.NewArrivalProducts  = viewModel.GenerateData(homeInfos.NewProducts);
                viewModel.BestEvaluated       = viewModel.GenerateData(homeInfos.BestEvaluated);
                viewModel.BuestSelled         = viewModel.GenerateData(homeInfos.BuestSelled);

                UserDialogs.Instance.HideLoading();
                
                this.BindingContext           = viewModel;

                if(App.User?.Token != null) 
                {
                    viewModel.ExecuteLoadPanierCommand(this);
                }
            }
            catch (Exception ex)
            {
                UserDialogs.Instance.HideLoading();
                await UserDialogs.Instance.AlertAsync(ex.Message, AppResources.alrt_msg_Alert, AppResources.alrt_msg_Ok);
            }
        }

        private async void listView_ItemTapped(object sender, Syncfusion.ListView.XForms.ItemTappedEventArgs e)
        {
            try
            {
                UserDialogs.Instance.ShowLoading(AppResources.txt_Waiting);
                var item = e.ItemData as Product;
                if (item == null)
                    return;

                Product p = await BoutiqueManager.LoadProdDetails(item.Id);
                await Navigation.PushAsync(new BtqProductDetailPage(p, false, false));

                // Manually deselect item.
                (sender as SfListView).SelectedItem = null;

                UserDialogs.Instance.HideLoading();
            }
            catch (Exception ex)
            {
                UserDialogs.Instance.HideLoading();
                await UserDialogs.Instance.AlertAsync(WSApi2.GetExceptionMessage(ex), AppResources.alrt_msg_Alert,
                    AppResources.alrt_msg_Ok);
            }
        }

        private void SeeAll_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new CataloguePage());
        }
    }
}