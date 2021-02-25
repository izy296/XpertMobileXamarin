using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace XpertMobileApp.Base
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class XBasePage : ContentPage
    {
        public XBasePage()
        {
            InitializeComponent();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
        }
    }
}