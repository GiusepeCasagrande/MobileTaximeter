using System;
using System.Threading.Tasks;
using Plugin.Geolocator.Abstractions;

namespace Taxi.Domain.Taximeter
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
        /// Gets or sets the current run cost.
        /// </summary>
        /// <value>The current run cost.</value>
        public decimal CurrentRunCost
        {
            get;
            set;
        }

        const decimal RunValue = 5;
        IGeolocator m_locator;
        TaxiPosition m_currentTaxiPosition;

        /// <summary>
        /// Initializes a new instance of the <see cref="T:Taxi.Domain.Taximeter.TaximeterService"/> class.
        /// </summary>
        /// <param name="locator">Locator.</param>
        public TaximeterService(IGeolocator locator)
        {
            m_locator = locator;
            m_locator.PositionChanged += (object sender, PositionEventArgs eventArgs) => MoveTaxi(eventArgs);
        }

        /// <summary>
        /// Resets the run.
        /// </summary>
        /// <returns>The run.</returns>
        public void ResetRun()
        {
            CurrentRunCost = 0;
        }

        /// <summary>
        /// Moves the taxi.
        /// </summary>
        /// <returns>The new position.</returns>
        /// <param name="eventArgs">Event arguments.</param>
        TaxiPosition MoveTaxi(PositionEventArgs eventArgs)
        {
            var newTaxiPosition = new TaxiPosition(eventArgs.Position);
            CalculateRunCost(m_currentTaxiPosition, newTaxiPosition);

            OnTaxiMoved();

            m_currentTaxiPosition = newTaxiPosition;

            return new TaxiPosition(eventArgs.Position.Latitude, eventArgs.Position.Longitude);
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
        public async Task<TaxiPosition> GetCurrentLocation()
        {
            var position = await m_locator.GetPositionAsync(10000);

            m_currentTaxiPosition = new TaxiPosition(position.Latitude, position.Longitude);

            return m_currentTaxiPosition;
        }

        /// <summary>
        /// Calculates the run cost.
        /// </summary>
        /// <returns>The run cost.</returns>
        /// <param name="oldPoint">Old point.</param>
        /// <param name="centerPoint">Center point.</param>
        public decimal CalculateRunCost(TaxiPosition oldPoint, TaxiPosition centerPoint)
        {
            var distance = TaxiPosition.DistanceInMetres(oldPoint.Latitude, oldPoint.Longitude, centerPoint.Latitude, centerPoint.Longitude);
            CurrentRunCost += distance / 1000 * RunValue;

            return CurrentRunCost;
        }
    }
}

