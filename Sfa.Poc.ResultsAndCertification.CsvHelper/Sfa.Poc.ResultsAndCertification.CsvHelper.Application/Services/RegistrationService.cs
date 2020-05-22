using Sfa.Poc.ResultsAndCertification.CsvHelper.Application.Interfaces;
using Sfa.Poc.ResultsAndCertification.CsvHelper.Application.Model;
using Sfa.Poc.ResultsAndCertification.CsvHelper.Data.Interfaces;
using Sfa.Poc.ResultsAndCertification.CsvHelper.Domain.Models;
using System;
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
        private readonly IRepository<TqRegistration> _tqRegistrationRepository;
        private readonly IRepository<TqSpecialismRegistration> _tqSpecialismRegistrationRepository;

        public RegistrationService(IRepository<TlPathway> pathwayRepository, ResultsAndCertificationDbContext context,
            IRepository<TqRegistration> tqRegistrationRepository, IRepository<TqSpecialismRegistration> tqSpecialismRegistrationRepository)
        {
            _pathwayRepository = pathwayRepository;
            _tqRegistrationRepository = tqRegistrationRepository;
            _tqSpecialismRegistrationRepository = tqSpecialismRegistrationRepository;
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
                        RawRow = "T level not found.", /* TODO: is Required? */
                        RowNum = x.RowNum
                    });
                }
            });

            return result;
        }

        public async Task ReadRegistrations(IList<TqRegistration> registrations)
        {
            var list = _tqRegistrationRepository.GetManyAsync(x => x.TqProvider.TqAwardingOrganisation.TlAwardingOrganisaton.UkPrn == 10009696,
            x => x.TqSpecialismRegistrations).ToList();
            var entitiesToLoad = 1000000;
            registrations = new List<TqRegistration>();
            var dateTimeNow = DateTime.Now;
            Random random = new Random();
            for (int i = 1; i <= entitiesToLoad; i++)
            {
                registrations.Add(new TqRegistration
                {
                    //Id = i,
                    UniqueLearnerNumber = 123456789, //random.Next(1, 100000),
                    Firstname = "Firstname " + i,
                    Lastname = "Lastname " + i,
                    //DateofBirth = DateTime.UtcNow.AddDays(-i),
                    TqProviderId = 1,
                    //StartDate = dateTimeNow.Date,
                    Status = 1
                });
            }

            //var save = await _tqRegistrationRepository.CreateManyAsync(registrations);
           var results = await _tqRegistrationRepository.BulkReadAsync(registrations, r => r.UniqueLearnerNumber, r => r.Firstname,
                r => r.Lastname, r => r.TqProviderId, r => r.Status);

            var specialismRegistrations = new List<TqSpecialismRegistration>();

            foreach(var r in results)
            {
                specialismRegistrations.Add(new TqSpecialismRegistration
                {
                    TqRegistrationId = r.Id                    
                });
            }
            var specialismResults = await _tqSpecialismRegistrationRepository.BulkReadAsync(specialismRegistrations, r => r.TqRegistrationId);
        }

        public async Task ProcessRegistrations(IList<TqRegistration> registrations)
        {
            var entitiesToLoad = 1000000;

            registrations = new List<TqRegistration>();
            var dateTimeNow = DateTime.Now;
            Random random = new Random();
            for (int i = 1; i <= entitiesToLoad; i++)
            {
                registrations.Add(new TqRegistration
                {
                    Id = i,
                    UniqueLearnerNumber = 123456789, //random.Next(1, 100000),
                    Firstname = "Firstname " + i,
                    Lastname = "Lastname " + i,
                    DateofBirth = DateTime.UtcNow.AddDays(100),
                    TqProviderId = 1,
                    StartDate = dateTimeNow.Date,
                    Status = 1
                });
            }
            await _tqRegistrationRepository.BulkInsertOrUpdateAsync(registrations);
        }
    }
}
