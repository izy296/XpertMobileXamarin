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
        public View_BSE_COMPTE SelectedCompte { get; set; }
        public Command LoadComptesCommand { get; set; }

        public ObservableCollection<View_TRS_TIERS> Tiers { get; set; }
        public View_TRS_TIERS SelectedTier { get; set; }
        public Command LoadTiersCommand { get; set; }

        public View_TRS_ENCAISS Item { get; set; }

        public NewEncaissementPage (View_TRS_ENCAISS item = null, EncaissDisplayType type = EncaissDisplayType.ENC)
		{
			InitializeComponent ();
            
            Motifs  = new ObservableCollection<BSE_ENCAISS_MOTIFS>();
            Comptes = new ObservableCollection<View_BSE_COMPTE>();
            Tiers   = new ObservableCollection<View_TRS_TIERS>();

            LoadIMotifsCommand = new Command(async () => await ExecuteLoadMotifsCommand(type));
            LoadComptesCommand = new Command(async () => await ExecuteLoadComptesCommand());
            LoadTiersCommand   = new Command(async () => await ExecuteLoadTiersCommand());

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
        }

        async void Save_Clicked(object sender, EventArgs e)
        {
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

            if (Tiers.Count == 0)
               LoadTiersCommand.Execute(null);

            if (Comptes.Count == 0)
               LoadComptesCommand.Execute(null);

            TiersPicker.SelectedIndexChanged += TiersPicker_SelectedIndexChanged;
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

        async Task ExecuteLoadTiersCommand()
        {
            /*
            if (IsBusy)
                return;
            */
            IsBusy = true;

            try
            {
                 Tiers.Clear();
                 var itemsT = await WebServiceClient.GetTiers("C");
                 foreach (var itemT in itemsT)
                 {
                     Tiers.Add(itemT);
                 }

                 if(Item != null)
                    SelectTier(Item.CODE_TIERS);
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

        private void SelectTier(string codeElem)
        {
            for (int i = 0; i < Tiers.Count; i++)
            {
                if (Tiers[i].CODE_TIERS == codeElem)
                {
                    TiersPicker.SelectedIndex = i;
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

        private void TiersPicker_SelectedIndexChanged(object sender, EventArgs e)
        {
            var tier = Tiers[TiersPicker.SelectedIndex];
            Item.CODE_TIERS = tier.CODE_TIERS;

            TierSolde.Text = string.Format("{0:F2} DA", tier.SOLDE_TIERS);
        }
    }
}
 