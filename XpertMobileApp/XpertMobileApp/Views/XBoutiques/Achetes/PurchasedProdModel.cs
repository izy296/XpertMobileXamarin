using Acr.UserDialogs;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Internals;
using Xpert.Common.DAO;
using Xpert.Common.WSClient.Helpers;
using XpertMobileApp.Api;
using XpertMobileApp.Api.ViewModels;
using XpertMobileApp.DAL;
using XpertMobileApp.Models;
using XpertMobileApp.Views;

namespace XpertMobileApp.ViewModels
{
    [Preserve(AllMembers = true)]
    public class PurchasedProdModel : BaseProdViewModel<View_PURCHASED_PROD, View_PURCHASED_PROD>
    {
        protected override string ContoleurName => "PURCHASED_PROD";

        public PurchasedProdModel() : base()
        {
            Title = "Produits achetés";
        }

        private void addProduct(View_PURCHASED_PROD item)
        {
            Product p = new Product()
            {
                Id = item?.CODE_PRODUIT,
                Name = item?.DESIGNATION_PRODUIT,
                Image = item?.IMAGE_URL,
                Price = item.PRIX_VENTE,
                PurchasedQte = item.QTE
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

        protected override void OnAfterLoadItems(IEnumerable<View_PURCHASED_PROD> list)
        {
            base.OnAfterLoadItems(list);

            foreach (var item in list)
            {
                addProduct(item);
            }
        }

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
            this.AddCondition<View_PURCHASED_PROD, string>(e => e.ID_USER, App.User.Token.userID);

            this.AddOrderBy<View_PURCHASED_PROD, decimal>(e => e.QTE, Sort.DESC);
            return qb.QueryInfos;
        }

    }
}
