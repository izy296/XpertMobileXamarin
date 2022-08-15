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

namespace XpertMobileApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ReclamationPopupPage : PopupPage, INotifyPropertyChanged
    {
        ReclamationViewModel viewModel;
        public ReclamationPopupPage(string CODE_ENTREE_DETAIL)
        {
            InitializeComponent();
            BindingContext = viewModel = new ReclamationViewModel(CODE_ENTREE_DETAIL);

        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

        }
        public async Task GetReclamation()
        {
            if (viewModel.Items.Count == 0)
            {
                await ExecuteLoadReclamation();
            }
        }
        async Task ExecuteLoadReclamation()
        {
            try
            {
                viewModel.Items.Clear();
                // liste des ventes
                await viewModel.Items.LoadMoreAsync();
                //viewModel.Items[0].NOTE_RECLAMATION;

            }
            catch (Exception ex)
            {
                await UserDialogs.Instance.AlertAsync(WSApi2.GetExceptionMessage(ex), AppResources.alrt_msg_Alert,
                    AppResources.alrt_msg_Ok);
            }
            finally
            {
                IsBusy = false;
            }
        }
    }
}

