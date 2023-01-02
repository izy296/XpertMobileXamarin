using Acr.UserDialogs;
using Syncfusion.Data.Extensions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Extended;
using Xpert.Common.DAO;
using Xpert.Common.WSClient.Helpers;
using XpertMobileApp.Api;
using XpertMobileApp.Api.ViewModels;
using XpertMobileApp.DAL;
using XpertMobileApp.Models;
using XpertMobileApp.Services;
using XpertMobileApp.SQLite_Managment;

namespace XpertMobileApp.ViewModels.Entree
{
    public static class EntreeTypes
    {
        public static string RetourMobile { get { return "ES10"; } }
    }

    public class EntreeViewModel : CrudBaseViewModel2<STK_ENTREE, View_STK_ENTREE>
    {
        public string TypeEntree = EntreeTypes.RetourMobile;

        public bool IsVtesList { get; set; } = false;

        decimal totalTurnover;
        public decimal TotalRetour
        {
            get { return totalTurnover; }
            set { SetProperty(ref totalTurnover, value); }
        }

        decimal totalMargin;
        public decimal TotalMargin
        {
            get { return totalMargin; }
            set { SetProperty(ref totalMargin, value); }
        }

        public View_TRS_TIERS SelectedTiers { get; set; }

        public EncaissDisplayType EncaissDisplayType { get; set; }
        public DateTime StartDate { get; set; } = DateTime.Now;
        public DateTime EndDate { get; set; } = DateTime.Now;

        public ObservableCollection<BSE_DOCUMENTS_TYPE> Types { get; set; }
        public BSE_DOCUMENTS_TYPE SelectedType { get; set; }

        public ObservableCollection<View_BSE_COMPTE> User { get; set; }
        public View_BSE_COMPTE SelectedUser { get; set; }
        public Command LoadUsersCommand { get; set; }

        protected override string ContoleurName
        {
            get
            {

                {
                    return "STK_ENTREE";
                }
            }
        }

        public EntreeViewModel(string typeEntree)
        {
            TypeEntree = typeEntree;

            Types = new ObservableCollection<BSE_DOCUMENTS_TYPE>();
            Title = "Retour Stock";
            base.InitConstructor();

            LoadExtrasDataCommand = new Command(async () => await ExecuteLoadExtrasDataCommand());
        }

        protected override QueryInfos GetFilterParams()
        {
            base.GetFilterParams();

            this.AddCondition<View_STK_ENTREE, DateTime?>(e => e.DATE_ENTREE, Operator.BETWEEN_DATE, StartDate, EndDate);
            //if (!string.IsNullOrEmpty(SelectedTiers?.CODE_TIERS))
            //    this.AddCondition<View_STK_ENTREE, string>(e => e.CODE_TIERS, SelectedTiers?.CODE_TIERS);


            //this.AddCondition<View_STK_ENTREE, string>(e => e.TYPE_ENTREE, TypeEntree);
            this.AddCondition<View_STK_ENTREE, string>(e => e.CREATED_BY, App.User.UserName);

            //if (!string.IsNullOrEmpty(SelectedType?.CODE_TYPE))
            //    this.AddCondition<View_STK_ENTREE, string>(e => e.TYPE_DOC, SelectedType?.CODE_TYPE);
            this.AddOrderBy<View_STK_ENTREE, DateTime?>(e => e.CREATED_ON, Sort.DESC);
            return qb.QueryInfos;
        }

        protected override void OnAfterLoadItems(IEnumerable<View_STK_ENTREE> list)
        {
            base.OnAfterLoadItems(list);

            int i = 0;
            foreach (var item in list)
            {
                i += 1;
                (item as BASE_CLASS).Index = i;
            }
        }

        async Task ExecuteLoadItemsCommand()
        {
            if (IsBusy)
                return;

            try
            {
                Items.Clear();

                // liste des entree
                await Items.LoadMoreAsync();
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

        async Task ExecuteLoadExtrasDataCommand()
        {

            if (IsLoadExtrasBusy)
                return;

            try
            {
                IsLoadExtrasBusy = true;

                Task<List<View_STK_ENTREE>> AllEntrees = UpdateDatabase.getInstance().Table<View_STK_ENTREE>().ToListAsync();
                if (AllEntrees != null && AllEntrees.Result != null)
                {
                    TotalRetour = AllEntrees.Result.Sum(x => x.TOTAL_TTC);
                }
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
                //  UserDialogs.Instance.ShowLoading(AppResources.txt_Loading);

                //Types.Clear();
                //var itemsC = await WebServiceClient.GetVenteTypes();

                //foreach (var itemC in itemsC)
                //{
                //    Types.Add(itemC);
                //}


                BSE_DOCUMENTS_TYPE empty = new BSE_DOCUMENTS_TYPE();
                empty.CODE_TYPE = "";
                empty.DESIGNATION_TYPE = AppResources.txt_All;
                Types.Insert(0, empty);

                //  UserDialogs.Instance.HideLoading();
            }
            catch (Exception ex)
            {
                //  UserDialogs.Instance.HideLoading();
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
