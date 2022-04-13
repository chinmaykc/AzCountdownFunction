using AutoMapper;
using FuncCountdown.DTOs;
using FuncCountdown.Interfaces;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FuncCountdown.Utilities
{
    public class EventDetailsRepo : IEventDetailsRepository
    {
        private readonly IMapper _mapper;
        private readonly ILogger _log;

        public EventDetailsRepo(ILogger log, IMapper maper) { _mapper = maper; _log = log; }
        public async Task<DTOs.EventDetails> GetEventDetailsByName(int userID, string eventName)
        {
            var entity = CreateDummyEvent();
            return _mapper.Map<DTOs.EventDetails>(entity);
        }

        public async Task<Dictionary<DateTime, string>> GetPublicHolidays()
        {
            return await GetHardcodedHolidays();
        }

        public async Task<Dictionary<DateTime, string>> GetHardcodedHolidays()
        {
            Dictionary<DateTime, string> holidays = new();
            holidays.Add(new DateTime(2022, 4, 14), "Ramzan");
            holidays.Add(new DateTime(2022, 4, 15), "Ambedkar Jayanti");

            return await Task.FromResult(holidays);
        }

        public static EventDetailsEntity CreateDummyEvent()
        {
            return new()
            {
                nUserID = 1,
                szEventName = "TEST_EVENT",
                szEventDate = System.DateTime.Today.AddDays(20).ToString("ddMMyyyy"),
                dtCreatedDTS = System.DateTime.Now
            };
        }

    }
}
