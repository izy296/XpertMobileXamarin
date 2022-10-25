using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using XpertMobileApp.DAL;
using XpertMobileApp.SQLite_Managment;
using XpertMobileApp.ViewModels;
using XpertMobileApp.Views.Encaissement;

namespace XpertMobileApp.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class EncaissementsPage : ContentPage
	{
        EncaissementsViewModel viewModel;

        public EncaissementsPage ()
		{
			InitializeComponent ();

            BindingContext = viewModel = new EncaissementsViewModel();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            if (viewModel.Items.Count == 0)
                LoadStats(EncaissDisplayType.ENC);

            if (viewModel.Comptes.Count == 0)
                viewModel.LoadExtrasDataCommand.Execute(null);
        }

        #region Afficher / ajouter un element

        async void OnItemSelected(object sender, SelectedItemChangedEventArgs args)
        {
            if (App.Online)
            {
                var item = args.SelectedItem as View_TRS_ENCAISS;
                if (item == null)
                    return;

                await Navigation.PushAsync(new EncaissementDetailPage(item));

                // Manually deselect item.
                ItemsListView.SelectedItem = null;
            }
            else
            {
                var item = args.SelectedItem as View_TRS_ENCAISS;
                var itemfromDB = await UpdateDatabase.getselectedItemEncaiss(item);
                if (itemfromDB == null || item ==null)
                    return;

                await Navigation.PushAsync(new EncaissementDetailPage(itemfromDB));

                // Manually deselect item.
                ItemsListView.SelectedItem = null;
            }
        }

        async void AddItem_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushModalAsync(new NavigationPage(new NewEncaissementPage(null, viewModel.EncaissDisplayType)));
        }

        #endregion

        #region filtres

        private EncaissDisplayType GetSelectedType(Button btn)
        {
            EncaissDisplayType selectedType = viewModel.EncaissDisplayType;
            switch (btn.ClassId)
            {
                case "all":
                    selectedType = EncaissDisplayType.All;
                    break;
                case "Encaiss":
                    selectedType = EncaissDisplayType.ENC;
                    break;
                case "Decaiss":
                    selectedType = EncaissDisplayType.DEC;
                    break;
            }

            return selectedType;
        }

        private void TypeFilter_Clicked(object sender, EventArgs e)
        {
            btn_All.BorderWidth =0;
            btn_Decaiss.BorderWidth = 0;
            btn_Encaiss.BorderWidth = 0;

            Button btn = (Button)sender;
            btn.BorderColor = Color.FromHex("#ffffff");
            btn.BorderWidth = 2;

            LoadStats(GetSelectedType(btn));
            viewModel.Totauxjournne();
        }

        private async void LoadStats(EncaissDisplayType type)
        {
            switch (type)
            {
                case EncaissDisplayType.All:
                    if (ToolbarItems.IndexOf(btn_Additem) >= 0)
                        ToolbarItems.Remove(btn_Additem);
                    break;
                case EncaissDisplayType.ENC:
                    if(ToolbarItems.IndexOf(btn_Additem) < 0)
                        ToolbarItems.Add(btn_Additem);
                    break;
                case EncaissDisplayType.DEC:
                    if (ToolbarItems.IndexOf(btn_Additem) < 0)
                        ToolbarItems.Add(btn_Additem);
                    break; 
            }

            if (viewModel.EncaissDisplayType != type)
            {
                viewModel.EncaissDisplayType = type;
                if (App.Online)
                {
                    viewModel.LoadItemsCommand.Execute(null);
                }
                else
                {
                    var items = await UpdateDatabase.LoadEncDec(viewModel.EncaissDisplayType.ToString());
                    viewModel.Items.Clear();
                    foreach (var item in items)
                    {
                        viewModel.Items.Add(item);
                    }
                }
            }
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
            //  viewModel.SelectedCompte = null;
            viewModel.ClearFilters();
            viewModel.LoadItemsCommand.Execute(null);
        }

        #endregion 
    }
}