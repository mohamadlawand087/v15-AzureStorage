using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace BlobSampleApp.Services
{
    public interface IBlobService
    {
         Task<string> GetBlob(string name, string containerName);
         Task<IEnumerable<string>> AllBlobs(string containerName);
         Task<bool> UploadBlob(string name, IFormFile file, string containerName);
         Task<bool> DeleteBlob(string name, string containerName);
    }
}