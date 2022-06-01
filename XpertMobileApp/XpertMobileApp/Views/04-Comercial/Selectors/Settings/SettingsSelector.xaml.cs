using System;
using Rg.Plugins.Popup.Pages;
using Xamarin.Forms;
using Rg.Plugins.Popup.Services;
using XpertMobileApp.ViewModels;
using XpertMobileApp.Helpers;
using System.Linq;
using System.Threading.Tasks;
using XpertMobileApp.Models;
using XpertMobileApp.DAL;
using XpertMobileApp.Api.Services;
using XpertMobileApp.SQLite_Managment;
using XpertMobileApp.Views._04_Comercial.Selectors.Settings;
using Newtonsoft.Json;
using System.ComponentModel;

namespace XpertMobileApp.Views
{
    public partial class SettingsSelector : PopupPage, INotifyPropertyChanged
    {

        public ContentPage pargentPage;

        SettingsSelectorViewModel viewModel;

        public EventHandler<string> data;

        public SettingsSelector()
        {
            InitializeComponent();
            BindingContext = viewModel = new SettingsSelectorViewModel();
            urlEntry.Text = "http://";
        }
        private void SendResponse()
        {
            SettingsSelectorViewModel viewModel;
            //if the data is sent null or empty
            if (string.IsNullOrEmpty(titreEntry.Text) && !string.IsNullOrEmpty(urlEntry.Text))
            {
                viewModel = new SettingsSelectorViewModel()
                {
                    Titre = "",
                    Url = urlEntry.Text.ToString()
                };
            }
            else if (!string.IsNullOrEmpty(titreEntry.Text) && string.IsNullOrEmpty(urlEntry.Text))
            {
                viewModel = new SettingsSelectorViewModel()
                {
                    Titre = titreEntry.Text.ToString(),
                    Url = ""
                };
            }
            else if (string.IsNullOrEmpty(titreEntry.Text) && string.IsNullOrEmpty(urlEntry.Text))
            {
                viewModel = new SettingsSelectorViewModel()
                {
                    Titre = "",
                    Url = ""
                };
            }
            else
            {
                viewModel = new SettingsSelectorViewModel()
                {
                    Titre = titreEntry.Text.ToString(),
                    Url = urlEntry.Text.ToString()
                };

            }

            data?.Invoke(this, JsonConvert.SerializeObject(viewModel));
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
        }

        //dismiss the pop up  
        private async void Btn_annuler_Clicked(object sender, EventArgs e)
        {
            await PopupNavigation.Instance.PopAsync();
        }

        private async void Btn_ajout_Clicked(object sender, EventArgs e)
        {
            SendResponse();
            await PopupNavigation.Instance.PopAsync();
        }
    }
}
