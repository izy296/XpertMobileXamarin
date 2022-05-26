using Acr.UserDialogs;
using Rg.Plugins.Popup.Pages;
using Rg.Plugins.Popup.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Xpert.Common.WSClient.Helpers;
using XpertMobileApp.Api.Managers;
using XpertMobileApp.DAL;
using XpertMobileApp.Helpers;
using XpertMobileApp.Models;
using XpertMobileApp.Services;
using XpertMobileApp.SQLite_Managment;
using XpertMobileApp.ViewModels;

namespace XpertMobileApp.Views._04_Comercial.Manquants
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class NewManquantPopupPage : PopupPage, INotifyPropertyChanged
    {
        public Command LoadMotiftCommand { get; set; }
        public string CurrentStream = Guid.NewGuid().ToString();
        public View_ACH_MANQUANTS Item { get; set; }
        private ProductSelectorManquant itemSelector;
        private ManquantsViewModel viewModel;
        public NewManquantPopupPage(View_ACH_MANQUANTS item = null)
        {
            InitializeComponent();            
            itemSelector = new ProductSelectorManquant(CurrentStream);
            BindingContext = viewModel = new ManquantsViewModel();
            Motif = new ObservableCollection<BSE_DOCUMENT_STATUS>();
            LoadMotiftCommand = new Command(async () => await ExecuteLoadMotifCommand());
            MessagingCenter.Subscribe<ProductSelectorManquant, View_STK_PRODUITS>(this, CurrentStream, async (obj, selectedItem) =>
            {
                viewModel.SearchedText = selectedItem;
                ent_Filter.Text = selectedItem.DESIGNATION_PRODUIT;
                ent_CodeProduite.Text = selectedItem.CODE_PRODUIT;
                ent_refProduite.Text = selectedItem.REFERENCE;
            });          
            if (item != null)
            {
                Item = item;
            }
            else
            {
                Item = new View_ACH_MANQUANTS();                
            }
            BindingContext = this;
        }
        protected override void OnAppearing()
        {
            base.OnAppearing();
            if (Motif.Count == 0)
                LoadMotiftCommand.Execute(null);
        }        
        private void SelectMotif(string codeElem)
        {
            for (int i = 0; i < Motif.Count; i++)
            {
                if (Motif[i].CODE_STATUS == codeElem)
                {
                    MotifPicker.SelectedIndex = i;
                    return;
                }
            }
        }
        private void MotifPicker_SelectedIndexChanged(object sender, EventArgs e)
        {
            var motif = Motif[MotifPicker.SelectedIndex];
            Item.CODE_TYPE = motif.CODE_STATUS;
        }
        private void OnCheckBoxCheckedChanged(object sender, CheckedChangedEventArgs e)
        {
            if (sender is CheckBox)
            {
                var checkbox = (CheckBox)sender;
                if (checkbox.IsChecked)
                {
                    Item.TREATED = true;
                }
                else
                {
                    Item.TREATED = false;
                }
            }
        }

        #region filtredata        
        public ObservableCollection<BSE_DOCUMENT_STATUS> Motif { get; set; }
        public BSE_DOCUMENT_STATUS SelectedMotif { get; set; }
        async Task ExecuteLoadExtrasDataCommand()
        {
            try
            {
                await ExecuteLoadMotifCommand();
            }
            catch (Exception ex)
            {
                await UserDialogs.Instance.AlertAsync(WSApi2.GetExceptionMessage(ex), AppResources.alrt_msg_Alert,
                    AppResources.alrt_msg_Ok);
            }
            finally
            {

            }
        }
        async Task<List<BSE_DOCUMENT_STATUS>> GetMotif()
        {
            List<BSE_DOCUMENT_STATUS> Motifs = await WebServiceClient.getManquantsTypes();
            return Motifs;
        }
        async Task<decimal> GetQteProduit(string codeProduit)
        {
            decimal qteProduit = await WebServiceClient.GetQteStockByProdeuct(codeProduit);
            return qteProduit;
        }
        async Task<decimal> GetQteGenProduit(string refProduit)
        {
            decimal qteGenProduit = await WebServiceClient.GetQteStockByReference(refProduit);
            return qteGenProduit;
        }
        async Task<List<BSE_DOCUMENT_STATUS>> GetMotifs()
        {
            List<BSE_DOCUMENT_STATUS> listMotif = await GetMotif();
            List<BSE_DOCUMENT_STATUS> listAllElem = new List<BSE_DOCUMENT_STATUS>();
            BSE_DOCUMENT_STATUS allElem;
            allElem = new BSE_DOCUMENT_STATUS();
            allElem.CODE_STATUS = "";
            allElem.NAME = "";
            listAllElem.Add(allElem);
            foreach (BSE_DOCUMENT_STATUS item in listMotif)
            {
                allElem = new BSE_DOCUMENT_STATUS();
                allElem.CODE_STATUS = item.CODE_STATUS;
                allElem.NAME = item.NAME;
                listAllElem.Add(allElem);
            }
            return listAllElem;
        }
        async Task ExecuteLoadMotifCommand()
        {
            if (App.Online)
            {
                try
                {
                    Motif.Clear();
                    var itemsC = await GetMotifs();
                    foreach (var itemC in itemsC)
                    {
                        Motif.Add(itemC);
                    }
                }
                catch (Exception ex)
                {
                    await UserDialogs.Instance.AlertAsync(WSApi2.GetExceptionMessage(ex), AppResources.alrt_msg_Alert,
                        AppResources.alrt_msg_Ok);
                }
            }
            else
            {
                Motif.Clear();
                var itemsC = await GetMotifs();
                foreach (var itemC in itemsC)
                {
                    Motif.Add(itemC);
                }
            }
        }

        #endregion       
        private async void btn_Cancel_Clicked(object sender, EventArgs e)
        {
            await PopupNavigation.Instance.PopAsync();
        }
        private async void btn_Search_Clicked(object sender, EventArgs e)
        {
            await PopupNavigation.Instance.PushAsync(itemSelector);
        }
        private async void cmd_Valide_Clicked(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(ent_Filter.Text))
                {
                    await DisplayAlert(AppResources.alrt_msg_Alert, AppResources.alrt_msg_MissingProduit, AppResources.alrt_msg_Ok);
                    return;
                }
                if (SelectedMotif == null)
                {
                    await DisplayAlert(AppResources.alrt_msg_Alert, AppResources.alrt_msg_MissingMotif, AppResources.alrt_msg_Ok);
                    return;
                }
                UserDialogs.Instance.ShowLoading(AppResources.txt_Waiting);
                if (App.Online)
                {
                    if (string.IsNullOrEmpty(Item.CODE_PRODUIT))
                    {
                        Item.CODE_PRODUIT = ent_CodeProduite.Text;
                        Item.DESIGNATION_PRODUIT = ent_Filter.Text;
                        Item.TYPE_NAME = SelectedMotif.NAME;
                        decimal temp;
                        var test = Decimal.TryParse(quantiteProduit.Text,out temp);
                        if (test)
                        {
                            if (temp==0)
                            {
                                await DisplayAlert(AppResources.alrt_msg_Alert, AppResources.alrt_msg_MissingQuantite, AppResources.alrt_msg_Ok);
                                return;
                            }
                            else
                            Item.QUANTITE = temp;
                        }
                        string codeProduit = await CrudManager.Manquant.AddItemAsync(Item);
                        await DisplayAlert(AppResources.alrt_msg_title_Manquant, AppResources.alrt_msg_ManquantSaved, AppResources.alrt_msg_Ok);
                        UserDialogs.Instance.HideLoading();
                        await PopupNavigation.PopAsync();
                        MessagingCenter.Send(App.MsgCenter, MCDico.ITEM_ADDED, Item);
                    }
                    else
                    {
                        await CrudManager.Manquant.UpdateItemAsync(Item);
                        MessagingCenter.Send(App.MsgCenter, MCDico.ITEM_ADDED, Item);
                    }                   
                }
            }
            catch (Exception ex)
            {
                UserDialogs.Instance.HideLoading();
                await DisplayAlert(AppResources.alrt_msg_Alert, AppResources.alrt_msg_Erreur_Traitement_Requéte + ex.Message, AppResources.alrt_msg_Ok);
            }
        }
        private async void ent_CodeProduite_TextChanged(object sender, TextChangedEventArgs e)
        {
            decimal QteProduit = await GetQteProduit(ent_CodeProduite.Text);
            quantite.Text = AppResources.txt_Qte+" : "+ QteProduit;            
            if (string.IsNullOrEmpty(ent_refProduite.Text))
            {
                quantiteGenerique.Text = AppResources.txt_QteGenerique + " : 0";
            }
            else
            {
                decimal QteRefProduit = await GetQteGenProduit(ent_refProduite.Text);
                quantiteGenerique.Text = AppResources.txt_QteGenerique + " : " + QteRefProduit;
            }                       
        }      
    }
}

