using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using XpertMobileApp.DAL;
using XpertMobileApp.Helpers;
using XpertMobileApp.Models;
using XpertMobileApp.Services;
using XpertMobileApp.ViewModels;
using XpertMobileSettingsPage.Helpers.Services;

namespace XpertMobileApp.Views.Encaissement
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class EncaissementDetailPage : ContentPage
	{
        ItemDetailViewModel<View_TRS_ENCAISS> viewModel;

        public EncaissementDetailPage(View_TRS_ENCAISS encaiss)
        {
            InitializeComponent();

            BindingContext = this.viewModel = new ItemDetailViewModel<View_TRS_ENCAISS>(encaiss);

            displayObject(typeof(View_TRS_ENCAISS), viewModel.Item);

            // TODO put into th generic view model 
            MessagingCenter.Subscribe<MsgCenter, View_TRS_ENCAISS>(this, MCDico.REFRESH_ITEM, async (obj, item) =>
            {
                displayObject(typeof(View_TRS_ENCAISS), item);
                // viewModel.Item = item;
            });
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
        }

        private void displayObject(Type type, object obj)
        {
            if (obj == null) return;

            sl_content.Children.Clear();
            foreach (PropertyDescriptor descriptor in TypeDescriptor.GetProperties(obj))
            {
                string name = descriptor.Name;
                object value = descriptor.GetValue(obj);
                value = getFormatedValue(value);

                var attribute = GetFieldInfosAttribue(type, name);

                if (attribute == null || !attribute.VisibleInFich) continue;

                string[] styleFN = new String[] { "lbl_fieldName" };
                var lbl_FieldName = new Label
                {
                    Text = TranslateExtension.GetTranslation(name) + " : ",
                    FontSize = Device.GetNamedSize(NamedSize.Medium, typeof(Label)),
                    FontAttributes = FontAttributes.Bold,
                    TextColor = Color.LightGray,
                    StyleClass = styleFN
                };
                sl_content.Children.Add(lbl_FieldName);

                string[] styleFV = new String[] { "lbl_fieldValue" };
                var lbl_FieldValue = new Label
                {
                    Text = Convert.ToString(value),
                    FontSize = Device.GetNamedSize(NamedSize.Small, typeof(Label)),
                    TextColor = Color.Black,
                    Margin = new Thickness(10, 0, 0, 0),
                    StyleClass = styleFV
                };
                sl_content.Children.Add(lbl_FieldValue);
            }
        }

        private string getFormatedValue(object value)
        {
            if (value == null) return "";

            string result = value.ToString();

            if (value is DateTime)
            {
                result = string.Format("{0:dd/MM/yyyy HH:mm}", value);
            }
            else if (value is decimal)
            {
                result = string.Format("{0:F2} DA", value);
            }
            else
            {
                result = string.Format("{0}", value);
            }

            return result;
        }

        private FieldInfos GetFieldInfosAttribue(Type type, string fieldName)
        {
            try
            { 
                var attrs = (FieldInfos[])type.GetProperty(fieldName).GetCustomAttributes(typeof(FieldInfos), false);
                if(attrs.Count()> 0)
                {
                    return attrs[0];
                }
            }
            catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return null;
        }

        public EncaissementDetailPage()
        {
            InitializeComponent();

            var item = new View_TRS_ENCAISS
            {
                NUM_ENCAISS = "Item 1",
                DESIGN_COMPTE = "This is a compte.."
            };

            viewModel = new ItemDetailViewModel<View_TRS_ENCAISS>(item);
            BindingContext = viewModel;
        }

        async void Delete_Clicked(object sender, EventArgs e)
        {
            var res = await DisplayAlert(AppResources.msg_Confirmation, AppResources.msg_DeleteConfirmation, AppResources.alrt_msg_Ok, AppResources.alrt_msg_Cancel);
            if (res)
            {
                MessagingCenter.Send(App.MsgCenter, MCDico.DELETE_ITEM, viewModel.Item);
                await Navigation.PopAsync();
            }
        }

        async void btnImprimer_Clicked(object sender, EventArgs e)
        {
            if (SettingsPage.printerLocal != null && SettingsPage.printerLocal.isConnected())
            {
                var tecketData = await WebServiceClient.GetDataTecketCaisseEncaisse(this.viewModel.Item.CODE_ENCAISS);
                if (tecketData == null)
                {
                    await DisplayAlert(AppResources.alrt_msg_Info, "Pas de donnees a imprimer !", AppResources.alrt_msg_Ok);
                }
                else
                {
                    PrinterHelper.PrintEncaisse(tecketData);
                }
            }
        }
        async void Cancel_Clicked(object sender, EventArgs e)
        {
            await Navigation.PopAsync();
        }

        private async void Update_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushModalAsync(new NavigationPage(new NewEncaissementPage(viewModel.Item)));
        }
    }
}