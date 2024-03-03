using Rg.Plugins.Popup.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using XpertMobileApp.DAL;
using XpertMobileApp.Models;
using XpertMobileApp.ViewModels;

namespace XpertMobileApp.Views.Templates
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class BaseFilter : ContentView
    {        
        private object pageBindingContext;        
        private string FilterShowOption = "ACH";
        XpertBaseFilterModel bindingFilterModel;

        private TiersSelector itemSelector;
        public string CurrentStream = Guid.NewGuid().ToString();
        public string ParentCurrentStream;

        public BaseFilter()
        {
            InitializeComponent();

            itemSelector = new TiersSelector(CurrentStream);

            BindingContext = bindingFilterModel = new XpertBaseFilterModel();
            
        }

        public void Show(string currentStreamString)
        {
            //BindingContext = obj;
            //this.pageBindingContext = obj;
            ParentCurrentStream = currentStreamString;

            MessagingCenter.Subscribe<TiersSelector, View_TRS_TIERS>(this, CurrentStream, async (obj, selectedItem) =>
            {
                bindingFilterModel.Filter_SelectedTiers = selectedItem;
                ent_SelectedTiers.Text = selectedItem.NOM_TIERS1;
            });
            this.ShowFilterPage();
        }

        private void ShowFilterPage()
        {
            
            this.IsVisible = true;
            if (FilterShowOption == "ACH")
            {
                //MainContent.Children[2].IsVisible = true;
                //MainContent.Children[3].IsVisible = true;
                //MainContent.Children[4].IsVisible = true;
                //MainContent.Children[5].IsVisible = true;
            }

            this.MainContent.FadeTo(1);
            this.MainContent.TranslateTo(this.MainContent.TranslationX, 0);
            //this.ShadowView.IsVisible = true;
        }

        public void Hide()
        {
            this.ShadowView.IsVisible = false;
            var fadeAnimation = new Animation(v => this.MainContent.Opacity = v, 1, 0);
            var translateAnimation = new Animation(v => this.MainContent.TranslationY = v, 0, this.MainContent.Height, null, () => { this.IsVisible = false; });

            var parentAnimation = new Animation { { 0.5, 1, fadeAnimation }, { 0, 1, translateAnimation } };
            parentAnimation.Commit(this, "HideSettings");
        }
        private void CloseSettings(object sender, EventArgs e)
        {
            this.Hide();
        }

        private void Button_OnClicked(object sender, EventArgs e)
        {
            this.Hide();            
        }

        private void lightTheme_Clicked(object sender, EventArgs e)
        {

        }

        private void darkTheme_Clicked(object sender, EventArgs e)
        {

        }

        private async void btn_Select_Clicked(object sender, EventArgs e)
        {
            itemSelector.SearchedType = "F";
            await PopupNavigation.Instance.PushAsync(itemSelector);
        }

        private void SendResponse()
        {            
            {
                // MessagingCenter.Send(App.MsgCenter, CurrentStream, viewModel.SelectedItem);
                

                MessagingCenter.Send(this, ParentCurrentStream, bindingFilterModel);
            }
        }

        private void btn_ApplyFilter_Clicked(object sender, EventArgs e)
        {            
            SendResponse();
            this.Hide();
        }

        private void btn_CancelFilter_Clicked(object sender, EventArgs e)
        {
            this.Hide();
        }

        private void PrimaryColors_SelectionChanged(object sender, Syncfusion.XForms.Buttons.SelectionChangedEventArgs e)
        {
            //AppSettings.Instance.SelectedPrimaryColor = this.PrimaryColorsView.SelectedIndex;
            //this.UpdatePrimaryColor();
        }
    }
}