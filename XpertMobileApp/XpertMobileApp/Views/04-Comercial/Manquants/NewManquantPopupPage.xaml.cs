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
using Xpert.Common.DAO;
using Xpert.Common.WSClient.Helpers;
using XpertMobileApp.Api.Managers;
using XpertMobileApp.DAL;
using XpertMobileApp.Helpers;
using XpertMobileApp.Models;
using XpertMobileApp.Services;
using XpertMobileApp.SQLite_Managment;
using XpertMobileApp.ViewModels;
using XpertMobileApp.Views.Templates;

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
        internal XpertSqlBuilder querry = new XpertSqlBuilder();
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
        /// <summary>
        /// Fonction qui permet de selectionner un motif
        /// </summary>
        /// <param name="codeElem"></param>
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
        /// <summary>
        /// Fonction qui indique le changement d'un motif
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MotifPicker_SelectedIndexChanged(object sender, EventArgs e)
        {
            var motif = Motif[MotifPicker.SelectedIndex];
            Item.CODE_TYPE = motif.CODE_STATUS;
        }
        /// <summary>
        /// Fonction qui indique lec changement de checkbox (traité)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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
        /// <summary>
        /// Fonction qui permet de récuperer les motifs en utilisant route api (getManquantsTypes)
        /// </summary>
        /// <returns></returns>
        async Task<List<BSE_DOCUMENT_STATUS>> GetMotif()
        {
            List<BSE_DOCUMENT_STATUS> Motifs = await WebServiceClient.getManquantsTypes();
            return Motifs;
        }
        /// <summary>
        /// Fonction qui permet de récuperer la quantité de produit à partir de son code produit en utilisant route api (GetQteStockByProdeuct)
        /// </summary>
        /// <param name="codeProduit"></param>
        /// <returns></returns>
        async Task<decimal> GetQteProduit(string codeProduit)
        {
            decimal qteProduit = await WebServiceClient.GetQteStockByProdeuct(codeProduit);
            return qteProduit;
        }
        /// <summary>
        /// Fonction qui permet de récuperer la qunatité génitique d'un produit à partir de son reference en utilisant route api 
        /// (GetQteStockByReference)
        /// </summary>
        /// <param name="refProduit"></param>
        /// <returns></returns>
        async Task<decimal> GetQteGenProduit(string refProduit)
        {
            decimal qteGenProduit = await WebServiceClient.GetQteStockByReference(refProduit);
            return qteGenProduit;
        }
        /// <summary>
        /// Fonction qui permet de récuperer les manquants déja inséré à partir de son code produit en utilisant route api
        /// (FindCurrentManquants)
        /// </summary>
        /// <param name="codeProduit"></param>
        /// <returns></returns>
        async Task<List<View_ACH_MANQUANTS>>FindCurrentManquants(string codeProduit)
        {
            List<View_ACH_MANQUANTS> currentManquants = await WebServiceClient.FindCurrent_Non_CF_Manquants(codeProduit);
            return currentManquants;
        }
        /// <summary>
        /// Fonction qui permet de remplir la liste des motifs
        /// </summary>
        /// <returns></returns>
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
                    SelectedMotif = Motif[0]; 
                }
                catch (Exception ex)
                {
                    await UserDialogs.Instance.AlertAsync(WSApi2.GetExceptionMessage(ex), AppResources.alrt_msg_Alert, AppResources.alrt_msg_Ok);
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
                SelectedMotif = Motif[0];
            }
        }
        /// <summary>
        /// Fonction qui permet d'insérer les manquants 
        /// </summary>
        public  async void InsertionManquant()
        {
            Item.CODE_PRODUIT = ent_CodeProduite.Text;
            Item.DESIGNATION_PRODUIT = ent_Filter.Text;
            Item.TYPE_NAME = SelectedMotif.NAME;          
            await CrudManager.Manquant.AddItemAsync(Item);
            await DisplayAlert(AppResources.alrt_msg_title_Manquant, AppResources.alrt_msg_ManquantSaved, AppResources.alrt_msg_Ok);
            await PopupNavigation.Instance.PopAsync();           
            UserDialogs.Instance.HideLoading();
            //MessagingCenter.Send(App.MsgCenter, MCDico.ITEM_ADDED, Item);         
        }  
        /// <summary>
        /// Fonction qui permet de fermer la popup
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        #endregion       
        private async void btn_Cancel_Clicked(object sender, EventArgs e)
        {
            await PopupNavigation.Instance.PopAsync();                       
        }
        /// <summary>
        /// Fonction qui permet d'afficher la liste des produits 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void btn_Search_Clicked(object sender, EventArgs e)
        {
            await PopupNavigation.Instance.PushAsync(itemSelector);
        }
        /// <summary>
        /// Fonction qui permet de valider le formulaire 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void cmd_Valide_Clicked(object sender, EventArgs e)
        {
            try
            {   // Vérification des champs popup est ce que sont null ou nn            
                if (string.IsNullOrEmpty(ent_Filter.Text))
                {
                    await DisplayAlert(AppResources.alrt_msg_Alert, AppResources.alrt_msg_MissingProduit, AppResources.alrt_msg_Ok);
                    return;
                }
                if (string.IsNullOrEmpty(SelectedMotif.NAME))
                {
                    await DisplayAlert(AppResources.alrt_msg_Alert, AppResources.alrt_msg_MissingMotif, AppResources.alrt_msg_Ok);
                    return;
                }
               
                if (!string.IsNullOrEmpty(quantiteProduit.Text))
                {
                    // Conversion de string en décimal en utilisant le TryParse
                    decimal temp ;
                    var test = Decimal.TryParse(quantiteProduit.Text, out temp);
                    if (temp <= 0)
                    {
                        await DisplayAlert(AppResources.alrt_msg_Alert, AppResources.alrt_msg_MissingQuantite, AppResources.alrt_msg_Ok);
                        UserDialogs.Instance.HideLoading();
                        return;
                    }
                    else
                        Item.QUANTITE = temp;
                }
                else
                {
                    await DisplayAlert(AppResources.alrt_msg_Alert, AppResources.alrt_msg_MissingQuantite, AppResources.alrt_msg_Ok);
                    return;
                }
                UserDialogs.Instance.ShowLoading(AppResources.txt_Waiting);
                if (App.Online)
                {
                    // Recupérer les manquants qui sont déja insérer 
                    List<View_ACH_MANQUANTS> currentMnaquant = await FindCurrentManquants(ent_CodeProduite.Text);
                    // Tester si le manquants existe ou nn et afficher une alerte pour insérer le méme manquant ou bien annuler
                    if (currentMnaquant.Count()!=0)
                    {
                        var validation = await DisplayAlert(AppResources.alrt_msg_Alert, AppResources.alrt_msg_ConfirmAjoutManquant, AppResources.alrt_msg_Ok , AppResources.alrt_msg_Cancel);
                        if(validation == false)
                        {
                            UserDialogs.Instance.HideLoading();
                            await PopupNavigation.Instance.PopAsync();
                        }
                        else
                        {
                            InsertionManquant();
                        }                       
                    }
                    else
                    {
                            InsertionManquant();
                    }
                    //await PopupNavigation.Instance.PopAsync();
                    MessagingCenter.Send(this, MCDico.ITEM_ADDED, "Commande refresh");
                }                                                    
            }
            catch (Exception ex)
            {
                UserDialogs.Instance.HideLoading();
                await DisplayAlert(AppResources.alrt_msg_Alert, AppResources.alrt_msg_Erreur_Traitement_Requéte + ex.Message, AppResources.alrt_msg_Ok);
            }
        }
        /// <summary>
        /// Fonction qui permet de récupérer le code produit et la réference d'un produit pour 
        /// l'affichage de la quantité et la quantité génitique d'un produit
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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

