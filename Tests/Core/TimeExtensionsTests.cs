using System;
using DateTimeExtensions;
using NUnit.Framework;

namespace DateTimeExtensions.Tests
{
    /// <summary>
    /// Summary description for DateTimeExtentionsTests
    /// </summary>
    [TestFixture]
    public class TimeExtensionsTests
    {
        private DateTime _monday;

        private DateTime _mondayMidnight;
        private DateTime _mondayNoon;

        [SetUp]
        public void Setup()
        {
            _monday = new DateTime(2008, 3, 3, 17, 15, 30); // monday 3rd of March, 2008, 17h 15m 30s

            _mondayMidnight = new DateTime(2008, 3, 3, 0, 0, 0);
            _mondayNoon = new DateTime(2008, 3, 3, 12, 0, 0);
        }

        [Test]
        public void ResetTimeToMidnight()
        {
            Assert.AreEqual(_mondayMidnight, _monday.Midnight());
        }

        [Test]
        public void ResetTimeToNoon()
        {
            Assert.AreEqual(_mondayNoon, _monday.Noon());
        }

        [Test]
        public void SetTimeToMinutePrecision()
        {
            DateTime expected = _mondayMidnight.AddHours(14).AddMinutes(30);
            Assert.AreEqual(expected, _monday.SetTime(14, 30));
        }

        [Test]
        public void SetTimeToSecondPrecision()
        {
            DateTime expected = _mondayMidnight.AddHours(14).AddMinutes(30).AddSeconds(15);
            Assert.AreEqual(expected, _monday.SetTime(14, 30, 15));
        }

        [Test]
        public void SetTimeToMillisecondPrecision()
        {
            DateTime expected = _mondayMidnight.AddHours(14).AddMinutes(30).AddSeconds(15).AddMilliseconds(7);
            Assert.AreEqual(expected, _monday.SetTime(14, 30, 15, 7));
        }
    }
}
