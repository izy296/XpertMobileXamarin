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
using XpertMobileApp.Views.Helper;

namespace XpertMobileApp.Views
{
    public partial class QteUpdater : PopupPage
    {
        public string CurrentStream;

        QteUpdaterViewModel viewModel;

        public event EventHandler<LotInfosEventArgs> LotInfosUpdated;
        public ObservableCollection<BSE_TABLE> Prices { get; set; }
        public View_TRS_TIERS Item { get; set; }
        public View_VTE_VENTE_LIVRAISON ItemVenteLivaison { get; set; }

        public List<View_BSE_PRODUIT_AUTRE_UNITE> unites;

        public decimal coeficiantUnite = 1;

        private bool retour = false;

        protected virtual void OnCBScaned(LotInfosEventArgs e)
        {
            EventHandler<LotInfosEventArgs> handler = LotInfosUpdated;
            if (handler != null)
            {
                handler(viewModel.Item, e);
            }
        }

        private void CheckIfRetour()
        {
            MessagingCenter.Subscribe<LotSelectorLivraisonUniteFamille, string>(this, "SetRetourTrue", (o, s) =>
            {
                retour = true;
            });
            MessagingCenter.Subscribe<VenteFormLivraisonPage, string>(this, "SetRetourTrue", (o, s) =>
            {
                retour = true;
            });
        }
        private void RemoveCheckIfRetour()
        {
            MessagingCenter.Unsubscribe<LotSelectorLivraisonUniteFamille, string>(this, "SetRetourTrue");
            MessagingCenter.Unsubscribe<VenteFormLivraisonPage, string>(this, "SetRetourTrue");
        }


        public QteUpdater(View_STK_STOCK item)
        {
            InitializeComponent();

            BindingContext = viewModel = new QteUpdaterViewModel(item);
            NUD_Price.Value = item.SelectedPrice;
            NUD_Qte.Value = item.SelectedQUANTITE;
            new Command(async async =>
            {
                ExecuteLoadUnite();
            }
            ).Execute(null);
            CheckIfRetour();
        }

        // constructeur pour passer un element de Type View_VTE_VENTE_LIVRAISON de la page VenteFormLivraisonPage
        public QteUpdater(View_VTE_VENTE_LIVRAISON item, string codeFamille, bool replaceIdStock = false)
        {
            InitializeComponent();

            ItemVenteLivaison = item;

            BindingContext = viewModel = new QteUpdaterViewModel(item, codeFamille, replaceIdStock);

            new Command(async async =>
            {
                UserDialogs.Instance.ShowLoading(AppResources.txt_Waiting);
                while (viewModel.Item == null)
                {
                    await Task.Delay(1000);
                }
                NUD_Price.Value = viewModel.Item.SelectedPrice;
                NUD_Qte.Value = viewModel.Item.SelectedQUANTITE;
                ExecuteLoadUnite();
            }
            ).Execute(null);
            CheckIfRetour();
        }

        private async void ExecuteLoadUnite()
        {
            try
            {
                UserDialogs.Instance.ShowLoading(AppResources.txt_Waiting);
                if (viewModel.Item.UnitesList == null)
                    viewModel.Item.UnitesList = await SQLite_Manager.GetUniteByProduit(viewModel.Item.CODE_PRODUIT) as List<View_BSE_PRODUIT_AUTRE_UNITE>;
                var unitesElements = viewModel.Item.UnitesList;
                unitesElements.Reverse();
                if (viewModel.Item.UnitesList.Count > 0)
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
                        qteUnite.Value = unite.SelectedQUANTITE;

                        QuantiteUniteLayout.Children.Add(uniteLable);
                        QuantiteUniteLayout.Children.Add(qteUnite);
                    }
                }
                UserDialogs.Instance.HideLoading();
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
                if ((Convert.ToDecimal(NUD_Qte.Value) <= viewModel.Item.QUANTITE) || retour)
                {
                    int i = 0;
                    foreach (var element in QuantiteUniteLayout.Children)
                    {
                        if (element.GetType() == typeof(SfNumericUpDown))
                        {
                            var qteUnite = (SfNumericUpDown)element;
                            viewModel.Item.UnitesList[i].SelectedQUANTITE = Convert.ToDecimal(qteUnite.Value);
                            i++;
                        }
                    }
                    eventArgs.Price = Convert.ToDecimal(NUD_Price.Value);
                    eventArgs.Quantity = Convert.ToDecimal(NUD_Qte.Value);
                    OnCBScaned(eventArgs);
                    RemoveCheckIfRetour();
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
