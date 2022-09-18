using Acr.UserDialogs;
using Rg.Plugins.Popup.Pages;
using Rg.Plugins.Popup.Services;
using Syncfusion.XForms.Buttons;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Xpert.Common.DAO;
using Xpert.Common.WSClient.Helpers;
using Xpert.Common.WSClient.Services;
using XpertMobileApp.Api;
using XpertMobileApp.Api.Managers;
using XpertMobileApp.Api.Services;
using XpertMobileApp.DAL;
using XpertMobileApp.Helpers;
using XpertMobileApp.Models;
using XpertMobileApp.Services;
using XpertMobileApp.SQLite_Managment;
using XpertMobileApp.ViewModels;
using XpertMobileApp.Views.Templates;

namespace XpertMobileApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ProduitsPopupFilter : PopupPage, INotifyPropertyChanged
    {
        private ProduitsViewModel viewModel;
        public string CurrentStream = Guid.NewGuid().ToString();
        public ProduitsPopupFilter(ProduitsViewModel viewModel)
        {
            InitializeComponent();
            this.viewModel = viewModel;
            BindingContext = this.viewModel;

            if (Constants.AppName == Apps.XCOM_Mob)
            {
                Labo_layout.IsVisible = false;
                tags_layout.IsVisible = false;
                checkBoxR.IsVisible = false;
            }
            else
            {
                fideleteRadioBtns.IsVisible = false;
            }


            MessagingCenter.Subscribe<ProductTagSelector, List<BSE_PRODUIT_TAG>>(this, MCDico.ITEM_SELECTED, async (obj, items) =>
            {
                viewModel.SelectedTag = items;

                TagsPicker.Text = "";
                for (int i = 0; i < items.Count; i++)
                {
                    TagsPicker.Text += items[i].DESIGNATION;
                    if (i != items.Count - 1)
                        TagsPicker.Text += ", ";
                }
            });
        }

        private async void Close_filter_popup(object sender, EventArgs e)
        {
            await PopupNavigation.Instance.PopAsync();
        }

        /// <summary>
        /// Apply the filter 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void ApplyFilter(object sender, EventArgs e)
        {
            viewModel.LoadItemsCommand.Execute(null);
            await PopupNavigation.Instance.PopAsync();
        }

        /// <summary>
        /// Btn to cancel the filter 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_CancelFilter_Clicked(object sender, EventArgs e)
        {
            //quantiteG.IsChecked = false;
            //quantiteE.IsChecked = false;
            //quantiteL.IsChecked = false;
            etatAll.IsChecked = false;
            etatActive.IsChecked = false;
            etatNonActive.IsChecked = false;
            TagsPicker.Text = "";
            viewModel.ClearFilters();
            viewModel.LoadItemsCommand.Execute(null);
        }

        private void CheckBox_StateChangedSM(object sender, StateChangedEventArgs e)
        {
            viewModel.CheckBoxSM = (bool)e.IsChecked;
        }

        private void CheckBox_StateChangedS(object sender, StateChangedEventArgs e)
        {
            viewModel.CheckBoxS = (bool)e.IsChecked;
        }

        private void CheckBox_StateChangedR(object sender, StateChangedEventArgs e)
        {
            viewModel.CheckBoxR = (bool)e.IsChecked;
        }

        private void FedeliteAll_StateChanged(object sender, StateChangedEventArgs e)
        {
            if (e.IsChecked.HasValue && e.IsChecked.Value)
            {
                viewModel.FidelOperator = (sender as SfRadioButton).ClassId;
            }
        }
        private void etatAll_StateChanged(object sender, StateChangedEventArgs e)
        {
            if (e.IsChecked.HasValue && e.IsChecked.Value)
            {
                viewModel.EtatOperator = (sender as SfRadioButton).ClassId;
            }
        }

        private async void buttonClick(object sender, EventArgs e)
        {
            ProductTagSelector productTagSelector = new ProductTagSelector();
            await PopupNavigation.Instance.PushAsync(productTagSelector);
        }

        private void Initialize_Product_Entry(object sender, EventArgs e)
        {
            if (viewModel.SearchedText != null)
            {
                viewModel.SearchedText = "";
                productEntry.Text = "";
            }
        }

        private void Initialize_Ref(object sender, EventArgs e)
        {
            if (viewModel.SearchedRef != null)
            {
                viewModel.SearchedRef = "";
                ent_SearchedRef.Text = "";
            }
        }

        private void Initialize_Laboratory_picker(object sender, EventArgs e)
        {
            if (viewModel.SelectedLabo != null)
            {
                viewModel.SelectedLabo = new BSE_PRODUIT_LABO();
                LaboPicker.SelectedIndex = 0;
            }
        }

        private void Initialize_TagProduct_picker(object sender, EventArgs e)
        {
            if (viewModel.SelectedTag != null)
            {
                viewModel.SelectedTag = new List<BSE_PRODUIT_TAG>();
                TagsPicker.Text = "";
            }
        }

        private void Initialize_Type_Picker(object sender, EventArgs e)
        {
            if (viewModel.SelectedType != null)
            {
                viewModel.SelectedType = new BSE_TABLE_TYPE();
                TypesPicker.SelectedIndex = 0;
            }
        }

        private void Initialize_Familly_Picker(object sender, EventArgs e)
        {
            if (viewModel.SelectedFamille != null)
            {
                viewModel.SelectedFamille = new BSE_TABLE();
                FamillesPicker.SelectedIndex = 0;
            }
        }
    }
}

