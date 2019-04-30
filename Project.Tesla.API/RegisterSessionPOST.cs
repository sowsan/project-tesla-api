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
    public static class RegisterSessionPOST
    {
        [FunctionName("RegisterSessionPOST")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "post", Route = "session")] HttpRequest req,
            ILogger log )

        {
            log.LogInformation("RegisterSessionPOST HTTP trigger function processed a request.");

            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            log.LogInformation(requestBody);

            Session session = JsonConvert.DeserializeObject<Session>(requestBody);
            session.SessionID = Guid.NewGuid().ToString();

            //await client.CreateDocumentAsync(UriFactory.CreateDocumentCollectionUri("TokenManagement", "project-tesla2"), session);
            await CreateSessionDocument("TokenManagement", "project-tesla2", session);

            return (ActionResult)new OkObjectResult(session.SessionID);
        }


        private static async Task CreateSessionDocument(string databaseName, string collectionName, Session session)
        {
            try
            {
                await CosmosDBClient.GetCustomClient().CreateDocumentAsync(UriFactory.CreateDocumentCollectionUri(databaseName, collectionName), session);

            }
            catch (DocumentClientException de)
            {
                throw de;
            }
        }
    }
}
