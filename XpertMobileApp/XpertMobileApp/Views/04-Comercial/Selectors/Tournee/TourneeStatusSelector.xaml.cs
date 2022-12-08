using Rg.Plugins.Popup.Pages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using XpertMobileApp.DAL;

namespace XpertMobileApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class TourneeStatusSelector : PopupPage
    {
        public StackLayout statusChanger;
        public TourneeStatusSelector()
        {
            InitializeComponent();

            statusChanger = (StackLayout)StatusChanger;
        }
    }
}