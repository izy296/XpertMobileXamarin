using Plugin.Connectivity;
using Rg.Plugins.Popup.Services;
using System;
using System.Collections.Generic;
using System.Linq;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using XpertMobileApp.Api;
using XpertMobileApp.DAL;
using XpertMobileApp.Helpers;
using XpertMobileApp.Models;
using XpertMobileApp.ViewModels;
using XpertMobileApp.Views._04_Comercial.Manquants;
using XpertMobileApp.Views.Templates;

namespace XpertMobileApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ManquantsPage : ContentPage
    {
        ManquantsViewModel viewModel;
        public string CurrentStream = Guid.NewGuid().ToString();
        public View_SYS_USER SelectedUsers { get; set; }
        private UserSelector itemSelector;
        public ManquantsPage()
        {
            InitializeComponent();
            itemSelector = new UserSelector(CurrentStream);
            BindingContext = viewModel = new ManquantsViewModel();
        }

        void OnItemSelected(object sender, SelectedItemChangedEventArgs args)
        {
            var item = args.SelectedItem as View_ACH_MANQUANTS;
            if (item == null)
                return;
            //await Navigation.PushAsync(new ProduitDetailPage(item.CODE_PRODUIT));
            if (ItemsListView.SelectedItem == item)
                ItemsListView.SelectedItem = null;
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            if (viewModel.Items.Count == 0)
                viewModel.LoadItemsCommand.Execute(null);
            if (viewModel.Types.Count == 0)
                viewModel.LoadExtrasDataCommand.Execute(null);
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();
        }

        /// <summary>
        /// Fonction qui permet d'appliquer un filtre
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_ApplyFilter_Clicked(object sender, EventArgs e)
        {
            viewModel.LoadItemsCommand.Execute(null);
        }

        /// <summary>
        /// Fonction qui permet d'afficher une popup pour l'ajout d'un manquant
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        async void AjoutItem_Clicked(object sender, EventArgs e)
        {
            NewManquantPopupPage form = new NewManquantPopupPage();
            await PopupNavigation.Instance.PushAsync(form);
        }

        /// <summary>
        /// Fonction qui permet de fermer la page de filtre 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_CancelFilter_Clicked(object sender, EventArgs e)
        {
            viewModel.LoadItemsCommand.Execute(null);
        }

        /// <summary>
        /// Show hide the filter section when clicking to the floating button
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void ShowHideFilter(object sender, EventArgs e)
        {
            ManquantsPopupFilter filter = new ManquantsPopupFilter(viewModel);
            //Load data for the first time ...
            await PopupNavigation.Instance.PushAsync(filter);
        }
    }
}