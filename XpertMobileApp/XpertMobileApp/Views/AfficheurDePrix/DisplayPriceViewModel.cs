using Acr.UserDialogs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xpert.Common.WSClient.Helpers;
using XpertMobileApp.DAL;
using XpertMobileApp.Models;
using XpertMobileApp.Services;

namespace XpertMobileApp.ViewModels
{
    public class DisplayPriceViewModel : BaseViewModel
    {

        private string bareCode;

        public string BareCode
        {
            get { return bareCode; }
            set
            {
                bareCode = value;
            }
        }
        private List<BSE_IMAGE_PUBLICITE> images;
        public List<BSE_IMAGE_PUBLICITE> Images
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
        }

        public void InitDisplay()
        {
            Item = new INFO_PROMOTION_DISPLAY_PRICE()
            {
                DESIGNATION_PRODUIT = "SCANNEZ UN PRODUIT",
                PRODUIT_RPIX = 0,
                IS_PROMOTION = false
            };
        }

        public void DisplayProductError()
        {
            Item = new INFO_PROMOTION_DISPLAY_PRICE()
            {
                DESIGNATION_PRODUIT = "PRODUIT NON TROUVEE",
                PRODUIT_RPIX = 0,
                IS_PROMOTION = false
            };
        }

        public async Task GetPubImages()
        {
            try
            {

                var res = await WebServiceClient.GetPubImages();
                if (res != null)
                {
                    if (res.Count > 0) 
                    {
                        Images = res;
                        MessagingCenter.Send(this, "StartSLider", "StartSLider");
                    }
                }
            }
            catch (Exception ex)
            {
                await UserDialogs.Instance.AlertAsync(WSApi2.GetExceptionMessage(ex), AppResources.alrt_msg_Alert,
                    AppResources.alrt_msg_Ok);
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
                await UserDialogs.Instance.AlertAsync(WSApi2.GetExceptionMessage(ex), AppResources.alrt_msg_Alert,
                    AppResources.alrt_msg_Ok);
            }
        }
    }
}
