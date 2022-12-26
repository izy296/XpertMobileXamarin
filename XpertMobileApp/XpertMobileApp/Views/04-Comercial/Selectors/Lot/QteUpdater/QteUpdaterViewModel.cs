using System;
using System.Collections.Generic;
using System.Linq;
using Xamarin.Forms;
using Xpert.Common.DAO;
using XpertMobileApp.Api.Services;
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

        public QteUpdaterViewModel(View_STK_PRODUITS item)
        {
            Item = new View_STK_STOCK()
            {
                CODE_PRODUIT = item.CODE_PRODUIT,
                CODE_MAGASIN = App.CODE_MAGASIN,
                COUT_ACHAT = item.PRIX_ACHAT_TTC,
                PPA = item.PPA,
                SHP = item.SHP,
                CODE_BARRE_LOT = item.CODE_BARRE,
                DESIGNATION_PRODUIT = item.DESIGNATION,
                PRIX_VENTE = item.PRIX_VENTE_HT,
                HAS_NEW_ID_STOCK = true,
            };
        }

        public QteUpdaterViewModel(View_VTE_VENTE_LIVRAISON item, string codeFamille,bool replaceID_STOCK=false)
        {
            if (!App.Online)
            {
                new Command(async () =>
                {
                    Item = await SQLite_Manager.GetProduitPrixUniteByCodeProduit(codeFamille, item.CODE_PRODUIT);
                    if (Item != null && replaceID_STOCK)
                    {
                        Item.ID_STOCK = item.CODE_PRODUIT.GetHashCode();
                    }
                }).Execute(null);
            }
        }
    }
}
