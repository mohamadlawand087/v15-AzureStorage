using System.Collections.Generic;
using System.Threading.Tasks;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Microsoft.AspNetCore.Http;

namespace BlobSampleApp.Services
{
    public class BlobService : IBlobService
    {
        private readonly BlobServiceClient _blobClient;

        public BlobService(BlobServiceClient blobClient)
        {
            _blobClient = blobClient;
        }
        
        public async Task<IEnumerable<string>> AllBlobs(string containerName)
        {
            // allow us to access the data inside the container
            var containerClient = _blobClient.GetBlobContainerClient(containerName);

            var files = new List<string>();
            
            var blobs = containerClient.GetBlobsAsync();

            await foreach(var item in blobs)
            {
                files.Add(item.Name);
            }

            return files;
        }

        public async Task<bool> DeleteBlob(string name, string containerName)
        {
            var containerClient = _blobClient.GetBlobContainerClient(containerName);
            var blobClient = containerClient.GetBlobClient(name);
            return await blobClient.DeleteIfExistsAsync();
        }

        public async Task<string> GetBlob(string name, string containerName)
        {
            // this will allow us access to the storage container
           var containerClient = _blobClient.GetBlobContainerClient(containerName);

           // this will allow us access to the file inside the container via the file name
           var blobClient = containerClient.GetBlobClient(name);

           return blobClient.Uri.AbsoluteUri;
        }

        public async Task<bool> UploadBlob(string name, IFormFile file, string containerName)
        {
           var containerClient = _blobClient.GetBlobContainerClient(containerName);

            // checking if the file exist 
            // if the file exist it will be replaced
            // if it doesn't exist it will create a temp space until its uploaded
           var blobClient = containerClient.GetBlobClient(name);

           var httpHeaders = new BlobHttpHeaders() {
               ContentType = file.ContentType
           };

           var res = await blobClient.UploadAsync(file.OpenReadStream(), httpHeaders);

           if(res != null)
                return true;

            return false;
        }
    }
}