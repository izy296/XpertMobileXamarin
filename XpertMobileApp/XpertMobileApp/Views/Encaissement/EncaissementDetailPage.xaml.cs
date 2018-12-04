using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Xpert.Pharm.DAL;
using XpertMobileApp.Helpers;
using XpertMobileApp.Models;
using XpertMobileApp.ViewModels;

namespace XpertMobileApp.Views.Encaissement
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class EncaissementDetailPage : ContentPage
	{
        ItemDetailViewModel<View_TRS_ENCAISS> viewModel;

        public EncaissementDetailPage(ItemDetailViewModel<View_TRS_ENCAISS> viewModel)
        {
            InitializeComponent();

            BindingContext = this.viewModel = viewModel;



            foreach (PropertyDescriptor descriptor in TypeDescriptor.GetProperties(viewModel.Item))
            {
                string name = descriptor.Name;
                object value = descriptor.GetValue(viewModel.Item);
                if(value is DateTime)
                {
                    value =  string.Format("{0:dd/MM/yyyy HH:mm}", value);
                } else if(value is decimal)
                {
                    value = string.Format("{0:F2} DA", value);
                }

                var attribute = GetFieldInfosAttribue(typeof(View_TRS_ENCAISS), name);

                if (attribute == null || !attribute.VisibleInFich) continue;

                string[] styleFN = new String[] { "lbl_fieldName" };
                var lbl_FieldName = new Label
                {
                    Text = TranslateExtension.GetTranslation(name),
                    FontSize = Device.GetNamedSize(NamedSize.Medium, typeof(Label)),
                    FontAttributes = FontAttributes.Bold,
                    StyleClass = styleFN
                };
                sl_content.Children.Add(lbl_FieldName);

                string[] styleFV = new String[] { "lbl_fieldValue" };
                var lbl_FieldValue = new Label
                {
                    Text       = Convert.ToString(value),
                    FontSize   = Device.GetNamedSize(NamedSize.Small, typeof(Label)),
                    TextColor = Color.Black,
                    StyleClass = styleFV
                };
                sl_content.Children.Add(lbl_FieldValue);
            }
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
    }
}