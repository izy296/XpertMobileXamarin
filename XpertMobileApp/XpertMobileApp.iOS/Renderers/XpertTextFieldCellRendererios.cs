using XpertMobileApp.CustomControls;
using Xamarin.Forms.Platform.iOS;
using Xamarin.Forms;
using XpertMobileApp.iOS.Renderers;

[assembly: ExportRenderer (typeof(XpertTextFieldCell), typeof(XpertTextFieldCellRenderer))]
namespace XpertMobileApp.iOS.Renderers
{
    public class XpertTextFieldCellRenderer : EntryCellRenderer
    {

    }
}
