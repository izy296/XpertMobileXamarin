using Acr.UserDialogs;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.PancakeView;
using Xamarin.Forms.Xaml;
using Xpert.Common.WSClient.Helpers;
using XpertMobileApp.Services;
using XpertMobileApp.Views._05_Officine.Chifa.Consommation;
using XpertMobileApp.Views._05_Officine.Chifa.FactureCHIFA;
using XpertMobileApp.Views._05_Officine.Chifa.FactureCHIFA;

namespace XpertMobileApp.Views._05_Officine.Chifa
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public class MenuChifaItem : INotifyPropertyChanged
    {
        public int ID { get; set; }
        public string TITLE { get; set; }
        public string BGCOLOR { get; set; }
        public string ICON { get; set; }
        public double nbFACTURE { get; set; }
        public double NbFACTURE
        {
            get
            {
                return nbFACTURE;
            }
            set
            {
                nbFACTURE = value;
                OnPropertyChanged("NbFACTURE");
            }
        }
        public decimal mtTotal { get; set; }
        public decimal MtTotal
        {
            get
            {
                return mtTotal;
            }
            set
            {
                mtTotal = value;
                OnPropertyChanged("MtTotal");
            }
        }
        public bool ShowStats { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            var changed = PropertyChanged;
            if (changed == null)
                return;

            changed.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
    public partial class ChifaMenu : ContentPage
    {
        private int totalFactByDay { get; set; }
        public int TotalFactByDay
        {
            get
            {
                return totalFactByDay;
            }
            set
            {
                totalFactByDay = value;
                OnPropertyChanged("TotalFactByDay");
            }
        }

        private decimal montFactureToday { get; set; }
        public decimal MontFactureToday
        {
            get
            {
                return montFactureToday;
            }
            set
            {
                montFactureToday = value;
                OnPropertyChanged("MontFactureToday");
            }
        }

        private ObservableCollection<MenuChifaItem> menuItems;
        public ObservableCollection<MenuChifaItem> MenuItems
        {
            get
            {
                return menuItems;
            }
            set
            {
                menuItems = value;
                OnPropertyChanged("MenuItems");
            }
        }
        public Command LoadCountTodayFacture { get; set; }
        public ChifaMenu()
        {
            InitializeComponent();
            BindingContext = this;

            /* Initialisation du menu CHIFA*/
            MenuItems = new ObservableCollection<MenuChifaItem>();

            LoadCountTodayFacture = new Command(async () => await ExecuteLoadCountFactureCHIFA());

            /* Ajout des items dans le menu CHIFA*/
            MenuItems.Add(new MenuChifaItem
            {
                ID = 0,
                TITLE = "Bordereau",
                BGCOLOR = "#f0f0f0",
                ShowStats = true
            });

            MenuItems.Add(new MenuChifaItem
            {
                ID = 1,
                TITLE = "Aujourd'hui",
                BGCOLOR = "#f0f0f0",
                ShowStats = true,
                NbFACTURE = TotalFactByDay,
                MtTotal = MontFactureToday

            });

            MenuItems.Add(new MenuChifaItem
            {
                ID = 2,
                TITLE = AppResources.pn_Beneficiaire,
                BGCOLOR = "#f0f0f0",
                ShowStats = false
            });
            MenuItems.Add(new MenuChifaItem
            {
                ID = 3,
                TITLE = "Consommation des Médicament",
                BGCOLOR = "#f0f0f0",
                ShowStats = false
            });

        }

        private void listView_SelectionChanged(object sender, Syncfusion.ListView.XForms.ItemSelectionChangedEventArgs e)
        {

        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            LoadCountTodayFacture.Execute(null);
        }
        async Task ExecuteLoadCountFactureCHIFA()
        {
            try
            {
                var CountFactureForToday = await WebServiceClient.GetTodayCountFacture();
                if (CountFactureForToday != null)
                {
                    TotalFactByDay = CountFactureForToday.TOTAL_NB_FACTURE;
                    MontFactureToday = CountFactureForToday.MONT_FACTURE;
                    var itemToRemove = MenuItems.Single(r => r.ID == 1);
                    MenuItems.Remove(itemToRemove);
                    MenuItems.Insert(1,new MenuChifaItem
                    {
                        ID = 1,
                        TITLE = "Aujourd'hui",
                        BGCOLOR = "#f0f0f0",
                        ShowStats = true,
                        NbFACTURE = TotalFactByDay,
                        MtTotal = MontFactureToday
                    });
                }
            }
            catch (Exception ex)
            {
                await UserDialogs.Instance.AlertAsync(WSApi2.GetExceptionMessage(ex), AppResources.alrt_msg_Alert,
                    AppResources.alrt_msg_Ok);
            }
        }
        private void NavigateFromCHIFAMenu(object sender, EventArgs e)
        {
            try
            {
                int id = ((sender as PancakeView).BindingContext as MenuChifaItem).ID;
                switch (id)
                {
                    case 0:
                        Navigation.PushAsync(new BordereauxChifaPage());
                        break;
                    case 1:
                        Navigation.PushAsync(new RapportJournalierFactureCHIFA());
                        break;
                    case 2:
                        Navigation.PushAsync(new BeneficiaresPage());
                        break;
                    case 3:
                        Navigation.PushAsync(new CHIFA_Consommation());
                        break;
                }

            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
    }
}