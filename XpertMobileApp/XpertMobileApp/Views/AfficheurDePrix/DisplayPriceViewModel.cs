using Acr.UserDialogs;
using Rg.Plugins.Popup.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xpert.Common.WSClient.Helpers;
using XpertMobileApp.DAL;
using XpertMobileApp.Models;
using XpertMobileApp.Services;
using XpertMobileApp.Views;

namespace XpertMobileApp.ViewModels
{
    public class DisplayPriceViewModel : BaseViewModel
    {

        public BSE_DISPLAY_PRICE_CONFIG displayPriceConfig;

        private string bareCode;

        public string BareCode
        {
            get { return bareCode; }
            set
            {
                bareCode = value;
            }
        }
        private ObservableCollection<BSE_IMAGE_PUBLICITE> images;
        public ObservableCollection<BSE_IMAGE_PUBLICITE> Images
        {
            get { return images; }
            set
            {
                SetProperty(ref images, value);
            }
        }

        private INFO_PROMOTION_DISPLAY_PRICE item;
        public INFO_PROMOTION_DISPLAY_PRICE Item
        {
            get { return item; }
            set { SetProperty(ref item, value); }
        }

        private bool showDisplayPrice = true;
        public bool ShowDisplayPrice
        {
            get
            {
                return showDisplayPrice;
            }
            set
            {
                SetProperty(ref showDisplayPrice, value);
            }
        }

        private bool standBy = false;

        public bool StandBy
        {
            get { return standBy; }
            set
            {
                SetProperty(ref standBy, value);
                ShowDisplayPrice = !value;
            }
        }

        public DisplayPriceViewModel()
        {
            InitDisplay();
            Images = new ObservableCollection<BSE_IMAGE_PUBLICITE>();
            displayPriceConfig = new BSE_DISPLAY_PRICE_CONFIG() { CarouselInterval = 7, ClearScreenInterval = 15, StandByInterval = 30 };
        }

        public void InitDisplay()
        {
            Item = new INFO_PROMOTION_DISPLAY_PRICE()
            {
                DESIGNATION_PRODUIT = "SCANNEZ UN PRODUIT",
                PRODUIT_PRIX = 0,
                IS_PROMOTION = false
            };
        }

        public void DisplayProductError()
        {
            Item = new INFO_PROMOTION_DISPLAY_PRICE()
            {
                DESIGNATION_PRODUIT = "PRODUIT NON TROUVEE",
                PRODUIT_PRIX = 0,
                IS_PROMOTION = false
            };
        }

        public async Task GetConfig()
        {
            try
            {
                BSE_DISPLAY_PRICE_CONFIG config = await WebServiceClient.GetDisplayPriceConfig();
                if (config != null)
                {
                    displayPriceConfig = config;
                }
            }
            catch (Exception ex)
            {
                CustomPopup AlertPopup = new CustomPopup(ex.Message, trueMessage: AppResources.alrt_msg_Ok);
                await PopupNavigation.Instance.PushAsync(AlertPopup);
            }
        }

        public async Task GetPubImages()
        {
            try
            {
                Images = new ObservableCollection<BSE_IMAGE_PUBLICITE>();
                var res = await WebServiceClient.GetPubImages();
                if (res != null)
                {
                    if (res.Count > 0 && Images.Count==0) 
                    {
                        Images = res;
                        foreach(var image in Images)
                        {
                            var x = image.PICTURE;
                        }
                        MessagingCenter.Send(this, "StartSLider", "StartSLider");
                    }
                }
            }
            catch (Exception ex)
            {
                CustomPopup AlertPopup = new CustomPopup(ex.Message, trueMessage: AppResources.alrt_msg_Ok);
                await PopupNavigation.Instance.PushAsync(AlertPopup);
            }
        }

        public async Task GetProductInfoByBareCode()
        {
            try
            {
                if (string.IsNullOrEmpty(bareCode))
                    return;

                var res = await WebServiceClient.GetProductAndPromotionByCodeBarre(BareCode);
                if (res != null)
                {
                    Item = res;
                }
                else
                {
                    DisplayProductError();
                }
                BareCode = "";
            }
            catch (Exception ex)
            {
                DisplayProductError();
                CustomPopup AlertPopup = new CustomPopup(ex.Message, trueMessage: AppResources.alrt_msg_Ok);
                await PopupNavigation.Instance.PushAsync(AlertPopup);
            }
        }
    }
}
