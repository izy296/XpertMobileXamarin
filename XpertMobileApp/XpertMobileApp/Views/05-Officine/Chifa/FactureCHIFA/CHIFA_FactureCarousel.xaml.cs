using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Extended;
using Xamarin.Forms.Xaml;
using XpertMobileApp.DAL;
using XpertMobileApp.Views._04_Comercial.Templates;

namespace XpertMobileApp.Views._05_Officine.Chifa.FactureCHIFA
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CHIFA_FactureCarousel : CarouselPage
    {
        string title { get; set; }
        public string Title
        {
            get
            {
                return title;
            }
            set
            {
                title = value;
                OnPropertyChanged("Title");
            }
        }
        private int index = 0;

        private int factureCount { get; set; } = 0;
        RapportJournalierFactureCHIFAViewModel viewModelRptJrnl { get; set; }
        public ObservableCollection<View_CFA_MOBILE_FACTURE> ListOfFacturesPage { get; set; }
        public CHIFA_FactureCarousel(RapportJournalierFactureCHIFAViewModel viewModel)
        {
            InitializeComponent();
            viewModelRptJrnl = viewModel;
            ListOfFacturesPage = new ObservableCollection<View_CFA_MOBILE_FACTURE>();
            //Populating the new list to show in the rotator 
            AddChildrenPage(viewModel.Items);
            BindingContext = this;
        }

        public void AddChildrenPage(InfiniteScrollCollection<View_CFA_MOBILE_FACTURE> listeFacture)
        {
            try
            {

                if (listeFacture != null)
                {
                    factureCount = viewModelRptJrnl.Items.Count();

                    if (ListOfFacturesPage.Count() > 0)
                    {
                        for (int i = 0; i < listeFacture.Count; i++)
                        {
                            if (!ListOfFacturesPage.Any(e => e.NUM_FACTURE == listeFacture[i].NUM_FACTURE))
                            {
                                ListOfFacturesPage.Add(listeFacture[i]);
                                Children.Add(new CHIFA_FactureDetailsTemplate(listeFacture[i]));
                            }
                        }
                    }
                    else
                    {
                        for (int i = 0; i < listeFacture.Count; i++)
                        {
                            ListOfFacturesPage.Add(listeFacture[i]);
                            Children.Add(new CHIFA_FactureDetailsTemplate(listeFacture[i]));
                        }
                    }

                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private async void CarouselPage_CurrentPageChanged(object sender, EventArgs e)
        {
            try
            {
                if (Children.IndexOf(CurrentPage) == (factureCount - 1))
                {
                    await viewModelRptJrnl.ExecuteLoadFactureByDateWithoutClear();
                    AddChildrenPage(viewModelRptJrnl.Items);
                }
                this.Title = ListOfFacturesPage[Children.IndexOf(CurrentPage)].NavigationBar_Title;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        private void CarouselPage_PagesChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {

        }
    }
}