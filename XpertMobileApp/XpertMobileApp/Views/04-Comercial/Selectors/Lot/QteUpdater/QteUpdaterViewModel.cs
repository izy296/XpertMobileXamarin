using System;
using System.Collections.Generic;
using System.Linq;
using Xpert.Common.DAO;
using XpertMobileApp.Api.ViewModels;
using XpertMobileApp.Models;

namespace XpertMobileApp.ViewModels
{
    public class QteUpdaterViewModel : BaseViewModel
    {
        public View_STK_STOCK Item { set; get; }
        public QteUpdaterViewModel(View_STK_STOCK item)
        {
            Item = item;
        }
    }
}
