using System;
using Plugin.Geolocator.Abstractions;

namespace Taxi.Domain.Taximeter
{
    /// <summary>
    /// Position.
    /// </summary>
    public class TaxiPosition : Position
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="T:Taxi.Domain.Taximeter.Position"/> class.
        /// </summary>
        /// <param name="latitude">Latitude.</param>
        /// <param name="longitude">Longitude.</param>
        public TaxiPosition(double latitude, double longitude)
        {
            Latitude = latitude;
            Longitude = longitude;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:Taxi.Domain.Taximeter.TaxiPosition"/> class.
        /// </summary>
        /// <param name="position">Position.</param>
        public TaxiPosition(Position position)
        {
            Latitude = position.Latitude;
            Longitude = position.Longitude;
        }

        /// <summary>
        /// Distances the in metres.
        /// </summary>
        /// <returns>The in metres.</returns>
        /// <param name="lat1">Lat1.</param>
        /// <param name="lon1">Lon1.</param>
        /// <param name="lat2">Lat2.</param>
        /// <param name="lon2">Lon2.</param>
        public static decimal DistanceInMetres(double lat1, double lon1, double lat2, double lon2)
        {

            if (lat1 == lat2 && lon1 == lon2)
                return 0.0m;

            var theta = lon1 - lon2;

            var distance = Math.Sin(deg2rad(lat1)) * Math.Sin(deg2rad(lat2)) +
                           Math.Cos(deg2rad(lat1)) * Math.Cos(deg2rad(lat2)) *
                           Math.Cos(deg2rad(theta));

            distance = Math.Acos(distance);
            if (double.IsNaN(distance))
                return 0.0m;

            distance = rad2deg(distance);
            distance = distance * 60.0 * 1.1515 * 1609.344;

            return Convert.ToDecimal(distance);
        }

        static double deg2rad(double deg)
        {
            return (deg * Math.PI / 180.0);
        }

        static double rad2deg(double rad)
        {
            return (rad / Math.PI * 180.0);
        }
    }
}

