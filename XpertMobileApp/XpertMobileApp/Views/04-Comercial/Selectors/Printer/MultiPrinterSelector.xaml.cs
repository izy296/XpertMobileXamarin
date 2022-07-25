using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XpertMobileApp.Api.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using XpertMobileApp.ViewModels;
using Rg.Plugins.Popup.Pages;
using XpertMobileApp.Helpers;
using Rg.Plugins.Popup.Services;
using XpertMobileApp.Models;
using XpertMobileApp.Views.Templates;
using System.Linq;
using XpertWebApi.Models;

namespace XpertMobileApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MultiPrinterSelector : PopupPage
    {

        MultiPrinterSelectorViewModel viewModel;
        List<string> listToSend = new List<string>();
        List<XPrinter> Printers = new List<XPrinter>();
        string printerName = "Null";
        private TaskCompletionSource<string> taskCompletionSource;
        public Task<string> PopupClosedTask { get { return taskCompletionSource.Task; } }
        public string type;


        public MultiPrinterSelector(List<XPrinter> Printers = null, string type = null)
        {
            InitializeComponent();
            BindingContext = viewModel = new MultiPrinterSelectorViewModel();

            this.Printers = Printers;

            this.type = type;

            ItemListView.ItemTapped += (object sender, ItemTappedEventArgs e) =>
            {
                // don't do anything if we just de-selected the row.
                if (e.Item == null) return;

                // Optionally pause a bit to allow the preselect hint.
                Task.Delay(500);

                // Deselect the item.
                if (sender is ListView lv) lv.SelectedItem = null;
            };

        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            if (type == "ADD")
                Printers.RemoveAt(0);
            viewModel.Items = Printers;
            taskCompletionSource = new TaskCompletionSource<string>();
        }

        private void OnItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            if (e.SelectedItem != null)
            {
                printerName = ((XPrinter)e.SelectedItem).Name;
                taskCompletionSource.SetResult(printerName);
                PopupNavigation.Instance.PopAsync();
            }
        }

        private async void Annuler_Button_Clicked(object sender, EventArgs e)
        {
            taskCompletionSource.SetResult("Null");
            await PopupNavigation.Instance.PopAsync();
        }
    }
}