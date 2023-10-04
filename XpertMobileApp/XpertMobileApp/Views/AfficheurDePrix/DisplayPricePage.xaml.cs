using Rg.Plugins.Popup.Services;
using System;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Timers;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using XpertMobileApp.Services;
using XpertMobileApp.ViewModels;

namespace XpertMobileApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class DisplayPricePage : ContentPage
    {
        DisplayPriceViewModel viewModel;
        Timer refreshDisplay;
        Timer standBy;

        public DisplayPricePage()
        {
            InitializeComponent();
            DependencyService.Get<IOrientaionService>().ReverseLandscape();
            DependencyService.Get<IStatusBar>().HideStatusBar();
            refreshDisplay = new Timer();
            standBy = new Timer();
            refreshDisplay.Interval = 15000;
            standBy.Interval = 30000;
            refreshDisplay.Elapsed += RefreshDisplay_Elapsed;
            standBy.Elapsed += StandBuy_Elapsed;

            BindingContext = viewModel = new DisplayPriceViewModel();
            standBy.Start();

            Device.BeginInvokeOnMainThread(async () =>
            {
                await viewModel.GetPubImages();

            });

            

            MessagingCenter.Subscribe<Application, string>(this, "BareCode", (o, e) =>
            {
                // do something whenever the message is sent
                Device.BeginInvokeOnMainThread(async () =>
                {
                    if (PopupNavigation.Instance.PopupStack.Count > 0)
                        await PopupNavigation.Instance.PopAllAsync();
                    HidePubImages();
                    refreshDisplay?.Stop();
                    standBy?.Stop();
                    viewModel.BareCode = e.ToString();
                    await GetProductInfo();
                    if (viewModel.Item.IS_PROMOTION)
                        labelPriceValue.TextDecorations = TextDecorations.Strikethrough;
                    else labelPriceValue.TextDecorations = TextDecorations.None;
                    refreshDisplay.Start();
                    standBy.Start();
                });
            });
            
            MessagingCenter.Subscribe<DisplayPriceViewModel, string>(this, "StartSLider", (o, e) =>
            {
                Device.StartTimer(TimeSpan.FromSeconds(7), (Func<bool>)(() =>
                {
                    Carousel.Position = (Carousel.Position + 1) % viewModel.Images.Count;
                    return true;
                }));
            });



        }

        private void StandBuy_Elapsed(object sender, ElapsedEventArgs e)
        {
            ShowPubImages();
        }

        private void ShowPubImages()
        {

            if (!viewModel.StandBy)
                viewModel.StandBy = true;
        }

        private void HidePubImages()
        {
            if (viewModel.StandBy)
                viewModel.StandBy = false;
        }

        private void RefreshDisplay_Elapsed(object sender, ElapsedEventArgs e)
        {
            viewModel.InitDisplay();
            refreshDisplay?.Stop();
        }

        public System.Drawing.Image GetImageFromStrim(byte[] dataImage)
        {
            if (dataImage == null) return null;
            try
            {
                using (MemoryStream ms = new MemoryStream(dataImage, 0, dataImage.Length))
                {
                    ms.Write(dataImage, 0, dataImage.Length);
                    //Set image variable value using memory stream.
                    return System.Drawing.Image.FromStream(ms, true);
                }
            }
            catch (Exception e)
            {
                return null;
            }

        }

        async Task GetProductInfo()
        {
            try
            {
                await viewModel.GetProductInfoByBareCode();
            }
            catch (Exception ex)
            {
                viewModel.DisplayProductError();
            }
        }

        private async void ShowConfig(object sender, EventArgs e)
        {
            SettingsDisplayPricePopup sp = new SettingsDisplayPricePopup();
            await PopupNavigation.Instance.PushAsync(sp);
        }
    }
}