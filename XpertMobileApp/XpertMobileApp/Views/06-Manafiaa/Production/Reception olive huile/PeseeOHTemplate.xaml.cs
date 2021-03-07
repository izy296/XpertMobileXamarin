using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace XpertMobileApp.Views.Templates
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class PeseeOHTemplate : ContentView
	{
		public PeseeOHTemplate()
		{
			InitializeComponent ();
 
          //  var lv = this.Parent as ListView;
          //  selectionImage.IsVisible = lv.SelectionMode == ListViewSelectionMode.None;

        }
	}
}