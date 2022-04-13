using System;
using System.Collections.Generic;
using System.Globalization;

namespace FuncCountdown.DTOs
{
    public class EventDetails
    {
        public string EventName { get; set; }
        public string EventDate { get; set; }
        public int UserID { get; set; }
        public DateTime CreatedDTS { get; set; }
        public DateTime LastUpdatedDTS { get; set; }
        public int Remaining_Days { get; set; }
        public int Remaining_WorkingDays => Remaining_Days - (Weekends + UpcomingHolidays);

        public int Weekdays { get; set; }
        public int Weekends { get; set; }
        public int UpcomingHolidays { get; set; }

        public List<string> HolidayNameList { get; set; }
        public List<KeyValuePair<DateTime, string>> HolidayList { get; set; }

        public DateTime EventDTS => DateTime.ParseExact(EventDate, "ddMMyyyy", CultureInfo.InvariantCulture);

        public DateTime CurrentDate => DateTime.Today;
    }
}
