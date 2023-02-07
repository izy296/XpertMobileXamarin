using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.PancakeView;
using Xamarin.Forms.Xaml;
using XpertMobileApp.Views._05_Officine.Chifa.FactureCHIFA;

namespace XpertMobileApp.Views._05_Officine.Chifa
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public class MenuChifaItem
    {
        public int ID { get; set; }
        public string TITLE { get; set; }
        public string BGCOLOR { get; set; }
        public string ICON { get; set; }
        public double NbFACTURE { get; set; }
        public double MtTotal { get; set; }
        public bool ShowStats { get; set; }
    }
    public partial class ChifaMenu : ContentPage
    {
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

        public ChifaMenu()
        {
            InitializeComponent();
            BindingContext = this;

            /* Initialisation du menu CHIFA*/
            MenuItems = new ObservableCollection<MenuChifaItem>();

            /* Ajout des items dans le menu CHIFA*/
            MenuItems.Add(new MenuChifaItem
            {
                ID = 0,
                TITLE = "Bordereau",
                BGCOLOR = "#f0f0f0",
                ShowStats = true
            }) ;

            MenuItems.Add(new MenuChifaItem
            {
                ID = 1,
                TITLE = "Aujourd'hui",
                BGCOLOR = "#f0f0f0",
                ShowStats = true
            });

            MenuItems.Add(new MenuChifaItem
            {
                ID = 2,
                TITLE = AppResources.pn_Beneficiaire,
                BGCOLOR = "#f0f0f0",
                ShowStats = false
            });

        }

        private void listView_SelectionChanged(object sender, Syncfusion.ListView.XForms.ItemSelectionChangedEventArgs e)
        {

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
                }

            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
    }
}