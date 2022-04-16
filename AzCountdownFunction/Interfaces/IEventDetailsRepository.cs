using FuncCountdown.DTOs;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FuncCountdown.Interfaces
{
    public interface IEventDetailsRepository
    {
        public Task<EventDetails> GetEventDetailsByName(int userID, string eventName);
        public Task<SortedDictionary<DateTime, string>> GetPublicHolidays();
    }
}
