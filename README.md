# azurefunctions.cosmosdb.changefeed.featuretogglechangelistener
Listens to feature toggle change events from the Azure CosmosDb change feed and publishes events onwards to a service bus queue. (Later there will be a function which processes this queue to push the events outwards to SignalR)
