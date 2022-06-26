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

namespace XpertMobileApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ProductTagSelector : PopupPage
    {

        ProductTagSelectorViewModel viewModel;
        List<string> listToSend=new List<string>();
        public ProductTagSelector()
        {
            InitializeComponent();
            BindingContext = viewModel = new ProductTagSelectorViewModel();

            ItemListView.ItemTapped += (object sender, ItemTappedEventArgs e) => {
                // don't do anything if we just de-selected the row.
                if (e.Item == null) return;

                // Optionally pause a bit to allow the preselect hint.
                Task.Delay(500);

                // Deselect the item.
                if (sender is ListView lv) lv.SelectedItem = null;
            };

            MessagingCenter.Subscribe<TagTemplate, string>(this, MCDico.ITEM_SELECTED, async (obj, item) =>
            {
                listToSend.Add(item);
                //await DisplayAlert("hello", listToSend.ToString(), "ok");
            });

            MessagingCenter.Subscribe<TagTemplate, string>(this, MCDico.REMOVE_ITEM, async (obj, item) =>
            {
                for (int i=0; i< listToSend.Count; i++)
                {
                    if (listToSend[i]==item)
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

            if (viewModel.Items.Count == 0)
                viewModel.LoadItemsCommand.Execute(null);
        }

        private void OnItemSelected(object sender, SelectedItemChangedEventArgs e)
        {

        }

        private async void Button_Clicked(object sender, EventArgs e)
        {
            List<BSE_PRODUIT_TAG> listTAG = new List<BSE_PRODUIT_TAG>();
            foreach (var item in viewModel.Items)
            {
                if (listToSend.Contains(item.DESIGNATION))
                {
                    listTAG.Add(item);
                }
            }
            MessagingCenter.Send(this, MCDico.ITEM_SELECTED, listTAG);
            await PopupNavigation.Instance.PopAsync();
        }

        private async void Annuler_Button_Clicked(object sender, EventArgs e)
        {
            List<BSE_PRODUIT_TAG> listTAG = new List<BSE_PRODUIT_TAG>();
            MessagingCenter.Send(this, MCDico.ITEM_SELECTED, listTAG);
            await PopupNavigation.Instance.PopAsync();
        }
    }
}