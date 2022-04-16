using AutoMapper;
using FuncCountdown.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace FuncCountdown.Functions
{
    public class DetailedCountdownFunction
    {
        private readonly IEventDetailsService _eventService;
        private readonly IMapper _mapper;

        public DetailedCountdownFunction(IEventDetailsService eventService, IMapper mapper)
        {
            _eventService = eventService;
            _mapper = mapper;
        }

        [FunctionName("GetCountDown_v2")]
        //public async Task<IActionResult> Run(
        //    [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = "v2/GetCountDown/{id}/{eventName}")] HttpRequest req,
        //    string id,
        //    string eventName,
        //    ILogger log)
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = "v2/GetCountDown")] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("GetCountDown_v2 function triggered");

            int.TryParse(req.Query["id"], out int userID);
            string eventName = req.Query["event"];

            var resp = await _eventService.CalculateCountdown(userID, eventName);

            if (resp != null)
                return new OkObjectResult(resp);
            else
                return new NotFoundObjectResult("Event not found");
        }
    }
}

