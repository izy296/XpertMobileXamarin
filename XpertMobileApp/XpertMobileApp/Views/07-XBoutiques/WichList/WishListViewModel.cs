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
    public class WishListViewModel : BaseProdViewModel<Wish_List, View_WishList>
    {
        protected override string ContoleurName => "WishList";

        public WishListViewModel(object page) : base(page)
        {
            Title = "Wishlist";
        }

        private void addProduct(View_WishList item)
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
                Name = item?.DESIGNATION,
                Image = item?.IMAGE_URL,
                Price = item.PRIX_VENTE,

                IMAGE_URL = item.IMAGE_URL,
                ImageList = listImgurl,

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

        protected override void OnAfterLoadItems(IEnumerable<View_WishList> list)
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
                this.AddCondition<View_WishList, string>(e => e.DESIGNATION, Operator.LIKE_ANY, SearchedText);

            if (!string.IsNullOrEmpty(SelectedFamille?.CODE))
                this.AddCondition<View_WishList, string>(e => e.CODE_FAMILLE, SelectedFamille?.CODE);

            if (!string.IsNullOrEmpty(SelectedType?.CODE_TYPE))
                this.AddCondition<View_WishList, string>(e => e.CODE_TYPE, SelectedType?.CODE_TYPE);

            if (OnlyNew)
                this.AddCondition<View_WishList, bool>(e => e.IS_NEW, "1");

            this.AddCondition<View_WishList, string>(e => e.ID_USER, App.User.Token.userID);

            this.AddOrderBy<View_WishList, string>(e => e.DESIGNATION);

            return qb.QueryInfos;
        }

    }
}
