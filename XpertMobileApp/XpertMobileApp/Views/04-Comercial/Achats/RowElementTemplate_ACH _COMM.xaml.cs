using Acr.UserDialogs;
using Rg.Plugins.Popup.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using XpertMobileApp.ViewModels;
using XpertMobileApp.Views._04_Comercial.TransfertDeFond;

namespace XpertMobileApp.Views.Templates
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class RowElementTemplate_ACH_COMM : ContentView
    {
        public RowElementTemplate_ACH_COMM()
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