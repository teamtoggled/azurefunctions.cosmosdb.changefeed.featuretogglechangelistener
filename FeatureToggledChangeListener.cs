using System;
using System.Collections.Generic;
using Microsoft.Azure.Documents;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

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

                log.LogInformation($"Raising event: {eventJson}");

                // TODO: Work as intended with multiple documents in the loop
                return eventJson;         
            }

            return null;                    
        }
    }
}
