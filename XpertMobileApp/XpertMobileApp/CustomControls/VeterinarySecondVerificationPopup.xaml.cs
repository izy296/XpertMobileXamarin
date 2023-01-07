using Acr.UserDialogs;
using Rg.Plugins.Popup.Pages;
using Rg.Plugins.Popup.Services;
using Syncfusion.XForms.Buttons;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Xpert.Common.DAO;
using Xpert.Common.WSClient.Helpers;
using Xpert.Common.WSClient.Services;
using XpertMobileApp.Api;
using XpertMobileApp.Api.Managers;
using XpertMobileApp.Api.Services;
using XpertMobileApp.DAL;
using XpertMobileApp.Helpers;
using XpertMobileApp.Models;
using XpertMobileApp.Services;
using XpertMobileApp.SQLite_Managment;
using XpertMobileApp.ViewModels;
using XpertMobileApp.Views.Templates;

namespace XpertMobileApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class VeterinarySecondVerificationPopup : PopupPage, INotifyPropertyChanged
    {
        // For getting the popup result ...
        private TaskCompletionSource<bool> taskCompletionSource;
        public Task<bool> PopupClosedTask { get { return taskCompletionSource.Task; } }

        public int Result;

        public VeterinarySecondVerificationPopup(String Message=null, string falseMessage = null, string partFalseMessage = null, string trueMessage = null)
        {
            InitializeComponent();
            this.CloseWhenBackgroundIsClicked = false;
            //messageLabel.Text = Message.ToString();
            taskCompletionSource = new TaskCompletionSource<bool>();
            if (!string.IsNullOrEmpty(falseMessage))
            {
                buttonNon.Text = falseMessage.ToString();
            }
            else
            {
                buttonNon.IsVisible = false;
            }
            if (!string.IsNullOrEmpty(partFalseMessage))
            {
                buttonPartNon.Text = partFalseMessage.ToString();
            }
            else
            {
                buttonYes.IsVisible = false;
            }
            if (!string.IsNullOrEmpty(trueMessage))
            {
                buttonYes.Text= trueMessage.ToString();
            }
            else
            {
                buttonYes.IsVisible = false;
            }
        }

        private async void Close_filter_popup(object sender, EventArgs e)
        {
            try
            {
                Result = -1;
                taskCompletionSource.SetResult(false);
                await PopupNavigation.Instance.PopAsync();
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        private async void AnswerWithYes(object sender, EventArgs e)
        {
            try
            {
                Result = 1;
                taskCompletionSource.SetResult(true);
                //MessagingCenter.Send(this, "VeterinaryPopup", entryNote.Text);
                await PopupNavigation.Instance.PopAsync();
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        private async void AnswerWithNo(object sender, EventArgs e)
        {
            try
            {
                Result = 0;
                taskCompletionSource.SetResult(true);
                await PopupNavigation.Instance.PopAsync();
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        private async void AnswerWithPartNo(object sender, EventArgs e)
        {
            try
            {
                Result = 2;
                taskCompletionSource.SetResult(true);
                await PopupNavigation.Instance.PopAsync();
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
    }
}

