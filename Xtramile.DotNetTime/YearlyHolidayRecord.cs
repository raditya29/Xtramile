using System;

namespace Xtramile.DotNetTime
{
    public class YearlyHolidayRecord
    {
        public int Day { get; }
        public int Month { get; }
        public HolidayType Type { get; }
        public DayOfWeek DayOfWeek { get; }
        public int Occurence { get; }

        public YearlyHolidayRecord(int day, int month, HolidayType type, DayOfWeek dayOfWeek = DayOfWeek.Monday, int occurence = 1)
        {
            Day = day;
            Month = month;
            Type = type;
            DayOfWeek = dayOfWeek;
            Occurence = occurence;
        }
    }
}
