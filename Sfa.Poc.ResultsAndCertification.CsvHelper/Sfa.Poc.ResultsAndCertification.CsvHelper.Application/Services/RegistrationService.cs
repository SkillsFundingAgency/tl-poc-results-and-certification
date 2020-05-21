using Sfa.Poc.ResultsAndCertification.CsvHelper.Application.Interfaces;
using Sfa.Poc.ResultsAndCertification.CsvHelper.Application.Model;
using Sfa.Poc.ResultsAndCertification.CsvHelper.Data.Interfaces;
using Sfa.Poc.ResultsAndCertification.CsvHelper.Domain.Models;
using System.Collections.Generic;
using System.Linq;
using Sfa.Poc.ResultsAndCertification.CsvHelper.Data;
using Microsoft.EntityFrameworkCore;
using Sfa.Poc.ResultsAndCertification.CsvHelper.Models;
using Sfa.Poc.ResultsAndCertification.CsvHelper.Common.CsvHelper.Model;
using System.Threading.Tasks;

namespace Sfa.Poc.ResultsAndCertification.CsvHelper.Application.Services
{
    public class RegistrationService : IRegistrationService
    {
        private readonly IRepository<TlPathway> _pathwayRepository;
        private readonly ResultsAndCertificationDbContext ctx;

        public RegistrationService(IRepository<TlPathway> pathwayRepository, ResultsAndCertificationDbContext context)
        {
            _pathwayRepository = pathwayRepository;
            ctx = context;
        }

        public async Task<IEnumerable<Tlevel>> GetAllTLevelsByAoUkprnAsync(long ukprn)
        {
            var result = await ctx.TqProvider
                .Include(x => x.TlProvider)
                .Include(x => x.TqAwardingOrganisation)
                .ThenInclude(x => x.TlAwardingOrganisaton)
                .Include(x => x.TqAwardingOrganisation)
                .ThenInclude(x => x.TlPathway)
                .ThenInclude(x => x.TlSpecialisms)
                .Include(x => x.TlProvider)
                .Where(x => x.TqAwardingOrganisation.TlAwardingOrganisaton.UkPrn == ukprn)
                .Select(x => new Tlevel
                {
                    ProviderUkprn = x.TlProvider.UkPrn,
                    TlPathwayId = x.TqAwardingOrganisation.TlPathway.Id,
                    TlSpecialisms = x.TqAwardingOrganisation.TlPathway.TlSpecialisms
                    .Select(s => new KeyValuePair<int, string>(s.Id, s.Name)).ToList()
                }).ToListAsync();

            return result;
        }

        public async Task<bool> SaveBulkRegistrationsAsync(IEnumerable<Registration> regdata, long ukprn)
        {
            // TOOD: 
            return await Task.Run(() => true);
        }

        public async Task<BulkRegistrationResponse> ValidateRegistrationTlevelsAsync(long ukprn, IEnumerable<Registration> regdata)
        {
            var aoProviderTlevels = await GetAllTLevelsByAoUkprnAsync(ukprn);

            var result = new BulkRegistrationResponse();

            regdata.ToList().ForEach(x =>
            {
                var isvalid = aoProviderTlevels.Any(t => t.ProviderUkprn == x.Ukprn &&
                            t.TlPathwayId == 100 /* && TODO*/
                //t.TlSpecialisms.Contains(200) && /*TODO: Spl-1 */
                //t.TlSpecialisms.Contains(300) /*TODO: Spl-1 */
                );

                if (!isvalid) 
                {
                    x.ValidationErrors.Add(new ValidationError
                    {
                        FieldName = "NA",
                        FieldValue = "NA",
                        RawRow = "T level not found.", /*TODO: */
                        RowNum = 0 /*TODO: */
                    });
                }
            });

            return result;
        }
    }
}
