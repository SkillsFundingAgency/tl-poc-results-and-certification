using Microsoft.Azure.WebJobs.Host.Bindings;
using Sfa.Poc.ResultsAndCertification.Functions.Extensions.Services;
using System.Threading.Tasks;

namespace Sfa.Poc.ResultsAndCertification.Functions.Extensions.Bindings
{
    internal class InjectBindingProvider : IBindingProvider
    {
        private readonly ServiceProviderHolder _serviceProviderHolder;

        public InjectBindingProvider(ServiceProviderHolder serviceProviderHolder) =>
            _serviceProviderHolder = serviceProviderHolder;

        public Task<IBinding> TryCreateAsync(BindingProviderContext context)
        {
            IBinding binding = new InjectBinding(_serviceProviderHolder, context.Parameter.ParameterType);
            return Task.FromResult(binding);
        }
    }
}
