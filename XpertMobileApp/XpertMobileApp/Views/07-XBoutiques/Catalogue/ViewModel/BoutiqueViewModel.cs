using System.Collections.Generic;
using Xamarin.Forms.Internals;
using Xpert.Common.DAO;
using XpertMobileApp.Api.ViewModels;
using XpertMobileApp.DAL;
using XpertMobileApp.Models;

namespace XpertMobileApp.ViewModels
{
    [Preserve(AllMembers = true)]
    public class BoutiqueViewModel : BaseProdViewModel<PRODUITS, View_PRODUITS>
    {

        public BoutiqueViewModel(object page) : base(page)
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
                ImageList = listImgurl,
                Price = item.PRIX_VENTE,
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
            
            if(SelectedProdSortType != null) 
            {
                this.AddOrderBy(SelectedProdSortType.Field, SelectedProdSortType.Direction);
            }
            else
            {
                this.AddOrderBy<View_PRODUITS, string>(e => e.DESIGNATION);
            }
            return qb.QueryInfos;
        }

    }
}
