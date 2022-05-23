using System;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using XpertMobileApp.ViewModels;

namespace XpertMobileApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class TresoreriePage : ContentPage
    {
        TresoreriePageViewModel viewModel;

        public TresoreriePage()
        {

            InitializeComponent();

            Title = AppResources.pn_home;

            BindingContext = viewModel = new TresoreriePageViewModel();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            //to check if connection exist or no 
            App.StatrtCheckIfInternet(this);

            if (viewModel.Items.Count == 0)
                viewModel.LoadItemsCommand.Execute(null);

            if (viewModel.Sessions.Count == 0)
                viewModel.LoadSessionsCommand.Execute(null);

            if (viewModel.Encaissements.Count == 0)
                viewModel.LoadEncaissStatCommand.Execute(null);
        }

        private void OnItemSelected(object sender, SelectedItemChangedEventArgs e)
        {

        }
    }
}