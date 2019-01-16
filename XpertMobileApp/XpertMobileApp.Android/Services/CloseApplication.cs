using Java.Lang;
using Xamarin.Forms;

[assembly: Dependency(typeof(XpertMobileApp.Droid.Services.CloseApplication))]
namespace XpertMobileApp.Droid.Services
{
    public class CloseApplication : XpertMobileApp.Services.ICloseApplication
    {
        public void closeApplication()
        {
             JavaSystem.Exit(0);
            // var activity = (Activity)Forms.Context;
            // activity.FinishAffinity();
        }
    }
}