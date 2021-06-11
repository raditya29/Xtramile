using System;

namespace Xtramile.DotNetTime
{
    public interface IDynamicHoliday
    {
        public DateTime ActualDate { get; }
    }

    public struct AlwaysSameDayHoliday : IDynamicHoliday
    {
        private readonly DateTime originalDateTime;

        public AlwaysSameDayHoliday(DateTime originalDateTime) => this.originalDateTime = originalDateTime;

        public DateTime ActualDate => originalDateTime;
    }

    public struct SameDayIfNotWeekendHoliday : IDynamicHoliday
    {
        private readonly DateTime originalDateTime;

        public SameDayIfNotWeekendHoliday(DateTime originalDateTime) => this.originalDateTime = originalDateTime;

        public DateTime ActualDate => 
            (DateCalculator.IsWeekDay(originalDateTime.DayOfWeek)) ? originalDateTime : originalDateTime.NextWeekDay();
    }

    public struct CertainDayInMonthHoliday : IDynamicHoliday
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

        public DateTime ActualDate => calculatedDate;
    }
}
