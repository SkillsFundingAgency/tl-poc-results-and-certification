using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Hosting;
using Microsoft.Azure.WebJobs.Logging;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Sfa.Poc.ResultsAndCertification.Functions.Application.Configuration;
using Sfa.Poc.ResultsAndCertification.Functions.Application.Interfaces;
using Sfa.Poc.ResultsAndCertification.Functions.Application.Repositories;
using Sfa.Poc.ResultsAndCertification.Functions.Application.Services;
using Sfa.Poc.ResultsAndCertification.Functions.Extensions;
using Sfa.Poc.ResultsAndCertification.Functions.Extensions.Config;
using Sfa.Poc.ResultsAndCertification.Functions.Extensions.Interfaces;
using System;

[assembly: WebJobsStartup(typeof(Sfa.Poc.ResultsAndCertification.Functions.Startup))]
namespace Sfa.Poc.ResultsAndCertification.Functions
{
    public class Startup : IWebJobsStartup
    {
        public void Configure(IWebJobsBuilder builder)
        {
            builder
                .AddAzureStorage()
                .AddAzureStorageCoreServices()
                .AddDependencyInjection<ServiceProviderBuilder>();
        }
    }

    internal class ServiceProviderBuilder : IServiceProviderBuilder
    {
        private readonly ILoggerFactory _loggerFactory;

        public ServiceProviderBuilder(ILoggerFactory loggerFactory) =>
            _loggerFactory = loggerFactory;

        public IServiceProvider Build()
        {
            var services = new ServiceCollection();

            RegisterServices(services);

            return services.BuildServiceProvider();
        }

        private void RegisterServices(IServiceCollection services)
        {
            var configuration = ResultsAndCertificationConfigurationLoader.Load(
                Environment.GetEnvironmentVariable("EnvironmentName"),
                    Environment.GetEnvironmentVariable("ConfigurationStorageConnectionString"),
                    Environment.GetEnvironmentVariable("Version"),
                    Environment.GetEnvironmentVariable("ServiceName")
                    );

            services.AddSingleton(configuration);
            services.AddScoped<IRepository, Repository>();
            services.AddTransient<IDataService, DataService>();
            services.AddSingleton<ILogger>(_ => _loggerFactory.CreateLogger(LogCategories.CreateFunctionUserCategory("Common")));
        }
    }
}
