using System;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows.Input;
using FreshMvvm;
using Plugin.Geolocator;
using Plugin.Permissions;
using Plugin.Permissions.Abstractions;
using Xamarin.Forms;
using Xamarin.Forms.Maps;


namespace Taxi.PageModels
{
    public class MapPageModel : FreshBasePageModel
    {
        public ICommand StartRunCommand
        {
            get;
            set;
        }

        public Position CenterPoint
        {
            get;
            set;
        }

        public string ButtonText
        {
            get;
            set;
        } = "Start";

        public Distance Radius
        {
            get;
            set;
        } = Distance.FromKilometers(5);

        Plugin.Geolocator.Abstractions.IGeolocator m_locator;

        public MapPageModel()
        {
        }

        public override async void Init(object initData)
        {
            base.Init(initData);
            StartRunCommand = new Command(async () => await GetCurrentLocation());
            await GetCurrentLocation();
        }

        async Task<Plugin.Geolocator.Abstractions.Position> GetCurrentLocation()
        {
            await AskPersmissionToUseGPS();

            m_locator = CrossGeolocator.Current;
            m_locator.DesiredAccuracy = 50;

            var position = await m_locator.GetPositionAsync(10000);

            CenterPoint = new Position(position.Latitude, position.Longitude);

            return position;
        }

        async Task AskPersmissionToUseGPS()
        {
            var status = await CrossPermissions.Current.CheckPermissionStatusAsync(Permission.Location);
            if (status != PermissionStatus.Granted)
            {
                if (await CrossPermissions.Current.ShouldShowRequestPermissionRationaleAsync(Permission.Location))
                {
                    await CoreMethods.DisplayAlert("Need location", "Gunna need that location", "Cancel");
                }

                var results = await CrossPermissions.Current.RequestPermissionsAsync(new[] { Permission.Location });
                status = results[Permission.Location];
            }
        }
    }
}

