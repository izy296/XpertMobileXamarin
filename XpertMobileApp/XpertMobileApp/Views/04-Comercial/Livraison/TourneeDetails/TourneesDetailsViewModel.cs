using Acr.UserDialogs;
using Syncfusion.Linq;
using Syncfusion.SfMaps.XForms;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xpert.Common.DAO;
using Xpert.Common.WSClient.Helpers;
using XpertMobileApp.Api.ViewModels;
using XpertMobileApp.DAL;
using XpertMobileApp.Models;
using XpertMobileApp.Services;
using XpertMobileApp.SQLite_Managment;
using XpertMobileApp.Views;

namespace XpertMobileApp.ViewModels
{

    public class TourneesDetailsViewModel : CrudBaseViewModel2<LIV_TOURNEE_DETAIL, View_LIV_TOURNEE_DETAIL>
    {
        public class VisitPin
        {
            public string ClientFullName { get; set; }
            //public string ClientPhone { get; set; }
            public double GpsLatitude { get; set; }
            public double GpsLongitude { get; set; }
        }

        string _CodeTournee;

        private TourneesDetailsPage myMapPage;
        public TourneesDetailsPage MyMapPage
        {
            get { return myMapPage; }
            set { SetProperty(ref myMapPage, value); }
        }
        public TourneesDetailsViewModel(string codeTournee)
        {
            Title = "Visites";
            _CodeTournee = codeTournee;
            Types = new ObservableCollection<BSE_TABLE_TYPE>();
            Familles = new ObservableCollection<BSE_TABLE>();

            //LoadExtrasDataCommand = new Command(async () => await ExecuteLoadExtrasDataCommand());
        }

        protected override QueryInfos GetFilterParams()
        {
            base.GetFilterParams();
            this.AddCondition<View_LIV_TOURNEE_DETAIL, string>(e => e.CODE_TOURNEE, _CodeTournee);
            this.AddOrderBy<View_LIV_TOURNEE_DETAIL, DateTime?>(e => e.CREATED_ON, Sort.DESC);
            return qb.QueryInfos;
        }

        protected override async void OnAfterLoadItems(IEnumerable<View_LIV_TOURNEE_DETAIL> list)
        {
            base.OnAfterLoadItems(list);

            int i = 0;
            foreach (var item in list)
            {
                i += 1;
                (item as BASE_CLASS).Index = i;
            }

            List<View_TRS_TIERS> clientsInfo = new List<View_TRS_TIERS>();

            if (App.Online)
            {
                clientsInfo = await WebServiceClient.GetClients(_CodeTournee);

            }
            else
            {
                clientsInfo = await SQLite_Manager.GetClients(_CodeTournee);
            }

            foreach (var info in clientsInfo)
            {
                foreach (var item in list)
                {
                    if (info.CODE_TIERS == item.CODE_TIERS)
                    {
                        item.GPS_LATITUDE_CLIENT = info.GPS_LATITUDE;
                        item.GPS_LONGITUDE_CLIENT = info.GPS_LONGITUDE;
                    }
                }
            }
            MyMapPage.RefreshMap(list);


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
    }

}
