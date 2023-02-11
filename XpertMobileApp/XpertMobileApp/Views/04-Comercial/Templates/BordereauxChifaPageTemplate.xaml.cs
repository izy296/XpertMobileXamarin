using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using XpertMobileApp.DAL;
using XpertMobileApp.Models;
using XpertMobileApp.Views._05_Officine.Chifa.FactureCHIFA;

namespace XpertMobileApp.Views.Templates
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class BordereauxChifaPageTemplate : ContentView
	{
		public BordereauxChifaPageTemplate()
		{
			InitializeComponent ();
		}

        private async void TapGestureRecognizer_NUM_FACTURE(object sender, EventArgs e)
        {
            var obj = (sender as Label).Text.Split(' ');
            RapportJournalierFactureCHIFAViewModel viewModelFacture = new RapportJournalierFactureCHIFAViewModel();
            viewModelFacture.Items.Add(new View_CFA_MOBILE_FACTURE()
            {
                NUM_FACTURE = obj[0]
            });
            await Navigation.PushAsync(new CHIFA_FactureCarousel(viewModelFacture));

        }

        private async void TapGestureRecognizer_NUM_ASSURE(object sender, EventArgs e)
        {
            var obj = (sender as Label).Text.Split('-');
            View_CFA_MOBILE_DETAIL_FACTURE beneficiare = new View_CFA_MOBILE_DETAIL_FACTURE()
            {
                RAND_AD = obj[1].Replace(" &#xf35a;", ""),
                NUM_ASSURE = obj[0]
            };
            await Navigation.PushAsync(new BeneficiaresDetailPage(beneficiare, false));
        }
    }
}