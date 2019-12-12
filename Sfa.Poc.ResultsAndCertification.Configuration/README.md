# Configuration #

POC to show how configuration should be loaded in a core web app.

This requires the following nuget packages:

```
WindowsAzure.Storage  (9.3.3)

```

Install the Azure Storage emulator - https://docs.microsoft.com/en-us/azure/storage/common/storage-use-emulator

Install the Azure storage explorer - https://azure.microsoft.com/en-us/features/storage-explorer/

Need the following in the config json:
```
  "EnvironmentName": "LOCAL",
  "ServiceName": "Sfa.Poc.ResultsAndCertification",
  "Version": "1.0"
```

appsettings.json will look as shown below

```
{
  "ConfigurationStorageConnectionString": "UseDevelopmentStorage=true;",
  "EnvironmentName": "LOCAL",
  "ServiceName": "Sfa.Poc.ResultsAndCertification",
  "Version": "1.0",

  "Logging": {
    "LogLevel": {
      "Default": "Warning"
    }
  },
  "AllowedHosts": "*"
}
```

In the storage explorer 
* Local & Attached .. Storage Accounts .. Emulator - Default Ports (Key) .. Tables

Add a table called `Configuration` and in the table add an entity with

* PartitionKey: LOCAL
* RowKey: Sfa.Poc.ResultsAndCertification_1.0
* Data as below

```
{
   "SqlConnectionString": "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=ResultsAndCertification;Integrated Security=True;MultipleActiveResultSets=True;"
}
```

The PartitionKey and RowKey should correspond with the values in the local appsettings.json
*  "EnvironmentName": "LOCAL",
*  "ServiceName": "Sfa.Poc.ResultsAndCertification",
*  "Version": "1.0"
