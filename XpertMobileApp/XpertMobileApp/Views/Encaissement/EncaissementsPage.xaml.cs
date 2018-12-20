using System;
using System.Linq;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Xpert.Pharm.DAL;
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

        async void OnItemSelected(object sender, SelectedItemChangedEventArgs args)
        {
            var item = args.SelectedItem as View_TRS_ENCAISS;
            if (item == null)
                return;

            await Navigation.PushAsync(new EncaissementDetailPage(item));

            // Manually deselect item.
            ItemsListView.SelectedItem = null;
        }

        async void AddItem_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushModalAsync(new NavigationPage(new NewEncaissementPage(null, viewModel.EncaissDisplayType)));
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            if (viewModel.Items.Count == 0)
                LoadStats(EncaissDisplayType.All);

            if (viewModel.Comptes.Count == 0)
                viewModel.LoadComptesCommand.Execute(null);
        }

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
            btn_All.BackgroundColor = Color.FromHex("#2196F3");
            btn_Decaiss.BackgroundColor = Color.FromHex("#2196F3");
            btn_Encaiss.BackgroundColor = Color.FromHex("#2196F3");

            Button btn = (Button)sender;
            btn.BackgroundColor = Color.FromHex("#51adf6");

            LoadStats(GetSelectedType(btn));
        }

        private void LoadStats(EncaissDisplayType type)
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
                viewModel.LoadItemsCommand.Execute(null);
            }
        }

        private void Filter_Clicked(object sender, EventArgs e)
        {
            FilterPanel.IsVisible = !FilterPanel.IsVisible;
        }

        private void btn_ApplyFilter_Clicked(object sender, EventArgs e)
        {
            // Pas possible de binder une date nullable a picker de date Xamarin en attendant de trouver une solution on affect manuelement
            // viewModel.StartDate = dp_StartDate.Date;
            // viewModel.EndDate = dp_EndDate.Date;            
            viewModel.LoadItemsCommand.Execute(null);
        }

        private void btn_CancelFilter_Clicked(object sender, EventArgs e)
        {
            // Anuler les filtres 
            //viewModel.StartDate = null;
            //viewModel.EndDate = null;
            viewModel.SelectedCompte = null;
            FilterPanel.IsVisible = false;
            viewModel.LoadItemsCommand.Execute(null);            
        }

        private void ComptePicker_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}