using XpertMobileApp.CustomControls;
using Xamarin.Forms.Platform.iOS;
using Xamarin.Forms;
using XpertMobileApp.iOS.Renderers;

[assembly: ExportRenderer (typeof(XpertCarouselView), typeof(XpertCarouselViewRenderer))]
namespace XpertMobileApp.iOS.Renderers
{
    public class XpertCarouselViewRenderer : CarouselViewRenderer
    {

    }
}
