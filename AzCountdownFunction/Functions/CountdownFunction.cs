﻿using AutoMapper;
using FuncCountdown.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace FuncCountdown.Functions
{
    public class CountdownFunction
    {
        private readonly IEventDetailsService _eventService;
        private readonly IMapper _mapper;

        public CountdownFunction(IEventDetailsService eventService, IMapper mapper)
        {
            _eventService = eventService;
            _mapper = mapper;
        }

        [FunctionName("GetCountdown")]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", Route = null)] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("GetCountdown function triggered");

            int.TryParse(req.Query["id"], out int userID);
            string eventName = req.Query["event"];

            var eventDetails = await _eventService.CalculateCountdown(userID, eventName);

            if (eventDetails != null)
            {
                var resp = _eventService.GetFilteredEventDetails(eventDetails);
                return new OkObjectResult(resp);
            }
            else
                return new NotFoundObjectResult("Event not found");
        }
    }
}
