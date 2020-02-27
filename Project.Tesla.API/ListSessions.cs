using System.Net;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Primitives;
using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;
using System.Linq;
using System.Collections.Generic;

public static async Task<List<Object>> Run(HttpRequest req, ILogger log)
{

         log.LogInformation("C# HTTP trigger function processed a request.");

            string EndpointUrl = "https://project-tesla2.documents.azure.com:443/";
            string PrimaryKey = "EMWH7grErJEQH1bXdPmAB9TShiZqpwZovSFJnXTIkIcRMVsOHSXOXzJDgxcUefWyt2cfuIL0qvLnpvGqswqMCQ==";

            DocumentClient client = new DocumentClient(new Uri(EndpointUrl), PrimaryKey, new ConnectionPolicy { EnableEndpointDiscovery = false });
            IQueryable<Object> list = client.CreateDocumentQuery<Object>(UriFactory.CreateDocumentCollectionUri("TokenManagement", "project-tesla2").ToString(), new SqlQuerySpec("SELECT * FROM c where c.type=\"session\""), new FeedOptions { MaxItemCount = -1, EnableCrossPartitionQuery = true });
            List<Object> results = list.ToList();

            return results;
}
