using Azure.Storage.Blobs;
using System.IO;
using System.Threading.Tasks;
using Weather.Core;
using Weather.Core.Interfaces.Repositories;

namespace Weather.Infrastructure.Azure.Repositories
{
    public class DeviceRepository : IDeviceRepository
    {
        private readonly AzureSettings _azureSettings;

        public DeviceRepository(AzureSettings azureSettings)
        {
            _azureSettings = azureSettings;
        }

        public async Task<Stream> GetDeviceDataAsync(string deviceId, string date, string sensorType)
        {
            var path = $"/{deviceId}/{date}/{sensorType}.csv";
            
            var blobClient = GetBlobClient(path);

            if (await blobClient.ExistsAsync())
            {
                var response = await blobClient.DownloadAsync();
                return response.Value.Content;
            }
            return null;
        }

        private BlobClient GetBlobClient(string path)
        {
            // get blob storage service
            BlobServiceClient blobServiceClient = new BlobServiceClient(_azureSettings.ConnectionString);
            // get container
            BlobContainerClient containerClient = blobServiceClient.GetBlobContainerClient("iotbackend");
            // get item in container
            BlobClient blobClient = containerClient.GetBlobClient(path);

            return blobClient;
        }
    }
}
