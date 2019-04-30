using System;
using System.IO;
using System.Threading.Tasks;
using System.Configuration;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Extensions.CosmosDB;
using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Project.Tesla.API.Model;
using Project.Tesla.API.Common;
using Newtonsoft.Json;

namespace Project.Tesla.API
{
    public static class PostEventPOST
    {
        [FunctionName("PostEventPOST")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "post", Route = "event")] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("PostEventPOST HTTP trigger function processed a request.");
            
            try {
                string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
                log.LogInformation(requestBody);

                Event eventDoc = JsonConvert.DeserializeObject<Event>(requestBody);
                if(eventDoc.EventID == null) {
                    eventDoc.EventID = Guid.NewGuid().ToString();
                }

                await CreateEventDocument("TokenManagement", "project-tesla2", eventDoc);

                return (ActionResult)new OkObjectResult(eventDoc.EventID);
            }
            catch(Exception ex) {
                log.LogError(ex.Message);
                return new BadRequestObjectResult(ex.Message);
            }
        }

        private static async Task CreateEventDocument(string databaseName, string collectionName, Event eventDoc)
        {
            try
            {
                await CosmosDBClient.GetCustomClient().CreateDocumentAsync(UriFactory.CreateDocumentCollectionUri(databaseName, collectionName), eventDoc);

            }
            catch (DocumentClientException de)
            {
                throw de;
            }
        }
    }
}
