using Microsoft.Azure.Storage;
using Microsoft.Azure.Storage.Blob;
using System;
using System.IO;
using System.Threading.Tasks;

namespace Sfa.Poc.ResultsAndCertification.CsvHelper.Common.CsvHelper.Service
{
    public class BlobStorageService : IBlobStorageService
    {
        public const string ContainerName = "Registrations";

        public async Task UploadFileAsync(string storageConnectionString, string containerName, string filePath, Stream stream)
        {
            var blobReference = await GetBlockBlobReference(storageConnectionString, containerName, filePath);
            await blobReference.UploadFromStreamAsync(stream);
        }

        public async Task<Stream> DownloadFileAsync(string storageConnectionString, string containerName, string filePath)
        {
            var blobReference = await GetBlockBlobReference(storageConnectionString, containerName, filePath);

            if (blobReference.ExistsAsync().Result)
            {
                var ms = new MemoryStream();
                await blobReference.DownloadToStreamAsync(ms);
                return ms;
            }
            else
            {
                return null;
            }
        }

        public async Task<bool> MoveFileAsync(string storageConnectionString, string containerName, string sourceFilePath, string destinationFilePath)
        {
            var sourceBlobReference = await GetBlockBlobReference(storageConnectionString, containerName, sourceFilePath);
            var destinationBlobReference = await GetBlockBlobReference(storageConnectionString, containerName, destinationFilePath);

            await destinationBlobReference.StartCopyAsync(sourceBlobReference);
            return await sourceBlobReference.DeleteIfExistsAsync();
        }

        private async Task<CloudBlockBlob> GetBlockBlobReference(string storageConnectionString, string containerName, string fileName)
        {
            var blobContainer = await GetContainerAsync(containerName, storageConnectionString);
            return blobContainer.GetBlockBlobReference(fileName);
        }

        private async Task<CloudBlobContainer> GetContainerAsync(string containerName, string storageConnectionString)
        {
            var storageAccount = CloudStorageAccount.Parse(storageConnectionString);
            var blobContainerReference = storageAccount.CreateCloudBlobClient().GetContainerReference(containerName);
            try
            {
                await blobContainerReference.CreateIfNotExistsAsync();
            }
            catch(Exception ex)
            {
                var excep = ex;
            }
            return blobContainerReference;
        }
    }
}
