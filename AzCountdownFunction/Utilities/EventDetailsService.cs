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

        public EventDetailsService(ILogger logger, IMapper mapper) : this(logger, mapper, null) { }

        public EventDetailsService(ILogger log, IMapper mapper, IEventDetailsRepository repo)
        {
            _mapper = mapper;
            _log = log;
            _eventRepo = repo?? new EventDetailsRepo(_log, _mapper);
        }

        public async Task<EventDetailsDto> CalculateCountdown()
        {
            return await CalculateCountdown(DEFAULT_EVENT_USERID, DEFAULT_EVENT_NAME);
        }

        public async Task<EventDetailsDto> CalculateCountdown(int userID, string eventName)
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

                eventDetails.UpcomingHolidays = GetHolidays(dictPublicHolidays, CurrentDate, out var holidayNames);
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
        public static int GetHolidays(Dictionary<DateTime, string> dictHolidays, DateTime holidaysUptoDate, out List<string> holidayNames)
        {
            holidayNames = new();
            int upcomingHolidays = 0;

            foreach (var holiday in dictHolidays)
            {
                if (holidaysUptoDate <= holiday.Key)
                {
                    holidayNames.Add($"{holiday.Key:D} - {holiday.Value}");
                    upcomingHolidays++;
                }
            }

            return upcomingHolidays;
        }
    }

}
