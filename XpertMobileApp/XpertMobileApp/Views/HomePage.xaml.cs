using System;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
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

            Title = AppResources.pn_home;

            BindingContext = viewModel = new HomeViewModel();
        }

        async Task ConnectUserAsync(object sender, EventArgs e)
        {

        }
    }
}