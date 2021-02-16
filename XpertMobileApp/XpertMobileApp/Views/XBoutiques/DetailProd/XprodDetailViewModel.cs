using System.Collections.ObjectModel;
using System.Runtime.Serialization;
using XpertMobileApp.Models;
using Xamarin.Forms;
using Xamarin.Forms.Internals;
using XpertMobileApp.DAL;
using System.Collections.Generic;
using XpertMobileApp.Api.ViewModels;

namespace XpertMobileApp.ViewModels
{
    [Preserve(AllMembers = true)]
    [DataContract]
    class XprodDetailViewModel : BaseProdViewModel<PRODUITS, View_PRODUITS>
    {

        private Product item;
        public Product Item
        {
            get { return item; }
            set { SetProperty(ref item, value); }
        }

        private bool canEval;
        public bool CanEval
        {
            get { return canEval; }
            set { SetProperty(ref canEval, value); }
        }
        
        public XprodDetailViewModel(Product prod, object page) : base(page)
        {
            Title = AppResources.txt_Details;
            Item = prod;
            Page = page;
            addProduct(Item);
        }

        private void addProduct(Product p)
        {
            p.PropertyChanged += (s, e) =>
            {
                var product = s as Product;
                ManagProdPropertyChang(product, e, Page);
            };

            Products.Add(p);
        }
    }
}
