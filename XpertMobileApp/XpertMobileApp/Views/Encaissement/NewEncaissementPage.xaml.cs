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
using XpertMobileApp.Models;
using XpertMobileApp.Services;

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

        public NewEncaissementPage ()
		{
			InitializeComponent ();
            
            Motifs  = new ObservableCollection<BSE_ENCAISS_MOTIFS>();
            Comptes = new ObservableCollection<View_BSE_COMPTE>();
            Tiers   = new ObservableCollection<View_TRS_TIERS>();

            LoadIMotifsCommand = new Command(async () => await ExecuteLoadMotifsCommand());
            LoadComptesCommand = new Command(async () => await ExecuteLoadComptesCommand());
            LoadTiersCommand   = new Command(async () => await ExecuteLoadTiersCommand());

            Item = new View_TRS_ENCAISS
            {
                DATE_ENCAISS = DateTime.Now,
                CODE_TYPE = "ENC"
            };

            BindingContext = this;
        }

        async void Save_Clicked(object sender, EventArgs e)
        {
            MessagingCenter.Send(this, "AddItem", Item);
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

        async Task ExecuteLoadMotifsCommand()
        {
            /*
            if (IsBusy)
                return;
            */

            IsBusy = true;

            try
            {
                Motifs.Clear();
                var itemsM = await WebServiceClient.GetMotifs();
                foreach (var item in itemsM)
                {
                    Motifs.Add(item);
                }
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
 