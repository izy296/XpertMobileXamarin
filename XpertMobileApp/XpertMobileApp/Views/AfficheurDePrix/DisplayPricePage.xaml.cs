using Acr.UserDialogs;
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
        Timer carouselTimer;

        public DisplayPricePage()
        {
            InitializeComponent();

            BindingContext = viewModel = new DisplayPriceViewModel();

            DependencyService.Get<IOrientaionService>().ReverseLandscape();
            DependencyService.Get<IStatusBar>().HideStatusBar();

            refreshDisplay = InitTimer(RefreshDisplay_Elapsed, ToMiliSeconds(viewModel.displayPriceConfig.ClearScreenInterval));
            standBy = InitTimer(StandBuy_Elapsed, ToMiliSeconds(viewModel.displayPriceConfig.StandByInterval));
            carouselTimer = InitTimer(CarouselTimer_Elapsed, ToMiliSeconds(viewModel.displayPriceConfig.CarouselInterval));

            Device.BeginInvokeOnMainThread(async () =>
            {
                try
                {
                    UserDialogs.Instance.ShowLoading("Récupération de Configuration");

                    await viewModel.GetConfig();

                    refreshDisplay.Interval = ToMiliSeconds(viewModel.displayPriceConfig.ClearScreenInterval);
                    standBy.Interval = ToMiliSeconds(viewModel.displayPriceConfig.StandByInterval);
                    carouselTimer.Interval = ToMiliSeconds(viewModel.displayPriceConfig.CarouselInterval);

                    standBy.Start();
                    refreshDisplay.Start();
                    carouselTimer.Start();

                    UserDialogs.Instance.HideLoading();
                }
                catch (Exception ex)
                {
                    UserDialogs.Instance.HideLoading();
                    CustomPopup AlertPopup = new CustomPopup(ex.Message, trueMessage: AppResources.alrt_msg_Ok);
                    await PopupNavigation.Instance.PushAsync(AlertPopup);
                }

            });

            Device.BeginInvokeOnMainThread(async () =>
            {
                try
                {
                    UserDialogs.Instance.ShowLoading("Récupération d'images promotionnelles");
                    await viewModel.GetPubImages();
                    UserDialogs.Instance.HideLoading();
                }
                catch (Exception ex)
                {
                    UserDialogs.Instance.HideLoading();
                    CustomPopup AlertPopup = new CustomPopup(ex.Message, trueMessage: AppResources.alrt_msg_Ok);
                    await PopupNavigation.Instance.PushAsync(AlertPopup);
                }


            });



            MessagingCenter.Subscribe<Application, string>(this, "BareCode", (o, e) =>
            {
                // do something whenever the message is sent
                Device.BeginInvokeOnMainThread(async () =>
                {
                    try
                    {
                        if (PopupNavigation.Instance.PopupStack.Count > 0)
                            await PopupNavigation.Instance.PopAllAsync();

                        HidePubImages();
                        refreshDisplay?.Stop();
                        standBy?.Stop();
                        viewModel.BareCode = e.ToString();
                        UserDialogs.Instance.ShowLoading();
                        await GetProductInfo();
                        UserDialogs.Instance.HideLoading();
                        if (viewModel.Item.IS_PROMOTION)
                            labelPriceValue.TextDecorations = TextDecorations.Strikethrough;
                        else labelPriceValue.TextDecorations = TextDecorations.None;
                        refreshDisplay.Start();
                        standBy.Start();
                    }
                    catch (Exception ex)
                    {
                        UserDialogs.Instance.HideLoading();
                        CustomPopup AlertPopup = new CustomPopup(ex.Message, trueMessage: AppResources.alrt_msg_Ok);
                        await PopupNavigation.Instance.PushAsync(AlertPopup);
                    }


                });
            });

            MessagingCenter.Subscribe<SettingsDisplayPricePopup, string>(this, "SyncConfig", (o, e) =>
            {
                Device.BeginInvokeOnMainThread(async () =>
                {
                    UserDialogs.Instance.ShowLoading();

                    await viewModel.GetConfig();

                    UserDialogs.Instance.HideLoading();

                    refreshDisplay?.Stop();
                    standBy?.Stop();
                    carouselTimer?.Stop();

                    refreshDisplay.Interval = ToMiliSeconds(viewModel.displayPriceConfig.ClearScreenInterval);
                    standBy.Interval = ToMiliSeconds(viewModel.displayPriceConfig.StandByInterval);
                    carouselTimer.Interval = ToMiliSeconds(viewModel.displayPriceConfig.CarouselInterval);

                    refreshDisplay?.Start();
                    standBy?.Start();
                    carouselTimer?.Start();

                    CustomPopup AlertPopup = new CustomPopup(AppResources.ap_update_success, trueMessage: AppResources.alrt_msg_Ok);
                    await PopupNavigation.Instance.PushAsync(AlertPopup);
                });
            });


            MessagingCenter.Subscribe<SettingsDisplayPricePopup, string>(this, "RefreshImages", (o, e) =>
            {
                Device.BeginInvokeOnMainThread(async () =>
                {
                    UserDialogs.Instance.ShowLoading("Récupération d'images promotionnelles");
                    await viewModel.GetPubImages();
                    UserDialogs.Instance.HideLoading();


                    CustomPopup AlertPopup = new CustomPopup("Récupération d'images promotionnelles terminé avec succeé", trueMessage: AppResources.alrt_msg_Ok);
                    await PopupNavigation.Instance.PushAsync(AlertPopup);
                });
            });

            MessagingCenter.Subscribe<DisplayPriceViewModel, string>(this, "StartSLider", (o, e) =>
            {
                if (carouselTimer == null)
                {
                    carouselTimer = InitTimer(CarouselTimer_Elapsed, ToMiliSeconds(viewModel.displayPriceConfig.CarouselInterval));
                    carouselTimer.Start();
                }

            });
        }

        private Timer InitTimer(ElapsedEventHandler elapsed, double interval = 0)
        {
            Timer newTimer = new Timer();

            if (interval != 0)
                newTimer.Interval = interval;

            newTimer.Elapsed += elapsed;
            newTimer.Enabled = true;
            timerCount++;
            return newTimer;
        }

        private async void CarouselTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            try
            {
                if (viewModel.Images.Count > 0)
                    Device.BeginInvokeOnMainThread(() => Carousel.Position = (Carousel.Position + 1) % viewModel.Images.Count);
            }
            catch (Exception ex)
            {
                CustomPopup AlertPopup = new CustomPopup(ex.Message, trueMessage: AppResources.alrt_msg_Ok);
                await PopupNavigation.Instance.PushAsync(AlertPopup);
            }
        }

        private double ToMiliSeconds(double seconds)
        {
            return seconds * 1000;
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
            Device.BeginInvokeOnMainThread(() => labelPriceValue.TextDecorations = TextDecorations.None);

            viewModel.InitDisplay();
            refreshDisplay?.Stop();
        }


        async Task GetProductInfo()
        {
            try
            {
                await viewModel.GetProductInfoByBareCode();
            }
            catch (Exception ex)
            {
                UserDialogs.Instance.HideLoading();
                CustomPopup AlertPopup = new CustomPopup(ex.Message, trueMessage: AppResources.alrt_msg_Ok);
                await PopupNavigation.Instance.PushAsync(AlertPopup);
            }
        }

        private async void ShowConfig(object sender, EventArgs e)
        {
            SettingsDisplayPricePopup sp = new SettingsDisplayPricePopup();
            await PopupNavigation.Instance.PushAsync(sp);
        }
    }
}