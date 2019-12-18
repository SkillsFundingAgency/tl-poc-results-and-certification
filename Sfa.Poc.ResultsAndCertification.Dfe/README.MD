# DfE Sign-In POC #

POC to integrate with DfE Sign-in.

Uses the same table configuration as the [Configuration Sample](../Sfa.Poc.ResultsAndCertification.Configuration/README.md).

The following needs to be in the config json (with appropriate connection strings):
```
  "EnvironmentName": "LOCAL",
  "ServiceName": "Sfa.Poc.ResultsAndCertification",
  "Version": "1.0"
```


{
   "SqlConnectionString": "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=ResultsAndCertification;Integrated Security=True;MultipleActiveResultSets=True;",
   
   "BlobStorageConnectionString": "UseDevelopmentStorage=true",
   "ResultsAndCertificationInternalApiUri": "https://localhost:5001",
   "DFESignInSettings": {
		"MetadataAddress": "<Specify MetaAddress from Dfe SignIn team>",
		"ClientId": "<ClientId of your application>",
		"ClientSecret": "<Need to get this from Dfe signin team>",
		"CallbackPath": "/auth/cb",
		"SignedOutCallbackPath": "/signout/complete",
                "LogoutPath": "/signin/signedout",
		"Issuer": "<Need to get this from Dfe signin team>",
		"Audience": "<Need to get this from Dfe signin team>",
		"APISecret": "<Need to get this from Dfe signin team>",
		"APIUri": "<Need to get this from Dfe signin team>",
	        "TokenEndpoint": "<Need to get this from Dfe signin team>",
                "Authority": "<Need to get this from Dfe signin team>"                
   }   
}