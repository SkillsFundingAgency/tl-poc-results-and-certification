using Microsoft.Extensions.DependencyInjection;
using Sfa.Poc.ResultsAndCertification.Functions.Extensions.Interfaces;
using System;

namespace Sfa.Poc.ResultsAndCertification.Functions.Extensions.Config
{
    public class ServiceProviderBuilder : IServiceProviderBuilder
    {
        private readonly Action<IServiceCollection> _configureServices;

        public ServiceProviderBuilder(Action<IServiceCollection> configureServices) =>
            _configureServices = configureServices;

        public IServiceProvider Build()
        {
            var services = new ServiceCollection();
            _configureServices(services);
            return services.BuildServiceProvider();
        }
    }
}
