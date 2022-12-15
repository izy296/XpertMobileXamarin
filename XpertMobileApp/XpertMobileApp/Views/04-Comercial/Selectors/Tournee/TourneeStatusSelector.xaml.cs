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

namespace XpertMobileApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class TourneeStatusSelector : PopupPage
    {
        public StackLayout statusChanger;
        private bool TourneeClosed;
        public TourneeStatusSelector()
        {
            InitializeComponent();

            MessagingCenter.Subscribe<TourneeVisitPage, string>(this, "TourneeClosed", async (obj, item) =>
            {
                await DisplayAlert(AppResources.alrt_msg_Info, "Décloturer la tournee et ressayer !", AppResources.alrt_msg_Ok);
                await PopupNavigation.PopAsync();
            });
            statusChanger = (StackLayout)StatusChanger;
            foreach (var status in Enum.GetValues(typeof(TourneeStatus)))
            {
                Button statusButton = new Button();

                statusButton.TextColor = Color.White;
                statusButton.CornerRadius = 15;
                statusButton.CommandParameter = (byte)status;
                if (status.ToString() == TourneeStatus.Planned.ToString())
                {
                    statusButton.Text = AppResources.tourneeStatusPlaned;
                    statusButton.BackgroundColor = Color.FromHex("#2A2D34");
                }
                else if (status.ToString() == TourneeStatus.EnRoute.ToString())
                {
                    statusButton.Text = AppResources.tourneeStatusInRoute;
                    statusButton.BackgroundColor = Color.FromHex("#6fc2e3");
                }
                else if (status.ToString() == TourneeStatus.Visited.ToString())
                {
                    statusButton.Text = AppResources.tourneeStatusVisited;
                    statusButton.BackgroundColor = Color.FromHex("#009DDC");
                }
                else if (status.ToString() == TourneeStatus.Delivered.ToString())
                {
                    statusButton.Text = AppResources.tourneeStatusDelivered;
                    statusButton.BackgroundColor = Color.FromHex("#009B72");
                }
                else if (status.ToString() == TourneeStatus.Canceled.ToString())
                {
                    statusButton.Text = AppResources.tourneeStatusCanceled;
                    statusButton.BackgroundColor = Color.FromHex("#F26430");
                }
                else if (status.ToString() == TourneeStatus.VisitedNotDelivered.ToString())
                {
                    statusButton.Text = AppResources.tourneeStatusVisitedNotDelivered;
                    statusButton.BackgroundColor = Color.FromHex("#6761A8");
                }
                else if (status.ToString() == TourneeStatus.NotVisited.ToString())
                {
                    statusButton.Text = AppResources.tourneeStatusNotVisited;
                    statusButton.BackgroundColor = Color.FromHex("#6761A8");
                }
                else if (status.ToString() != TourneeStatus.Started.ToString() && status.ToString() != TourneeStatus.Closed.ToString())
                {
                    statusButton.Text = AppResources.tourneeStatusPlaned;
                    statusButton.BackgroundColor = Color.FromHex("#2A2D34");
                }

                statusButton.Clicked += StatusClicked;
                if (status.ToString() != TourneeStatus.Started.ToString() && status.ToString() != TourneeStatus.Closed.ToString())
                    statusChanger.Children.Add(statusButton);
            }

        }

        protected async void StatusClicked(object sender, EventArgs e)
        {
            var button = sender as Button;
            var tourneeNewStatus = (TourneeStatus)button.CommandParameter;
            MessagingCenter.Send(this, "VisitStatusChanged", tourneeNewStatus);
            await PopupNavigation.PopAsync();
        }
    }
}