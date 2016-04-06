﻿using CoreLocation;
using Foundation;
using UIKit;

namespace Taxi.iOS
{
    [Register("AppDelegate")]
    public partial class AppDelegate : global::Xamarin.Forms.Platform.iOS.FormsApplicationDelegate
    {
        protected CLLocationManager locationManager = null;

        public override bool FinishedLaunching(UIApplication app, NSDictionary options)
        {
            global::Xamarin.Forms.Forms.Init();
            global::Xamarin.FormsMaps.Init();


            LoadApplication(new App());

            return base.FinishedLaunching(app, options);
        }
    }
}

