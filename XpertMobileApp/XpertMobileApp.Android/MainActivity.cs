using Android.App;
using Android.Content.PM;
using Android.OS;
using Acr.UserDialogs;
using Plugin.FirebasePushNotification;
using Android.Content;
using Android;
using Android.Support.V4.App;
using Android.Util;

namespace XpertMobileApp.Droid
{
    [Activity(Label = "XpertDelivery", Icon = "@mipmap/icon", Theme = "@style/MainTheme", MainLauncher = false, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        internal static MainActivity Instance { get; private set; }

        static string[] PERMISSIONS_NEED = {
            Manifest.Permission.ReadPhoneState,
            Manifest.Permission.Camera
        };

        protected override void OnCreate(Bundle savedInstanceState)
        {
            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;

            base.OnCreate(savedInstanceState);

            Instance = this;

            // Init popup plugin
            Rg.Plugins.Popup.Popup.Init(this, savedInstanceState);


            Xamarin.Forms.Forms.SetFlags("SwipeView_Experimental");
            global::Xamarin.Forms.Forms.Init(this, savedInstanceState);

            Xamarin.Essentials.Platform.Init(this, savedInstanceState);

            UserDialogs.Init(this);

            FFImageLoading.Forms.Platform.CachedImageRenderer.Init(true);

            ZXing.Net.Mobile.Forms.Android.Platform.Init();

            // FirebaseApp.InitializeApp(Application.Context);

            LoadApplication(new App());

            FirebasePushNotificationManager.ProcessIntent(this, Intent);
        }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            global::ZXing.Net.Mobile.Android.PermissionsHandler.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }

        protected override void OnNewIntent(Intent intent)
        {
            base.OnNewIntent(intent);
            FirebasePushNotificationManager.ProcessIntent(this, intent);
        }

        public override void OnBackPressed()
        {
            // Init popup plugin Back button Pressed
            if (Rg.Plugins.Popup.Popup.SendBackPressed(base.OnBackPressed))
            {
                // Do something if there are some pages in the `PopupStack`
            }
            else
            {
                // Do something if there are not any pages in the `PopupStack`
            }
        }

        static readonly int REQUEST_READ_PHONE_STATE = 0;

        private void InitPermissions()
        {
            if (ActivityCompat.CheckSelfPermission(this, Manifest.Permission.ReadPhoneState) != (int)Permission.Granted)
            {
                RequestReadPhoneStatePermission();
            }
        }

        public static void RequestReadPhoneStatePermission()
        {
            Log.Info("DeviceInfos", "CAMERA permission has NOT been granted. Requesting permission.");

            if (ActivityCompat.ShouldShowRequestPermissionRationale(MainActivity.Instance, Manifest.Permission.Camera))
            {
                // Provide an additional rationale to the user if the permission was not granted
                // and the user would benefit from additional context for the use of the permission.
                // For example if the user has previously denied the permission.
                Log.Info("DeviceInfo", "Displaying camera permission rationale to provide additional context.");

                UserDialogs.Instance.AlertAsync("Read phone informations permission is needed toactivate your licence",
                                                "Alert", "Ok");
                ActivityCompat.RequestPermissions(MainActivity.Instance, new string[] { Manifest.Permission.ReadPhoneState }, REQUEST_READ_PHONE_STATE);

                /*
                Snackbar.Make(layout, "Read phone informations permission is needed toactivate your licence", Snackbar.LengthIndefinite).SetAction("Ok", new Action<Android.Views.View>(delegate (Android.Views.View obj) {
                    ActivityCompat.RequestPermissions(MainActivity.Instance, new string[] { Manifest.Permission.ReadPhoneState }, REQUEST_CAMERA);
                })).Show();
                */
            }
            else
            {
                // Camera permission has not been granted yet. Request it directly.
                ActivityCompat.RequestPermissions(MainActivity.Instance, new string[] { Manifest.Permission.ReadPhoneState }, REQUEST_READ_PHONE_STATE);
            }

        }
    }
}