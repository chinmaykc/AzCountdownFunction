using FuncCountdown.Interfaces;
using FuncCountdown.Utilities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace FuncCountdown.Functions
{
    public static class CountdownFunction
    {
        [FunctionName("GetCountdown")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("GetCountdown function triggered");

            /// Default Implementation Provided in Template
            /*
            string name = req.Query["name"];

            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            dynamic data = JsonConvert.DeserializeObject(requestBody);
            name = name ?? data?.name;
            */

            IEventDetailsService _eventService = new EventDetailsService(log, MapperDependencyResolver._Mapper);

            var resp = await _eventService.CalculateCountdown();

            if (resp != null)
                return new OkObjectResult(resp);
            else
                return new NotFoundObjectResult("Event not found");
        }
    }
}
