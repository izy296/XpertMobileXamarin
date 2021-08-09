using XpertMobileApp.CustomControls;
using Xamarin.Forms.Platform.iOS;
using Xamarin.Forms;
using XpertMobileApp.iOS.Renderers;

[assembly: ExportRenderer (typeof(XpertSwipeView), typeof(XpertSwipeViewRenderer))]
namespace XpertMobileApp.iOS.Renderers
{
    public class XpertSwipeViewRenderer : SwipeViewRenderer
    {

    }
}
