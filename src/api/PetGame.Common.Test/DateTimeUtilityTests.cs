using System;
using FluentAssertions;
using NUnit.Framework;

namespace PetGame.Common.Test
{
    [TestFixture]
    public class DateTimeUtilityTests
    {
        [TestCase("2019-03-10T03:00:00Z", 600, "2019-03-09T17:00:00Z")]
        [TestCase("2019-03-10T03:00:00Z", 0, "2019-03-10T03:00:00Z")]
        [TestCase("2019-03-09T17:00:00Z", -600, "2019-03-10T03:00:00Z")]
        [Description("ConvertLocalTimeToUTC() returns the correct time for a given time zone offset (and time)")]
        public void ConvertLocalTimeToUTC_WithCertainOffset_ReturnsCorrectResult(DateTime mockLocalTime, int mockTimeZoneOffSetMinutes, DateTime expected)
        {
            // SETUP / TEST
            var result = DateTimeUtility.ConvertLocalTimeToUTC(mockLocalTime, mockTimeZoneOffSetMinutes);

            // ASSERT
            result.Should().Be(expected);
        }

        [TestCase(600)]
        [TestCase(0)]
        [TestCase(-600)]
        [Description("GetLocalDateTimeNow() returns the correct time for a given time zone offset")]
        public void GetLocalDateTimeNow_WithCertainOffset_ReturnsCorrectResult(int mockTimeZoneOffSetMinutes)
        {
            // SETUP 
            var expected = DateTime.UtcNow + TimeSpan.FromMinutes(mockTimeZoneOffSetMinutes);

            // TEST 
            var result = DateTimeUtility.GetLocalDateTimeNow(mockTimeZoneOffSetMinutes);

            // ASSERT
            result.Should().BeCloseTo(expected);
        }
    }
}