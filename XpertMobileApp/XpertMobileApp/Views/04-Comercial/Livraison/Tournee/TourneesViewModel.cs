using Acr.UserDialogs;
using Syncfusion.Linq;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xpert.Common.DAO;
using Xpert.Common.WSClient.Helpers;
using XpertMobileApp.Api;
using XpertMobileApp.Api.Managers;
using XpertMobileApp.Api.ViewModels;
using XpertMobileApp.DAL;
using XpertMobileApp.Models;
using XpertMobileApp.Services;
using XpertMobileApp.SQLite_Managment;
using XpertMobileApp.Views._03_CommonPages.Synchronisation;

namespace XpertMobileApp.ViewModels
{

    public class TourneesViewModel : CrudBaseViewModel2<LIV_TOURNEE, View_LIV_TOURNEE>
    {

        DateTime startDate;
        public DateTime StartDate
        {
            get { return startDate; }
            set { SetProperty(ref startDate, value); }
        }

        DateTime endDate = DateTime.Now;
        public DateTime EndDate
        {
            get { return endDate; }
            set { SetProperty(ref endDate, value); }
        }

        public TourneesViewModel()
        {
            Title = "Mes tournées";
            if (App.Online)
                StartDate = DateTime.Now;
            //Types = new ObservableCollection<BSE_TABLE_TYPE>();
            //Familles = new ObservableCollection<BSE_TABLE>();

            //LoadExtrasDataCommand = new Command(async () => await ExecuteLoadExtrasDataCommand());
        }

        protected override QueryInfos GetFilterParams()
        {
            base.GetFilterParams();
            this.AddCondition<View_LIV_TOURNEE, DateTime>(e => e.DATE_TOURNEE, Operator.BETWEEN_DATE, StartDate, EndDate);
            this.AddCondition<View_LIV_TOURNEE, TourneeStatus>(e => e.ETAT_TOURNEE, (byte)TourneeStatus.Planned);
            if (App.User.UserGroup != "AD")
            {
                this.AddCondition<View_LIV_TOURNEE, string>(e => e.CODE_VENDEUR, App.User.UserName);
                this.AddCondition<View_LIV_TOURNEE, string>(e => e.CODE_MAGASIN, App.CODE_MAGASIN);
            }
            //this.AddCondition(e => e.CODE_VENDEUR, user);
            this.AddOrderBy<View_LIV_TOURNEE, DateTime>(e => e.DATE_TOURNEE, Sort.DESC);
            return qb.QueryInfos;
        }

        protected override void OnAfterLoadItems(IEnumerable<View_LIV_TOURNEE> list)
        {
            base.OnAfterLoadItems(list);

            int i = 0;
            foreach (var item in list)
            {
                i += 1;
                (item as BASE_CLASS).Index = i;
            }
        }

        #region filters data
        private string searchedText;
        public string SearchedText
        {
            get { return searchedText; }
            set { SetProperty(ref searchedText, value); }
        }

        private string searchedRef;
        public string SearchedRef
        {
            get { return searchedRef; }
            set { SetProperty(ref searchedRef, value); }
        }


        public ObservableCollection<BSE_TABLE_TYPE> Types { get; set; }
        private BSE_TABLE_TYPE selectedType;
        public BSE_TABLE_TYPE SelectedType
        {
            get { return selectedType; }
            set { SetProperty(ref selectedType, value); }
        }

        public ObservableCollection<BSE_TABLE> Familles { get; set; }
        private BSE_TABLE selectedFamille;
        public BSE_TABLE SelectedFamille
        {
            get { return selectedFamille; }
            set { SetProperty(ref selectedFamille, value); }
        }

        public override void ClearFilters()
        {
            base.ClearFilters();
            searchedText = "";
            SelectedType = null;
            SelectedFamille = null;
        }

        async Task ExecuteLoadExtrasDataCommand()
        {

            if (IsLoadExtrasBusy)
                return;

            try
            {
                IsLoadExtrasBusy = true;
                await ExecuteLoadFamillesCommand();
                await ExecuteLoadTypesCommand();
            }
            catch (Exception ex)
            {
                await UserDialogs.Instance.AlertAsync(WSApi2.GetExceptionMessage(ex), AppResources.alrt_msg_Alert,
                    AppResources.alrt_msg_Ok);
            }
            finally
            {
                IsLoadExtrasBusy = false;
            }
        }

