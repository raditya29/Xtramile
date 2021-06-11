using System;
using System.Collections;
using System.Collections.Generic;
using Xunit;

namespace Xtramile.DotNetTime.Test
{
    public class DynamicHolidayTest
    {
        [Theory]
        [InlineData(25, 4, 2021, 25, 4, 2021)]
        public void AlwaysSameDayHoliday_Class_Returns_Correct_ActualDate(int day, int month, int year, 
            int actualDay, int actualMonth, int actualYear)
        {
            var originalDate = new DateTime(year, month, day);

            Assert.Equal(new DateTime(actualYear, actualMonth, actualDay), new AlwaysSameDayHoliday(originalDate).ActualDate);
        }

        [Theory]
        [InlineData(1, 1, 2021, 1, 1, 2021)] // falls on weekday
        [InlineData(19, 6, 2021, 21, 6, 2021)] // falls on weekend
        [InlineData(20, 6, 2021, 21, 6, 2021)] // falls on weekend
        public void SameDayIfNotWeekendHoliday_Class_Returns_Correct_ActualDate(int day, int month, int year,
            int actualDay, int actualMonth, int actualYear)
        {
            var originalDate = new DateTime(year, month, day);

            Assert.Equal(new DateTime(actualYear, actualMonth, actualDay), new SameDayIfNotWeekendHoliday(originalDate).ActualDate);
        }

        [Theory]
        [InlineData(1, 2, 6, 14, 6, 2021)] // second monday in june
        [InlineData(5, 4, 6, 25, 6, 2021)] // fourth friday in june
        public void CertainDayInMonthHoliday_Class_Returns_Correct_ActualDate(int dayOfWeek, int occurence, int month, 
            int actualDay, int actualMonth, int actualYear)
        {
            Assert.Equal(new DateTime(actualYear, actualMonth, actualDay),
                new CertainDayInMonthHoliday((DayOfWeek)dayOfWeek, occurence, month).ActualDate);
        }
    }
}
