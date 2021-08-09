using XpertMobileApp.CustomControls;
using Xamarin.Forms.Platform.iOS;
using Xamarin.Forms;
using XpertMobileApp.iOS.Renderers;

[assembly: ExportRenderer (typeof(XpertSwitchCell), typeof(XpertSwitchCellRenderer))]
namespace XpertMobileApp.iOS.Renderers
{
    public class XpertSwitchCellRenderer : SwitchCellRenderer
    {

    }
}
