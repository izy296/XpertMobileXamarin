using Acr.UserDialogs;
using Rg.Plugins.Popup.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace XpertMobileApp.Views.Templates
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class RowElementTemplate_ACH : ContentView
    {
        public RowElementTemplate_ACH()
        {
            InitializeComponent();
        }

        private async void showReclamation(object sender, EventArgs e)
        {
            ReclamationPopupPage reclamationInfo = new ReclamationPopupPage(this.CodeReclamaion.Text.ToString());
            UserDialogs.Instance.ShowLoading(AppResources.txt_Waiting);
            await reclamationInfo.GetReclamation();
            UserDialogs.Instance.HideLoading();
            await PopupNavigation.Instance.PushAsync(reclamationInfo);
        }
    }

}