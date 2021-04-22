using System.Collections.Generic;
using Xamarin.Forms.Internals;
using Xpert.Common.DAO;
using XpertMobileApp.Api.ViewModels;
using XpertMobileApp.DAL;
using XpertMobileApp.Models;

namespace XpertMobileApp.ViewModels
{
    [Preserve(AllMembers = true)]
    public class PurchasedProdModel : BaseProdViewModel<View_PURCHASED_PROD, View_PURCHASED_PROD>
    {
        protected override string ContoleurName => "PURCHASED_PROD";

        public PurchasedProdModel(object page) : base(page)
        {
            Title = "Produits achetés";
        }

        private void addProduct(View_PURCHASED_PROD item)
        {
            List<string> listImgurl = new List<string>();

            // Création des urls des images du produit
            if (item.ImageList != null)
            {
                foreach (var str in item.ImageList)
                {
                    string val = App.RestServiceUrl.Replace("api/", "") + string.Format("Images/GetImage?codeImage={0}", str);
                    listImgurl.Add(val);
                }
            }

            Product p = new Product()
            {
                Id = item?.CODE_PRODUIT,
                Name = item?.DESIGNATION_PRODUIT,
                Image = item?.IMAGE_URL,
                IMAGE_URL = item.IMAGE_URL,
                CODE_DEFAULT_IMAGE = item.CODE_DEFAULT_IMAGE,
                ImageList = listImgurl,
                Price = item.PRIX_VENTE,
                PurchasedQte = item.QTE,

                ReviewValue = item.NOTE,
                Ratings = item.NBR_VOTES.ToString() + " Votes",
                Wished = item.Wished
            };

            p.PropertyChanged += (s, e) =>
            {
                var product = s as Product;
                ManagProdPropertyChang(product, e, Page);
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
            //this.AddCondition<View_PURCHASED_PROD, string>(e => e.ID_USER, App.User.Token.userID);

            this.AddOrderBy<View_PURCHASED_PROD, decimal>(e => e.QTE, Sort.DESC);
            return qb.QueryInfos;
        }

    }
}
