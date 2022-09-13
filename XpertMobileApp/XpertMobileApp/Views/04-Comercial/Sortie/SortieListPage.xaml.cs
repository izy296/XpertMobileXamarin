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
    public partial class SortieListPage : ContentPage
    {
        private string typeDoc = "SS";
        SortieListViewModel viewModel;
        public string CurrentStream = Guid.NewGuid().ToString();
        SYS_MOBILE_PARAMETRE parames;
        List<SYS_OBJET_PERMISSION> permissions;
        private UserSelector itemSelector;
        XpertBaseFilterModel filterObjectTest;
        private bool opened = false;

        public SortieListPage()
        {
            InitializeComponent();

            itemSelector = new UserSelector(CurrentStream);

            BindingContext = viewModel = new SortieListViewModel(typeDoc);

            //Code responsabel a la selectore de Motifs

            MessagingCenter.Subscribe<BaseFilter, XpertBaseFilterModel>(this, CurrentStream, async (obj, selectedFilter) =>
            {
                filterObjectTest = selectedFilter;
                viewModel.StartDate = filterObjectTest.Filter_Start_Date;
                viewModel.EndDate = filterObjectTest.Filter_End_Date;
                viewModel.RefDocum = filterObjectTest.Filter_GlobalSearch;
                //viewModel.SelectedTiers = filterObjectTest.Filter_SelectedTiers;
                btn_ApplyFilter_Clicked(null, null);
            });
        }

        async void OnItemSelected(object sender, SelectedItemChangedEventArgs args)
        {
            var item = args.SelectedItem as View_STK_SORTIE;
            if (item == null)
                return;

            await Navigation.PushAsync(new SortieDetailPage(item));

            // Manually deselect item.
            ItemsListView.SelectedItem = null;
        }

        /*        async void AddItem_Clicked(object sender, EventArgs e)
                {
                    await Navigation.PushAsync(new AchatFormPage(null, typeDoc, motifDoc));
                }*/

        protected override async void OnAppearing()
        {
            base.OnAppearing();

            var connectivity = CrossConnectivity.Current;
            if (connectivity.IsConnected)
            {
                parames = await AppManager.GetSysParams();
                permissions = await AppManager.GetPermissions();

                //LoadStats();
                if (viewModel.Motif.Count == 0 && viewModel.Status.Count == 0)
                {
                    LoadStats();
                    viewModel.LoadExtrasDataCommand.Execute(null);
                }
            }

            if (!AppManager.HasAdmin)
            {
                ApplyVisibility();
            }
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();
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


            /*            if (this.BaseFilterView.IsVisible)
                            BaseFilterView.Hide();
                        else
                            this.BaseFilterView.Show(CurrentStream);*/
        }

        private void btn_ApplyFilter_Clicked(object sender, EventArgs e)
        {
            viewModel.LoadItemsCommand.Execute(null);
        }

        private void ComptePicker_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private async void btn_Select_Clicked(object sender, EventArgs e)
        {
            itemSelector.SearchedType = "";

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
            SortiePopupFilter filter = new SortiePopupFilter(viewModel);
            //Load data for the first time ...
            await PopupNavigation.Instance.PushAsync(filter);
        }
    }
}