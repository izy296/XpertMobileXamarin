using Rg.Plugins.Popup.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Xpert.Pharm.DAL;
using XpertMobileApp.Helpers;
using XpertMobileApp.Models;
using XpertMobileApp.Services;
using XpertMobileApp.ViewModels;

namespace XpertMobileApp.Views.Encaissement
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class NewEncaissementPage : ContentPage
	{

        public View_TRS_TIERS SelectedTiers { get; set; }

        public ObservableCollection<BSE_ENCAISS_MOTIFS> Motifs { get; set; }
        private BSE_ENCAISS_MOTIFS selectedMotif;
        public BSE_ENCAISS_MOTIFS SelectedMotif
        {
            get
            {
                return selectedMotif;
            }
            set
            {
                selectedMotif = value;
            }
        }
        public Command LoadIMotifsCommand { get; set; }

        public ObservableCollection<View_BSE_COMPTE> Comptes { get; set; }

        private View_BSE_COMPTE selectedCompte;
        public View_BSE_COMPTE SelectedCompte
        {
            get
            {
                return selectedCompte;
            }
            set
            {
                selectedCompte = value;
            }
        }

        public Command LoadComptesCommand { get; set; }

        public View_TRS_ENCAISS Item { get; set; }

        public NewEncaissementPage (View_TRS_ENCAISS item = null, EncaissDisplayType type = EncaissDisplayType.ENC)
		{
			InitializeComponent ();

            itemSelector = new ItemSelector();

            Motifs  = new ObservableCollection<BSE_ENCAISS_MOTIFS>();
            Comptes = new ObservableCollection<View_BSE_COMPTE>();

            LoadIMotifsCommand = new Command(async () => await ExecuteLoadMotifsCommand(type));
            LoadComptesCommand = new Command(async () => await ExecuteLoadComptesCommand());

            if(item != null)
            {
                Item = item;
            } 
            else
            {
                Item = new View_TRS_ENCAISS
                {
                    DATE_ENCAISS = DateTime.Now,
                    CODE_TYPE = type.ToString()
                };
            }

            BindingContext = this;

            MessagingCenter.Subscribe<ItemSelector, View_TRS_TIERS>(this, MCDico.ITEM_SELECTED, async (obj, selectedItem) =>
            {
                SelectedTiers = selectedItem;

                Item.CODE_TIERS = SelectedTiers.CODE_TIERS;
                TierSolde.Text = string.Format("{0:F2} DA", SelectedTiers.SOLDE_TIERS);
                ent_SelectedTiers.Text = selectedItem.NOM_TIERS1;
            });
        }

        async void Save_Clicked(object sender, EventArgs e)
        {
            if (SelectedTiers == null)
            {
                await DisplayAlert(AppResources.alrt_msg_Alert, AppResources.error_ThirdNotEmpty, AppResources.alrt_msg_Ok);
                return;
            }

            if (SelectedCompte == null)
            {
                await DisplayAlert(AppResources.alrt_msg_Alert, AppResources.error_AccountNotEmpty, AppResources.alrt_msg_Ok);
                return;
            }

            if(string.IsNullOrEmpty(Item.CODE_ENCAISS))
            { 
                MessagingCenter.Send(this, MCDico.ADD_ITEM, Item);
            } else
            {
                MessagingCenter.Send(this, MCDico.UPDATE_ITEM, Item);
            }

            await Navigation.PopModalAsync();
        }

        async void Cancel_Clicked(object sender, EventArgs e)
        {
            await Navigation.PopModalAsync();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            if (Motifs.Count == 0)
                LoadIMotifsCommand.Execute(null);

            if (Comptes.Count == 0)
               LoadComptesCommand.Execute(null);

        }

        async Task ExecuteLoadMotifsCommand(EncaissDisplayType type)
        {
            /*
            if (IsBusy)
                return;
            */

            IsBusy = true;

            try
            {
                Motifs.Clear();

                var itemsM = await WebServiceClient.GetMotifs(type.ToString());
                foreach (var item in itemsM)
                {
                    Motifs.Add(item);
                }

                if(Item != null)
                    SelectMotif(Item.CODE_MOTIF);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
            finally
            {
                IsBusy = false;
            }
        }

        async Task ExecuteLoadComptesCommand()
        {
            /*
            if (IsBusy)
                return;
            */

            IsBusy = true;

            try
            {
                Comptes.Clear();
                var itemsC = await WebServiceClient.getComptes();
                foreach (var itemC in itemsC)
                {
                    Comptes.Add(itemC);
                }

                if(Item != null)
                    SelectCompte(Item.CODE_COMPTE);

            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
            finally
            {
                IsBusy = false;
            }
        }

        private void SelectMotif(string codeElem)
        {
            for (int i = 0; i < Motifs.Count; i++)
            {
                if(Motifs[i].CODE_MOTIF == codeElem)
                {
                    MotifsPicker.SelectedIndex = i;
                    return;
                }
            }
        }

        private void SelectCompte(string codeElem)
        {
            for (int i = 0; i < Comptes.Count; i++)
            {
                if (Comptes[i].CODE_COMPTE == codeElem)
                {
                    ComptesPicker.SelectedIndex = i;
                    return;
                }
            }
        }

        private void MotifPicker_SelectedIndexChanged(object sender, EventArgs e)
        {
            var motif = Motifs[MotifsPicker.SelectedIndex];
            Item.CODE_MOTIF = motif.CODE_MOTIF;
        }

        private void ComptePicker_SelectedIndexChanged(object sender, EventArgs e)
        {
            var compte = Comptes[ComptesPicker.SelectedIndex];
            Item.CODE_COMPTE = compte.CODE_COMPTE;            
        }

        private ItemSelector itemSelector;

        private async void btn_Select_Clicked(object sender, EventArgs e)
        {
            await PopupNavigation.Instance.PushAsync(itemSelector);
        }
    }
}
 