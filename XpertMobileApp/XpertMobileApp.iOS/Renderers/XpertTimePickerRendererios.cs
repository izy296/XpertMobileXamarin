using XpertMobileApp.CustomControls;
using Xamarin.Forms.Platform.iOS;
using Xamarin.Forms;
using XpertMobileApp.iOS.Renderers;

[assembly: ExportRenderer (typeof(XpertTimePicker), typeof(XpertTimePickerRenderer))]
namespace XpertMobileApp.iOS.Renderers
{
    public class XpertTimePickerRenderer : TimePickerRenderer
    {

    }
}
