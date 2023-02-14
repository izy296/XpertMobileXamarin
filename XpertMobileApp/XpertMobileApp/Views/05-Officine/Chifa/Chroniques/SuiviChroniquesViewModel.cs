using Acr.UserDialogs;
using Rg.Plugins.Popup.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Extended;
using Xpert.Common.WSClient.Helpers;
using XpertMobileApp.DAL;
using XpertMobileApp.Services;
using XpertMobileApp.Views;

namespace XpertMobileApp.Api.ViewModels
{
    public class SuiviChroniquesViewModel : CrudBaseViewModel2<CFA_MOBILE_DETAIL_FACTURE, View_CFA_MOBILE_DETAIL_FACTURE>
    {
        public class PeriodObject
        {
            public int Value { get; set; }
            public string Name { get; set; }
        }

        private string title { get; set; }
        public string Title
        {
            get
            {
                return title;
            }
            set
            {
                title = value;
                OnPropertyChanged("Title");
            }
        }

        private List<PeriodObject> periods { get; set; }
        public List<PeriodObject> Periods
        {
            get
            {
                return periods;
            }
            set
            {
                periods = value;
                OnPropertyChanged("Period");
            }
        }

        private PeriodObject selectedPeriod { get; set; } = new PeriodObject()
        {
            Value = 90,
            Name = "90 prochains jours"
        };

        public PeriodObject SelectedPeriod
        {
            get
            {
                return selectedPeriod;
            }
            set
            {
                selectedPeriod = value;
                new Command(async () =>
                {
                    await ExecuteLoadItemsCommand();
                }).Execute(null);
                OnPropertyChanged("SelectedPeriod");
            }
        }

        private string selectedDateFilter { get; set; } = "DF";
        public string SelectedDateFilter
        {
            get
            {
                return selectedDateFilter;
            }
            set
            {
                selectedDateFilter = value;
                OnPropertyChanged("SelectedDateFilter");
            }
        }

        public enum tabType
        {
            MALADES = 0,
            MEDICAMENTS = 1,
        }

        private tabType selectedtab
        {
            get; set;
        } = tabType.MALADES;
        public tabType selectedTab
        {
            get
            {
                return selectedtab;
            }
            set
            {
                selectedtab = value;
                new Command(async () =>
                {
                    await ExecuteLoadItemsCommand();
                }).Execute(null);

            }
        }

        public SuiviChroniquesViewModel()
        {
            Title = AppResources.pn_ChronicFollowUp;
            Periods = new List<PeriodObject>();
            //Traitments = new ObservableCollection<View_CFA_MOBILE_DETAIL_FACTURE>();
            Items = new InfiniteScrollCollection<View_CFA_MOBILE_DETAIL_FACTURE>();

            var jours = 90;
            var text = AppResources.psc_Jours_Prochain;
            for (int i = 0; i < 10; i++)
            {
                if (i == 5)
                    text = AppResources.psc_Jours_Precedents; 

                Periods.Add(new PeriodObject()
                {
                    Value = (i<=4) ? jours:(jours*-1),
                    Name = jours + text
                });

                if (i <= 4)
                {
                    if (jours == 90)
                        jours = 60;
                    else if (jours == 60)
                        jours = 30;
                    else if (jours == 30)
                        jours = 15;
                    else if (jours == 15)
                        jours = 7;
                }
                else if (i >= 5)
                {
                    if (jours == 7)
                        jours = 15;
                    else if (jours == 15)
                        jours = 30;
                    else if (jours == 30)
                        jours = 60;
                    else if (jours == 60)
                        jours = 90;
                }

            }

        }

        internal override async Task ExecuteLoadItemsCommand()
        {
            try
            {
                if (IsBusy)
                    return;
                IsBusy = true;
                //await Items.LoadMoreAsync();
                UserDialogs.Instance.ShowLoading();
                if (selectedTab == tabType.MALADES)
                    Items = new InfiniteScrollCollection<View_CFA_MOBILE_DETAIL_FACTURE>(await WebServiceClient.SelectPatients(SelectedPeriod.Value,SelectedDateFilter));
                else Items = new InfiniteScrollCollection<View_CFA_MOBILE_DETAIL_FACTURE>(await WebServiceClient.SelectMedications(SelectedPeriod.Value, SelectedDateFilter));
                OnPropertyChanged("Items");
                UserDialogs.Instance.HideLoading();
            }
            catch (Exception ex)
            {
                UserDialogs.Instance.HideLoading();
                CustomPopup AlertPopup = new CustomPopup(WSApi2.GetExceptionMessage(ex), trueMessage: AppResources.alrt_msg_Ok);
                await PopupNavigation.Instance.PushAsync(AlertPopup);
            }
            finally
            {
                IsBusy = false;
            }
        }

    }
}
