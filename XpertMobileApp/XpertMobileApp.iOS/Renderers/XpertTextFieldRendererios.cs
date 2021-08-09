using CoreGraphics; 
using Foundation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;
using XpertMobileApp.CustomControls;
using XpertMobileApp.iOS.Renderers;

[assembly: ExportRenderer(typeof(XpertTextField), typeof(XpertEntryRendererIos))]
namespace XpertMobileApp.iOS.Renderers
{
    public class XpertEntryRendererIos : EntryRenderer
    {
        protected override void OnElementChanged(ElementChangedEventArgs<Entry> e)
        {
            base.OnElementChanged(e);
            
            if (e.OldElement == null) 
            {
                //Control.Layer.CornerRadius = 10;
                //Control.Layer.BorderWidth = 2f;
                //Control.Layer.BorderColor = Color.DeepPink.ToCGColor();
                //Control.Layer.BackgroundColor = Color.LightGray.ToCGColor();

                //Control.LeftView = new UIKit.UIView(new CGRect(0, 0, 10, 0));
                Control.LeftViewMode = UIKit.UITextFieldViewMode.Always;
            }
        }
    }
}