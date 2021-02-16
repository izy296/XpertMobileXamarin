using Acr.UserDialogs;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xpert.Common.DAO;
using Xpert.Common.WSClient.Helpers;
using XpertMobileApp.DAL;
using XpertMobileApp.Models;
using XpertMobileApp.Views;
using XpertMobileApp.Views.XLogin;

namespace XpertMobileApp.Api.ViewModels
{
    public class BaseProdViewModel<T1, TView> : CrudBaseViewModel3<T1, TView>
    where T1 : new()
    where TView : new()
    {
        private int totalOrderedItems = 0;
        private decimal totalPrice = 0;

        #region Fields

        public ObservableCollection<Product> Products { get; set; }

        public ObservableCollection<Product> Products1 { get; set; }

        public ObservableCollection<Product> Orders { get; set; }

        public Command<object> AddCommand { get; set; }

        public Command<object> OrderListCommand { get; set; }

        public Command<object> RemoveOrderCommand { get; set; }

        public Command<object> OpenMenuCommand { get; set; }

        public Command CheckoutCommand { get; set; }

        public int TotalOrderedItems
        {
            get { return totalOrderedItems; }
            set
            {
                if (totalOrderedItems != value)
                {
                    SetProperty(ref totalOrderedItems, value);
                }
            }
        }

        public decimal TotalPrice
        {
            get { return totalPrice; }
            set
            {
                if (totalPrice != value)
                {
                    totalPrice = value;
                    SetProperty(ref totalPrice, value);
                }
            }
        }

        private ProdSortType selectedProdSortType;
        public ProdSortType SelectedProdSortType
        {
            get { return selectedProdSortType; }
            set
            {
                if (selectedProdSortType != value)
                {
                    selectedProdSortType = value;
                    SetProperty(ref selectedProdSortType, value);
                }
            }
        }

        public ObservableCollection<ProdSortType> SortTypes { get; set; }

        internal object Page { get; set; }

        public BaseProdViewModel(object page) : base()
        {
            Page = page;

            Title = AppResources.pn_Catalogues;
            Products = new ObservableCollection<Product>();
            Products1 = new ObservableCollection<Product>();
            Orders = new ObservableCollection<Product>();

            Types = new ObservableCollection<BSE_TABLE_TYPE>();
            Familles = new ObservableCollection<BSE_TABLE>();
            LoadExtrasDataCommand = new Command(async () => await ExecuteLoadExtrasDataCommand());

            AddCommand = new Command<object>(AddQuantity);
            OrderListCommand = new Command<object>(NavigateOrdersPage);
            OpenMenuCommand = new Command<object>(OpenMenu);
            CheckoutCommand = new Command(CheckOut);
            RemoveOrderCommand = new Command<object>(RemoveOrder);

            SortTypes = new ObservableCollection<ProdSortType>()
                  {
                    new ProdSortType
                    {
                        Id = "1",
                        Title = "Designation",
                        Field="DESIGNATION",
                        Direction = Sort.ASC
                    },
                    new ProdSortType
                    {
                        Id = "2",
                        Title = "Prix",
                        Field="PRIX_VENTE",
                        Direction = Sort.ASC
                    },
                    new ProdSortType
                    {
                        Id = "3",
                        Title = "Note",
                        Field="NOTE",
                        Direction = Sort.DESC
                    },
                    new ProdSortType
                    {
                        Id = "4",
                        Title = "Votes",
                        Field="NBR_VOTES",
                        Direction = Sort.DESC
                    },
                };
        }

        #endregion

        #region Méthodes

        internal override Task Reload(object page)
        {
            Items.Clear();
            Products.Clear();
            // Rechargement de la liste des produit dans le panier
            ExecuteLoadPanierCommand(page);

            return base.Reload(page);
        }

        private async void CheckOut(object obj)
        {
            var checkout = await Application.Current.MainPage.DisplayAlert("Confirmation", "Voulez vous vraiment envoyer cette commande ?", "Oui", "non");
            if (checkout)
            {
                try
                {
                    UserDialogs.Instance.ShowLoading(AppResources.txt_Waiting);
                    customer cust = new customer()
                    {
                        address1 = "Dellys"
                    };

                    Order order = new Order()
                    {
                        customer = cust,
                        date = DateTime.Now,
                        total = totalPrice,
                        items = new List<addToCard>()
                    };

                    foreach (var item in Orders)
                    {
                        var itemCard = new addToCard()
                        {
                            CODE_PRODUIT = item.Id,
                            QUANTITE = item.Quantity,
                            PRIX_VENTE = item.Price,
                            ID_USER = App.User.Token.userID,
                            ID_PANIER = BoutiqueManager.PanierElem[0].ID_PANIER,
                        };

                        order.items.Add(itemCard);
                    }

                    var res = await BoutiqueManager.AddOrder(order);
                    if (!string.IsNullOrEmpty(res))
                    {
                        while (Orders.Count > 0)
                        {
                            Orders[Orders.Count - 1].Quantity = 0;
                        }

                        await Application.Current.MainPage.DisplayAlert("", "Votre commande a été envoyée avec succès", "OK");

                        Device.BeginInvokeOnMainThread(() =>
                        {
                            if (obj != null)
                                (obj as ContentPage).Navigation.PopAsync();
                        });
                    }
                    else
                    {
                        await UserDialogs.Instance.AlertAsync("Une erreur inconnu est survenu lors de la création de la commande!", AppResources.alrt_msg_Alert,
                            AppResources.alrt_msg_Ok);
                    }
                    UserDialogs.Instance.HideLoading();
                }
                catch (Exception ex)
                {
                    UserDialogs.Instance.HideLoading();
                    await UserDialogs.Instance.AlertAsync(ex.Message, AppResources.alrt_msg_Alert,
                            AppResources.alrt_msg_Ok);
                }

            }
        }

        private void NavigateLoginPage(object obj)
        {
            if (obj is BaseView)
            {
                var sampleView = obj as BaseView;
                TabbedForm identificationPage = new TabbedForm();
                sampleView.Navigation.PushAsync(identificationPage);
            }
        }

        private void NavigateOrdersPage(object obj)
        {
            if(obj is BaseView) 
            { 
                var sampleView = obj as BaseView;
                var ordersPage = new CartPage();
                ordersPage.BindingContext = this;
                sampleView.Navigation.PushAsync(ordersPage);
            }
        }


        private void RemoveOrder(object obj)
        {
            var p = obj as Product;
            p.Quantity = 0;
        }

        private void OpenMenu(object obj)
        {
            MasterDetailPage RootPage = Application.Current.MainPage as MasterDetailPage;
            RootPage.IsPresented = true;
        }

        private void AddQuantity(object obj)
        {
            var p = obj as Product;
            p.Quantity += 1;

            /*
            Product orderedP = Orders.Where(x => x.Id == p.Id).FirstOrDefault();
            
            if(orderedP != null)
                orderedP.Quantity += 1;
            */
        }
        
        #endregion

        #region filters data

        public string SearchedText { get; set; } = "";

        public bool OnlyNew { get; set; }

        public ObservableCollection<BSE_TABLE_TYPE> Types { get; set; }
        public BSE_TABLE_TYPE SelectedType { get; set; }

        public ObservableCollection<BSE_TABLE> Familles { get; set; }
        public BSE_TABLE SelectedFamille { get; set; }

        async Task ExecuteLoadExtrasDataCommand()
        {

            if (IsLoadExtrasBusy)
                return;

            try
            {
                IsLoadExtrasBusy = true;
                await ExecuteLoadFamillesCommand();
                await ExecuteLoadTypesCommand();
                //await ExecuteLoadPanierCommand();
            }
            catch (Exception ex)
            {
                await UserDialogs.Instance.AlertAsync(WSApi2.GetExceptionMessage(ex), AppResources.alrt_msg_Alert,
                    AppResources.alrt_msg_Ok);
            }
            finally
            {
                IsLoadExtrasBusy = false;
            }
        }

        async Task ExecuteLoadTypesCommand()
        {

            try
            {
                Types.Clear();

                var itemsC = await BoutiqueManager.GetProduitTypes();

                BSE_TABLE_TYPE allElem = new BSE_TABLE_TYPE();
                allElem.CODE_TYPE = "";
                allElem.DESIGNATION_TYPE = AppResources.txt_All;
                itemsC.Add(allElem);

                foreach (var itemC in itemsC)
                {
                    Types.Add(itemC);
                }
            }
            catch (Exception ex)
            {
                await UserDialogs.Instance.AlertAsync(WSApi2.GetExceptionMessage(ex), AppResources.alrt_msg_Alert,
                    AppResources.alrt_msg_Ok);
            }
        }

        async Task ExecuteLoadFamillesCommand()
        {
            try
            {
                Familles.Clear();
                var itemsC = await BoutiqueManager.GetProduitFamilles();

                BSE_TABLE allElem = new BSE_TABLE();
                allElem.CODE = "";
                allElem.DESIGNATION = AppResources.txt_All;
                Familles.Add(allElem);

                foreach (var itemC in itemsC)
                {
                    Familles.Add(itemC);
                }
            }
            catch (Exception ex)
            {
                await UserDialogs.Instance.AlertAsync(WSApi2.GetExceptionMessage(ex), AppResources.alrt_msg_Alert,
                    AppResources.alrt_msg_Ok);
            }
        }

        public async Task ExecuteLoadPanierCommand(object page)
        {
            if (App.User?.Token == null)
                return;
            try
            {
                // UserDialogs.Instance.ShowLoading(AppResources.txt_Waiting);

                Orders.Clear();
                BoutiqueManager.PanierElem = await BoutiqueManager.GetPanier();
                TotalPrice = 0;
                foreach (var item in BoutiqueManager.PanierElem)
                {
                    Product p = new Product()
                    {
                        Id = item.CODE_PRODUIT,
                        Name = item.DESIGNATION,
                        Image = item.IMAGE_URL,
                        IMAGE_URL = item.IMAGE_URL,
                        Price = item.PRIX_VENTE,
                        TotalPrice = item.PRIX_VENTE * item.QUANTITE,
                        Quantity = item.QUANTITE
                    };

                    p.PropertyChanged += (s, e) =>
                    {
                        var product = s as Product;
                        ManagProdPropertyChang(product, e, page);
                    };

                    Orders.Add(p);
                    TotalPrice = TotalPrice + p.TotalPrice;
                    // UserDialogs.Instance.HideLoading();
                }
                TotalOrderedItems = Orders.Count;
            }
            catch (Exception ex)
            {
                // UserDialogs.Instance.HideLoading();
                await UserDialogs.Instance.AlertAsync(WSApi2.GetExceptionMessage(ex), AppResources.alrt_msg_Alert,
                    AppResources.alrt_msg_Ok);
            }
        }

        private decimal totalPrice2 = 0;
        public decimal TotalPrice2
        {
            get { return totalPrice2; }
            set
            {
                if (totalPrice2 != value)
                {
                    totalPrice2 = value;
                    SetProperty(ref totalPrice2, value);
                }
            }
        }

        public void ManagProdPropertyChang(Product product, System.ComponentModel.PropertyChangedEventArgs e, object page) 
        {
            if (e.PropertyName == "Quantity")
            {
                if (Orders.Contains(product) && product.Quantity <= 0)
                {
                    Orders.Remove(product);
                    BoutiqueManager.PanierElem.RemoveAll(x => x.CODE_PRODUIT == product.Id);
                    BoutiqueManager.RemoveCartItem(product.Id);
                }
                else if (Orders.Contains(product) && product.Quantity <= 0)
                {
                    View_PANIER pan = BoutiqueManager.PanierElem.Where(y => y.CODE_PRODUIT == product.Id).FirstOrDefault();
                    if (pan != null)
                    {
                        pan.QUANTITE = product.Quantity;

                        addToCard ci = new addToCard()
                        {
                            CODE_PRODUIT = product.Id,
                            ID_USER = App.User.Token.userID,
                            QUANTITE = product.Quantity,
                        };
                        BoutiqueManager.AddCartItem(ci);
                    }
                }
                else if (!Orders.Contains(product) && product.Quantity > 0)
                {
                    if (!string.IsNullOrEmpty(App.User?.Token?.userID)) 
                    {
                        BoutiqueManager.PanierElem.Add(new View_PANIER()
                        {
                            CODE_PRODUIT = product.Id,
                            DESIGNATION = product.Name,
                            ID_USER = App.User.Token.userID,
                            CODE_DEFAULT_IMAGE = product.CODE_DEFAULT_IMAGE,
                            QUANTITE = product.Quantity
                        });
                        Orders.Add(product);

                        addToCard ci = new addToCard()
                        {
                            CODE_PRODUIT = product.Id,
                            ID_USER = App.User.Token.userID,
                            QUANTITE = product.Quantity,
                        };
                        BoutiqueManager.AddCartItem(ci);
                    }
                    else
                    {
                        product.Quantity = 0;
                        NavigateLoginPage(page);
                    }
                }

                decimal total = 0;
                for (int j = 0; j < Orders.Count; j++)
                {
                    var order = Orders[j];
                    total = total + order.TotalPrice;
                }

                TotalOrderedItems = Orders.Count;
                TotalPrice = total;
            }


        }
        #endregion
    }
}
