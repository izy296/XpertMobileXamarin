using Acr.UserDialogs;
using Rg.Plugins.Popup.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using XpertMobileApp.DAL;
using XpertMobileApp.Helpers;
using XpertMobileApp.Models;
using XpertMobileApp.Services;
using XpertMobileApp.SQLite_Managment;
using XpertMobileApp.ViewModels;

namespace XpertMobileApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class NewEncaissementPage : ContentPage
    {

        public View_TRS_TIERS SelectedTiers { get; set; }
        public string CurrentStream = Guid.NewGuid().ToString();
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

        public NewEncaissementPage(View_TRS_ENCAISS item = null, EncaissDisplayType type = EncaissDisplayType.ENC)
        {
            InitializeComponent();

            itemSelector = new TiersSelector(CurrentStream);

            Motifs = new ObservableCollection<BSE_ENCAISS_MOTIFS>();
            Comptes = new ObservableCollection<View_BSE_COMPTE>();

            LoadIMotifsCommand = new Command(async () => await ExecuteLoadMotifsCommand(type));
            LoadComptesCommand = new Command(async () => await ExecuteLoadComptesCommand());

            if (item != null)
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

            MessagingCenter.Subscribe<TiersSelector, View_TRS_TIERS>(this, CurrentStream, async (obj, selectedItem) =>
            {
                SelectedTiers = selectedItem;

                Item.CODE_TIERS = SelectedTiers.CODE_TIERS;
                TierSolde.Text = string.Format("{0:N02} DA", SelectedTiers.SOLDE_TIERS);
                ent_SelectedTiers.Text = selectedItem.NOM_TIERS1;
            });
        }

        async void Save_Clicked(object sender, EventArgs e)
        {
            /*
            if (SelectedTiers == null && Constants.AppName == Apps.XPH_Mob)
            {
                await DisplayAlert(AppResources.alrt_msg_Alert, AppResources.error_ThirdNotEmpty, AppResources.alrt_msg_Ok);
                return;
            }
            */
            if (SelectedCompte == null)
            {
                await DisplayAlert(AppResources.alrt_msg_Alert, AppResources.error_AccountNotEmpty, AppResources.alrt_msg_Ok);
                return;
            }
            if (App.Online)
            {
                if (string.IsNullOrEmpty(Item.CODE_ENCAISS))
                {
                    MessagingCenter.Send(App.MsgCenter, MCDico.ADD_ITEM, Item);
                }
                else
                {
                    MessagingCenter.Send(App.MsgCenter, MCDico.UPDATE_ITEM, Item);
                }
                await Navigation.PopModalAsync();
            }
            else
            {
                if (Item.CODE_TYPE == "ENC")
                {
                    if (Item.TOTAL_ENCAISS <= 0)
                    {
                        await UserDialogs.Instance.AlertAsync("Veuillez verifier le montant!", AppResources.alrt_msg_Alert, AppResources.alrt_msg_Ok);
                    }
                    else
                    {
                        UserDialogs.Instance.ShowLoading(AppResources.txt_Waiting);
                        await UpdateDatabase.AjoutEnciassement(Item);
                        await UserDialogs.Instance.AlertAsync("Encaissement a été effectuée avec succès!", AppResources.alrt_msg_Alert, AppResources.alrt_msg_Ok);
                        await Navigation.PopModalAsync();
                    }
                }
                else
                {
                    if (Item.TOTAL_ENCAISS >= 0)
                    {
                        await UserDialogs.Instance.AlertAsync("Veuillez verifier le montant!", AppResources.alrt_msg_Alert, AppResources.alrt_msg_Ok);
                    }
                    else
                    {
                        UserDialogs.Instance.ShowLoading(AppResources.txt_Waiting);
                        await UpdateDatabase.AjoutEnciassement(Item);
                        await UserDialogs.Instance.AlertAsync("Encaissement a été effectuée avec succès!", AppResources.alrt_msg_Alert, AppResources.alrt_msg_Ok);
                        await Navigation.PopModalAsync();
                    }
                }
            }
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
            if (App.Online)
            {
                try
                {
                    Motifs.Clear();

                    var itemsM = await WebServiceClient.GetMotifs(type.ToString());
                    foreach (var item in itemsM)
                    {
                        Motifs.Add(item);
                    }

                    if (Item != null)
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
            else
            {
                try
                {
                    Motifs.Clear();

                    var itemsM = await SQLite_Manager.getMotifs(type.ToString());
                    foreach (var item in itemsM)
                    {
                        Motifs.Add(item);
                    }

                    if (Item != null)
                    {
                        SelectMotif(Motifs[0].CODE_MOTIF);
                        //SelectMotif(Item.CODE_MOTIF);
                    }
                }
                catch (Exception ex)
                {
                    await UserDialogs.Instance.AlertAsync("veuillez synchroniser svp !!", AppResources.alrt_msg_Alert, AppResources.alrt_msg_Ok);
                }
                finally
                {
                    IsBusy = false;
                }
            }
        }

        async Task ExecuteLoadComptesCommand()
        {
            /*
            if (IsBusy)
                return;
            */

            IsBusy = true;
            if (App.Online)
            {
                try
                {
                    Comptes.Clear();
                    var itemsC = await WebServiceClient.getComptes();
                    foreach (var itemC in itemsC)
                    {
                        Comptes.Add(itemC);
                    }

                    if (Item != null)
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
            else
            {
                try
                {
                    Comptes.Clear();
                    var itemsC = await SQLite_Manager.getComptes();
                    foreach (var itemC in itemsC)
                    {
                        Comptes.Add(itemC);
                    }

                    if (Item != null)
                    {
                        //SelectCompte(Item.CODE_COMPTE);
                        List<SYS_USER> users = await SQLite_Manager.getInstance().Table<SYS_USER>().ToListAsync();
                        var code_compte = users.Where(x => x.ID_USER == App.User.UserName.ToUpper()).FirstOrDefault()?.CODE_COMPTE;
                        if (!(string.IsNullOrEmpty(code_compte)))
                        {
                            SelectCompte(code_compte);
                        }
                    }
                }
                catch (Exception e)
                {
                    await UserDialogs.Instance.AlertAsync("veuillez synchroniser svp !!", AppResources.alrt_msg_Alert, AppResources.alrt_msg_Ok);
                }
                finally
                {
                    IsBusy = false;
                }
            }
        }

        private void SelectMotif(string codeElem)
        {
            for (int i = 0; i < Motifs.Count; i++)
            {
                if (Motifs[i].CODE_MOTIF == codeElem)
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

        private TiersSelector itemSelector;

        private async void btn_Select_Clicked(object sender, EventArgs e)
        {
            await PopupNavigation.Instance.PushAsync(itemSelector);
        }
    }
}
