using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Sfa.Poc.ResultsAndCertification.Functions.Extensions.Bindings;
using Sfa.Poc.ResultsAndCertification.Functions.Extensions.Interfaces;
using Sfa.Poc.ResultsAndCertification.Functions.Extensions.Services;
using System;

namespace Sfa.Poc.ResultsAndCertification.Functions.Extensions.Config
{
    public static class InjectWebJobsBuilderExtensions
    {
        public static IWebJobsBuilder AddDependencyInjection<TServiceProviderBuilder>(this IWebJobsBuilder builder)
            where TServiceProviderBuilder : IServiceProviderBuilder
        {
            if (builder == null)
            {
                throw new ArgumentNullException(nameof(builder));
            }

            builder.Services.AddSingleton(typeof(IServiceProviderBuilder), typeof(TServiceProviderBuilder));

            AddCommonDependencyInjection(builder);

            return builder;
        }

        private static void AddCommonDependencyInjection(IWebJobsBuilder builder)
        {
            builder.Services.AddSingleton(provider =>
            {
                var serviceProviderBuilder = provider.GetRequiredService<IServiceProviderBuilder>();
                return new ServiceProviderHolder(serviceProviderBuilder.Build());
            });

            builder.AddExtension<InjectConfiguration>();
            //builder.Services.AddSingleton<ServiceProviderHolder>();
            builder.Services.AddSingleton<InjectBindingProvider>();
            builder.Services.TryAddEnumerable(ServiceDescriptor.Singleton<IFunctionFilter, ScopeCleanupFilter>());
           
        }
    }
}
