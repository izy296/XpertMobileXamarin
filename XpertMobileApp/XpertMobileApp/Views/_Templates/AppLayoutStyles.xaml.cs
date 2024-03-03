using Xamarin.Forms.Internals;
using Xamarin.Forms.Xaml;

namespace XpertMobileApp.Views.Templates
{
    /// <summary>
    /// Class helps to reduce repetitive markup and allows to change the appearance of apps more easily.
    /// </summary>
    [Preserve(AllMembers = true)]
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AppLayoutStyles
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Styles" /> class.
        /// </summary>
        public AppLayoutStyles()
        {
            this.InitializeComponent();
        }
    }
}