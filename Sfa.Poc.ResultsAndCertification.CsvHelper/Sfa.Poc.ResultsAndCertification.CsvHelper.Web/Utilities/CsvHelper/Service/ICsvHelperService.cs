using Microsoft.AspNetCore.Http;
using Sfa.Poc.ResultsAndCertification.CsvHelper.Web.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Sfa.Poc.ResultsAndCertification.CsvHelper.Web.Utilities.CsvHelper.Service
{
    public interface ICsvHelperService
    {
        Task<IEnumerable<Registration>> ReadDataAsync(IFormFile file);
        Task DownloadRegistrationsCsvAsync(IEnumerable<Registration> students, string path);
    }
}
