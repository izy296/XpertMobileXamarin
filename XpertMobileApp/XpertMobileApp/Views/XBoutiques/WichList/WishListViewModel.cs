#region Copyright Syncfusion Inc. 2001-2019.
// Copyright Syncfusion Inc. 2001-2019. All rights reserved.
// Use of this code is subject to the terms of our license.
// A copy of the current license can be obtained at any time by e-mailing
// licensing@syncfusion.com. Any infringement will be prosecuted under
// applicable laws. 
#endregion

using Acr.UserDialogs;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using Xamarin.Forms.Internals;
using Xpert.Common.DAO;
using Xpert.Common.WSClient.Helpers;
using XpertMobileApp;
using XpertMobileApp.Api;
using XpertMobileApp.Api.ViewModels;
using XpertMobileApp.DAL;
using XpertMobileApp.Models;
using XpertMobileApp.Services;
using XpertMobileApp.Views;

namespace XpertMobileApp.ViewModels
{
    [Preserve(AllMembers = true)]
    public class WishListViewModel : CrudBaseViewModel3<Wish_List, View_WishList>
    {

        protected override string ContoleurName => "WishList";

        private int totalOrderedItems = 0;
        private decimal totalPrice = 0;

        public ObservableCollection<Product> Products { get; set; }

        public ObservableCollection<Product> Products1 { get; set; }

        public ObservableCollection<Product> Orders { get; set; }

        public Command<object> AddCommand { get; set; }

        public Command<object> OrderListCommand { get; set; }

        public Command<object> RemoveOrderCommand { get; set; }


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

        public WishListViewModel()
        {
            Title = "Wishlist";
            Products = new ObservableCollection<Product>();
            Products1 = new ObservableCollection<Product>();
            Orders = new ObservableCollection<Product>();

            Types = new ObservableCollection<BSE_TABLE_TYPE>();
            Familles = new ObservableCollection<BSE_TABLE>();
            LoadExtrasDataCommand = new Command(async () => await ExecuteLoadExtrasDataCommand());
            /*
            if (Device.Idiom == TargetIdiom.Tablet)
                AddProducts(0, 11);
            else
                AddProducts(0, 6);
            */
            AddCommand = new Command<object>(AddQuantity);
            OrderListCommand = new Command<object>(NavigateOrdersPage);
            CheckoutCommand = new Command(CheckOut);
            RemoveOrderCommand = new Command<object>(RemoveOrder);

        }

        private void RemoveOrder(object obj)
        {
            var p = obj as Product;
            p.Quantity = 0;
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

        private void NavigateOrdersPage(object obj)
        {
            var sampleView = obj as BaseView;
            var ordersPage = new CartPage();
            ordersPage.BindingContext = this;
            sampleView.Navigation.PushAsync(ordersPage);
        }

        private void AddQuantity(object obj)
        {
            var p = obj as Product;
            p.Quantity += 1;
        }

        private void addProduct(View_WishList item)
        {
            Product p = new Product()
            {
                Id = item?.CODE_PRODUIT,
                Name = item?.DESIGNATION,
                Image = item?.IMAGE_URL,
                Price = item.PRIX_VENTE
            };

            p.PropertyChanged += (s, e) =>
            {
                var product = s as Product;
                if (e.PropertyName == "Quantity")
                {
                    if (Orders.Contains(product) && product.Quantity <= 0)
                    {
                        Orders.Remove(product);

                        BoutiqueManager.PanierElem.RemoveAll(x => x.CODE_PRODUIT == product.Id);
                        // BoutiqueManager.RemoveCartItem(item.CODE_PRODUIT);

                    }
                    else if (!Orders.Contains(product) && product.Quantity > 0)
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
                        /*
                        CartItem ci = new CartItem()
                        {
                            CODE_PRODUIT = item.CODE_PRODUIT,
                            ID_USER = App.User.Id,
                            QUANTITE = product.Quantity,
                        };
                        BoutiqueManager.AddCartItem(ci);
                        */
                    }

                    TotalOrderedItems = Orders.Count;
                    TotalPrice = 0;
                    for (int j = 0; j < Orders.Count; j++)
                    {
                        var order = Orders[j];
                        TotalPrice = TotalPrice + order.TotalPrice;
                    }
                }
            };

            Products.Add(p);
        }

        protected override void OnAfterLoadItems(IEnumerable<View_WishList> list)
        {
            base.OnAfterLoadItems(list);

            foreach (var item in list)
            {
                addProduct(item);
            }
        }

        #region filer
        protected override QueryInfos GetFilterParams()
        {
            base.GetFilterParams();
            /*
           if (!string.IsNullOrEmpty(SearchedText))
               this.AddCondition<View_WishList, string>(e => e.DESIGNATION, Operator.LIKE, SearchedText);


           if (!string.IsNullOrEmpty(SelectedFamille?.CODE))
               this.AddCondition<View_STK_PRODUITS, string>(e => e.CODE_FAMILLE, SelectedFamille?.CODE);

           if (!string.IsNullOrEmpty(SelectedType?.CODE_TYPE))
               this.AddCondition<View_STK_PRODUITS, string>(e => e.TYPE_PRODUIT, SelectedType?.CODE_TYPE);

           if (OnlyNew)
               this.AddCondition<View_STK_PRODUITS, bool>(e => e.IS_NEW, "1");

           this.AddCondition<View_STK_PRODUITS, bool>(e => e.SHOW_CATALOG, "1");
           */
            this.AddOrderBy<View_WishList, string>(e => e.CODE_PRODUIT);
            return qb.QueryInfos;
        }
        #endregion

        #region filters data

        public string SearchedText { get; set; } = "";

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
                await ExecuteLoadPanierCommand();
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

        async Task ExecuteLoadPanierCommand()
        {
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
                        CODE_DEFAULT_IMAGE = item.CODE_DEFAULT_IMAGE,
                        Price = item.PRIX_VENTE,
                        TotalPrice = item.PRIX_VENTE * item.QUANTITE,
                        Quantity = item.QUANTITE
                    };

                    Orders.Add(p);
                    TotalPrice = TotalPrice + p.TotalPrice;
                    TotalOrderedItems = Orders.Count;

                }
                // UserDialogs.Instance.HideLoading();
            }
            catch (Exception ex)
            {
                // UserDialogs.Instance.HideLoading();
                await UserDialogs.Instance.AlertAsync(WSApi2.GetExceptionMessage(ex), AppResources.alrt_msg_Alert,
                    AppResources.alrt_msg_Ok);
            }
        }
        #endregion
    }
}
