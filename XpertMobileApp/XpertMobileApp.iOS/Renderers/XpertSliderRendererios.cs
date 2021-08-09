using XpertMobileApp.CustomControls;
using Xamarin.Forms.Platform.iOS;
using Xamarin.Forms;
using XpertMobileApp.iOS.Renderers;

[assembly: ExportRenderer (typeof(XpertSlider), typeof(XpertSliderRenderer))]
namespace XpertMobileApp.iOS.Renderers
{
    public class XpertSliderRenderer : SliderRenderer
    {

    }
}
