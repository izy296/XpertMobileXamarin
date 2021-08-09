using XpertMobileApp.CustomControls;
using Xamarin.Forms.Platform.iOS;
using Xamarin.Forms;
using XpertMobileApp.iOS.Renderers;

[assembly: ExportRenderer (typeof(XpertDatePicker), typeof(XpertDatePickerRenderer))]
namespace XpertMobileApp.iOS.Renderers
{
    public class XpertDatePickerRenderer : DatePickerRenderer
    {

    }
}
