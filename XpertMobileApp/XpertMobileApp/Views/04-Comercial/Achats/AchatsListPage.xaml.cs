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
using XpertMobileApp.Views._04_Comercial.Achats;
using XpertMobileApp.Views.Templates;

namespace XpertMobileApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AchatsListPage : ContentPage
    {
        public string CurrentStream = Guid.NewGuid().ToString();
        private bool tapped { get; set; } = false;
        private string typeDoc = "LF";
        private string motifDoc = AchRecMotifs.PesageReception;
        AchatsListViewModel viewModel;

        SYS_MOBILE_PARAMETRE parames;
        List<SYS_OBJET_PERMISSION> permissions;

        XpertBaseFilterModel filterObjectTest;

        public AchatsListPage()
        {
            InitializeComponent();

            itemSelector = new TiersSelector(CurrentStream);

            BindingContext = viewModel = new AchatsListViewModel(typeDoc);

            MessagingCenter.Subscribe<BaseFilter, XpertBaseFilterModel>(this, CurrentStream, async (obj, selectedFilter) =>
            {
                filterObjectTest = selectedFilter;
                viewModel.StartDate = filterObjectTest.Filter_Start_Date;
                viewModel.EndDate = filterObjectTest.Filter_End_Date;
                viewModel.RefDocum = filterObjectTest.Filter_GlobalSearch;
                viewModel.SelectedTiers = filterObjectTest.Filter_SelectedTiers;
                btn_ApplyFilter_Clicked(null, null);
            });

            if (!App.Online && Constants.AppName == Apps.XCOM_Mob)
            {
                if (ToolbarItems.Count > 0)
                    ToolbarItems.RemoveAt(0);
            }
        }
            async void AddNewAchat(object sender, EventArgs e)
            {
                await Navigation.PushAsync(new FournisseursList());
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

                if (viewModel.Items.Count <= 0)
                    LoadStats();
                viewModel.LoadItemsCommand.Execute(null);
                //viewModel.LoadExtrasDataCommand.Execute(null);
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

        private TiersSelector itemSelector;
        private async void btn_Select_Clicked(object sender, EventArgs e)
        {
            itemSelector.SearchedType = "CF";
            await PopupNavigation.Instance.PushAsync(itemSelector);
        }

        private void ShowHide_Clicked(object sender, EventArgs e)
        {
            if (tapped)
            {
                floatingButton.Opacity = 1;
                SummariesInfos.HeightRequest = SummariesListView.HeightRequest = 30;
                tapped = !tapped;
                arrow_img.RotateTo(0, 400, Easing.Linear);
            }
            else
            {
                floatingButton.Opacity = 0.2;
                if (viewModel.HasAdmin && viewModel.Summaries.Count > 0)
                {
                    SummariesInfos.HeightRequest = SummariesListView.HeightRequest = gridSamuary.HeightRequest = 220;
                }
                else
                {
                    SummariesInfos.HeightRequest = SummariesListView.HeightRequest = gridSamuary.HeightRequest = 55;
                }

                tapped = !tapped;
                arrow_img.RotateTo(180, 400, Easing.Linear);
            }
        }

        private async void EmployeeView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                if (e.CurrentSelection.Count != 0)
                {
                    var item = e.CurrentSelection[0] as View_ACH_DOCUMENT;
                    if (item != null)
                    {
                        //UserDialogs.Instance.AlertAsync(); 
                        await Navigation.PushAsync(new AchatDetailPage(item));
                        ClientsView.SelectedItem = null;
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private async void ShowHideFilter(object sender, EventArgs e)
        {
            AchatsPopupFilter filter = new AchatsPopupFilter(viewModel);
            //Load data for the first time ...
            await PopupNavigation.Instance.PushAsync(filter);
        }

        private void BaseFilterView_BindingContextChanged(object sender, EventArgs e)
        {

        }
    }
}