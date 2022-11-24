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
using System.Collections.ObjectModel;
using XpertMobileApp.SQLite_Managment;
using Syncfusion.SfNumericUpDown.XForms;

namespace XpertMobileApp.Views
{
    public partial class QteUpdater : PopupPage
    {
        public string CurrentStream;

        QteUpdaterViewModel viewModel;

        public event EventHandler<LotInfosEventArgs> LotInfosUpdated;
        public ObservableCollection<BSE_TABLE> Prices { get; set; }
        public View_TRS_TIERS Item { get; set; }

        public List<View_BSE_PRODUIT_AUTRE_UNITE> unites;

        public decimal coeficiantUnite = 1;

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
            new Command(async => ExecuteLoadUnite()).Execute(null);
        }

        private async void ExecuteLoadUnite()
        {
            try
            {
                unites = await SQLite_Manager.GetUniteByProduit(viewModel.Item.CODE_PRODUIT) as List<View_BSE_PRODUIT_AUTRE_UNITE>;
                var unitesElements = unites;
                unitesElements.Reverse();
                if (unites.Count > 0)
                {
                    foreach (var unite in unitesElements)
                    {
                        Label uniteLable = new Label();
                        uniteLable.Text = unite.DESIGNATION_UNITE.ToString() + " - x " + unite.COEFFICIENT.ToString();
                        uniteLable.FontSize = 15;
                        SfNumericUpDown qteUnite = new SfNumericUpDown();
                        qteUnite.Minimum = 0;
                        qteUnite.SelectAllOnFocus = true;
                        qteUnite.MaximumDecimalDigits = 2;
                        qteUnite.ParsingMode = ParsingMode.Double;
                        qteUnite.FontSize = 15;
                        qteUnite.TextAlignment = TextAlignment.Center;
                        qteUnite.TextColor = Color.Black;
                        qteUnite.AllowNull = false;
                        qteUnite.VerticalOptions = LayoutOptions.Center;
                        qteUnite.ValueChangeMode = ValueChangeMode.OnKeyFocus;

                        QuantiteUniteLayout.Children.Add(uniteLable);
                        QuantiteUniteLayout.Children.Add(qteUnite);
                    }
                }
            }
            catch (Exception ex)
            {
                await UserDialogs.Instance.AlertAsync(ex.Message, AppResources.alrt_msg_Alert, AppResources.alrt_msg_Ok);
            }

        }


        private async void OnClose(object sender, EventArgs e)
        {
            await PopupNavigation.Instance.PopAsync();
        }

        //private void PricePicker_SelectedIndexChanged(object sender, EventArgs e)
        //{
        //    var itm = Prices[PricePicker.SelectedIndex];
        //    Item.CODE_LIEUX = itm.CODE;
        //}

        private async void OnValidate(object sender, EventArgs e)
        {
            try
            {

                LotInfosEventArgs eventArgs = new LotInfosEventArgs();
                if (Convert.ToDecimal(NUD_Qte.Value) <= viewModel.Item.QUANTITE)
                {
                    decimal qteU = 0;//(Convert.ToDecimal(ButtonQteUnite.Value) * coeficiantUnite);
                    int i = 0;
                    foreach (var element in QuantiteUniteLayout.Children)
                    {
                        if (element.GetType()== typeof(SfNumericUpDown))
                        {
                            var qteUnite = (SfNumericUpDown)element;
                            qteU += Convert.ToDecimal(qteUnite.Value) * unites[i].COEFFICIENT;
                            i++;
                        }
                    }
                    eventArgs.Price = Convert.ToDecimal(NUD_Price.Value);
                    eventArgs.Quantity = Convert.ToDecimal(NUD_Qte.Value) + qteU;
                    OnCBScaned(eventArgs);
                    await PopupNavigation.Instance.PopAsync();
                }
                else
                {
                    await UserDialogs.Instance.AlertAsync(" Quantité stock insuffisante ! \n La quantité stock = " + viewModel.Item.QUANTITE, AppResources.alrt_msg_Alert, AppResources.alrt_msg_Ok);
                }
            }
            catch (Exception ex)
            {
                await UserDialogs.Instance.AlertAsync(WSApi2.GetExceptionMessage(ex), AppResources.alrt_msg_Alert, AppResources.alrt_msg_Ok);
            }
        }
    }
}
