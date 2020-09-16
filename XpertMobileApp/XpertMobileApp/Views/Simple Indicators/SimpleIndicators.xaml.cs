using System;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using XpertMobileApp.ViewModels;

namespace XpertMobileApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SimpleIndicators : ContentPage
    {
        SimpleIndicatorsModel viewModel;

        public SimpleIndicators()
        {
            
            InitializeComponent();

            // Title = AppResources.pn_home;
            // lblUser.Text = App.User?.UserName;
            // lblClientName.Text = App.Settings.ClientName;
            BindingContext = viewModel = new SimpleIndicatorsModel();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            if (viewModel.Items.Count == 0)
                viewModel.LoadItemsCommand.Execute(null);

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
    }
}