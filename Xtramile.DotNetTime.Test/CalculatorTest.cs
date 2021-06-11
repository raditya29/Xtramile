using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace Xtramile.DotNetTime.Test
{
    public class CalculatorTest
    {
        [Theory]
        [InlineData(11, 6, 2021, 14, 6, 2021)]
        [InlineData(31, 12, 2021, 3, 1, 2022)]
        public void NextWeekDay_Return_Correct_Result(int day, int month, int year, int nextDay, int nextMonth, int nextYear)
        {
            var inputDate = new DateTime(year, month, day);

            Assert.Equal(new DateTime(nextYear, nextMonth, nextDay), inputDate.NextWeekDay());
        }

        [Theory]
        [InlineData(13, 8, 2014, 21, 8, 2014, 6)]
        [InlineData(13, 8, 2014, 19, 8, 2014, 4)]
        [InlineData(11, 6, 2021, 28, 6, 2021, 11)]
        public void RemainingWeekDays_IncludesEndDate_Return_Correct_Result(short fromDay, short fromMonth, short fromYear, 
            short endDay, short endMonth, short endYear, 
            int expectedCount)
        {
            var fromDate = new DateTime(fromYear, fromMonth, fromDay);
            var endDate = new DateTime(endYear, endMonth, endDay);

            int businessDays = fromDate.RemainingWeekDays(endDate);

            Assert.Equal(expectedCount, businessDays);
        }

        [Theory]
        [InlineData(7, 8, 2014, 11, 8, 2014, 1)]
        [InlineData(13, 8, 2014, 21, 8, 2014, 5)]
        [InlineData(13, 8, 2014, 19, 8, 2014, 3)]
        [InlineData(11, 6, 2021, 28, 6, 2021, 10)]
        public void RemainingWeekDays_ExcludesEndDate_Return_Correct_Result(short fromDay, short fromMonth, short fromYear,
            short endDay, short endMonth, short endYear,
            int expectedCount)
        {
            var fromDate = new DateTime(fromYear, fromMonth, fromDay);
            var endDate = new DateTime(endYear, endMonth, endDay);

            int businessDays = fromDate.RemainingWeekDays(endDate, false);

            Assert.Equal(expectedCount, businessDays);
        }

        [Theory]
        [ClassData(typeof(RemainingBusinessDaysTestData))]
        public void RemainingBusinessDays_Substract_FixedHoliday_Return_Correct_Result(short fromDay, short fromMonth, short fromYear,
            short endDay, short endMonth, short endYear,
            int expectedCount, DateTime[] fixedPublicHolidays)
        {
            var fromDate = new DateTime(fromYear, fromMonth, fromDay);
            var endDate = new DateTime(endYear, endMonth, endDay);

            int businessDays = fromDate.RemainingBusinessDays(endDate, fixedPublicHolidays);

            Assert.Equal(expectedCount, businessDays);
        }

        [Theory]
        [InlineData(10, 6, 2021, 16, 6, 2021, 2)]
        [InlineData(31, 12, 2021, 3, 1, 2022, 0)]
        [InlineData(31, 12, 2021, 4, 1, 2022, 0)]
        [InlineData(31, 12, 2021, 5, 1, 2022, 1)]
        [InlineData(31, 12, 2021, 6, 1, 2022, 2)]
        public void RemainingBusinessDays_Substract_DynamicHoliday_Return_Correct_Result(short fromDay, short fromMonth, short fromYear,
            short endDay, short endMonth, short endYear, int expectedCount)
        {
            // arrange
            var fromDate = new DateTime(fromYear, fromMonth, fromDay);
            var endDate = new DateTime(endYear, endMonth, endDay);
            int[] yearsRange = Enumerable.Range(fromDate.Year, (endDate.Year - fromDate.Year + 1)).ToArray(); // years included in date range
            IYearlyHolidayCollectionFactory holidaysFactory = new YearlyDynamicHolidayCollectionFactory();
            // act
            var publicHolidays = yearsRange.SelectMany(year => holidaysFactory.GenerateHolidaysFor(year));
            int businessDays = fromDate.RemainingBusinessDays(endDate, publicHolidays);

            Assert.Equal(expectedCount, businessDays);
        }
    }

    internal class RemainingBusinessDaysTestData : IEnumerable<object[]>
    {
        private static DateTime[] fixedPublicHolidays = new DateTime[] {
            new DateTime(2014, 7, 17), new DateTime(2014, 8, 6), new DateTime(2014, 8, 15)
        };

        public IEnumerator<object[]> GetEnumerator()
        {
            yield return new object[] { 13, 8, 2014, 21, 8, 2014, 4, fixedPublicHolidays };
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}
