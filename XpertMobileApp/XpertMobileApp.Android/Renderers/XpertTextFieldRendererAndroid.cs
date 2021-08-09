using Android.App;
using Android.Content;
using Android.Graphics.Drawables;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using XpertMobileApp.CustomControls;
using XpertMobileApp.Droid.CustomControls;

[assembly: ExportRenderer(typeof(XpertMobileApp.CustomControls.XpertTextField), typeof(XpertMobileApp.Droid.CustomControls.XpertTextFieldRendererAndroid))]
namespace XpertMobileApp.Droid.CustomControls
{
    public class XpertTextFieldRendererAndroid : EntryRenderer
    {
        protected override void OnElementChanged(ElementChangedEventArgs<Entry> e)
        {
            base.OnElementChanged(e);
            if(e.OldElement == null)
            {
                var gradientDrawable = new GradientDrawable();
                gradientDrawable.SetCornerRadius(10f);
                //gradientDrawable.SetStroke(5, Android.Graphics.Color.DeepPink);
                gradientDrawable.SetColor(Android.Graphics.Color.ParseColor("#F5F5F5"));
                Control.SetBackground(gradientDrawable);

                Control.SetPadding(50, Control.PaddingTop, Control.PaddingRight, Control.PaddingBottom);
            }
        }
    }
}