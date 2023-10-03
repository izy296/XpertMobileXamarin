using System.ComponentModel;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using XpertMobileApp.Services;

namespace XpertMobileApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class DisplayPricePage : INotifyPropertyChanged
    {
        private string bareCode;

        public string BareCode
        {
            get { return bareCode; }
            set { 
                bareCode = value;
                BareCodeChanged();
            }
        }

        public DisplayPricePage()
        {
            InitializeComponent();
            DependencyService.Get<IOrientaionService>().ReverseLandscape();
            DependencyService.Get<IStatusBar>().HideStatusBar();

            BindingContext= this;

            MessagingCenter.Subscribe<Application, string>(this, "BareCode",(o,e) => {
                // do something whenever the message is sent
                Device.BeginInvokeOnMainThread(() => {
                    BareCode = e.ToString();
                });
            });

        }

        private void BareCodeChanged()
        {
            var code = BareCode;

        }
    }
}