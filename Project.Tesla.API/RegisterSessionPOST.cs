using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Extensions.CosmosDB;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Project.Tesla.API.Model;

using Newtonsoft.Json;

namespace Project.Tesla.API
{
    public static class RegisterSessionPOST
    {
        [FunctionName("RegisterSessionPOST")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)] HttpRequest req,
            ILogger log, [CosmosDB(
                databaseName: "TokenManagement",
                collectionName: "project-tesla2",
                ConnectionStringSetting = "TokenManagementDBConnection")] IAsyncCollector<Session> sessions )

        {
            log.LogInformation("RegisterSessionPOST HTTP trigger function processed a request.");

            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            log.LogInformation(requestBody);

            dynamic data = JsonConvert.DeserializeObject(requestBody);

            return (ActionResult)new OkObjectResult("ok");
        }
    }
}
