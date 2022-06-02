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
			InitializeComponent ();
            itemSelector = new UserSelector(CurrentStream);
            BindingContext = viewModel = new ManquantsViewModel();
            MessagingCenter.Subscribe<UserSelector, View_SYS_USER>(this, CurrentStream, async (obj, selectedItem) =>
            {
               viewModel.SelectedUsers = selectedItem;
                ent_SelectedUsers.Text = selectedItem.ID_USER;
            });           
        }
        async void OnItemSelected(object sender, SelectedItemChangedEventArgs args)
        {
            var item = args.SelectedItem as View_ACH_MANQUANTS;
            if (item == null)
                return;
            await Navigation.PushAsync(new ProduitDetailPage(item.CODE_PRODUIT));           
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
        /// <summary>
        /// Fonction qui permet de vider les champs de filtre
        /// </summary>
        private void ClearFilters()
        {
            ent_SelectedUsers.Text = "";
            TypesPicker.SelectedItem = TypesPicker.ItemsSource[0];
            TypesProduitPicker.SelectedItem = TypesProduitPicker.ItemsSource[0];
            StockMin.IsChecked = false;
        }
        private void Filter_Clicked(object sender, EventArgs e)
        {
            FilterPanel.IsVisible = !FilterPanel.IsVisible;
        }
        /// <summary>
        /// Fonction qui permet d'appliquer un filtre
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_ApplyFilter_Clicked(object sender, EventArgs e)
        {        
            viewModel.LoadItemsCommand.Execute(null);
            FilterPanel.IsVisible = false;
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
        private  void btn_CancelFilter_Clicked(object sender, EventArgs e)
        {           
            FilterPanel.IsVisible = false;
            ClearFilters();
            viewModel.LoadItemsCommand.Execute(null);          
        }
        /// <summary>
        /// Fonction qui permet d'afficher la listes des utilisateurs dans le filtre
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void btn_Select_Clicked(object sender, EventArgs e)
        {
            itemSelector.SearchedType = "";
            await PopupNavigation.Instance.PushAsync(itemSelector);
        }
        /// <summary>
        /// Fonction qui permet de detecter le changement de checkbox (afficher le stock minimum)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CheckBox_CheckedChanged(object sender, CheckedChangedEventArgs e)
        {
            if (sender is CheckBox)
            {
                var checkbox = (CheckBox)sender;
                if (checkbox.IsChecked)
                {
                    viewModel.StockMinimum = true;
                }
                else
                {
                    viewModel.StockMinimum = false;
                }
            }
        }
    }
}