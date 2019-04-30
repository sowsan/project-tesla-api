using System;
using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;

namespace Project.Tesla.API.Common
{
    public static class CosmosDBClient
    {

        public static DocumentClient GetCustomClient()
        {

            DocumentClient customClient = new DocumentClient(
                new Uri("https://project-tesla2.documents.azure.com:443/"),
                "EMWH7grErJEQH1bXdPmAB9TShiZqpwZovSFJnXTIkIcRMVsOHSXOXzJDgxcUefWyt2cfuIL0qvLnpvGqswqMCQ ==",
                new ConnectionPolicy
                {
                    ConnectionMode = ConnectionMode.Direct,
                    ConnectionProtocol = Protocol.Tcp,
                    // Customize retry options for Throttled requests
                    RetryOptions = new RetryOptions()
                    {
                        MaxRetryAttemptsOnThrottledRequests = 10,
                        MaxRetryWaitTimeInSeconds = 30
                    }
                });

            // Customize PreferredLocations
            customClient.ConnectionPolicy.PreferredLocations.Add(LocationNames.EastUS);
            customClient.ConnectionPolicy.PreferredLocations.Add(LocationNames.EastUS2);

            return customClient;
        }
    }
}
