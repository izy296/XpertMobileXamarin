using System;
using System.Collections.Generic;
using System.Linq;
using Xamarin.Forms;
using Xpert.Common.DAO;
using XpertMobileApp.Api.ViewModels;
using XpertMobileApp.DAL;
using XpertMobileApp.Models;
using XpertMobileApp.SQLite_Managment;

namespace XpertMobileApp.ViewModels
{
    public class QteUpdaterViewModel : BaseViewModel
    {
        public View_STK_STOCK Item { set; get; }
        public QteUpdaterViewModel(View_STK_STOCK item)
        {
            Item = item;
        }

        public QteUpdaterViewModel(View_VTE_VENTE_LIVRAISON item,string codeFamille)
        {
            if (!App.Online)
            {
                new Command(async() => {
                    Item = await SQLite_Manager.GetProduitPrixUniteByCodeProduit(codeFamille, item.CODE_PRODUIT);
                }).Execute(null);
            }
        }
    }
}
