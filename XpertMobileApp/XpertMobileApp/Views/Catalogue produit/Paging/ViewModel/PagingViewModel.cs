#region Copyright Syncfusion Inc. 2001-2019.
// Copyright Syncfusion Inc. 2001-2019. All rights reserved.
// Use of this code is subject to the terms of our license.
// A copy of the current license can be obtained at any time by e-mailing
// licensing@syncfusion.com. Any infringement will be prosecuted under
// applicable laws. 
#endregion

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
using XpertMobileApp.DAL;

namespace SampleBrowser.SfListView
{
    [Preserve(AllMembers = true)]
    public class PagingViewModel : INotifyPropertyChanged
    {    
        private PagingProductRepository pagingProductRepository;
    
        public ObservableCollection<View_STK_PRODUITS> pagingProducts { get; set; }

        public PagingViewModel()
        {
            pagingProductRepository = new PagingProductRepository();
            pagingProducts = new ObservableCollection<View_STK_PRODUITS>();
            //GenerateSource();
        }
       

        private void GenerateSource()
        {
            var index = 0;
            Assembly assembly = typeof(Paging).GetTypeInfo().Assembly;
            for (int i = 0; i < pagingProductRepository.Names.Count(); i++)
            {
                if (index == 21)
                    index = 0;

                var name = pagingProductRepository.Names1[index];
                var p = new View_STK_PRODUITS()
                {
                    DESIGNATION = pagingProductRepository.Names[i],
                    DESIGN_FAMILLE = "M�dicaments",
                    PRIX_VENTE_TTC = (decimal)pagingProductRepository.Prices[i],
                    ReviewValue = (decimal)pagingProductRepository.ReviewValue[i],
                    Ratings = pagingProductRepository.Ratings[i],
                    Offer = pagingProductRepository.Offer[i],

                    #if COMMONSB
                        Image = "defaultProdImg.png"
                    #else                
                        IMAGE_URL = "defaultProdImg.png"
                    #endif
                };

                index++;
                pagingProducts.Add(p);
            }
        }

        private void RaisePropertyChanged(string name)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(name));
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
