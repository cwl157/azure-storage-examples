using Microsoft.Azure.Storage;
using Microsoft.Azure.Storage.File;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace MusicManager.App.FileAccess
{
    public class AzureFileAccess : IFileAccess
    {
        private readonly CloudFileShare _share;

        public AzureFileAccess(string connString, string shareName)
        {
            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(connString);
            // Create a CloudFileClient object for credentialed access to Azure Files.
            CloudFileClient fileClient = storageAccount.CreateCloudFileClient();
            // Get a reference to the file share.
            _share = fileClient.GetShareReference(shareName);
        }

        public async Task<string> Read(string filePath)
        {
            CloudFile file = getFile(filePath);

            return await file.DownloadTextAsync();
        }

        public async Task Write(string filePath, string data)
        {
            CloudFile file = getFile(filePath);
            await file.UploadTextAsync(data);
        }

        private CloudFile getFile(string filepath)
        {
            CloudFileDirectory rootDir = _share.GetRootDirectoryReference();
            CloudFile file = rootDir.GetFileReference(filepath);
            return file;
        }
    }
}
