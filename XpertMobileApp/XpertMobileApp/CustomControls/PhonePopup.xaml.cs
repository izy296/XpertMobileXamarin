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
    public partial class PhonePopup : PopupPage, INotifyPropertyChanged
    {
        // For getting the popup result ...
        private TaskCompletionSource<bool> taskCompletionSource;
        public Task<bool> PopupClosedTask { get { return taskCompletionSource.Task; } }

        private string result { get; set; }
        public string Result { get { return result; } 
        set
            {
                result = value;
                OnPropertyChanged("Result");
            }
        }

        public PhonePopup(String Message, string falseMessage = null, string trueMessage = null, string PhoneNumber = "", List<string> listPhones=null)
        {
            InitializeComponent();
            this.CloseWhenBackgroundIsClicked = false;
            BindingContext = this;
            messageLabel.Text = Message.ToString();
            Result = "";
            if (listPhones != null)
            {
                NumTele.IsVisible = false;
                TeleList.ItemsSource = listPhones;
            }
            else TeleList.IsVisible = false;

            taskCompletionSource = new TaskCompletionSource<bool>();
            if (PhoneNumber != "")
            {
                NumTele.Text = PhoneNumber;
            }
            if (!string.IsNullOrEmpty(falseMessage))
            {
                buttonNon.Text = falseMessage.ToString();
            }
            else
            {
                buttonNon.IsVisible = false;
            }
            if (!string.IsNullOrEmpty(trueMessage))
            {
                buttonYes.Text = trueMessage.ToString();
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
                if (TeleList.ItemsSource==null)
                    Result = NumTele.Text;
                else
                {
                    Result = Result.Replace("-", "").Replace(".", "").Replace(" ", "");
                }
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
                if (TeleList.ItemsSource == null)
                    Result = NumTele.Text;
                else
                {
                    Result = Result.Replace("-", "").Replace(".", "").Replace(" ", "");
                }
                taskCompletionSource.SetResult(true);
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
                if (TeleList.ItemsSource == null)
                    Result = NumTele.Text;
                else
                {
                    Result = Result.Replace("-", "").Replace(".", "").Replace(" ", "");
                }
                taskCompletionSource.SetResult(false);
                await PopupNavigation.Instance.PopAsync();
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
    }
}

