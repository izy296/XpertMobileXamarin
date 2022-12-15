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
    public partial class TourneePopup : PopupPage
    {
        public View_LIV_TOURNEE tournee { get; set; }
        public bool IsBusy { get; set; } // to avoid multiple button clicks events
        public TourneePopup(View_LIV_TOURNEE tournee = null)
        {
            InitializeComponent();
            if (tournee != null)
            {
                this.tournee = tournee;
                if (this.tournee.ETAT_TOURNEE == TourneeStatus.Planned)
                {
                    StatusButton.Text = AppResources.tourneePopUpStart;
                    StatusButton.BackgroundColor = Color.Green;
                    StatusButton.TextColor = Color.White;
                }
                else if (this.tournee.ETAT_TOURNEE == TourneeStatus.Started)
                {
                    StatusButton.Text = AppResources.tourneePopUpClose;
                    StatusButton.BackgroundColor = Color.FromHex("#F26430");
                    StatusButton.TextColor = Color.White;
                }
                else if (this.tournee.ETAT_TOURNEE == TourneeStatus.Closed)
                {
                    StatusButton.Text = AppResources.tourneePopUpOpen;
                    StatusButton.BackgroundColor = Color.LightBlue;
                    StatusButton.TextColor = Color.White;
                }
            }
        }



        private async void DetailsClicked(object sender, EventArgs e)
        {
            if (IsBusy)
                return;
            IsBusy = true;
            MessagingCenter.Send(this, "TourneeDetails", tournee);
            IsBusy = false;
            await PopupNavigation.Instance.PopAsync();
        }

        private async void ChangeTourneeStatus(object sender, EventArgs e)
        {
            if (IsBusy)
                return;
            IsBusy = true;
            MessagingCenter.Send(this, "UpdateTourneeStatus", tournee);
            IsBusy = false;
            await PopupNavigation.Instance.PopAsync();
        }
    }
}