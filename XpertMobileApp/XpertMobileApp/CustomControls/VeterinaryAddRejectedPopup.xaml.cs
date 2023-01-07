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
    public partial class VeterinaryAddRejectedPopup : PopupPage, INotifyPropertyChanged
    {
        // For getting the popup result ...
        private TaskCompletionSource<bool> taskCompletionSource;
        public Task<bool> PopupClosedTask { get { return taskCompletionSource.Task; } }

        public List<PRESTATION_REJECTED> Result;
        public List<PRESTATION_REJECTED> selectedList = new List<PRESTATION_REJECTED>();

        public VeterinaryAddRejectedPopup(String Message, string falseMessage = null, string trueMessage = null)
        {
            InitializeComponent();
            this.CloseWhenBackgroundIsClicked = false;
            messageLabel.Text = Message.ToString();
            taskCompletionSource = new TaskCompletionSource<bool>();
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

            MessagingCenter.Subscribe<AchatFormPageAbattoire, List<PRESTATION_REJECTED>>(this, "RejectedList", async (obj, selectedItem) =>
             {
                 Device.BeginInvokeOnMainThread(() =>
                 {
                     if (XpertHelper.IsNotNullAndNotEmpty(selectedItem))
                     {
                         if (ListRejected.Children.Count > 0 && ((Entry)((Grid)ListRejected.Children[0]).Children[0]).Text != "")
                             ListRejected.Children.Remove(ListRejected.Children[0]);

                         foreach (var item in selectedItem)
                         {
                             AddNewRejectedRow("Ajout Réjecté", item.DESIGNATION_REJET);
                             //Entry newEntry = new Entry();
                             //newEntry.Text = item.DESIGNATION_REJET;
                             //ListRejected.Children.Add(newEntry);
                         }
                     }
                 });
             });
        }

        private async void Close_filter_popup(object sender, EventArgs e)
        {
            try
            {
                taskCompletionSource.SetResult(false);
                await PopupNavigation.Instance.PopAsync();
                MessagingCenter.Unsubscribe<AchatFormPageAbattoire, List<PRESTATION_REJECTED>>(this, "RejectedList");
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
                var children = ListRejected.Children;
                foreach (Grid child in children)
                {
                    var gridChildren = child.Children;
                    foreach (var gridChild in gridChildren)
                    {
                        if (gridChild.GetType() == typeof(Entry))
                            if (((Entry)gridChild).Text != "")
                                selectedList.Add(new PRESTATION_REJECTED()
                                {
                                    DESIGNATION_REJET = ((Entry)gridChild).Text,
                                });
                    }

                }
                Result = selectedList;
                taskCompletionSource.SetResult(true);
                await PopupNavigation.Instance.PopAsync();
                MessagingCenter.Unsubscribe<AchatFormPageAbattoire, List<PRESTATION_REJECTED>>(this, "RejectedList");
                MessagingCenter.Send(this, "VeterinaryPopup", entryNote.Text);
            }
            catch (Exception ex)
            {

            }
        }

        private async void AnswerWithNo(object sender, EventArgs e)
        {
            try
            {
                taskCompletionSource.SetResult(true);
                await PopupNavigation.Instance.PopAsync();
                MessagingCenter.Unsubscribe<AchatFormPageAbattoire, List<PRESTATION_REJECTED>>(this, "RejectedList");
                MessagingCenter.Send(this, "VeterinaryPopup", entryNote.Text);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        private void AddNewRejectedRow(string placeholder = null, string textButton = null)
        {
            Grid grid = new Grid();
            grid.RowDefinitions.Add(new RowDefinition() { Height = GridLength.Star });
            grid.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(2, GridUnitType.Star) });
            grid.ColumnDefinitions.Add(new ColumnDefinition() { Width = GridLength.Star });

            Entry newEntry = new Entry();
            ImageButton removeButton = new ImageButton();

            if (placeholder != null)
                newEntry.Placeholder = placeholder;

            if (textButton != null)
                newEntry.Text = textButton;

            newEntry.SetValue(Grid.RowProperty, 0);
            newEntry.SetValue(Grid.ColumnProperty, 0);
            removeButton.Source = "closeIcon.png";
            removeButton.BackgroundColor = Color.White;
            removeButton.Padding = 10;
            removeButton.SetValue(Grid.RowProperty, 0);
            removeButton.SetValue(Grid.ColumnProperty, 1);
            removeButton.Clicked += Button_Clicked;
            grid.Children.Add(newEntry);
            grid.Children.Add(removeButton);
            ListRejected.Children.Add(grid);
        }

        private void AddNewRejected(object sender, EventArgs e)
        {
            Device.InvokeOnMainThreadAsync(() =>
            {
                AddNewRejectedRow("Ajout Réjecté");
            });
        }

        private void Button_Clicked(object sender, EventArgs e)
        {
            View parent = (View)((ImageButton)sender).Parent;
            ListRejected.Children.Remove(parent);
        }
    }
}

