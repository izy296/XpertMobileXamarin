using Acr.UserDialogs;
using Rg.Plugins.Popup.Pages;
using Rg.Plugins.Popup.Services;
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
using XpertMobileApp.Api.Managers;
using XpertMobileApp.Api.Services;
using XpertMobileApp.DAL;
using XpertMobileApp.Helpers;
using XpertMobileApp.Models;
using XpertMobileApp.Services;
using XpertMobileApp.SQLite_Managment;
using XpertMobileApp.ViewModels;
using XpertMobileApp.Views.Templates;

namespace XpertMobileApp.Views._04_Comercial.TransfertDeFond
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ReclamationPopupPage : PopupPage, INotifyPropertyChanged
    {
        View_ACH_RECLAMATIONS reclamation;
        ReclamationViewModel viewModel;

        public ReclamationPopupPage(string CODE_ENTREE_DETAIL)
        {
            InitializeComponent();
            BindingContext = viewModel = new ReclamationViewModel(CODE_ENTREE_DETAIL);
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            if (viewModel.Items.Count == 0)
                viewModel.LoadItemsCommand.Execute(null);
            viewModel.bla = viewModel.Items[0].NOTE_RECLAMATION;
        }
    }
}

