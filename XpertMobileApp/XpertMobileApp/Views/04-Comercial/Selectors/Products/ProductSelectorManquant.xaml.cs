using System;
using Rg.Plugins.Popup.Pages;
using Xamarin.Forms;
using Rg.Plugins.Popup.Services;
using XpertMobileApp.ViewModels;
using XpertMobileApp.Helpers;
using System.Linq;
using System.Threading.Tasks;
using XpertMobileApp.DAL;
using XpertMobileApp.Models;

namespace XpertMobileApp.Views
{
    public partial class ProductSelectorManquant : PopupPage
    {
        ProductManaquantSelectorViewModel viewModel;
        public string CurrentStream = Guid.NewGuid().ToString();
        public string CodeTiers
        {
            get
            {
                return viewModel.CodeTiers;
            }
            set
            {
                viewModel.CodeTiers = value;
            }
        }
        public bool AutoriserReception
        {
            get
            {
                return viewModel.AutoriserReception;
            }
            set
            {
                viewModel.AutoriserReception = value;
            }
        }
        public string SearchedType { get; set; } = "";
        public ProductSelectorManquant(string stream)
        {
            CurrentStream = stream;
            InitializeComponent();
            BindingContext = viewModel = new ProductManaquantSelectorViewModel();
            MessagingCenter.Subscribe<MsgCenter, View_STK_PRODUITS>(this, MCDico.ITEM_ADDED, async (obj, item) =>
            {
                if (viewModel.Items.Count > 0)
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
            if (string.IsNullOrEmpty(CurrentStream))
                MessagingCenter.Send(this, MCDico.ITEM_SELECTED, viewModel.SelectedItem);
            else
            {                
                MessagingCenter.Send(this, CurrentStream, viewModel.SelectedItem);
            }
        }
        private async void OnItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            SendResponse();
            await PopupNavigation.Instance.PopAsync();
        }
        private async void btnOnSearch_Clicked(object sender, EventArgs e)
        {
            viewModel.LoadItemsCommand.Execute(null);
        }       
    }
}
