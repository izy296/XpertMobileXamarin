using XpertMobileApp.CustomControls;
using Xamarin.Forms.Platform.iOS;
using Xamarin.Forms;
using XpertMobileApp.iOS.Renderers;

[assembly: ExportRenderer (typeof(XpertEditor), typeof(XpertEditorRenderer))]
namespace XpertMobileApp.iOS.Renderers
{
    public class XpertEditorRenderer : EditorRenderer
    {

    }
}
