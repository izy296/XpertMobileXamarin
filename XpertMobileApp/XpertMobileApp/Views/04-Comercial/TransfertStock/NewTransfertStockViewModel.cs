using Acr.UserDialogs;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xpert.Common.DAO;
using Xpert.Common.WSClient.Helpers;
using XpertMobileApp.Api;
using XpertMobileApp.Api.ViewModels;
using XpertMobileApp.DAL;
using XpertMobileApp.Models;
using XpertMobileApp.Services;

namespace XpertMobileApp.Views._04_Comercial.TransfertStock
{
    public class NewTransfertStockViewModel : CrudBaseViewModel2<STK_STOCK, View_STK_STOCK>
    {
        public Command LoadMagasinsCommand { get; set; }
        public BSE_MAGASINS SelectedMagasinSource { get; set; }
        public BSE_MAGASINS SelectedMagasinDest { get; set; }
        public ObservableCollection<BSE_MAGASINS> Magasin { get; set; }
        public ObservableCollection<View_STK_STOCK> AllProductInStock { get; set; }
        private int numOfProductSelected { get; set; } = 0;
        public int NumOfProductSelected
        {
            get
            {
                return numOfProductSelected;
            }
            set
            {
                numOfProductSelected = value;
                OnPropertyChanged("NumOfProductSelected");
            }
        }
        private decimal montantTotal { get; set; } = 0;
        public decimal MontantTotal
        {
            get
            {
                return montantTotal;
            }
            set
            {
                montantTotal = value;
                OnPropertyChanged("MontantTotal");
            }
        }


        string currentQB = null;

        public NewTransfertStockViewModel()
        {
            Magasin = new ObservableCollection<BSE_MAGASINS>();
            AllProductInStock = new ObservableCollection<View_STK_STOCK>();
            LoadMagasinsCommand = new Command(async () => await ExecuteLoadMagasinsCommand());
        }

        protected override QueryInfos GetFilterParams()
        {
            base.GetFilterParams();

            if (!string.IsNullOrEmpty(App.Settings.DefaultMagasinVente))
            {
                this.AddCondition<View_STK_STOCK, string>(e => e.CODE_MAGASIN, App.Settings.DefaultMagasinVente);
            }
            this.AddCondition<View_STK_STOCK, bool>(e => e.IS_BLOCKED, 0);
            this.AddCondition<View_STK_STOCK, decimal>(e => e.QUANTITE, Operator.GREATER, 0);

            // this.AddCondition<View_STK_STOCK, bool>(e => e.IS_VALID, 1);

            this.AddOrderBy<View_STK_STOCK, string>(e => e.NUM_DOC, Sort.DESC);

            return qb.QueryInfos;
        }
        protected override void OnAfterLoadItems(IEnumerable<View_STK_STOCK> list)
        {
            base.OnAfterLoadItems(list);

            int i = 0;
            foreach (var item in list)
            {
                i += 1;
                (item as BASE_CLASS).Index = i;
            }
        }

        internal override async Task ExecuteLoadItemsCommand()
        {
            if (IsBusy)
                return;

            try
            {
                IsBusy = true;
                if (currentQB != null && currentQB != GetFilterParams().StringCondition)
                {
                    currentQB = GetFilterParams().StringCondition;
                    Items.Clear();
                }
                else
                {
                    if (Items.Count >= ElementsCount && Items.Count != 0)
                        return;
                    currentQB = GetFilterParams().StringCondition;
                }
                await Items.LoadMoreAsync();
                IsBusy = false;
            }
            catch (Exception ex)
            {
                await UserDialogs.Instance.AlertAsync(WSApi2.GetExceptionMessage(ex), AppResources.alrt_msg_Alert,
                    AppResources.alrt_msg_Ok);
            }
            finally
            {
                IsBusy = false;
            }

        }
        public async Task ExecuteLoadMagasinsCommand()
        {
            try
            {
                Magasin.Clear();
                if (App.Online)
                {

                    var itemsC = await WebServiceClient.GetListeMagasin();
                    BSE_MAGASINS allElem = new BSE_MAGASINS();
                    allElem.CODE = "";
                    allElem.DESIGNATION = AppResources.txt_All;
                    Magasin.Add(allElem);

                    if (Magasin.Count == 1)
                    {
                        foreach (var itemC in itemsC)
                        {
                            Magasin.Add(itemC);
                        }
                    }

                }
            }
            catch (Exception ex)
            {
                await UserDialogs.Instance.AlertAsync(WSApi2.GetExceptionMessage(ex), AppResources.alrt_msg_Alert,
                    AppResources.alrt_msg_Ok);
            }
        }

        public async Task ExecuteLoadStockCommand()
        {
            try
            {
                AllProductInStock.Clear();
                if (App.Online)
                {
                    StockManager sm = new StockManager();
                    var products = await sm.SelectAllByMagasin(App.CODE_MAGASIN);

                    if (AllProductInStock.Count <= 0)
                    {
                        foreach (var product in products)
                        {
                            AllProductInStock.Add(product);
                        }
                    }

                }
            }
            catch (Exception ex)
            {
                await UserDialogs.Instance.AlertAsync(WSApi2.GetExceptionMessage(ex), AppResources.alrt_msg_Alert,
                    AppResources.alrt_msg_Ok);
            }
        }

        public Command PullTORefresh
        {
            get
            {
                return new Command(async () =>
                {
                    if (IsBusy)
                    {
                        return;
                    }
                    currentQB = "Empty";
                    await ExecuteLoadStockCommand();
                    IsBusy = false;
                });
            }
        }
    }
}
