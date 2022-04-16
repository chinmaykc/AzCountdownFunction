using AutoMapper;
using FuncCountdown.DTOs;
using FuncCountdown.Interfaces;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.Json;
using System.Threading.Tasks;

namespace FuncCountdown.Utilities
{
    public class EventDetailsRepo : IEventDetailsRepository
    {
        private readonly IMapper _mapper;
        private readonly ILogger _log;
        private JsonDBContext _dbContext;

        public EventDetailsRepo(ILogger log, IMapper maper) 
        { 
            _mapper = maper; 
            _log = log ?? ApplicationLogger.logger;
            _dbContext = new JsonDBContext();
        }
        
        public async Task<DTOs.EventDetails> GetEventDetailsByName(int userID, string eventName)
        {
            var entity = _dbContext.UserEvents.First(x => x.nUserID == userID 
                                    && x.szEventName.Equals(eventName, StringComparison.InvariantCultureIgnoreCase));

            return await Task.FromResult(_mapper.Map<DTOs.EventDetails>(entity));
            //return _mapper.Map<DTOs.EventDetails>(entity);
        }

        public async Task<SortedDictionary<DateTime, string>> GetPublicHolidays()
        {
            return await GetHolidays();
        }

        public async Task<SortedDictionary<DateTime, string>> GetHolidays()
        {
            var holidays = _dbContext.Holidays;
            return await Task.FromResult(holidays);
        }

    }
}
