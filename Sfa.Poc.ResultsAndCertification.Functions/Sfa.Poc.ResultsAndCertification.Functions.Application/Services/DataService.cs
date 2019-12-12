using Sfa.Poc.ResultsAndCertification.Functions.Application.Configuration;
using Sfa.Poc.ResultsAndCertification.Functions.Application.Entities;
using Sfa.Poc.ResultsAndCertification.Functions.Application.Interfaces;
using System;
using System.Threading.Tasks;

namespace Sfa.Poc.ResultsAndCertification.Functions.Application.Services
{
    public class DataService : IDataService
    {
        private readonly ResultsAndCertificationConfiguration _configuration;
        private readonly IRepository _repository;

        public DataService(ResultsAndCertificationConfiguration configuration, IRepository repository)
        {
            _configuration = configuration;
            _repository = repository;
        }

        public async Task<DataResponse> GetConfigurationData()
        {
            var additionalData = await GetData();

            return new DataResponse
            {                
                EnvironmentName = Environment.GetEnvironmentVariable("EnvironmentName"),
                ServiceName = Environment.GetEnvironmentVariable("ServiceName"),
                Version = Environment.GetEnvironmentVariable("Version"),
                Message = $"ConnectionString: {_configuration.SqlConnectionString}",
                AdditionalData = additionalData
            };
        }

        private async Task<string> GetData()
        {
            var data = await _repository.GetStaticData();
            return string.Join(",", data);
        }
    }
}
