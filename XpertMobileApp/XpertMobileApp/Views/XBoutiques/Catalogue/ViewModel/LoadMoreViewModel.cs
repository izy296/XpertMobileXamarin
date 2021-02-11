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
    public class LoadMoreViewModel : BaseProdViewModel<PRODUITS, View_PRODUITS>
    {

        public LoadMoreViewModel() : base()
        {
            /*
            if (Device.Idiom == TargetIdiom.Tablet)
                AddProducts(0, 11);
            else
                AddProducts(0, 6);
            */
        }

        private void addProduct(View_PRODUITS item) 
        {
            List<string> listImgurl = new List<string>();

            // Création des urls des images du produit
            if(item.ImageList != null) 
            { 
                foreach (var str in item.ImageList)
                {
                    string val = App.RestServiceUrl.Replace("api/", "") + string.Format("Images/GetImage?codeImage={0}", str);
                    listImgurl.Add(val);
                }
            }

            Product p = new Product()
            {
                Id = item.CODE_PRODUIT,
                Name = item.DESIGNATION,
                Category = item.DESIGNATION_FAMILLE,
                Image = item.IMAGE_URL,
                IMAGE_URL = item.IMAGE_URL,
                CODE_DEFAULT_IMAGE = item.CODE_DEFAULT_IMAGE,
                ImageList = listImgurl,
                Price = item.PRIX_VENTE,
                ReviewValue = item.NOTE,
                Ratings = item.NBR_VOTES.ToString() + " Votes",
                Wished = item.Wished
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
                        BoutiqueManager.RemoveCartItem(item.CODE_PRODUIT);
                    }
                    else if (!Orders.Contains(product) && product.Quantity > 0) 
                    {
                        BoutiqueManager.PanierElem.Add(new View_PANIER()
                        {
                            CODE_PRODUIT = product.Id,
                            DESIGNATION  = product.Name,
                            ID_USER      = App.User.Token.userID,
                            CODE_DEFAULT_IMAGE    = product.CODE_DEFAULT_IMAGE,
                            QUANTITE     = product.Quantity
                        });
                        Orders.Add(product);

                        addToCard ci = new addToCard()
                        {
                            CODE_PRODUIT = item.CODE_PRODUIT,
                            ID_USER = App.User.Token.userID,
                            QUANTITE = product.Quantity,
                        };
                        BoutiqueManager.AddCartItem(ci);
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

        protected override void OnAfterLoadItems(IEnumerable<View_PRODUITS> list)
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

            if (!string.IsNullOrEmpty(SearchedText))
                this.AddCondition<View_PRODUITS, string>(e => e.DESIGNATION, Operator.LIKE_ANY, SearchedText);

            
            if (!string.IsNullOrEmpty(SelectedFamille?.CODE))
                this.AddCondition<View_PRODUITS, string>(e => e.CODE_FAMILLE, SelectedFamille?.CODE);
           
            if (!string.IsNullOrEmpty(SelectedType?.CODE_TYPE))
                this.AddCondition<View_PRODUITS, string>(e => e.CODE_TYPE, SelectedType?.CODE_TYPE);

            if (OnlyNew)
                this.AddCondition<View_STK_PRODUITS, bool>(e => e.IS_NEW, "1");
            /*
           this.AddCondition<View_STK_PRODUITS, bool>(e => e.SHOW_CATALOG, "1");
           */

            this.AddOrderBy<View_PRODUITS, string>(e => e.DESIGNATION);

            return qb.QueryInfos;
        }

    }
}
