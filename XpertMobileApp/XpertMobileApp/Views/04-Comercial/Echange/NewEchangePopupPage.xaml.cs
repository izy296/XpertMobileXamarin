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
using Xpert.Common.WSClient.Services;
using XpertMobileApp.Api.Managers;
using XpertMobileApp.Api.Services;
using XpertMobileApp.DAL;
using XpertMobileApp.Helpers;
using XpertMobileApp.Models;
using XpertMobileApp.Services;
using XpertMobileApp.SQLite_Managment;
using XpertMobileApp.ViewModels;
using XpertMobileApp.Views.Templates;

namespace XpertMobileApp.Views._04_Comercial.Echange
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class NewEchangePopupPage : PopupPage, INotifyPropertyChanged
    {
        public ObservableCollection<View_TRS_TIERS> ListeTiers { get; set; }
        public ObservableCollection<View_BSE_MAGASIN> ListeMagasin { get; set; }
        public View_BSE_MAGASIN MagasinSelected { get; set; }
        public View_TRS_TIERS TiersSelected { get; set; }
        public DateTime date { get; set; } = DateTime.Now;
        public DateTime dateEcheance { get; set; } = DateTime.Now;
        public decimal solde { get; set; } = 0;
        public decimal TotalHT { get; set; } = 0;
        public decimal TotalHtRemise { get; set; } = 0;
        public decimal TotalTVA { get; set; } = 0;
        public decimal TotalPPA { get; set; } = 0;
        public decimal TotalSHP { get; set; } = 0;
        public decimal TotalTTC { get; set; } = 0;
        public BSE_MODE_REG modeReglement { get; set; }

        EchangeListViewModel viewModel;
        private bool isVisible { get; set; } = false;

        public Command LoadTiersCommand { get; set; }

        public NewEchangePopupPage()
        {
            InitializeComponent();
            ListeTiers = new ObservableCollection<View_TRS_TIERS>();
            ListeMagasin = new ObservableCollection<View_BSE_MAGASIN>();
            viewModel = new EchangeListViewModel();
            GetFournisseur();
            GetMagasin();
            BindingContext = this;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
        }
        private async void ShowHideFormByStep(object sender, EventArgs e)
        {
            if (isVisible)
            {
                isVisible = !isVisible;
                FirstStepLayout.IsVisible = true;
                SecondStepLayout.IsVisible = false;
            }
            else
            {
                if (await CheckFields())
                {
                    isVisible = !isVisible;
                    FirstStepLayout.IsVisible = false;
                    SecondStepLayout.IsVisible = true;
                }

            }
        }

        /// <summary>
        /// Close the popup page
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void btn_Cancel_Clicked(object sender, EventArgs e)
        {
            await PopupNavigation.Instance.PopAsync();
        }


        /// <summary>
        /// Avoir la liste des fournisseurs 
        /// </summary>
        private async void GetFournisseur()
        {
            if (App.Online)
            {
                try
                {
                    UserDialogs.Instance.ShowLoading(AppResources.txt_Waiting);

                    ListeTiers.Clear();

                    var itemsListeTiers = await WebServiceClient.GetListeTiers("F", null);


                    itemsListeTiers.Insert(0, new View_TRS_TIERS()
                    {
                        CODE_TIERS = "",
                        NOM_TIERS = "",
                    });

                    foreach (var itemListeTiers in itemsListeTiers)
                    {
                        ListeTiers.Add(itemListeTiers);
                    }

                    UserDialogs.Instance.HideLoading();

                }
                catch (Exception ex)
                {
                    UserDialogs.Instance.HideLoading();
                    CustomPopup AlertPopup = new CustomPopup(WSApi2.GetExceptionMessage(ex), trueMessage: AppResources.alrt_msg_Ok);
                    await PopupNavigation.Instance.PushAsync(AlertPopup);
                }
            }
        }


        /// <summary>
        /// Avoir la liste des magasins
        /// </summary>
        private async void GetMagasin()
        {
            if (App.Online)
            {
                try
                {
                    UserDialogs.Instance.ShowLoading(AppResources.txt_Waiting);

                    ListeMagasin.Clear();

                    var itemsListeMagasin = await WebServiceClient.GetListeMagasin();


                    itemsListeMagasin.Insert(0, new View_BSE_MAGASIN()
                    {
                        CODE_MAGASIN = "",
                        DESIGN_MAGASIN = ""
                    });

                    foreach (var itemListeMagasin in itemsListeMagasin)
                    {
                        ListeMagasin.Add(itemListeMagasin);
                    }

                    UserDialogs.Instance.HideLoading();
                }
                catch (Exception ex)
                {
                    UserDialogs.Instance.HideLoading();
                    CustomPopup AlertPopup = new CustomPopup(WSApi2.GetExceptionMessage(ex), trueMessage: AppResources.alrt_msg_Ok);
                    await PopupNavigation.Instance.PushAsync(AlertPopup);
                }
            }
        }

        private async Task<bool> CheckFields()
        {
            try
            {
                if (TiersSelected == null)
                {
                    CustomPopup AlertPopup = new CustomPopup("Tiers Manquant", trueMessage: AppResources.alrt_msg_Ok);
                    await PopupNavigation.Instance.PushAsync(AlertPopup);
                    return false;
                }

                if (blFournisseur.Text == null || blFournisseur == null)
                {
                    CustomPopup AlertPopup = new CustomPopup("bl fournisseur manquant", trueMessage: AppResources.alrt_msg_Ok);
                    await PopupNavigation.Instance.PushAsync(AlertPopup);
                    return false;
                }

                if (MagasinSelected == null)
                {
                    CustomPopup AlertPopup = new CustomPopup("Magasin Manquant", trueMessage: AppResources.alrt_msg_Ok);
                    await PopupNavigation.Instance.PushAsync(AlertPopup);
                    return false;
                }

                if (date == null)
                {
                    CustomPopup AlertPopup = new CustomPopup("Date Manquante", trueMessage: AppResources.alrt_msg_Ok);
                    await PopupNavigation.Instance.PushAsync(AlertPopup);
                    return false;
                }
                if (dateEcheance == null)
                {
                    CustomPopup AlertPopup = new CustomPopup("Date Echeance manquante", trueMessage: AppResources.alrt_msg_Ok);
                    await PopupNavigation.Instance.PushAsync(AlertPopup);
                    return false;
                }

                return true;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        private async void btn_Add_Echange(object sender, EventArgs e)
        {
            if (App.Online)
            {
                try
                {
                    if (await CheckFields())
                    {
                        //Collect the data ...
                        View_ACH_DOCUMENT ItemEchange = new View_ACH_DOCUMENT()
                        {
                            CODE_TIERS = TiersSelected.CODE_TIERS,
                            TYPE_DOC = "LF",
                            CODE_MAGASIN = MagasinSelected.CODE_MAGASIN,
                            MODE_REGLEMENT = modeReglement != null ? modeReglement.CODE_MODE : "1",
                            REF_TIERS = blFournisseur.Text,
                            CREATED_ON = date,
                            DATE_DOC = DateTime.Now,
                            CREATED_BY = App.Settings.ClientName,
                            CODE_MOTIF = "ES02",
                            MODE_CAL_MT_ECHANGE = TiersSelected.MODE_CAL_MT_ECHANGE,
                            STATUS_DOC = "12",
                            TOTAL_FOURN_HT = this.TotalHT,
                            TOTAL_FOURN_HT_REMISE = this.TotalHtRemise,
                            TOTAL_FOURN_TVA = this.TotalTVA,
                            TOTAL_FOURN_PPA = this.TotalPPA,
                            TOTAL_FOURN_SHP = this.TotalSHP,
                            TOTAL_FOURN_TTC = this.TotalTTC
                        };

                        // TO-DO ADD SOME CODE TO ADD ECHANGE ...
                        UserDialogs.Instance.ShowLoading(AppResources.txt_Waiting);

                        /*------ INSERT THE ITEM ------*/
                        await CrudManager.Achats.AddItemAsync(ItemEchange);

                        UserDialogs.Instance.HideLoading();

                        await PopupNavigation.Instance.PopAsync();

                        /*----- RELOADING ITEMS TO SEE THE NEW ONE -----*/
                        MessagingCenter.Send(this, MCDico.ITEM_ADDED, "Commande refresh");

                        /*------ SUCCESS MESSAGE ----*/
                        CustomPopup AlertPopup = new CustomPopup("Echange créer avec succées !", trueMessage: AppResources.alrt_msg_Ok);
                        await PopupNavigation.Instance.PushAsync(AlertPopup);
                    };
                }
                catch (Exception ex)
                {
                    UserDialogs.Instance.HideLoading();
                    throw ex;
                }
            }
        }
    }
}

