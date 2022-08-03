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
    public partial class RowElementTemplate_CMD : ContentView
    {
        public RowElementTemplate_CMD()
        {
            InitializeComponent();
        }

        void Handle_ValueChanged(object sender, Syncfusion.SfNumericUpDown.XForms.ValueEventArgs e)
        {
            /*
            if (e.Value.ToString() == "0" || e.Value.ToString() == "0.0")
                appleAddButton.IsEnabled = false;
            else
                appleAddButton.IsEnabled = true;
            this.AppleCost.Text = "$" + Convert.ToDouble(e.Value) * 0.49;
            */
        }
    }
}