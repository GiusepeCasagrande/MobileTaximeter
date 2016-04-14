using System.Diagnostics;
using CoreLocation;
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

            LogFonts();

            return base.FinishedLaunching(app, options);
        }

        /// <summary>
        /// Logs the installed fonts to the debug window.
        /// </summary>
        private void LogFonts()
        {
            foreach (NSString family in UIFont.FamilyNames)
            {
                foreach (NSString font in UIFont.FontNamesForFamilyName(family))
                {
                    Debug.WriteLine(@"{0}", font);
                }
            }
        }
    }
}

