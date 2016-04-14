using System;
using System.Threading;
using System.Threading.Tasks;
using Plugin.Geolocator.Abstractions;

namespace Taxi.Domain.UnitTests
{
    public class GeoLocatorMock : IGeolocator
    {
        bool m_litening;

        public GeoLocatorMock()
        {
        }

        public bool AllowsBackgroundUpdates
        {
            get
            {
                throw new NotImplementedException();
            }

            set
            {
                throw new NotImplementedException();
            }
        }

        public double DesiredAccuracy
        {
            get
            {
                throw new NotImplementedException();
            }

            set
            {
                throw new NotImplementedException();
            }
        }

        public bool IsGeolocationAvailable
        {
            get
            {
                return true;
            }
        }

        public bool IsGeolocationEnabled
        {
            get
            {
                return true;
            }
        }

        public bool IsListening
        {
            get
            {
                return m_litening;
            }
        }

        public bool PausesLocationUpdatesAutomatically
        {
            get
            {
                throw new NotImplementedException();
            }

            set
            {
                throw new NotImplementedException();
            }
        }

        public bool SupportsHeading
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public event EventHandler<PositionEventArgs> PositionChanged;
        public void OnPositionChanged()
        {
            if (PositionChanged != null)
            {
                PositionChanged(this,
                                new PositionEventArgs(new Position
                                {
                                    Latitude = 0,
                                    Longitude = 2
                                }));
            }
        }
        public event EventHandler<PositionErrorEventArgs> PositionError;

        public Task<Position> GetPositionAsync(int timeoutMilliseconds = -1, CancellationToken? token = default(CancellationToken?), bool includeHeading = false)
        {
            Task<Position> result = Task<Position>.Factory.StartNew(() =>
            {
                return new Position
                {
                    Latitude = 0,
                    Longitude = 1
                };
            });

            return result;
        }

        public Task<bool> StartListeningAsync(int minTime, double minDistance, bool includeHeading = false)
        {
            Task<bool> result = Task<bool>.Factory.StartNew(() => m_litening = true);

            return result;
        }

        public Task<bool> StopListeningAsync()
        {
            Task<bool> result = Task<bool>.Factory.StartNew(() => m_litening = false);

            return result;

        }
    }
}

