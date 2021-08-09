using XpertMobileApp.CustomControls;
using Xamarin.Forms.Platform.iOS;
using Xamarin.Forms;
using XpertMobileApp.iOS.Renderers;

[assembly: ExportRenderer (typeof(XpertProgressBar), typeof(XpertProgressBarRenderer))]
namespace XpertMobileApp.iOS.Renderers
{
    public class XpertProgressBarRenderer : ProgressBarRenderer
    {

    }
}
