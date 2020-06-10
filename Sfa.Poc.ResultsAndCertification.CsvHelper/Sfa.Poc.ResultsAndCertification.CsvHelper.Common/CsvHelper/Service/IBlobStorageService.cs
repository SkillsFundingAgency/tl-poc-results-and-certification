using System.IO;
using System.Threading.Tasks;

namespace Sfa.Poc.ResultsAndCertification.CsvHelper.Common.CsvHelper.Service
{
    public interface IBlobStorageService
    {
        Task UploadFileAsync(string storageConnectionString, string containerName, string filePath, Stream stream);
        Task UploadFromByteArrayAsync(string storageConnectionString, string containerName, string filePath, byte[] data);
        Task<Stream> DownloadFileAsync(string storageConnectionString, string containerName, string filePath);
        Task<bool> MoveFileAsync(string storageConnectionString, string containerName, string sourceFilePath, string destinationFilePath);
    }
}
