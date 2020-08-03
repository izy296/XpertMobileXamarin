using XpertMobileApp.Services;
using Xamarin.Forms;

[assembly: Dependency(typeof(XpertMobileApp.IOS.CloseApplication))]
namespace XpertMobileApp.IOS
{
    public class CloseApplication : ICloseApplication
    {
        public void closeApplication()
        {
            System.Threading.Thread.CurrentThread.Abort();
        }
    }
}