using Azure.Storage.Blobs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebProjectAircraftForum.Data
{
    public interface IUpload
    {
        BlobContainerClient GetBlobContainer(string connString, string containerName);
    }
}
