using System;
using Rg.Plugins.Popup.Pages;
using Xamarin.Forms;
using Rg.Plugins.Popup.Services;
using XpertMobileApp.ViewModels;
using XpertMobileApp.Helpers;
using System.Linq;
using System.Collections.Generic;
using XpertMobileApp.Models;

namespace XpertMobileApp.Views
{
    public partial class AnnexTiersSelector : PopupPage
    {

        public string CODE_DOC { get; set; }
        public decimal PrixPrestation { get; set; }
        public decimal TotalQteProduite
        {
            get;
            set;
        }

        AnnexTiersSelectorViewModel viewModel;

        public bool IS_ACHAT
        {
            get
            {
                return viewModel.IS_ACHAT;
            }
            set
            {
                viewModel.IS_ACHAT = value;
            }
        }

        public List<VIEW_ACH_INFO_ANEX> CurrentAnnex
        {
            get
            {
                return viewModel.CurrentAnnex;
            }
            set
            {
                viewModel.CurrentAnnex = value;
                UpdateQte();
            }
        }

        public AnnexTiersSelector()
        {
            InitializeComponent();

            BindingContext = viewModel = new AnnexTiersSelectorViewModel();

           // viewModel.LoadItemsCommand.Execute(null);
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            /*
            if(viewModel.Items.Count == 0 && this.IS_ACHAT == false)
            {
                viewModel.LoadItemsCommand.Execute(null);
            }
            */
        }

        private async void OnClose(object sender, EventArgs e)
        {

            MessagingCenter.Send(this, MCDico.ITEM_SELECTED, viewModel.Items.ToList());
            
            await PopupNavigation.Instance.PopAsync();
        }

        private void ItemsListView_SelectionChanged(object sender, Syncfusion.ListView.XForms.ItemSelectionChangedEventArgs e)
        {
            btnSelect.IsEnabled = true;
        }

        private void btn_ApplyFilter_Clicked(object sender, EventArgs e)
        {
            if(viewModel.Items.Where(x=>x.NOM_TIERS == TiersName.Text).Count() == 0 
                && !string.IsNullOrEmpty(TiersQte.Text))
            {
                viewModel.Items.Add(new VIEW_ACH_INFO_ANEX()
                {
                    NOM_TIERS = TiersName.Text,
                    CODE_DOC = CODE_DOC,
                    PRIX_PRODUIT = PrixPrestation,
                    QUANTITE_APPORT = Convert.ToDecimal(TiersQte.Text.Trim())
                });

                TiersQte.Text = "";
                TiersName.Text = "";

                UpdateQte();
            }
        }

        private void Btn_Delete_Clicked(object sender, EventArgs e)
        {
            var elem = (sender as Button).BindingContext as VIEW_ACH_INFO_ANEX;
            viewModel.Items.Remove(elem);
            UpdateQte();
        }

        private void UpdateQte()
        {
            decimal qteTotalApport = viewModel.Items.Sum(x => x.QUANTITE_APPORT);
            lbl_TotalQte.Text = qteTotalApport.ToString("N2") + " Kg";

            if(TotalQteProduite > 0)
            { 
                foreach (var item in CurrentAnnex)
                {
                        var qtePercent = (item.QUANTITE_APPORT * 100) / qteTotalApport;
                        item.QUANTITE_PRODUITE = (qtePercent * TotalQteProduite) / 100;
                }
            }
        }
    }
}
