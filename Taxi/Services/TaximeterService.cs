using System;
using System.Threading.Tasks;
using Plugin.Geolocator;
using Plugin.Geolocator.Abstractions;

namespace Taxi.Services
{
    public class TaximeterService
    {
        public event EventHandler TaxiMoved;

        protected void OnTaxiMoved()
        {
            if (TaxiMoved != null)
                TaxiMoved(this, EventArgs.Empty);
        }

        public event EventHandler RunStarted;

        protected void OnRunStarted()
        {
            if (RunStarted != null)
                RunStarted(this, EventArgs.Empty);
        }

        public event EventHandler RunStoped;

        protected void OnRunStoped()
        {
            if (RunStoped != null)
                RunStoped(this, EventArgs.Empty);
        }

        /// <summary>
        /// Gets or sets the is running.
        /// </summary>
        /// <value>The is running.</value>
        public bool IsRunning
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the run cost.
        /// </summary>
        /// <value>The run cost.</value>
        public string RunCost
        {
            get;
            set;
        }

        const decimal RunValue = 5;
        decimal m_cost = 0;
        IGeolocator m_locator;
        Position m_currentPosition;

        /// <summary>
        /// Initializes a new instance of the <see cref="T:Taxi.Services.TaximeterService"/> class.
        /// </summary>
        public TaximeterService()
        {
            m_locator = CrossGeolocator.Current;
            m_locator.PositionChanged += (object sender, PositionEventArgs eventArgs) => MoveTaxi(eventArgs);
        }

        /// <summary>
        /// Resets the run.
        /// </summary>
        /// <returns>The run.</returns>
        public void ResetRun()
        {
            m_cost = 0;
        }

        /// <summary>
        /// Moves the taxi.
        /// </summary>
        /// <returns>The new position.</returns>
        /// <param name="eventArgs">Event arguments.</param>
        Xamarin.Forms.Maps.Position MoveTaxi(PositionEventArgs eventArgs)
        {
            RunCost = CalculateRunCost(m_currentPosition, eventArgs.Position);

            OnTaxiMoved();

            m_currentPosition = eventArgs.Position;

            return new Xamarin.Forms.Maps.Position(eventArgs.Position.Latitude, eventArgs.Position.Longitude);
        }

        /// <summary>
        /// Starts the run.
        /// </summary>
        /// <returns>The run.</returns>
        public void StartRun()
        {
            ResetRun();
            IsRunning = true;
            m_locator.StartListeningAsync(1000, 0);
            OnRunStarted();
        }

        /// <summary>
        /// Stops the run.
        /// </summary>
        /// <returns>The run.</returns>
        public void StopRun()
        {
            IsRunning = false;
            m_locator.StopListeningAsync();
            OnRunStoped();
        }

        /// <summary>
        /// Gets the current location.
        /// </summary>
        /// <returns>The current location.</returns>
        public async Task<Position> GetCurrentLocation()
        {
            m_locator.DesiredAccuracy = 50;

            var position = await m_locator.GetPositionAsync(10000);

            m_currentPosition = position;

            return position;
        }

        /// <summary>
        /// Calculates the run cost.
        /// </summary>
        /// <returns>The run cost.</returns>
        /// <param name="oldPoint">Old point.</param>
        /// <param name="centerPoint">Center point.</param>
        public string CalculateRunCost(Position oldPoint, Position centerPoint)
        {
            var distance = DistanceInMetres(oldPoint.Latitude, oldPoint.Longitude, centerPoint.Latitude, centerPoint.Longitude);
            m_cost += distance / 1000 * RunValue;

            return m_cost.ToString("C");
        }

        /// <summary>
        /// Distances the in metres.
        /// </summary>
        /// <returns>The in metres.</returns>
        /// <param name="lat1">Lat1.</param>
        /// <param name="lon1">Lon1.</param>
        /// <param name="lat2">Lat2.</param>
        /// <param name="lon2">Lon2.</param>
        public decimal DistanceInMetres(double lat1, double lon1, double lat2, double lon2)
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

