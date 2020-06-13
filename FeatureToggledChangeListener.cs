using System;
using System.Collections.Generic;
using Microsoft.Azure.Documents;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.ServiceBus;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace azurefunctions.cosmosdb.changefeed.featuretogglechangelistener
{
    public static class FeatureToggledChangeListener
    {
        [FunctionName("FeatureToggledChangeListener")]
        public static void Run([CosmosDBTrigger(
            databaseName: "%DatabaseName%",
            collectionName: "%CollectionName%",
            ConnectionStringSetting = "CosmosDbConnection",
            LeaseCollectionName = "leases",
            CreateLeaseCollectionIfNotExists = true)]IReadOnlyList<Document> input, ILogger log,
            [ServiceBus("%ServiceBusQueueName%", Connection = "ServiceBusConnection", EntityType = EntityType.Queue)] ICollector<string> queueCollector)
        {
            foreach(var doc in input)
            {
                var featureToggleChangedEvent = new FeatureToggleChangedEvent()
                {
                    ConfigurationId = doc.GetPropertyValue<Guid>("configurationId"),
                    SignalRConnectionStringVaultUrl = doc.GetPropertyValue<string>("signalRVaultUrl"),
                    FeatureName = doc.GetPropertyValue<string>("name"),
                    NewValue = doc.GetPropertyValue<bool>("state")
                };       

                var eventJson = JsonConvert.SerializeObject(featureToggleChangedEvent);

                log.LogInformation($"Added event to list for raising: {eventJson}");

                queueCollector.Add(eventJson);         
            }
        }
    }
}
