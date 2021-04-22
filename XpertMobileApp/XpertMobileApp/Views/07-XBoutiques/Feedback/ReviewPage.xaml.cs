
using Xamarin.Forms.Internals;
using Xamarin.Forms.Xaml;
using XpertMobileApp.Models;
using XpertMobileApp.ViewModels.Feedback;

namespace XpertMobileApp.Views.Feedback
{
    /// <summary>
    /// Page to get review from customer
    /// </summary>
    [Preserve(AllMembers = true)]
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ReviewPage
    {
        ReviewPageViewModel viewModel;
        public ReviewPage(Product item)
        {
            viewModel = new ReviewPageViewModel(item);
            this.BindingContext = viewModel;
            InitializeComponent();
           // this.ProductImage.Source = "defaultProdImg.png";
        }
    }
}