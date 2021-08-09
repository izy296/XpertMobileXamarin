using XpertMobileApp.CustomControls;
using Xamarin.Forms.Platform.iOS;
using Xamarin.Forms;
using XpertMobileApp.iOS.Renderers;

[assembly: ExportRenderer (typeof(XpertCheckBox), typeof(XpertCheckBoxRenderer))]
namespace XpertMobileApp.iOS.Renderers
{
    public class XpertCheckBoxRenderer : CheckBoxRenderer
    {

    }
}
