using NUnit.Framework;
using System;
using Taxi.Domain.Taximeter;
using System.Threading.Tasks;

namespace Taxi.Domain.UnitTests
{
    [TestFixture]
    public class TaximeterServiceTest
    {
        TaximeterService m_service;
        GeoLocatorMock m_geolocator;

        [SetUp]
        public void Setup()
        {
            m_geolocator = new GeoLocatorMock();
            m_service = new TaximeterService(m_geolocator);
        }

        [Test]
        public void ResetRun_RunReseted()
        {
            m_service.CurrentRunCost = 100;
            m_service.ResetRun();
            Assert.AreEqual(0, m_service.CurrentRunCost);
        }

        [Test]
        public async Task MoveTaxi_NewPosition_CurrentPositionAndCostChanged()
        {
            m_service.ValuePerKilometer = 1;
            await m_service.StartRun();

            m_geolocator.OnPositionChanged();

            // 1 degree in Lat/Lon equals to 111.189576959989m kilometers
            decimal expected = 111.2M;
            Assert.AreEqual(expected, Decimal.Round(m_service.Distance / 1000, 1));
            Assert.AreEqual(expected, Decimal.Round(m_service.CurrentRunCost, 1));
        }

        [Test]
        public async Task StartRun_RunStarted()
        {
            bool actual = false;
            m_service.RunStarted += (sender, e) => actual = true;
            await m_service.StartRun();

            Assert.IsTrue(actual);
            Assert.IsTrue(m_service.IsRunning);
        }

        [Test]
        public async Task StopRun_RunStoped()
        {
            bool actual = false;
            m_service.RunStoped += (sender, e) => actual = true;
            await m_service.StartRun();

            Assert.IsTrue(m_service.IsRunning);
            Assert.IsFalse(actual);

            m_service.StopRun();

            Assert.IsFalse(m_service.IsRunning);
            Assert.IsTrue(actual);
        }

        [Test]
        public async Task GetCurrentLocation_CurrentLocation()
        {
            var expected = new TaxiPosition(0, 1);
            var actual = await m_service.GetCurrentLocation();

            Assert.AreEqual(expected.Latitude, actual.Latitude);
            Assert.AreEqual(expected.Longitude, actual.Longitude);
        }
    }
}

