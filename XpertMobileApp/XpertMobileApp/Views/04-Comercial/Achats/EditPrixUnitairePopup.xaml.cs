using Rg.Plugins.Popup.Pages;
using Rg.Plugins.Popup.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using Syncfusion.XForms.Buttons;
using System.Threading.Tasks;
using Syncfusion.XForms.TextInputLayout;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using XpertMobileApp.DAL;
using XpertMobileApp.Models;
using XpertMobileApp.SQLite_Managment;
using XpertMobileApp.Services;

namespace XpertMobileApp.Views._04_Comercial.Achats
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    // Define a custom event argument 
    public class EditAchatArgs
    {
        public decimal? QUANTITY { get; set; } = 0;
        public decimal PRIX_UNITAIRE { get; set; } = 0;
    }
    public partial class EditPrixUnitairePopup : PopupPage, INotifyPropertyChanged
    {
        // For getting the popup result ...
        private TaskCompletionSource<bool> taskCompletionSource;
        public Task<bool> PopupClosedTask { get { return taskCompletionSource.Task; } }

        private decimal? countUnite = 0;
        public decimal? CountUnite
        {
            get
            {
                return countUnite;
            }
            set
            {
                countUnite = value;
                OnPropertyChanged("CountUnite");
            }
        }

        private decimal? countColis = 0;
        public decimal? CountColis
        {
            get
            {
                return countColis;
            }
            set
            {
                countColis = value;
                OnPropertyChanged("CountColis");
            }
        }

        private decimal? quantityTotal = 0;

        public decimal? QuantityTotal
        {
            get { return quantityTotal; }
            set
            {
                quantityTotal = value;
                OnPropertyChanged("QuantityTotal");
            }
        }

        private EditAchatArgs result;
        private List<View_BSE_PRODUIT_AUTRE_UNITE> unitesList { get; set; }
        public List<View_BSE_PRODUIT_AUTRE_UNITE> UnitesList
        {
            get
            {
                return unitesList;
            }
            set
            {
                unitesList = value;
                OnPropertyChanged("UnitesList");
            }
        }
        public EditAchatArgs Result
        {
            get { return result; }
            set
            {
                result = value;
                OnPropertyChanged("Result");
            }
        }
        private View_ACH_DOCUMENT_DETAIL produit { get; set; }
        public View_ACH_DOCUMENT_DETAIL Produit
        {
            get
            {
                return produit;
            }
            set
            {
                produit = value;
                OnPropertyChanged("Produit");
            }
        }

        public EditPrixUnitairePopup(View_ACH_DOCUMENT_DETAIL Produit)
        {
            InitializeComponent();
            this.CloseWhenBackgroundIsClicked = false;
            taskCompletionSource = new TaskCompletionSource<bool>();
            Result = new EditAchatArgs();
            UnitesList = new List<View_BSE_PRODUIT_AUTRE_UNITE>();
            if (Produit != null)
            {
                this.Produit = Produit;
                produit_label.Text = Produit.DESIGNATION_PRODUIT;
                QuantityTotal = result.QUANTITY = CountUnite = produit.QUANTITE;
                result.PRIX_UNITAIRE = produit.PRIX_UNITAIRE;
            }
            BindingContext = this;
        }
        protected override async void OnAppearing()
        {
            base.OnAppearing();
            if (UnitesList != null && UnitesList.Count <= 0)
            {
                if (App.Online)
                {
                    // get unite by produit 
                    UnitesList = await WebServiceClient.GetProduitAutreUnite(Produit.CODE_PRODUIT);
                }
                else
                {
                    UnitesList = await SQLite_Manager.GetUniteByProduit(Produit.CODE_PRODUIT) as List<View_BSE_PRODUIT_AUTRE_UNITE>;
                }

                if (UnitesList.Count > 0)
                {
                    foreach (var unite in UnitesList)
                    {
                        //Definning the grid ...

                        Grid grid = new Grid();
                        grid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(58) });
                        grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(6, GridUnitType.Star) });
                        grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(48) });
                        grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(48) });

                        // add the entry to the grid 

                        SfTextInputLayout inputLayout = new SfTextInputLayout
                        {
                            Hint = "Colis",
                            Margin = new Thickness(5, 0, 5, 0),
                            InputViewPadding = new Thickness(14),
                            IsHintAlwaysFloated = true,
                            FocusedColor = (Color)Application.Current.Resources["Primary"],
                            UnfocusedColor = (Color)Application.Current.Resources["Primary"],
                            ContainerType = ContainerType.Outlined,
                            OutlineCornerRadius = 5,

                            InputView = new Entry
                            {
                                FontSize = 12,
                            },
                            HelperLabelStyle = new LabelStyle
                            {
                                FontSize = 15
                            }
                        };

                        // definning the entry ..
                        Entry entry = new Entry
                        {
                            FontSize = 14,
                            Keyboard = Keyboard.Numeric,
                            Text = "0",
                        };

                        // add the entry inside the layout...
                        inputLayout.InputView = entry;

                        // add the Layout inside the grid...
                        grid.Children.Add(inputLayout, 0, 0);

                        // add the Buttons to the Grid 
                        SfButton buttonAdd = new SfButton
                        {
                            VerticalOptions = LayoutOptions.End,
                            Text = "+" + unite.COEFFICIENT.ToString("0.00"),
                            TextColor = Color.White,
                            CornerRadius = 50,
                            BackgroundColor = Color.FromHex("#7EC384"),
                            BorderColor = Color.FromHex("#7EC384"),
                            BorderWidth = 1,
                            FontSize = 14,
                            HeightRequest = 48,
                            WidthRequest = 48,
                        };


                        buttonAdd.CommandParameter = unite.COEFFICIENT;
                        buttonAdd.Clicked += AddUnite;



                        //add the second button to the grid 
                        SfButton subsButton = new SfButton
                        {
                            VerticalOptions = LayoutOptions.End,
                            Text = "-" + unite.COEFFICIENT.ToString("0.00"),
                            TextColor = Color.White,
                            CornerRadius = 50,
                            BackgroundColor = Color.FromHex("#e65b65"),
                            BorderColor = Color.FromHex("#e65b65"),
                            BorderWidth = 1,
                            FontSize = 14,
                            HeightRequest = 48,
                            WidthRequest = 48,
                        };
                        subsButton.CommandParameter = unite.COEFFICIENT;
                        subsButton.Clicked += SubstractUnite;
                        grid.Children.Add(buttonAdd, 2, 0);
                        grid.Children.Add(subsButton, 1, 0);
                        // add the grid to the stack layout
                        inputsWrapper.Children.Add(grid);
                    }
                }
            }
        }
        private async void Close_filter_popup(object sender, EventArgs e)
        {
            try
            {
                Result.QUANTITY = QuantityTotal;
                taskCompletionSource.SetResult(true);
                await PopupNavigation.Instance.PopAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        private async void Close_filter_popup_with_Icon(object sender, EventArgs e)
        {
            try
            {
                taskCompletionSource.SetResult(false);
                await PopupNavigation.Instance.PopAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        private void AddUnite(object sender, EventArgs e)
        {
            if (CountUnite >= 0)
            {

                var coeff = ((SfButton)sender).CommandParameter as decimal?;
                if (coeff == null)
                {
                    coeff = 1;
                    CountUnite += coeff;
                }
                else
                {
                    CountColis += coeff;
                    ((((sender as SfButton).Parent as Grid).Children[0] as SfTextInputLayout).InputView as Entry).Text = (countColis / coeff).ToString();
                }
                QuantityTotal = countUnite + CountColis;
            }
        }

        private void SubstractUnite(object sender, EventArgs e)
        {
            var coeff = ((SfButton)sender).CommandParameter as decimal?;
            if (coeff == null)
            {
                if (countUnite > 0)
                {
                    coeff = 1;
                    CountUnite -= coeff;
                }
            }
            else
            {
                if (countUnite - coeff > 0)
                {
                    CountColis -= coeff;
                    ((((sender as SfButton).Parent as Grid).Children[0] as SfTextInputLayout).InputView as Entry).Text = (countColis / coeff).ToString();
                }
            }
            QuantityTotal = countUnite + CountColis;
        }


        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

}