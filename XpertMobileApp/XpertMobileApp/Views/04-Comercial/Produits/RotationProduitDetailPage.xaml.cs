using Acr.UserDialogs;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Extended;
using Xamarin.Forms.Xaml;
using Xpert.Common.WSClient.Helpers;
using XpertMobileApp.DAL;
using XpertMobileApp.Models;
using XpertMobileApp.Services;
using XpertMobileApp.ViewModels;

namespace XpertMobileApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class RotationProduitDetailPage : ContentPage, INotifyPropertyChanged
    {

        #region Attributes

        #region INotifyPropertyChanged
        protected bool SetProperty<T>(ref T backingStore, T value,
            string propertyName = "",
            Action onChanged = null)
        {
            if (EqualityComparer<T>.Default.Equals(backingStore, value))
                return false;

            backingStore = value;
            onChanged?.Invoke();
            OnPropertyChanged(propertyName);
            return true;
        }


        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName = "")
        {
            var changed = PropertyChanged;
            if (changed == null)
                return;

            changed.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion

        private ObservableCollection<RotationDesProduitsDetails> itemRows;
        public ObservableCollection<RotationDesProduitsDetails> ItemRows
        {
            get { return itemRows; }
            set { SetProperty(ref itemRows, value); }
        }

        bool isBusy = false;
        public bool IsBusy
        {
            get { return isBusy; }
            set { SetProperty(ref isBusy, value); }
        }

        public enum RotationDetailsType
        { All, VTE, ACH }
        private RotationDesProduits item;
        public RotationDesProduits Item
        {
            get { return item; }
            set { item = value; }
        }

        DateTime? startDate = DateTime.Now.AddMonths(-1);
        public DateTime? StartDate
        {
            get { return startDate; }
            set { startDate = value; }
        }

        DateTime? endDate = DateTime.Now;
        public DateTime? EndDate
        {
            get { return endDate; }
            set { endDate = value; }
        }

        private RotationDetailsType rotationDisplayType = RotationDetailsType.All;
        public RotationDetailsType RotationDisplayType
        {
            get { return rotationDisplayType; }
            set
            {
                SetProperty(ref rotationDisplayType, value);
            }
        }
        
        #endregion

        public RotationProduitDetailPage(RotationDesProduits prod)
        {
            InitializeComponent();

            Title = prod.CODE_PRODUIT;

            this.Item = prod;
        }

        public RotationProduitDetailPage(RotationDesProduits prod,DateTime? startDate, DateTime? endDate)
        {
            InitializeComponent();

            Title = prod.CODE_PRODUIT;

            this.Item = prod;

            this.startDate = startDate;
            this.endDate = endDate;
        }

        public RotationProduitDetailPage(string codeProduit)
        {
            InitializeComponent();

            Title = codeProduit;

        }



        private async void LoadItemsComamand()
        {
            try
            {
                if (await App.IsConected())
                {
                    UserDialogs.Instance.ShowLoading(AppResources.txt_Loading);
                    //if (Item==null)
                    //{
                    //    Item = await WebServiceClient.getr(Item.CODE_PRODUIT, StartDate?.ToString("yyyyMMdd"), EndDate?.ToString("yyyyMMdd"), ((int)RotationDisplayType).ToString());
                    //}
                    ItemRows.Clear();
                    //StartDate.ToString("yyyyMMdd")
                    var items = await WebServiceClient.GetRotationProduitDetails(Item.CODE_PRODUIT, StartDate?.ToString("yyyyMMdd"), EndDate?.ToString("yyyyMMdd"), ((int)RotationDisplayType).ToString());
                    ItemRows = new ObservableCollection<RotationDesProduitsDetails>(items);
                    UserDialogs.Instance.HideLoading();

                    int i = 0;
                    foreach (var item in ItemRows)
                    {
                        i += 1;
                        (item as BASE_CLASS).Index = i;
                        item.DESIGNATION = Item.DESIGNATION;
                    }


                }
            }
            catch (Exception ex)
            {
                await UserDialogs.Instance.AlertAsync(WSApi2.GetExceptionMessage(ex), AppResources.alrt_msg_Alert,
                    AppResources.alrt_msg_Ok);
            }


        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            BindingContext = this;
            ItemRows = new ObservableCollection<RotationDesProduitsDetails>();
            LoadItemsComamand();
            RefreshSubTitle();

        }


        private void RefreshSubTitle()
        {
            if (RotationDisplayType == RotationDetailsType.All)
            {
                SubTitle.Text = AppResources.rdp_Touts;
            }
            else if (RotationDisplayType == RotationDetailsType.ACH)
            {
                SubTitle.Text = AppResources.rdp_Ach;
            }
            else if (RotationDisplayType == RotationDetailsType.VTE)
            {
                SubTitle.Text = AppResources.rdp_Vte;
            }
        }

        private void TypeFilter_Clicked(object sender, EventArgs e)
        {

            if (((Flex.Controls.FlexButton)sender).Text == AppResources.rdp_Touts)
            {
                RotationDisplayType = RotationDetailsType.All;
            }
            else if (((Flex.Controls.FlexButton)sender).Text == AppResources.rdp_Ach)
            {
                RotationDisplayType = RotationDetailsType.ACH;
            }
            else if (((Flex.Controls.FlexButton)sender).Text == AppResources.rdp_Vte)
            {
                RotationDisplayType = RotationDetailsType.VTE;
            }
            RefreshSubTitle();
            LoadItemsComamand();
        }

        private void Handle_Change(object sender, DateChangedEventArgs e)
        {
            IsBusy = true;
            LoadItemsComamand();
            IsBusy = false;
        }

        private void RefreshView_Refreshing(object sender, EventArgs e)
        {
            LoadItemsComamand();
            IsBusy = false;
        }
    }
}