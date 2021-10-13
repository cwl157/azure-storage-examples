using Azure.Identity;
using Azure.Storage.Blobs;
using System;
using System.IO;
using System.Threading.Tasks;

namespace AzureStorageExamples.BlobWithIAM
{
    class Program
    {
        static async Task Main(string[] args)
        {
            ClientSecretCredential cred = new ClientSecretCredential(ConfigConstants.TenantId, ConfigConstants.ClientId, ConfigConstants.ClientSecret);
            BlobServiceClient blobServiceClient = new BlobServiceClient(new Uri(ConfigConstants.StorateUrl), cred);

            string input = "";

            while (input != "4")
            {
                Console.WriteLine("");
                Console.WriteLine("This application is to demonstrate uploading / downloading files to azure blob storage secured with azure AD and IAM");
                Console.WriteLine("");
                Console.WriteLine("Main Menu");
                Console.WriteLine("1. Create Blob Container");
                Console.WriteLine("2. Upload Blob");
                Console.WriteLine("3. Download Blob");
                Console.WriteLine("4. Exit");
                input = Console.ReadLine();

                if (input == "1")
                {
                    Console.WriteLine("Create blob container");
                    Console.Write("Enter blob container name: ");
                    string containerName = Console.ReadLine();

                    try
                    {
                        // Create the container and return a container client object
                        BlobContainerClient containerClient = await blobServiceClient.CreateBlobContainerAsync(containerName.ToLower());
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                    
                }

                else if (input == "2")
                {
                    Console.WriteLine("Upload a file to blob storage");
                    Console.Write("Enter full local file path: ");
                    string localFilePath = Console.ReadLine();
                    Console.Write("Enter blob container name: ");
                    string containerName = Console.ReadLine();

                    try
                    {
                        BlobContainerClient containerClient = blobServiceClient.GetBlobContainerClient(containerName.ToLower());
                        // Get a reference to a blob
                        string fileName = Path.GetFileName(localFilePath);
                        BlobClient blobClient = containerClient.GetBlobClient(fileName);

                        Console.WriteLine("Uploading to Blob storage as blob:\n\t {0}\n", blobClient.Uri);

                        // Upload data from the local file
                        await blobClient.UploadAsync(localFilePath, true);
                        Console.WriteLine("Upload Complete");
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                   
                }

                else if (input == "3")
                {
                    Console.WriteLine("Download Blob");

                    Console.Write("Container name: ");
                    string containerName = Console.ReadLine();

                    Console.Write("Blob name: ");
                    string blobName = Console.ReadLine();

                    Console.Write("Full Destination Path: ");
                    string destinationPath = Console.ReadLine();
                    string localFilePath = destinationPath + "\\" + blobName;

                    try
                    {
                        BlobContainerClient containerClient = blobServiceClient.GetBlobContainerClient(containerName.ToLower());
                        BlobClient blobClient = containerClient.GetBlobClient(blobName);

                        Console.WriteLine("\nDownloading blob to\n\t{0}\n", localFilePath);

                        // Download the blob's contents and save it to a file
                        await blobClient.DownloadToAsync(localFilePath);
                        Console.WriteLine("Download Complete");
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                
                }
                else if (input == "4")
                {
                    Console.WriteLine("Bye");
                }
                else
                {
                    Console.WriteLine("Invalid input");
                }
            }
        }
    }
}
