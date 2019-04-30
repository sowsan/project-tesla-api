using System;
using System.IO;
using System.Threading.Tasks;
using System.Configuration;
using System.Linq;
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
    public static class ViewSessionGET
    {
        [FunctionName("ViewSessionGET")]
        public static IActionResult Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", Route = "session/{id}")] HttpRequest req, string id,
            ILogger log)
        {
            log.LogInformation("ViewSesison HTTP trigger function processed a request.");
            Session session = ExecuteSimpleQuery("TokenManagement", "project-tesla2", id);
            return id != null
                ? (ActionResult)new OkObjectResult(session)
                : new BadRequestObjectResult("Please pass sessionID on the query string");
        }

        private static Session ExecuteSimpleQuery(string databaseName, string collectionName, string sessionID)
        {
            // Set some common query options.
            FeedOptions queryOptions = new FeedOptions { MaxItemCount = -1, EnableCrossPartitionQuery = true };

            // Find the Sesison by its sessionid.
            IQueryable<Session> sessionQuery = CosmosDBClient.GetCustomClient().CreateDocumentQuery<Session>(
                UriFactory.CreateDocumentCollectionUri(databaseName, collectionName),
                "SELECT TOP 1 * FROM c WHERE c.sessionID = '" + sessionID + "' ORDER BY c._ts DESC",
                queryOptions);

            Console.WriteLine("Running LINQ query...");
            return sessionQuery.ToList().FirstOrDefault();
        }
            
    }
}
