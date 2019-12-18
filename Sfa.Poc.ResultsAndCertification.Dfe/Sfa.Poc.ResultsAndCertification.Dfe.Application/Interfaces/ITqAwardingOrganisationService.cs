using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Sfa.Poc.ResultsAndCertification.Dfe.Domain.Models;
using Sfa.Poc.ResultsAndCertification.Dfe.Models;

namespace Sfa.Poc.ResultsAndCertification.Dfe.Application.Interfaces
{
    public interface ITqAwardingOrganisationService
    {
        IQueryable<TqAwardingOrganisation> GetTqAwardingOrganisations();
        Task<IList<TqAwardingOrganisationDetails>> GetTqAwardingOrganisationsAsync();
        Task<TqAwardingOrganisationDetails> GetTqAwardingOrganisationDetailsByIdAsync(int awardingOrganisationId);
        Task<ServiceResponse<TqAwardingOrganisationDetails>> GetTqAwardingOrganisationDetailsByCodeAsync(string code);
    }
}
