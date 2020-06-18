using Microsoft.AspNetCore.Http;
using Sfa.Poc.ResultsAndCertification.CsvHelper.Common.CsvHelper.Model;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace Sfa.Poc.ResultsAndCertification.CsvHelper.Common.CsvHelper.Service
{
    public interface ICsvHelperService<in TImportModel, TModel, TResponseModel> where TModel : class, new() where TResponseModel : ResponseBaseModel, new() where TImportModel : BaseModel
    {
        Task<TResponseModel> ValidateAndParseFileAsync(TImportModel fileImportDto);
        Task<IEnumerable<Registration>> ReadDataAsync(IFormFile file);
        Task DownloadRegistrationsCsvAsync(IEnumerable<Registration> students, string path);
        Task<byte[]> WriteErrorFile(List<ValidationError> validationErrors);
    }
}
