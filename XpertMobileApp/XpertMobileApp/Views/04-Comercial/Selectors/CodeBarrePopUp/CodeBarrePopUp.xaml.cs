using System;
using Rg.Plugins.Popup.Pages;
using Xamarin.Forms;
using Rg.Plugins.Popup.Services;
using XpertMobileApp.ViewModels;
using XpertMobileApp.Helpers;
using XpertMobileApp.Models;
using System.Linq;
using XpertMobileApp.DAL;
using XpertMobileApp.Api.Services;
using System.Collections.Generic;
using Acr.UserDialogs;

namespace XpertMobileApp.Views
{
    public partial class CodeBarrePopUp : PopupPage
    {

        public string title, message, codeBarre;

        public CodeBarrePopUp(string title,string message,string codeBarre)
        {
            InitializeComponent();
            this.title = title;
            this.message = message;
            this.codeBarre = codeBarre;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            messageText.Text = message;
            cBText.Text = codeBarre;
            //titleText.Text = title;
        }

        private async void OnClose(object sender, EventArgs e)
        {
            await PopupNavigation.Instance.PopAsync();
        }

    }
}
