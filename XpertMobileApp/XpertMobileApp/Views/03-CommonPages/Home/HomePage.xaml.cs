using Acr.UserDialogs;
using System;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Xpert.Common.WSClient.Helpers;
using XpertMobileApp.Api;
using XpertMobileApp.Models;
using XpertMobileApp.SQLite_Managment;
using XpertMobileApp.ViewModels;

namespace XpertMobileApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class HomePage : ContentPage
    {
        HomeViewModel viewModel;

        public HomePage()
        {
            
            InitializeComponent();

           // Title = AppResources.pn_home;
            lblUser.Text = App.User?.UserName;
            lblClientName.Text = App.Settings.ClientName;
            BindingContext = viewModel = new HomeViewModel();
        }

        protected async override void OnAppearing()
        {
            base.OnAppearing();
            try
            {
                UserDialogs.Instance.ShowLoading(AppResources.txt_Loading);
                if(Constants.AppName != Apps.X_BOUTIQUE) 
                { 
                    var param = await AppManager.GetSysParams();
                    var permissions = await AppManager.GetPermissions();
                }
                UserDialogs.Instance.HideLoading();
            }
            catch (Exception ex)
            {
                UserDialogs.Instance.HideLoading();
                await UserDialogs.Instance.AlertAsync(WSApi2.GetExceptionMessage(ex), AppResources.alrt_msg_Alert,
                    AppResources.alrt_msg_Ok);
            }

            if (viewModel.Items.Count == 0) 
            {
                viewModel.LoadItemsCommand.Execute(null);
            }
            if (viewModel.Sessions.Count == 0)
                viewModel.LoadSessionsCommand.Execute(null);
        }

        private void OnItemSelected(object sender, SelectedItemChangedEventArgs e)
        {

        }

        private void listView_SelectionChanged(object sender, Syncfusion.ListView.XForms.ItemSelectionChangedEventArgs e)
        {

        }

        private void btn_Refresh_Clicked(object sender, EventArgs e)
        {
            viewModel.LoadItemsCommand.Execute(null);
        }

        private async void Btn_Menu_Clicked(object sender, EventArgs e)
        {
            MainPage RootPage = Application.Current.MainPage as MainPage;
            string id = ((sender as Button).Parent.Parent.Parent.BindingContext as TDB_SIMPLE_INDICATORS).CODE_ANALYSE;
            if (id=="34")//Export
            {
                await Upload();
            }
            else if (id == "35")//Import
            {
                await Download();
            }
            else
            {
                var page = RootPage.GetMenuPage(Convert.ToInt32(id));
                await Navigation.PushAsync(page);
                // await RootPage.NavigateFromMenu(Convert.ToInt32(id));
            }
        }

        public async Task Download()
        {
            await UpdateDatabase.synchroniseDownload();
        }

        public async Task Upload()
        {
            await UpdateDatabase.synchroniseUpload();
        }
    }
}