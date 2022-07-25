using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using XpertMobileApp.Helpers;

namespace XpertMobileApp.Views.Templates
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PrinterTemplate : ContentView
    {

        public PrinterTemplate()
        {
            InitializeComponent();
            MessagingCenter.Subscribe<PrinterSelector, string>(this, "LIST", (o, e) =>
            {
                checkBox.IsVisible = false;
            });
        }

        private void CheckBox_StateChanged(object sender, Syncfusion.XForms.Buttons.StateChangedEventArgs e)
        {
            if ((bool)e.IsChecked)
            {
                MessagingCenter.Send(this, MCDico.ITEM_SELECTED, checkBox.Text);
            }
            else
            {
                MessagingCenter.Send(this, MCDico.REMOVE_ITEM, checkBox.Text);
            }
        }
    }
}