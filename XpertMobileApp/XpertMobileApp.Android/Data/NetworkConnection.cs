using Android.Content;
using Android.Net;
using itMobileApp.Data;
using Xamarin.Forms;
using XpertMobileApp.Droid.Data;

[assembly: Dependency(typeof(NetworkConnection))]

namespace XpertMobileApp.Droid.Data
{
    public class NetworkConnection : INetworkConnection
    {
        public bool IsConnected { get; set; }

        public void CheckNetworkConnexction()
        {
            var connectivityManager = (ConnectivityManager)Android.App.Application.Context.GetSystemService(Context.ConnectivityService);
            var activeNetworkInfo = connectivityManager.ActiveNetworkInfo;
            IsConnected = (activeNetworkInfo != null && activeNetworkInfo.IsConnectedOrConnecting);
        }
    }
}