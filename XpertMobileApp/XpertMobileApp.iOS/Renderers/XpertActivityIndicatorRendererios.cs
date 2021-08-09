using XpertMobileApp.CustomControls;
using Xamarin.Forms.Platform.iOS;
using Xamarin.Forms;
using XpertMobileApp.iOS.Renderers;

[assembly: ExportRenderer (typeof(XpertActivityIndicator), typeof(XpertActivityIndicatorRenderer))]
namespace XpertMobileApp.iOS.Renderers
{
    public class XpertActivityIndicatorRenderer : ActivityIndicatorRenderer
    {

    }
}
