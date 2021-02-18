using System.Collections.ObjectModel;
using System.Runtime.Serialization;
using XpertMobileApp.Models;
using Xamarin.Forms;
using Xamarin.Forms.Internals;
using XpertMobileApp.DAL;
using System.Collections.Generic;
using XpertMobileApp.Api.ViewModels;
using System.Threading.Tasks;
using XpertMobileApp.Api;
using XpertMobileApp.Views;
using System;
using Acr.UserDialogs;
using Xpert.Common.WSClient.Helpers;

namespace XpertMobileApp.ViewModels
{
    /// <summary>
    /// ViewModel for home page.
    /// </summary>
    [Preserve(AllMembers = true)]
    [DataContract]
    public class ProductHomePageViewModel : BaseProdViewModel<PRODUITS, View_PRODUITS>
    {
        public ProductHomePageViewModel(object page) : base(page)
        {
            Title = AppResources.pn_home;
        }

        #region Fields

        private ObservableCollection<Product> newArrivalProduts;

        private ObservableCollection<Product> offerProduts;

        private ObservableCollection<Product> recommendedProduts;

        private ObservableCollection<Product> bestEvaluated;

        private ObservableCollection<Product> buestSelled;

        private Command itemSelectedCommand;

        private string bannerImage;

        #endregion

        #region Public properties

        /// <summary>
        /// Gets or sets the property that has been bound with Xamarin.Forms Image, which displays the banner image.
        /// </summary>
        [DataMember(Name = "bannerimage")]
        public string BannerImage
        {
            get { return "http://localhost:8089/header.jpg"; }
            set { this.bannerImage = value; }
        }

        /// <summary>
        /// Gets or sets the property that has been bound with list view, which displays the collection of products from json.
        /// </summary>

        [DataMember(Name = "bestEvaluated")]
        public ObservableCollection<Product> BestEvaluated
        {
            get
            {
                return this.bestEvaluated;
            }

            set
            {
                if (this.bestEvaluated == value)
                {
                    return;
                }

                this.bestEvaluated = value;
                SetProperty(ref bestEvaluated, value);
            }
        }

        [DataMember(Name = "buestSelled")]
        public ObservableCollection<Product> BuestSelled
        {
            get
            {
                return this.buestSelled;
            }

            set
            {
                if (this.buestSelled == value)
                {
                    return;
                }

                this.buestSelled = value;
                SetProperty(ref buestSelled, value);
            }
        }

        [DataMember(Name = "newarrivalproducts")]
        public ObservableCollection<Product> NewArrivalProducts
        {
            get
            {
                return this.newArrivalProduts;
            }

            set
            {
                if (this.newArrivalProduts == value)
                {
                    return;
                }

                this.newArrivalProduts = value;
                SetProperty(ref newArrivalProduts, value);
            }
        }

        /// <summary>
        /// Gets or sets the property that has been bound with list view, which displays the collection of products from json.
        /// </summary>
        [DataMember(Name = "offerproducts")]
        public ObservableCollection<Product> OfferProducts
        {
            get
            {
                return this.offerProduts;
            }

            set
            {
                if (this.offerProduts == value)
                {
                    return;
                }

                this.offerProduts = value;
                SetProperty(ref offerProduts, value);
            }
        }

        /// <summary>
        /// Gets or sets the property that has been bound with list view, which displays the collection of products from json.
        /// </summary>
        [DataMember(Name = "recommendedproducts")]
        public ObservableCollection<Product> RecommendedProducts
        {
            get
            {
                return this.recommendedProduts;
            }

            set
            {
                if (this.recommendedProduts == value)
                {
                    return;
                }

                this.recommendedProduts = value;
                SetProperty(ref recommendedProduts, value);
            }
        }

        #endregion

        #region Command

        /// <summary>
        /// Gets the command that will be executed when an item is selected.
        /// </summary>
        public Command ItemSelectedCommand
        {
            get
            {
                return this.itemSelectedCommand = new Command(async (object obj) => await this.ItemSelected(obj));

              //  return this.itemSelectedCommand ?? (this.itemSelectedCommand = new Command(this.ItemSelected));
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Invoked when an item is selected.
        /// </summary>
        /// <param name="attachedObject">The Object</param>
        private async Task ItemSelected(object attachedObject)
        {

        }


        internal ObservableCollection<Product> GenerateData(List<View_PRODUITS> list) 
        {
            ObservableCollection<Product> res = new ObservableCollection<Product>();
            foreach (var item in list)
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

                res.Add(new Models.Product()
                {
                    Id = item.CODE_PRODUIT,
                    Name = item.DESIGNATION,
                    Image = item.IMAGE_URL,
                    IMAGE_URL = item.IMAGE_URL,
                    CODE_DEFAULT_IMAGE = item.CODE_DEFAULT_IMAGE,
                    ImageList = listImgurl,
                    Price = item.PRIX_VENTE,
                    ReviewValue = item.NOTE,
                    Ratings = item.NBR_VOTES.ToString("") + " Votes",
                    Wished = item.Wished,
                    PurchasedQte = item.QTE_VENDU
                });
            }

            return res;
        }

        private void OpenMenu(object obj)
        {
            MasterDetailPage RootPage = Application.Current.MainPage as MasterDetailPage;
            RootPage.IsPresented = true;
        }
        #endregion
    }
}