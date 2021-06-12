using System;

namespace Xtramile.DotNetTime
{
    public interface IHoliday
    {
        public DateTime Date { get; }
    }

    public struct AlwaysSameDayHoliday : IHoliday
    {
        private readonly DateTime originalDateTime;

        public AlwaysSameDayHoliday(DateTime originalDateTime) => this.originalDateTime = originalDateTime;

        public DateTime Date => originalDateTime;
    }

    public struct SameDayIfNotWeekendHoliday : IHoliday
    {
        private readonly DateTime originalDateTime;

        public SameDayIfNotWeekendHoliday(DateTime originalDateTime) => this.originalDateTime = originalDateTime;

        public DateTime Date => 
            (DateCalculator.IsWeekDay(originalDateTime.DayOfWeek)) ? originalDateTime : originalDateTime.NextWeekDay();
    }

    public struct CertainDayInMonthHoliday : IHoliday
    {
        private readonly DateTime calculatedDate;

        public CertainDayInMonthHoliday(DayOfWeek dayOfWeek, int occurence, int month)
        {
            var firstDayOfMonth = new DateTime(DateTime.Now.Year, month, 1);
            int firstOccurenceDay;
            if ((int)dayOfWeek - (int)firstDayOfMonth.DayOfWeek >= 0)
                firstOccurenceDay = firstDayOfMonth.Day + ((int)dayOfWeek - (int)firstDayOfMonth.DayOfWeek);
            else
                firstOccurenceDay = firstDayOfMonth.Day + ((int)dayOfWeek - (int)firstDayOfMonth.DayOfWeek + 7);

            calculatedDate = new DateTime(firstDayOfMonth.Year, firstDayOfMonth.Month, firstOccurenceDay + (occurence - 1) * 7);
        }

        public DateTime Date => calculatedDate;
    }
}
