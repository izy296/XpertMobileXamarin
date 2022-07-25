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
    public partial class PrinterSelector : PopupPage
    {

        PrinterSelectorViewModel viewModel;
        List<string> listToSend = new List<string>();
        List<XPrinter> Printers = new List<XPrinter>();
        public string type;


        public PrinterSelector(List<XPrinter> Printers = null, string type = null)
        {
            InitializeComponent();
            BindingContext = viewModel = new PrinterSelectorViewModel();

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

            MessagingCenter.Subscribe<PrinterTemplate, string>(this, MCDico.ITEM_SELECTED, async (obj, item) =>
            {
                listToSend.Add(item);
                //await DisplayAlert("hello", listToSend.ToString(), "ok");
            });

            MessagingCenter.Subscribe<PrinterTemplate, string>(this, MCDico.REMOVE_ITEM, async (obj, item) =>
            {
                for (int i = 0; i < listToSend.Count; i++)
                {
                    if (listToSend[i] == item)
                    {
                        listToSend.RemoveAt(i);
                    }
                }
                //await DisplayAlert("hello", listToSend.ToString(), "ok");
            });

        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            if (type=="ADD")
            Printers.RemoveAt(0);
            viewModel.Items = Printers;
            if (type == "LIST")
            {
                MessagingCenter.Send(this, "LIST", "LIST");
                submitBtn.IsVisible = false;
                cancelBtn.Text="Ok";
            }
        }

        private void OnItemSelected(object sender, SelectedItemChangedEventArgs e)
        {

        }

        private async void Button_Clicked(object sender, EventArgs e)
        {
            List<XPrinter> listPRINTER = new List<XPrinter>();
            if (listToSend.Count>0)
            foreach (var printerName in listToSend)
            {
                foreach (var item in viewModel.Items)
                {
                    if (printerName == item.Name)
                        listPRINTER.Add(item);
                }
            }
            if (type != null)
            {
                if (type=="ADD")
                    MessagingCenter.Send(this, MCDico.ITEM_SELECTED, listPRINTER);
                else if (type=="REMOVE")
                    MessagingCenter.Send(this, MCDico.REMOVE_ITEM, listPRINTER);
                else if (type=="LIST")
                    MessagingCenter.Send(this, MCDico.ITEM_SELECTED, new List<XPrinter>());
                else MessagingCenter.Send(this, MCDico.ITEM_SELECTED, listPRINTER);
            }
            else
                MessagingCenter.Send(this, MCDico.ITEM_SELECTED, listPRINTER);
            await PopupNavigation.Instance.PopAsync();
        }

        private async void Annuler_Button_Clicked(object sender, EventArgs e)
        {
            List<XPrinter> listPRINTER = new List<XPrinter>();
            MessagingCenter.Send(this, MCDico.ITEM_SELECTED, listPRINTER);
            await PopupNavigation.Instance.PopAsync();
        }
    }
}