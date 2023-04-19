using Rg.Plugins.Popup.Pages;
using Rg.Plugins.Popup.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using XpertMobileApp.DAL;
using XpertMobileApp.Models;

namespace XpertMobileApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class TourneeOpenSelector : PopupPage
    {

        private TaskCompletionSource<bool> taskCompletionSource;
        public Task<bool> PopupClosedTask { get { return taskCompletionSource.Task; } }

        public View_LIV_TOURNEE Result;
        private View_LIV_TOURNEE SelectedItem;
        private List<View_LIV_TOURNEE> items;
        public List<View_LIV_TOURNEE> Items
        {
            get 
            {
                return items;
            }
            set
            {
                items = value;
                OnPropertyChanged("Items");
            }
        }



        public TourneeOpenSelector(String Message, string falseMessage = null, string trueMessage = null)
        {
            InitializeComponent();
            BindingContext = this;
            this.CloseWhenBackgroundIsClicked = false;
            messageLabel.Text = Message.ToString();
            taskCompletionSource = new TaskCompletionSource<bool>();
        }

        private async void Close_filter_popup(object sender, EventArgs e)
        {
            try
            {
                Result = SelectedItem;
                taskCompletionSource.SetResult(true);
                await PopupNavigation.Instance.PopAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public TourneeOpenSelector(List<View_LIV_TOURNEE> items)
        {
            InitializeComponent();
            BindingContext = this;
            if (items!= null)
                Items= items;
        }

        private async void ListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                if (e.CurrentSelection.Count > 0)
                {
                    SelectedItem = e.CurrentSelection[0] as View_LIV_TOURNEE;
                    Result = SelectedItem;
                    taskCompletionSource.SetResult(true);
                    await PopupNavigation.Instance.PopAsync();
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
    }
}