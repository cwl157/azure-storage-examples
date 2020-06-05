using Microsoft.Azure.Cosmos;
using Microsoft.Azure.Storage;
using Microsoft.Azure.Storage.File;
using System;
using System.Data.SqlClient;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace MusicManager.Setup
{
    class Program
    {
        static async Task Main(string[] args)
        {
            Console.WriteLine("Music Manager setup app. Setup the file shares and database necessary for MusicManager");
            string input = "";

            while (input != "5")
            {
                Console.WriteLine("1. Setup local file share"); 
                Console.WriteLine("2. Setup azure file share"); 
                Console.WriteLine("3. Setup azure sql database");
                Console.WriteLine("4. Setup azure cosmos db");
                Console.WriteLine("5. Exit");
                input = Console.ReadLine();

                if (input == "1")
                {
                    Console.Write("File Path: ");
                    string path = Console.ReadLine();
                    if (File.Exists(path))
                    {
                        Console.WriteLine("File at " + path + " already exists");
                    }
                    else
                    {
                        try
                        {
                            using (FileStream fs = File.Create(path))
                            {
                                byte[] info = new UTF8Encoding(true).GetBytes("[]");
                                // Add some information to the file.
                                fs.Write(info, 0, info.Length);
                            }
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine("Failed to create file: " + e.Message);
                        }
                    }
                }
                else if (input == "2")
                {
                    Console.WriteLine("Enter azure files connection string");
                    string connectionString = Console.ReadLine();
                    Console.WriteLine("Enter azure files share name");
                    string shareName = Console.ReadLine();
                    Console.WriteLine("FileName");
                    string fileName = Console.ReadLine();

                    CloudStorageAccount storageAccount = CloudStorageAccount.Parse(connectionString);
                    CloudFileClient fileClient = storageAccount.CreateCloudFileClient();
                    CloudFileShare share = fileClient.GetShareReference(shareName);

                    if (share.Exists())
                    {
                        Console.WriteLine("Share already exists");
                    }
                    else
                    {
                        try
                        {
                            share.Create();

                            CloudFileDirectory rootDir = share.GetRootDirectoryReference();
                            CloudFile file = rootDir.GetFileReference(fileName);
                            await file.UploadTextAsync("[]");
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine("Failed to create azure file share: " + e.Message);
                        }
                    }
                }

                else if (input == "3")
                {
                    Console.WriteLine("Enter sql connection string");
                    string connectionString = Console.ReadLine();
                    using (SqlConnection conn = new SqlConnection(connectionString))
                    {
                        using (SqlCommand command = conn.CreateCommand())
                        {
                            string sql = File.ReadAllText(@".\create_db_objects\create_table.sql");
                            command.Connection.Open();
                            command.CommandType = System.Data.CommandType.Text;
                            command.CommandText = sql;
                            
                            await command.ExecuteNonQueryAsync();

                            sql = File.ReadAllText(@".\create_db_objects\addAlbum.sql");
                            command.CommandText = sql;
                            await command.ExecuteNonQueryAsync();

                            sql = File.ReadAllText(@".\create_db_objects\editAlbum.sql");
                            command.CommandText = sql;
                            await command.ExecuteNonQueryAsync();

                            sql = File.ReadAllText(@".\create_db_objects\getAlbum.sql");
                            command.CommandText = sql;
                            await command.ExecuteNonQueryAsync();

                            sql = File.ReadAllText(@".\create_db_objects\getAlbums.sql");
                            command.CommandText = sql;
                            await command.ExecuteNonQueryAsync();

                            sql = File.ReadAllText(@".\create_db_objects\deleteAlbum.sql");
                            command.CommandText = sql;
                            await command.ExecuteNonQueryAsync();


                        }
                    }
                }

                else if (input == "4")
                {
                    Console.WriteLine("Azure cosmos db endpoint");
                    string endpoint = Console.ReadLine();
                    Console.WriteLine("Azure cosmos db Primary Key");
                    string primaryKey = Console.ReadLine();
                    Console.WriteLine("Azure cosmos db database id");
                    string databaseId = Console.ReadLine();
                    Console.WriteLine("Azure cosmos db container id");
                    string containerId = Console.ReadLine();

                    CosmosClient cosmosClient = new CosmosClient(endpoint, primaryKey);

                    //var db = cosmosClient.GetDatabase(databaseId);

                    try
                    {
                        Database db = await cosmosClient.CreateDatabaseIfNotExistsAsync(databaseId);
                        var container = await db.CreateContainerIfNotExistsAsync(containerId, "/Id");
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine("Failed to create cosmos db or container: " + e.Message);
                    }
                }

                else if (input == "5")
                {
                    Console.WriteLine("Bye");
                }
            }
        }
    }
}
