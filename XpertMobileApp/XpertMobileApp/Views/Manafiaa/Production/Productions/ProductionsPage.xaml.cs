using Acr.UserDialogs;
using Rg.Plugins.Popup.Services;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Xpert.Common.WSClient.Helpers;
using XpertMobileApp.Api;
using XpertMobileApp.Api.Managers;
using XpertMobileApp.DAL;
using XpertMobileApp.Helpers;
using XpertMobileApp.Models;
using XpertMobileApp.Services;
using XpertMobileApp.ViewModels;
using XpertMobileApp.Views.Encaissement;
using ZXing.Net.Mobile.Forms;

namespace XpertMobileApp.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class ProductionsPage : ContentPage
	{
        private string typeDoc = "LF";
        public string MotifDoc { get; set; }
        public string CurrentStream = Guid.NewGuid().ToString();
        ProductionsViewModel viewModel;

        SYS_MOBILE_PARAMETRE parames;
        List<SYS_OBJET_PERMISSION> permissions;

        public ProductionsPage(string motifDoc)
		{
			InitializeComponent ();

            MotifDoc = motifDoc;

            itemSelector = new TiersSelector(CurrentStream);

            BindingContext = viewModel = new ProductionsViewModel(typeDoc, motifDoc);

            if (StatusPicker.ItemsSource != null && StatusPicker.ItemsSource.Count > 0)
            {
                StatusPicker.SelectedItem = StatusPicker.ItemsSource[1];
            }

            MessagingCenter.Subscribe<TiersSelector, View_TRS_TIERS>(this, CurrentStream, async (obj, selectedItem) =>
            {
                viewModel.SelectedTiers = selectedItem;
                ent_SelectedTiers.Text = selectedItem.NOM_TIERS1;
            });

            viewModel.SelectedDocs.CollectionChanged += SlectedItempsChanged;
        }


        private void SlectedItempsChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
        }

        async void OnItemSelected(object sender, SelectedItemChangedEventArgs args)
        {
            var item = args.SelectedItem as View_PRD_AGRICULTURE;
            if (item == null)
                return;

            await Navigation.PushAsync(new ProductionFormPage(item, typeDoc, MotifDoc));

            // Manually deselect item.
            ItemsListView.SelectedItem = null;                        
        }

        async void AddItem_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new ProductionFormPage(null, typeDoc, MotifDoc));
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();

            parames = await AppManager.GetSysParams();
            permissions = await AppManager.GetPermissions();

            if (!AppManager.HasAdmin)
            {
                ApplyVisibility();
            }

            //if (viewModel.Items.Count == 0)
            LoadStats();
        }

        private void ApplyVisibility()
        {
            //btn_Additem.IsEnabled = viewModel.hasEditHeader;
        }

        private async void LoadStats()
        {
            viewModel.LoadItemsCommand.Execute(null);
        }

        private void Filter_Clicked(object sender, EventArgs e)
        {
            FilterPanel.IsVisible = !FilterPanel.IsVisible;
        }

        private void btn_ApplyFilter_Clicked(object sender, EventArgs e)
        {        
            viewModel.LoadItemsCommand.Execute(null);
        }

        private void btn_CancelFilter_Clicked(object sender, EventArgs e)
        {
            FilterPanel.IsVisible = false;
            viewModel.LoadItemsCommand.Execute(null);            
        }

        private void ComptePicker_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private TiersSelector itemSelector;
        private async void btn_Select_Clicked(object sender, EventArgs e)
        {
            itemSelector.SearchedType = "CF";
            await PopupNavigation.Instance.PushAsync(itemSelector);
        }

        private async void btn_VeridUser_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new VerificationUserPage(""));
        }

        private async void btn_Scaner_Clicked(object sender, EventArgs e)
        {
            var scaner = new ZXingScannerPage();
            await Navigation.PushAsync(scaner);
            scaner.OnScanResult += (result) =>
            {
                scaner.IsScanning = false;
                Device.BeginInvokeOnMainThread(async () =>
                {
                    await Navigation.PopAsync();

                    await OpenFormAsync(result.Text);
                });
            };
        }

        private async System.Threading.Tasks.Task OpenFormAsync(string text)
        {
            try
            {
                string[] str = text.Split('-');
                if (str.Count() > 0)
                {
                    string CodeTiers = str[1];
                    string CodeDoc = str[0];

                     var elem = await CrudManager.ProductionInfosManager.GetProductionFromCodeReception(CodeDoc);
                    
                     if (elem != null)
                     {
                        await Navigation.PushAsync(new ProductionFormPage(elem, typeDoc, MotifDoc));
                     }
                     else
                     {
                        await UserDialogs.Instance.AlertAsync("Aucun document de production trouvé pour cette réception!", AppResources.alrt_msg_Alert,
            AppResources.alrt_msg_Ok);
                    }
                   
                }
            }
            catch (Exception ex)
            {
                await UserDialogs.Instance.AlertAsync(WSApi2.GetExceptionMessage(ex), AppResources.alrt_msg_Alert,
    AppResources.alrt_msg_Ok);
            }
        }
    }
}