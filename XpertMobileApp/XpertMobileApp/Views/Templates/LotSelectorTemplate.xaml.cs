using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using XpertMobileApp.Models;

namespace XpertMobileApp.Views.Templates
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class LotSelectorTemplate : ContentView
	{
		public LotSelectorTemplate()
		{
			InitializeComponent ();
		}

        private void DelQte_Clicked(object sender, EventArgs e)
        {
			var lot = ((sender as Button).BindingContext as View_STK_STOCK);
			lot.SelectedQUANTITE = 0;

		}
    }
}