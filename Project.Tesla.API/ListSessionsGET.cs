using System;
using System.IO;
using System.Threading.Tasks;
using System.Configuration;
using System.Linq;
using System.Collections.Generic;
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
    public static class ListSessionsGET
    {
        [FunctionName("ListSessionsGET")]
        public static IActionResult Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", Route = "sessions/{status}")] HttpRequest req, 
            string status,
            ILogger log)
        {
            log.LogInformation("ListSesison HTTP trigger function processed a request.");

            try {
                List<Session> sessions = ExecuteSimpleQuery("TokenManagement", "project-tesla2", status);
                return status != null
                    ? (ActionResult)new OkObjectResult(sessions)
                    : new BadRequestObjectResult("Please pass status on the query string");
            }
            catch(Exception ex) {
                log.LogError(ex.Message);
                return new BadRequestObjectResult(ex.Message);
            }
        }

        private static List<Session> ExecuteSimpleQuery(string databaseName, string collectionName, string status)
        {
            // Set some common query options.
            FeedOptions queryOptions = new FeedOptions { MaxItemCount = -1, EnableCrossPartitionQuery = true };

            // Find the Sesison by its sessionid.
            IQueryable<Session> sessionQuery = CosmosDBClient.GetCustomClient().CreateDocumentQuery<Session>(
                UriFactory.CreateDocumentCollectionUri(databaseName, collectionName),
                "SELECT * FROM c WHERE c.status = '" + status + "' ORDER BY c._ts DESC",
                queryOptions);

            Console.WriteLine("Running LINQ query...");
            return sessionQuery.ToList();
        }
    }
}
