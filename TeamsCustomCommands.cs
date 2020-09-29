using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace Netcoreconf.CustomCommands
{
    public static class TeamsCustomCommands
    {
        [FunctionName("TeamsCustomCommands")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            dynamic data = JsonConvert.DeserializeObject(requestBody);
            string item = data?.item;
            string action = data?.action;

            string responseMessage = "No item specified.";

            switch (action.ToLowerInvariant())
            {
                case "leave":
                    responseMessage = "Notify => user left the meeting.";
                    break;
                case "late":
                    responseMessage = "Notify => user running late.";
                    break;
                case "on":
                case "off":
                default:
                    responseMessage = $"Action executed => turned {item} {action}.";
                    break;
            }

            log.LogInformation($"{responseMessage}");

            return new OkObjectResult(responseMessage);
        }
    }
}
