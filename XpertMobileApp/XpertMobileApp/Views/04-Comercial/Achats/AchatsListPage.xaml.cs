using Plugin.Connectivity;
using Rg.Plugins.Popup.Services;
using System;
using System.Collections.Generic;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using XpertMobileApp.Api;
using XpertMobileApp.DAL;
using XpertMobileApp.Models;
using XpertMobileApp.ViewModels;
using XpertMobileApp.Views.Templates;

namespace XpertMobileApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AchatsListPage : ContentPage
    {
        private string typeDoc = "LF";
        private string motifDoc = AchRecMotifs.PesageReception;
        AchatsListViewModel viewModel;
        public string CurrentStream = Guid.NewGuid().ToString();
        SYS_MOBILE_PARAMETRE parames;
        List<SYS_OBJET_PERMISSION> permissions;

        XpertBaseFilterModel filterObjectTest;

        private bool opened = false; //Concerning the filter 

        public AchatsListPage()
        {
            InitializeComponent();

            itemSelector = new TiersSelector(CurrentStream);

            BindingContext = viewModel = new AchatsListViewModel(typeDoc);

            //if (StatusPicker.ItemsSource != null && StatusPicker.ItemsSource.Count > 0)
            //{
            //    StatusPicker.SelectedItem = StatusPicker.ItemsSource[1];
            //}

            //MessagingCenter.Subscribe<TiersSelector, View_TRS_TIERS>(this, CurrentStream, async (obj, selectedItem) =>
            //{
            //    viewModel.SelectedTiers = selectedItem;
            //    ent_SelectedTiers.Text = selectedItem.NOM_TIERS1;
            //});

            MessagingCenter.Subscribe<BaseFilter, XpertBaseFilterModel>(this, CurrentStream, async (obj, selectedFilter) =>
            {
                filterObjectTest = selectedFilter;
                viewModel.StartDate = filterObjectTest.Filter_Start_Date;
                viewModel.EndDate = filterObjectTest.Filter_End_Date;
                viewModel.RefDocum = filterObjectTest.Filter_GlobalSearch;
                viewModel.SelectedTiers = filterObjectTest.Filter_SelectedTiers;
                btn_ApplyFilter_Clicked(null, null);
            });

        }

        async void OnItemSelected(object sender, SelectedItemChangedEventArgs args)
        {
            var item = args.SelectedItem as View_ACH_DOCUMENT;
            if (item == null)
                return;

            await Navigation.PushAsync(new AchatDetailPage(item));

            // Manually deselect item.
            ItemsListView.SelectedItem = null;
        }

        async void AddItem_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new AchatFormPage(null, typeDoc, motifDoc));
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();
        }
        protected override async void OnAppearing()
        {
            base.OnAppearing();



            var connectivity = CrossConnectivity.Current;
            if (connectivity.IsConnected)
            {
                parames = await AppManager.GetSysParams();
                permissions = await AppManager.GetPermissions();

                LoadStats();

                viewModel.LoadExtrasDataCommand.Execute(null);
            }

            if (!AppManager.HasAdmin)
            {
                ApplyVisibility();
            }
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
            //FilterPanel.IsVisible = !FilterPanel.IsVisible;           


            if (this.BaseFilterView.IsVisible)
                BaseFilterView.Hide();
            else
                this.BaseFilterView.Show(CurrentStream);
        }

        private void btn_ApplyFilter_Clicked(object sender, EventArgs e)
        {
            viewModel.LoadItemsCommand.Execute(null);
        }

        private void btn_CancelFilter_Clicked(object sender, EventArgs e)
        {
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

        private void BaseFilterView_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {

        }

        private void BaseFilterView_BindingContextChanged(object sender, EventArgs e)
        {

        }

        private async void ShowHideFilter(object sender, EventArgs e)
        {
            AchatsPopupFilter filter = new AchatsPopupFilter(viewModel);
            //Load data for the first time ...
            await PopupNavigation.Instance.PushAsync(filter);
        }
    }
}