using Acr.UserDialogs;
using Rg.Plugins.Popup.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xpert.Common.DAO;
using Xpert.Common.WSClient.Helpers;
using XpertMobileApp.Api.ViewModels;
using XpertMobileApp.DAL;
using XpertMobileApp.Helpers;
using XpertMobileApp.Models;
using XpertMobileApp.Services;

namespace XpertMobileApp.Views._04_Comercial.Echange
{
    public class EchangeListViewModel : CrudBaseViewModel2<STK_ECHANGE, View_STK_ECHANGE>
    {
        public DateTime StartDate { get; set; } = DateTime.ParseExact("2022-01-01", "yyyy-MM-dd", System.Globalization.CultureInfo.InvariantCulture);
        public DateTime EndDate { get; set; } = DateTime.Now;
        public ObservableCollection<View_STK_MOTIF_ECHANGE> Motifs { get; set; }
        public ObservableCollection<BSE_TIERS_TYPE> TypeTiers { get; set; }
        public ObservableCollection<View_TRS_TIERS> ListeTiers { get; set; }
        public View_STK_MOTIF_ECHANGE MotifSelected { get; set; }
        public BSE_TIERS_TYPE TypeTiersSelected { get; set; }
        public View_TRS_TIERS TiersSelected { get; set; }

        public Command LoadDataCommand { get; set; }
        public EchangeListViewModel()
        {
            Motifs = new ObservableCollection<View_STK_MOTIF_ECHANGE>();
            TypeTiers = new ObservableCollection<BSE_TIERS_TYPE>();
            ListeTiers = new ObservableCollection<View_TRS_TIERS>();
            LoadDataCommand = new Command(async () => await ExecuteLoadData());
            MessagingCenter.Subscribe<NewEchangePopupPage, string>(this, MCDico.ITEM_ADDED, async (obj, selectedItem) =>
            {
                await ExecuteLoadData();
            });
            LoadSummaries = true;
        }
        protected override QueryInfos GetFilterParams()
        {
            base.GetFilterParams();
            this.AddCondition<View_STK_ECHANGE, DateTime?>(e => e.CREATED_ON, Operator.BETWEEN_DATE, StartDate, EndDate);

            if (!string.IsNullOrEmpty(MotifSelected?.DESIGNATION_MOTIF))
                this.AddCondition<View_STK_ECHANGE, string>(e => e.CODE_MOTIF, MotifSelected?.CODE_MOTIF);

            if (!string.IsNullOrEmpty(TypeTiersSelected?.DESIGNATION_TYPE))
                this.AddCondition<View_STK_ECHANGE, string>(e => e.TYPE_TIERS, TypeTiersSelected?.CODE_TYPE);

            if (!string.IsNullOrEmpty(TiersSelected?.NOM_TIERS))
                this.AddCondition<View_STK_ECHANGE, string>(e => e.NOM_TIERS, TiersSelected?.NOM_TIERS);

            return qb.QueryInfos;
        }

        protected override void OnAfterLoadItems(IEnumerable<View_STK_ECHANGE> list)
        {
            base.OnAfterLoadItems(list);
            int i = 0;
            foreach (var item in list)
            {
                i += 1;
                (item as BASE_CLASS).Index = i;
            }
        }

        public async Task ExecuteLoadData()
        {
            if (App.Online)
            {
                try
                {
                    UserDialogs.Instance.ShowLoading(AppResources.txt_Waiting);

                    TypeTiers.Clear();
                    Motifs.Clear();
                    ListeTiers.Clear();
                    var itemsMotif = await WebServiceClient.GetMotifEchange();
                    var itemsTypeTiers = await WebServiceClient.GetTypeTiers();

                    var itemsListeTiers = await WebServiceClient.GetListeTiers(null, "true");

                    itemsMotif.Insert(0, new View_STK_MOTIF_ECHANGE()
                    {
                        DESIGNATION_MOTIF = AppResources.txt_All,
                        CODE_MOTIF = ""
                    });

                    itemsTypeTiers.Insert(0, new BSE_TIERS_TYPE()
                    {
                        CODE_TYPE = "",
                        DESIGNATION_TYPE = AppResources.txt_All
                    });
                    itemsListeTiers.Insert(0, new View_TRS_TIERS()
                    {
                        CODE_TIERS = "",
                        NOM_TIERS = "",
                    });

                    foreach (var itemTypeTiers in itemsTypeTiers)
                        TypeTiers.Add(itemTypeTiers);


                    foreach (var itemMotif in itemsMotif)
                        Motifs.Add(itemMotif);

                    foreach (var itemListeTiers in itemsListeTiers)
                    {
                        ListeTiers.Add(itemListeTiers);
                    }

                    UserDialogs.Instance.HideLoading();

                }
                catch (Exception ex)
                {
                    UserDialogs.Instance.HideLoading();
                    CustomPopup AlertPopup = new CustomPopup(WSApi2.GetExceptionMessage(ex), trueMessage: AppResources.alrt_msg_Ok);
                    await PopupNavigation.Instance.PushAsync(AlertPopup);
                }
            }
        }
        public async void UpdateTiersList(string typeTiers)
        {
            if (App.Online)
            {
                try
                {
                    UserDialogs.Instance.ShowLoading(AppResources.txt_Waiting);
                    ListeTiers.Clear();
                    var itemsListeTiers = await WebServiceClient.GetListeTiers(typeTiers, "true");
                    itemsListeTiers.Insert(0, new View_TRS_TIERS()
                    {
                        CODE_TIERS = "",
                        NOM_TIERS = "",
                    });
                    foreach (var itemListeTiers in itemsListeTiers)
                    {
                        ListeTiers.Add(itemListeTiers);
                    }

                    UserDialogs.Instance.HideLoading();
                }
                catch (Exception ex)
                {
                    UserDialogs.Instance.HideLoading();
                    await UserDialogs.Instance.AlertAsync(WSApi2.GetExceptionMessage(ex), AppResources.alrt_msg_Alert,
                        AppResources.alrt_msg_Ok);
                }
            }
        }
    }
}
