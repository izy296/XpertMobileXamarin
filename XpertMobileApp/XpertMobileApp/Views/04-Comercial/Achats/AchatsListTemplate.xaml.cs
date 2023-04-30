using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using XpertMobileApp.DAL;
using XpertMobileApp.Views._04_Comercial.Achats;

namespace XpertMobileApp.Views.Templates
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AchatsListTemplate : ContentView
    {
        public AchatsListTemplate()
        {
            InitializeComponent();
        }

        private void ShowDetailSales(object sender, EventArgs e)
        {
            try
            {
                View_ACH_DOCUMENT item = (sender as Frame).Parent.Parent.Parent.BindingContext as View_ACH_DOCUMENT;
                Task.Run(async () => await Navigation.PushAsync(new NewAchatPage(item:item)));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void OnUpdateSwipeItemInvoked(object sender, EventArgs e)
        {

        }
    }
}