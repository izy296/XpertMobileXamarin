#region Copyright Syncfusion Inc. 2001-2019.
// Copyright Syncfusion Inc. 2001-2019. All rights reserved.
// Use of this code is subject to the terms of our license.
// A copy of the current license can be obtained at any time by e-mailing
// licensing@syncfusion.com. Any infringement will be prosecuted under
// applicable laws. 
#endregion
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Internals;

namespace XpertMobileApp.Models
{
    [Preserve(AllMembers = true)]

    public class Product : INotifyPropertyChanged
    {
        public string Id { get; set; }
        private decimal quantity = 0;
        private decimal totalPrice = 0;

        public string Name { get; set; }

        public ImageSource Image { get; set; }
        public string IMAGE_URL { get; set; }

        public string Weight { get; set; }
        public string Category { get; set; }


        private string description;
        public string Description
        {
            get { return description; }
            set
            {
                if (description != value)
                {
                    description = value;
                    RaisePropertyChanged("Description");
                }
            }
        }

        public List<string> ImageList { get; set; }
        public decimal Price { get; set; }

        public bool IS_NEW { get; set; }

        public decimal OLD_PRICE
        {
            get
            {
                decimal red = (Price * Reduction) / 100;
                return Price + red;
            }
        }

        

        private bool wished;
        public bool Wished
        {
            get { return wished; }
            set
            {
                if (wished != value)
                {
                    wished = value;
                    RaisePropertyChanged("Wished");
                }
            }
        }

        public string Ratings { get; set; } = "1500 Votes";
        public int Reduction { get; set; } = 15;
        public decimal ReviewValue { get; set; } = (decimal)4.5;

        public decimal UserReviewValue { get; set; }

        public decimal Quantity
        {
            get { return quantity; }
            set
            {
                if (quantity != value)
                {
                    quantity = value;
                    TotalPrice = quantity * Price;
                    RaisePropertyChanged("Quantity");
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
                    RaisePropertyChanged("TotalPrice");
                }
            }
        }
        public string CODE_DEFAULT_IMAGE { get; set; }
        private void RaisePropertyChanged(string name)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(name));
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
