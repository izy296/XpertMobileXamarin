using System;

using Android.App;
using Android.Content.PM;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using Acr.UserDialogs;
using Plugin.FirebasePushNotification;
using Android.Content;
using Android;
using Android.Support.V4.App;
using Android.Util;
using Android.Support.Design.Widget;
using FFImageLoading.Forms.Platform;
using Firebase;
using XpertMobileApp.Views;
using System.Threading.Tasks;
using Xamarin.Forms.Platform.Android;
using Xamarin.Forms;
using AndroidX.Navigation;
using Rg.Plugins.Popup.Extensions;
using Rg.Plugins.Popup.Services;
using ListView = Xamarin.Forms.ListView;

namespace XpertMobileApp.Droid
{
    // changer LaunchMode en singletop et supprimer MainLauncher car SplashActivty a MainLauncher défini sur true
    [Activity(Label = "XpertMobile OFFICINE", Icon = "@drawable/icon", Theme = "@style/MainTheme", LaunchMode = LaunchMode.SingleTop, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
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

            UserDialogs.Init(this);

            FFImageLoading.Forms.Platform.CachedImageRenderer.Init(true);

            ZXing.Net.Mobile.Forms.Android.Platform.Init();

            GoogleVisionBarCodeScanner.Droid.RendererInitializer.Init();

            FirebaseApp.InitializeApp(Application.ApplicationContext);

            Xamarin.Forms.Forms.SetFlags("SwipeView_Experimental");
            global::Xamarin.Forms.Forms.Init(this, savedInstanceState);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);

            LoadApplication(new App());

            // fonction qui empêche l'application de se fermer par une exception

            AndroidEnvironment.UnhandledExceptionRaiser += (s, e) =>
            {
                try
                {
                    throw e.Exception;
                }
                catch (InvalidOperationException ex)
                {
                    if (e.Exception.Message.Contains("The page has been pushed already"))
                        e.Handled = true;
                }
                catch (Exception ex)
                {
                    e.Handled = true;
                    UserDialogs.Instance.AlertAsync(ex.Message, "Alert", "Ok");
                }

            };

            // function que lance avant l'execution de PopupNavigation.PushAsync 
            // confirme qui le popupstack est vide pour eviter les exceptions

            PopupNavigation.Instance.Pushing += (s, e) =>
            {
                if (PopupNavigation.Instance.PopupStack.Count != 0)
                {
                    var lastPopup = PopupNavigation.Instance.PopupStack[PopupNavigation.Instance.PopupStack.Count - 1];
                    if (lastPopup == e.Page)
                        PopupNavigation.Instance.PopAsync();
                }
            };



            //          System.InvalidOperationException
            //Message = The page has been pushed already.Pop or remove the page before to push it again



            FirebasePushNotificationManager.ProcessIntent(this, Intent);

            // verifier les permission 
            /*
            if (Build.VERSION.SdkInt >= BuildVersionCodes.M)
            {
                if (ApplicationContext.CheckSelfPermission(Android.Manifest.Permission.Camera) != Android.Content.PM.Permission.Granted)
                {
                    RequestPermissions(new String[] { Android.Manifest.Permission.Camera }, 1);
                }
                else
                {
                }

                if (ApplicationContext.CheckSelfPermission(
                        Manifest.Permission.AccessCoarseLocation) != Android.Content.PM.Permission.Granted)
                {
                    RequestPermissions(new String[] { Manifest.Permission.AccessCoarseLocation },
                                    1);
                }
                if (ApplicationContext.CheckSelfPermission(Manifest.Permission.ReadExternalStorage) != Android.Content.PM.Permission.Granted)
                {

                    RequestPermissions(new String[] {
                        Manifest.Permission.WriteExternalStorage,
                        Manifest.Permission.ReadExternalStorage}, 1);
                    return;
                }
                else
                {


                }

            }
            else
            {

            }
            */
            // InitPermissions();
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

        bool _isBackPressed = false;
        public override void OnBackPressed()
        {
            // Init popup plugin Back button Pressed
            /*            if (Rg.Plugins.Popup.Popup.SendBackPressed(base.OnBackPressed))
                        {
                            // Do something if there are some pages in the `PopupStack`
                        }
                        else
                        {
                            // Do something if there are not any pages in the `PopupStack`
                        }*/

            //var mainPage = App.Current.MainPage;

            //var dd = NavigationPage.CurrentPageProperty;
            try
            {

                // popup est le nombre des fenêtres  (comme le filtre de saisie par dans sortie de stock) ouvert

                var popup = PopupNavigation.PopupStack.Count;


                if (popup != 0)
                {
                    PopupNavigation.PopAsync();
                    return;
                }

                int pagesOpen = ((MasterDetailPage)App.Current.MainPage).Detail.Navigation.NavigationStack.Count;

                // testi si il ya des pages ouvert avec la counteur
                // de stack si il est egal ou inferieur a 1 donc l'utilisateur est dans le HomePage
                // alors afficher un avertisement pour quiter

                if (pagesOpen <= 1)
                {
                    ConfirmWithDialog();
                }
                else
                {
                    // vérifier s'il y a des pages ouvertes à partir de MenuPage
                    //if (pagesOpen >2)
                    //{
                    // supprimer toutes les pages de la pile et revenir à l'accueil

                    base.OnBackPressed();

                    //if (((NavigationPage)((MasterDetailPage)App.Current.MainPage).Detail).RootPage==HomePage))
                    //{
                    //    new MenuPage("1");
                    //}

                    //TODO Empty ListViewMenu item selected

                    //} 
                    //else
                    //    // exécuter l'action habituelle du bouton de retour
                    //    ((MasterDetailPage)App.Current.MainPage).Detail.Navigation.PopToRootAsync();

                }

            }
            catch (InvalidCastException ex)
            {
                // si le code jeter un exception de InvalidCastException
                // l'application est dans la fenetre de configuration et authentification
                // et la classe MasterDetailPage est n'est pas initialisé encors
                base.OnBackPressed();
            }



            //if (index == -1)
            //{
            //    App.Current.MainPage = new NavigationPage();  
            //    App.Current.MainPage.Navigation.PushAsync(new HomePage());
            //}


            //var app = (XpertMobileApp.App)App.Current;
            //if (app.PromptToConfirmExit)
            //{
            //    if (app.IsToastExitConfirmation)
            //        ConfirmWithToast();
            //    else
            //        ConfirmWithDialog();

            //    return;
            //}
            //base.OnBackPressed();

            //RunOnUiThread(
            //    async () =>
            //    {
            //        var isCloseApp = await AlertAsync(this, "NameOfApp", "Do you want to close this app?", "Yes", "No");

            //        if (isCloseApp)
            //        {
            //            //this.FinishAffinity();
            //        }
            //    });

            //var mainPage = App.Current.MainPage;

            //Device.BeginInvokeOnMainThread(async () =>
            //{
            //    var result = await mainPage.DisplayAlert("Confirmation", "Voulez vous quitter l'application ?", "Oui", "Non");
            //    if (result)
            //    {
            //        System.Diagnostics.Process.GetCurrentProcess().CloseMainWindow();
            //    }
            //});


        }

        public Task<bool> AlertAsync(Context context, string title, string message, string positiveButton, string negativeButton)
        {
            var tcs = new TaskCompletionSource<bool>();

            using (var db = new AlertDialog.Builder(context))
            {
                db.SetTitle(title);
                db.SetMessage(message);
                db.SetPositiveButton(positiveButton, (sender, args) => { tcs.TrySetResult(true); });
                db.SetNegativeButton(negativeButton, (sender, args) => { tcs.TrySetResult(false); });
                db.Show();
            }

            return tcs.Task;
        }

        private void ConfirmWithDialog()
        {
            using (var alert = new AlertDialog.Builder(this))
            {
                alert.SetTitle(AppResources.msg_ExitTitle);
                alert.SetMessage(AppResources.msg_ExitWarning);
                alert.SetPositiveButton(AppResources.exit_Button_Yes, (sender, args) => { FinishAffinity(); });
                alert.SetNegativeButton(AppResources.exit_Button_No, (sender, args) => { }); // do nothing

                var dialog = alert.Create();
                dialog.Show();
            }
            return;
        }

        private void ConfirmWithToast()
        {
            if (_isBackPressed)
            {
                FinishAffinity(); // inform Android that we are done with the activity
                return;
            }

            _isBackPressed = true;
            Toast.MakeText(this, "Press back again to exit", ToastLength.Short).Show();

            // Disable back to exit after 2 seconds.
            new Handler().PostDelayed(() => { _isBackPressed = false; }, 2000);
            return;
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