using System.IO;
using System.Threading.Tasks;

namespace Sfa.Poc.ResultsAndCertification.CsvHelper.Common.CsvHelper.Service
{
    public interface IBlobStorageService
    {
        Task UploadFileAsync(string filePath, Stream stream);
        Task<Stream> DownloadFileAsync(string filePath);
        Task<bool> MoveFileAsync(string sourceFilePath, string destinationFilePath);
    }
}
