using FuncCountdown.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FuncCountdown.Interfaces
{
    public interface IEventDetailsService
    {
        public Task<EventDetailsDto> CalculateCountdown();
        public Task<EventDetailsDto> CalculateCountdown(int userID, string eventName);
    }
}
