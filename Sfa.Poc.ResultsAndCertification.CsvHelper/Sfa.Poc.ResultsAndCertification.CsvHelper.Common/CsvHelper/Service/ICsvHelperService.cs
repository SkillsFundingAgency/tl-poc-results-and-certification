using Microsoft.AspNetCore.Http;
using Sfa.Poc.ResultsAndCertification.CsvHelper.Common.CsvHelper.Model;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Sfa.Poc.ResultsAndCertification.CsvHelper.Common.CsvHelper.Service
{
    public interface ICsvHelperService
    {
        Task<IEnumerable<Registration>> ReadDataAsync(IFormFile file);
        Task DownloadRegistrationsCsvAsync(IEnumerable<Registration> students, string path);
    }
}
