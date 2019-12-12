using Sfa.Poc.ResultsAndCertification.Functions.Application.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Sfa.Poc.ResultsAndCertification.Functions.Application.Repositories
{
    public class Repository : IRepository
    {
        public Task<IEnumerable<string>> GetStaticData()
        {
            var data = new List<string> { "Route", "Pathway", "Specialisms" };
            return Task.FromResult<IEnumerable<string>>(data);
        }
    }
}
