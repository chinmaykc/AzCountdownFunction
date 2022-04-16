using AutoMapper;
using FuncCountdown.DTOs;
using FuncCountdown.Interfaces;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace FuncCountdown.Utilities
{
    public class EventDetailsService : IEventDetailsService
    {
        private readonly IMapper _mapper;
        private readonly ILogger _log;
        private readonly IEventDetailsRepository _eventRepo;

        private const int DEFAULT_EVENT_USERID = 1;
        private const string DEFAULT_EVENT_NAME = "TEST_EVENT";

        public EventDetailsService(ILogger log, IMapper mapper, IEventDetailsRepository repo)
        {
            _mapper = mapper;
            _eventRepo = repo;
            _log = log ?? ApplicationLogger.logger;
        }

        public EventDetailsMinifiedDto GetFilteredEventDetails(EventDetailsDto eventDetails)
        {
            return new EventDetailsMinifiedDto(eventDetails.EventName,
                eventDetails.EventDTS.ToString("dd-MM-yyyy"),
                eventDetails.Remaining_Days,
                eventDetails.CurrentDate.ToString("dd-MM-yyyy"));
        }

        public async Task<EventDetailsDto> CalculateCountdown(
            int userID = DEFAULT_EVENT_USERID, 
            string eventName = DEFAULT_EVENT_NAME)
        {
            return await CalculateEventCountdown(userID > 0 ? userID : DEFAULT_EVENT_USERID, eventName ?? DEFAULT_EVENT_NAME);
        }

        public async Task<EventDetailsDto> CalculateEventCountdown(int userID, string eventName)
        {
            _log.LogInformation($"Calculating countdown for {userID}-{eventName}");
            var CurrentDate = System.DateTime.Today;

            try
            {
                var eventDetails = await _eventRepo.GetEventDetailsByName(userID, eventName);
                var dictPublicHolidays = await _eventRepo.GetPublicHolidays();

                if (eventDetails?.EventDTS < CurrentDate)
                    throw new Exception("Event has already passed");

                eventDetails.Remaining_Days = (int)(eventDetails.EventDTS - CurrentDate).TotalDays;

                var daysOfWeek = GetTotalWeedaysAndWeekends(CurrentDate, eventDetails.Remaining_Days);
                eventDetails.Weekdays = daysOfWeek.Item1;
                eventDetails.Weekends = daysOfWeek.Item2;

                eventDetails.UpcomingHolidays = GetHolidays(dictPublicHolidays, CurrentDate, eventDetails.EventDTS, out var holidayNames);
                eventDetails.HolidayNameList = holidayNames;

                _log.LogInformation($"Countdown Calculation Completed for {userID}-{eventName}");

                return _mapper.Map<EventDetailsDto>(eventDetails);
            }
            catch (Exception ex)
            {
                _log.LogError($"Countdown Calculation FAILED for {userID}-{eventName} - {ex.Message}");
            }
            return null;
        }

        public static Tuple<int, int> GetTotalWeedaysAndWeekends(DateTime startingFromDate, int noOfDaysToConsider)
        {
            int weekdays, weekends;
            weekdays = weekends = 0;

            for (int i = 1; i < noOfDaysToConsider; i++)
            {
                switch (startingFromDate.AddDays(i).DayOfWeek)
                {
                    case DayOfWeek.Saturday:
                    case DayOfWeek.Sunday:
                        weekends++;
                        break;
                    default:
                        weekdays++;
                        break;
                }
            }

            return new Tuple<int, int>(weekdays, weekends);
        }

        /// <returns>Returns number of upcomingHolidays</returns>
        public static int GetHolidays(SortedDictionary<DateTime, string> dictHolidays, 
            DateTime fromDate, DateTime toDate, out List<string> holidayNames)
        {
            var holidaysToConsider = dictHolidays.Where(h => h.Key >= fromDate && h.Key <= toDate);

            holidayNames = new();
            int upcomingHolidays = 0;
            string holidayPrefix;

            foreach (var holiday in holidaysToConsider)
            {
                //Check if Holiday Falls on weekend
                switch (holiday.Key.DayOfWeek)
                {
                    case DayOfWeek.Saturday:
                    case DayOfWeek.Sunday:
                        holidayPrefix = " [Falls on Weekend]";
                        break;
                    default:
                        holidayPrefix = "";
                        upcomingHolidays++;
                        break;
                }

                holidayNames.Add($"{holiday.Key:D} - {holiday.Value}{holidayPrefix}");
            }

            return upcomingHolidays;
        }
    }

}
