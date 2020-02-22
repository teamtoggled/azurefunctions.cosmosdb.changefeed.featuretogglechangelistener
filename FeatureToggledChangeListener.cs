using System;
using System.Collections.Generic;
using Microsoft.Azure.Documents;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;

namespace azurefunctions.cosmosdb.changefeed.featuretogglechangelistener
{
    public static class FeatureToggledChangeListener
    {
        [FunctionName("FeatureToggledChangeListener")]
        [return: ServiceBus("%ServiceBusQueueName%", Connection = "ServiceBusConnection")]
        public static string Run([CosmosDBTrigger(
            databaseName: "%DatabaseName%",
            collectionName: "%CollectionName%",
            ConnectionStringSetting = "CosmosDbConnection",
            LeaseCollectionName = "leases")]IReadOnlyList<Document> input, ILogger log)
        {
            
            log.LogInformation("Documents modified " + input.Count);
            log.LogInformation("First document Id " + input[0].Id);
        
            return input[0].Id;
        }
    }
}
