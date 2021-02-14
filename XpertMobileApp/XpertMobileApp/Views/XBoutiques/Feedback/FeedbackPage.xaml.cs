using Xamarin.Forms.Internals;
using Xamarin.Forms.Xaml;
using XpertMobileApp.ViewModels.Feedback;

namespace XpertMobileApp.Views.Feedback
{
    /// <summary>
    /// Page to show feedback list
    /// </summary>
    [Preserve(AllMembers = true)]
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class FeedbackPage
    {
        FeedbackViewModel viewModel;
        public FeedbackPage()
        {
            viewModel = new FeedbackViewModel();
            this.BindingContext = viewModel;
            InitializeComponent();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            if (viewModel.Items.Count == 0)
                viewModel.LoadMoreItemsCommand.Execute(listView);
        }

        private async void pullToRefresh_Refreshing(object sender, System.EventArgs e)
        {
            pullToRefresh.IsRefreshing = true;
            await viewModel.Reload(this);
            pullToRefresh.IsRefreshing = false;
        }
    }
}