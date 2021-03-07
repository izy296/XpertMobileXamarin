using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using XpertMobileApp.ViewModels.XLogin;

namespace XpertMobileApp.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class ProfilPage : ContentPage
	{
        ProfilePageViewModel viewModel;

        public ProfilPage()
		{
            viewModel = new ProfilePageViewModel();
            viewModel.Title = "Profile";
            BindingContext = viewModel;
            InitializeComponent ();
        }

        protected async override void OnAppearing()
        {
            base.OnAppearing();

            var res = await viewModel.GetBll().GetItemAsync(App.User.Token.userID);
            viewModel.Email = res.Email;
            viewModel.Item = res;

        }
    }
}
 