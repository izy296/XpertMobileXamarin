using Acr.UserDialogs;
using Flex.Controls;
using Rg.Plugins.Popup.Services;
using Syncfusion.ListView.XForms;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.PancakeView;
using Xamarin.Forms.Xaml;
using XpertMobileApp.Api.Services;
using XpertMobileApp.Api.ViewModels;
using XpertMobileApp.Base;
using XpertMobileApp.DAL;
using XpertMobileApp.Services;

namespace XpertMobileApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class BordereauxChifaPage : XBasePage
    {
        public BordereauxChifaViewModel viewModel;

        private bool isScrolling { get; set; } = false;
        public bool IsScrolling { get; set; }
        private bool clicked { get; set; } = false;
        public bool Clicked
        {
            get
            {
                return clicked;
            }
            set
            {
                clicked = value;
                OnPropertyChanged("Clicked");
            }
        }
        public BordereauxChifaPage()
        {
            InitializeComponent();
            BindingContext = viewModel = new BordereauxChifaViewModel();
            MessagingCenter.Subscribe<BordereauxSelector, View_CFA_BORDEREAUX_CHIFA>(this, "Search", async (obj, s) =>
               {
                   viewModel.Item = s;

                   await viewModel.RefreshData();
                   var list = AFactures_section.Children.Where(x => x.StyleId != "this").ToList();
                   foreach (var button in list)
                   {
                       AFactures_section.Children.Remove(button);
                   };
                   AFactures_btn.IsVisible = true;
               });
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();

            //await viewModel.ExecuteLoadItemsCommand();
            //await viewModel.ExecuteLoadLastBordereaux();
            //await viewModel.ExecuteLoadBordereauxInfo();
            //await viewModel.ExecuteLoadFacturesCommand();
            await viewModel.ExecuteLoadLastsBordereaux();
            expander.IsExpanded = true;
        }


        public override void SearchCommand()
        {
            base.SearchCommand();
            Device.BeginInvokeOnMainThread(async () =>
            {
                await viewModel.ExecuteSearch(SearchBarText);
            });
        }

        private void OnItemSelected(object sender, SelectionChangedEventArgs e)
        {

        }

        private async void ExecutePullToRefresh(object sender, EventArgs e)
        {
            await viewModel.ExecutePullToRefresh();
        }

        private async void ButtonClicked(object sender, EventArgs e)
        {
            if (Clicked)
                return;
            Clicked = true;
            var item = ((sender as PancakeView).BindingContext as View_CFA_BORDEREAUX_CHIFA);
            var elements = ((sender as PancakeView).Parent.Parent.Parent as VisualContainer).Children;
            foreach (var element in elements)
            {
                var x = (((Grid)((ContentView)element).Content).Children[0]);
                (x as PancakeView).BackgroundColor = Color.FromHex("#057665");
            }
                (sender as PancakeView).BackgroundColor = Color.FromHex("#068975");

            viewModel.Item = item;

            await viewModel.RefreshData();
            var list = AFactures_section.Children.Where(x => x.StyleId != "this").ToList();
            foreach (var button in list)
            {
                AFactures_section.Children.Remove(button);
            };
            AFactures_btn.IsVisible = true;
            Clicked = false;
        }

        private void AFactures_btn_Clicked(object obj)
        {
            AFactures_btn_Clicked(obj, new EventArgs());
        }


        private void BordereauxListView_ScrollStateChanged(object sender, ScrollStateChangedEventArgs e)
        {
            //IsScrolling = e.ScrollState == ScrollState.Dragging ? true : false;
        }

        private async void Borderaux_Clicked(object sender, EventArgs e)
        {
            await PopupNavigation.Instance.PushAsync(new BordereauxSelector());
        }

        private async void AFactures_btn_Clicked(object sender, EventArgs e)
        {
            UserDialogs.Instance.ShowLoading();
            viewModel.FacturesAnlayse = new ObservableCollection<View_CFA_MOBILE_FACTURE>(await WebServiceClient.AnalyseFactures(viewModel.Item.NUM_BORDEREAU));

            if (XpertHelper.IsNotNullAndNotEmpty(viewModel.FacturesAnlayse))
            {
                //Hide element
                AFactures_btn.IsVisible = false;
                //create button for each group
                FlexButton psychothrope, ordPlus3000, chroniques, Chevauchements;
                psychothrope = new FlexButton()
                {
                    FontSize = (double)NamedSize.Default,
                    CornerRadius = 15,
                    Padding = 8,
                    ForegroundColor = Color.White,
                    BorderColor = Color.White,
                    BackgroundColor = Color.LightSeaGreen,
                    BorderThickness = 1,
                    VerticalOptions = LayoutOptions.FillAndExpand,
                    HorizontalOptions = LayoutOptions.FillAndExpand,
                    IsEnabled = true,
                    ClickedCommand = new Command(FilterByCategory),
                    //ClickedCommandParameter = this
                };
                ordPlus3000 = new FlexButton()
                {
                    FontSize = (double)NamedSize.Default,
                    CornerRadius = 15,
                    Padding = 8,
                    ForegroundColor = Color.White,
                    BorderColor = Color.White,
                    BackgroundColor = Color.LightSeaGreen,
                    BorderThickness = 1,
                    VerticalOptions = LayoutOptions.FillAndExpand,
                    HorizontalOptions = LayoutOptions.FillAndExpand,
                    IsEnabled = true,
                    ClickedCommand = new Command(FilterByCategory),
                    //ClickedCommandParameter = this
                };
                chroniques = new FlexButton()
                {
                    FontSize = (double)NamedSize.Default,
                    CornerRadius = 15,
                    Padding = 8,
                    ForegroundColor = Color.White,
                    BorderColor = Color.White,
                    BackgroundColor = Color.LightSeaGreen,
                    BorderThickness = 1,
                    VerticalOptions = LayoutOptions.FillAndExpand,
                    HorizontalOptions = LayoutOptions.FillAndExpand,
                    IsEnabled = true,
                    ClickedCommand = new Command(FilterByCategory),
                    //ClickedCommandParameter = 
                };
                Chevauchements = new FlexButton()
                {
                    FontSize = (double)NamedSize.Default,
                    CornerRadius = 15,
                    Padding = 8,
                    ForegroundColor = Color.White,
                    BorderColor = Color.White,
                    BackgroundColor = Color.LightSeaGreen,
                    BorderThickness = 1,
                    VerticalOptions = LayoutOptions.FillAndExpand,
                    HorizontalOptions = LayoutOptions.FillAndExpand,
                    IsEnabled = true,
                    ClickedCommand = new Command(FilterByCategory),
                    //ClickedCommandParameter = 
                };
                chroniques.StyleId = 0.ToString();
                psychothrope.StyleId = 1.ToString();
                ordPlus3000.StyleId = 2.ToString();
                Chevauchements.StyleId = 3.ToString();


                psychothrope.Text = "Psychothrope (" + viewModel.FacturesAnlayse.Where(x => x.GROUPE_ANALYSE_FACTURE == 1).Count() + ")";
                chroniques.Text = "Chroniques (" + viewModel.FacturesAnlayse.Where(x => x.GROUPE_ANALYSE_FACTURE == 0).Count() + ")";
                ordPlus3000.Text = "Plus de  3000 DA (" + viewModel.FacturesAnlayse.Where(x => x.GROUPE_ANALYSE_FACTURE == 2).Count() + ")";
                Chevauchements.Text = "Chevauchements (" + viewModel.FacturesAnlayse.Where(x => x.GROUPE_ANALYSE_FACTURE == 3).GroupBy(x=>x.NUM_FACTURE).Count() + ")";

                psychothrope.ClickedCommandParameter = psychothrope;
                chroniques.ClickedCommandParameter = chroniques;
                ordPlus3000.ClickedCommandParameter = ordPlus3000;
                Chevauchements.ClickedCommandParameter = Chevauchements;

                //Padding = "8"
                //                            Text = "Analyse des Factures"
                //                            FontSize = "Small"
                //                            CornerRadius = "15"
                //                            ForegroundColor = "White"
                //                            BackgroundColor = "LightSeaGreen"
                //                            BorderColor = "White"
                //                            BorderThickness = "1"
                //                            Clicked = "AFactures_btn_Clicked"

                //added them to the section
                AFactures_section.Children.Add(psychothrope);
                AFactures_section.Children.Add(chroniques);
                AFactures_section.Children.Add(ordPlus3000);
                AFactures_section.Children.Add(Chevauchements);
                //UserDialogs.Instance.HideLoading();
            }
            else
            {
                //show error
                CustomPopup mssg = new CustomPopup("la resultat d'analyse est vide");
                await PopupNavigation.Instance.PushAsync(mssg);

            }
            UserDialogs.Instance.HideLoading();
        }

        public void ResetButtons()
        {
            var buttonList = AFactures_section.Children;
            foreach (var button in buttonList)
            {
                ((FlexButton)button).BackgroundColor = Color.LightSeaGreen;
            }
        }
        private async void FilterByCategory(object obj)
        {
            var sender = (FlexButton)obj;
            if (viewModel.SelectedCategory == -1)
                viewModel.SelectedCategory = int.Parse(sender.StyleId);
            else if (viewModel.SelectedCategory == int.Parse(sender.StyleId))
                viewModel.SelectedCategory = -1;
            else
                viewModel.SelectedCategory = int.Parse(sender.StyleId);
            ResetButtons();
            if (viewModel.SelectedCategory != -1)
                sender.BackgroundColor = Color.FromHex("#137770");
            viewModel.ChifaFacturesList.Clear();
            await viewModel.ExecuteLoadFacturesCommand();
        }

        private async void CentrePicker_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (viewModel != null && viewModel.Item.NUM_BORDEREAU != null)
                await viewModel.ExecutePullToRefresh();
        }
    }
}