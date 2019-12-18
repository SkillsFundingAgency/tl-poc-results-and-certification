### GDS Web Layout ###

To show how to set up  basic website using GDS styles

Based on the [ApplyService](https://github.com/SkillsFundingAgency/tl-matching/tree/master/src/Sfa.Tl.Matching.Web)

you can just copy the /wwwroot folder but remember to empty the application.css file as that has all the custom styles for the apply project.

- Added wwwroot assets
- Added javascripts
- Added stylesheets
- removed images, css, lib, js and favicon
- cleared application.css
- copied layout - but removed not relevant stuff

Add below config to appsettings.json

{
  "ConfigurationStorageConnectionString": "UseDevelopmentStorage=true;",
  "EnvironmentName": "LOCAL",
  "ServiceName": "SFA.Poc.ResultsAndCertification",
  "Version": "1.0",
  "ApplicationInsights": {
    "InstrumentationKey": "123456xyz7689"
  },

  "Logging": {
    "IncludeScopes": false,
    "LogLevel": {
      "Default": "Debug",
      "System": "Information",
      "Microsoft": "Information"
    }
  },
  "AllowedHosts": "*"
}
