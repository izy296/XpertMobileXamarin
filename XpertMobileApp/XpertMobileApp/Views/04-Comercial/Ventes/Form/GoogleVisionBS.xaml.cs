using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace XpertMobileApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class GoogleVisionBS : ContentPage
    {
        public string barodeScanned;
        public GoogleVisionBS()
        {
            InitializeComponent();

            GoogleVisionBarCodeScanner.Methods.AskForRequiredPermission();
        }

        private async void CameraView_OnDetected(object sender, GoogleVisionBarCodeScanner.OnDetectedEventArg e)
        {
            List<GoogleVisionBarCodeScanner.BarcodeResult> obj = e.BarcodeResults;

            string result = string.Empty;
            
            OnUserSubmitted(obj[0].DisplayValue);
        }

        public event EventHandler<string> UserSubmitted;
        protected virtual void OnUserSubmitted(string e)
        {
            UserSubmitted?.Invoke(this, e);
        }

        private async Task RowScan_ClickedAsync(object sender, EventArgs e)
        {
            string result = await DisplayPromptAsync(AppResources.codebarre_titre_message, AppResources.codebarre_corps_message);

            if (!string.IsNullOrEmpty(result))
            {
                OnUserSubmitted(result);
            }
            else if (result!=null) {
                await DisplayAlert(AppResources.codebarre_titre_message, AppResources.codebarre_error_corps_message, AppResources.alrt_msg_Ok);
            }
        }

        private void RowScan_Clicked(object sender, EventArgs e)
        {
            RowScan_ClickedAsync(sender, e);
        }
    }
}