using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

using Xamarin.Forms;
using Xamarin.Forms.Internals;
using XpertMobileApp.DAL;
using XpertMobileApp.Api.ViewModels;
using Xpert.Common.DAO;

namespace XpertMobileApp.ViewModels.Feedback
{
    /// <summary>
    /// ViewModel for feedback page.
    /// </summary>
    [Preserve(AllMembers = true)]
    public class FeedbackViewModel : CrudBaseViewModel3<AVIS, View_AVIS>
    {
        #region Constructor

        protected override string ContoleurName
        {
            get
            {
                return "BSE_AVIS";
            }
        }

        public FeedbackViewModel()
        {
            // this.FeedbackInfo = new ObservableCollection<Review>();

            this.FilterCommand = new Command(this.OnFilterTapped);
            this.SortCommand = new Command(this.OnSortTapped);
            this.ItemSelectedCommand = new Command(this.ItemSelected);
        }

        #endregion

        protected override QueryInfos GetFilterParams()
        {
            base.GetFilterParams();

            this.AddOrderBy<View_AVIS, DateTime?>(e => e.CREATED_ON, Sort.DESC);

            return qb.QueryInfos;
        }


        #region Properties

        /// <summary>
        /// Gets or sets the value for feedback info.
        /// </summary>
        //public ObservableCollection<Review> FeedbackInfo { get; set; }

        /// <summary>
        /// Gets or sets the value for filter command.
        /// </summary>
        public Command FilterCommand { get; set; }

        /// <summary>
        /// Gets or sets the value for sort command.
        /// </summary>
        public Command SortCommand { get; set; }

        /// <summary>
        /// Gets or sets the command that will be executed when an item is selected.
        /// </summary>
        public Command ItemSelectedCommand { get; set; }

        #endregion

        #region Methods

        /// <summary>
        /// Invoked when the sort button is clicked.
        /// </summary>
        /// <param name="obj">The Object</param>
        private void OnSortTapped(object obj)
        {
            // Do something
        }

        /// <summary>
        /// Invoked when the filter button is clicked.
        /// </summary>
        /// <param name="obj">The Object</param>
        private void OnFilterTapped(object obj)
        {
            // Do something
        }

        /// <summary>
        /// Invoked when an item is selected.
        /// </summary>
        private void ItemSelected(object selectedItem)
        {
            // Do something
        }

        #endregion
    }
}