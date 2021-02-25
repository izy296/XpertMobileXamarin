using System;
using Rg.Plugins.Popup.Pages;
using Xamarin.Forms;
using Rg.Plugins.Popup.Services;
using XpertMobileApp.ViewModels;
using XpertMobileApp.Helpers;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using XpertMobileApp.Models;
using Acr.UserDialogs;
using Xpert.Common.WSClient.Helpers;
using XpertMobileApp.DAL;

namespace XpertMobileApp.Views
{
    public partial class QteUpdater : PopupPage
    {
        public string CurrentStream;

        QteUpdaterViewModel viewModel;
        
        public event EventHandler<LotInfosEventArgs> LotInfosUpdated;

        protected virtual void OnCBScaned(LotInfosEventArgs e)
        {
            EventHandler<LotInfosEventArgs> handler = LotInfosUpdated;
            if (handler != null)
            {
                handler(viewModel.Item, e);
            }
        }

        public QteUpdater(View_STK_STOCK item)
        {
            InitializeComponent();

            BindingContext = viewModel = new QteUpdaterViewModel(item);
            NUD_Price.Value = item.SelectedPrice;
            NUD_Qte.Value = item.SelectedQUANTITE;
        }

        private async void OnClose(object sender, EventArgs e)
        {
            await PopupNavigation.Instance.PopAsync();
        }

        private async void OnValidate(object sender, EventArgs e)
        {
            try
            {

                LotInfosEventArgs eventArgs = new LotInfosEventArgs();
                eventArgs.Price = Convert.ToDecimal(NUD_Price.Value);
                eventArgs.Quantity = Convert.ToDecimal(NUD_Qte.Value);
                OnCBScaned(eventArgs);
                await PopupNavigation.Instance.PopAsync();
            }
            catch (Exception ex)
            {
                await UserDialogs.Instance.AlertAsync(WSApi2.GetExceptionMessage(ex), AppResources.alrt_msg_Alert,
AppResources.alrt_msg_Ok);
            }
        }
    }
}
