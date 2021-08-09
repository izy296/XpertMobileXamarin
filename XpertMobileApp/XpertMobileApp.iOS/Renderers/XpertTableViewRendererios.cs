using XpertMobileApp.CustomControls;
using Xamarin.Forms.Platform.iOS;
using Xamarin.Forms;
using XpertMobileApp.iOS.Renderers;

[assembly: ExportRenderer (typeof(XpertTableView), typeof(XpertTableViewRenderer))]
namespace XpertMobileApp.iOS.Renderers
{
    public class XpertTableViewRenderer : TableViewRenderer
    {

    }
}