        async Task ExecuteLoadTypesCommand()
        {

            try
            {
                Types.Clear();
                var itemsC = await WebServiceClient.GetProduitTypes();

                BSE_TABLE_TYPE allElem = new BSE_TABLE_TYPE();
                allElem.CODE_TYPE = "";
                allElem.DESIGNATION_TYPE = AppResources.txt_All;
                Types.Add(allElem);

                foreach (var itemC in itemsC)
                {
                    Types.Add(itemC);
                }
            }
            catch (Exception ex)
            {
                await UserDialogs.Instance.AlertAsync(WSApi2.GetExceptionMessage(ex), AppResources.alrt_msg_Alert,
                    AppResources.alrt_msg_Ok);
            }
        }

        async Task ExecuteLoadFamillesCommand()
        {
            try
            {

                Familles.Clear();
                var itemsC = await WebServiceClient.GetProduitFamilles();

                BSE_TABLE allElem = new BSE_TABLE();
                allElem.CODE = "";
                allElem.DESIGNATION = AppResources.txt_All;
                Familles.Add(allElem);

                foreach (var itemC in itemsC)
                {
                    Familles.Add(itemC);
                }
            }
            catch (Exception ex)
            {
                await UserDialogs.Instance.AlertAsync(WSApi2.GetExceptionMessage(ex), AppResources.alrt_msg_Alert,
                    AppResources.alrt_msg_Ok);
            }
        }

        #endregion

        #region filter Data Offline 

        public async override Task<List<View_LIV_TOURNEE>> SelectByPageFromSqlLite(QueryInfos filter)
        {

            var sqliteRes = await base.SelectByPageFromSqlLite(filter);
            if (StartDate == null)
            {
                sqliteRes = sqliteRes.Where(e => StartDate.Date.CompareTo(((DateTime)e.DATE_TOURNEE).Date) <= 0 && EndDate.Date.CompareTo(((DateTime)e.DATE_TOURNEE).Date) >= 0).ToList();
            }
            return sqliteRes;
        }
        #endregion

        public async Task UpdateTourneStatus(View_LIV_TOURNEE item)
        {
            try
            {
                bool answer = false;
                if (item.ETAT_TOURNEE == TourneeStatus.Started)
                {
                    answer = await App.Current.MainPage.DisplayAlert(AppResources.alrt_msg_Alert, AppResources.tourneeStatusClosedMessage, AppResources.exit_Button_Yes, AppResources.exit_Button_No);
                    if (answer)
                    {
                        item.ETAT_TOURNEE = TourneeStatus.Closed;
                    }
                }
                else if (item.ETAT_TOURNEE == TourneeStatus.Planned)
                {
                    answer = await App.Current.MainPage.DisplayAlert(AppResources.alrt_msg_Alert, AppResources.tourneeStatusStartMessage, AppResources.exit_Button_Yes, AppResources.exit_Button_No);
                    if (answer)
                    {
                        item.ETAT_TOURNEE = TourneeStatus.Started;
                    }
                }
                else if (item.ETAT_TOURNEE == TourneeStatus.Closed)
                {
                    answer = await App.Current.MainPage.DisplayAlert(AppResources.alrt_msg_Alert,AppResources.tourneeStatusReopenMessage, AppResources.exit_Button_Yes, AppResources.exit_Button_No);
                    if (answer)
                    {
                        item.ETAT_TOURNEE = TourneeStatus.Started;
                    }

                }

                if (answer)
                    if (App.Online)
                    {

                        await CrudManager.Tournee.UpdateItemAsync(item);
                        await UserDialogs.Instance.AlertAsync("Mise a jour avec success", AppResources.alrt_msg_Alert, AppResources.alrt_msg_Ok);

                    }
                    else
                    {
                        await SQLite_Manager.GetInstance().UpdateAsync(item);
                        await UserDialogs.Instance.AlertAsync("Mise a jour avec success", AppResources.alrt_msg_Alert, AppResources.alrt_msg_Ok);
                    }

                if (item.ETAT_TOURNEE == TourneeStatus.EnRoute && answer)
                {
                    MessagingCenter.Send(this, "TourneeDetails", item);
                }
            }
            catch (Exception ex)
            {
                await UserDialogs.Instance.AlertAsync(ex.Message, AppResources.alrt_msg_Alert, AppResources.alrt_msg_Ok);
            }

        }

    }

}
