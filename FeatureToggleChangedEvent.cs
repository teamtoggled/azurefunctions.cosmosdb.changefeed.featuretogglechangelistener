using System;

namespace azurefunctions.cosmosdb.changefeed.featuretogglechangelistener
{
    public class FeatureToggleChangedEvent
    {
        public string SignalRConnectionStringVaultUrl {get; set;}
        public Guid ConfigurationId {get; set;}
        public string FeatureName {get; set;}
        public bool NewValue {get; set;}
    }
}
