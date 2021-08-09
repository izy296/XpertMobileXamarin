using XpertMobileApp.CustomControls;
using Xamarin.Forms.Platform.iOS;
using Xamarin.Forms;
using XpertMobileApp.iOS.Renderers;

[assembly: ExportRenderer (typeof(XpertSwitch), typeof(XpertSwitchRenderer))]
namespace XpertMobileApp.iOS.Renderers
{
    public class XpertSwitchRenderer : SwitchRenderer
    {

    }
}
