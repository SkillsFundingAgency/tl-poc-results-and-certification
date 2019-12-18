using Microsoft.Azure.WebJobs.Host.Config;
using Microsoft.Extensions.DependencyInjection;
using Sfa.Poc.ResultsAndCertification.Functions.Application.Configuration;
using Sfa.Poc.ResultsAndCertification.Functions.Application.Interfaces;
using Sfa.Poc.ResultsAndCertification.Functions.Application.Repositories;
using Sfa.Poc.ResultsAndCertification.Functions.Application.Services;
using Sfa.Poc.ResultsAndCertification.Functions.Extensions.Bindings;
using System;

namespace Sfa.Poc.ResultsAndCertification.Functions.Extensions.Config
{
    internal class InjectConfiguration : IExtensionConfigProvider
    {
        public readonly InjectBindingProvider _injectBindingProvider;

        public InjectConfiguration(InjectBindingProvider injectBindingProvider) =>
            _injectBindingProvider = injectBindingProvider;

        public void Initialize(ExtensionConfigContext context)
        {
            //var services = new ServiceCollection();
            //RegisterServices(services);
            //var serviceProvider = services.BuildServiceProvider(true);

            context.AddBindingRule<InjectAttribute>()
                   .Bind(_injectBindingProvider);
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
        }

        //public void Initialize(ExtensionConfigContext context) => context
        //        .AddBindingRule<InjectAttribute>()
        //        .Bind(_injectBindingProvider);
    }
}
