using Azure.Storage.Blobs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebProjectAircraftForum.Data;

namespace WebProjectAircraftForum.Services
{
    public class UploadService : IUpload
    {
        public BlobContainerClient GetBlobContainer(string connString, string containerName )
        {
            var blobServiceClient = new BlobServiceClient(connString);

            return blobServiceClient.GetBlobContainerClient(containerName);
        }
    }
}
