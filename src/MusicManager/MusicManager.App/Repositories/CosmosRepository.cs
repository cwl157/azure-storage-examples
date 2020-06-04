using Microsoft.Azure.Cosmos;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using MusicManager.App.Models;

namespace MusicManager.App.Repositories
{
    public class CosmosRepository : IAlbumRepository
    {
        private Container _container;
        private CosmosClient _client;

        public CosmosRepository(string endpointUri, string primaryKey, string databaseId, string containerId)
        {
            _client = new CosmosClient(endpointUri, primaryKey);
            _container = _client.GetContainer(databaseId, containerId);
        }
        public async Task Add(Album a)
        {
            List<Album> result = new List<Album>();
            int lastId = 0;
            var sqlQueryText = "SELECT MAX(c.Id) as Id FROM album c";

            QueryDefinition queryDefinition = new QueryDefinition(sqlQueryText);
            FeedIterator<CosmosAlbum> queryResultSetIterator = _container.GetItemQueryIterator<CosmosAlbum>(queryDefinition);

            List<CosmosAlbum> albums = new List<CosmosAlbum>();

            while (queryResultSetIterator.HasMoreResults)
            {
                FeedResponse<CosmosAlbum> currentResultSet = await queryResultSetIterator.ReadNextAsync();
                foreach (CosmosAlbum dbAlbum in currentResultSet)
                {
                    lastId = dbAlbum.Id;
                }
            }

            CosmosAlbum ca = new CosmosAlbum();
         
            ca.Id = lastId + 1;
            ca.Identifier = ca.Id.ToString();
            ca.Artist = a.Artist;
            ca.Title = a.Title;
            ca.Year = a.Year;
            ca.Price = a.Price;
            ca.Store = a.Store;
            ca.Symbol = a.Symbol;
            ca.Location = a.Location;
            ca.Format = a.Format;

            try
            {
                // Read the item to see if it exists.  
                ItemResponse<CosmosAlbum> albumResponse = await _container.ReadItemAsync<CosmosAlbum>(ca.Identifier, new PartitionKey(ca.Id));
                Console.WriteLine("Item in database with id: {0} already exists\n", albumResponse.Resource.Id);
            }
            catch (CosmosException ex) when (ex.StatusCode == HttpStatusCode.NotFound)
            {
                // Create an item in the container representing the album.
                ItemResponse<CosmosAlbum> albumResponse = await _container.CreateItemAsync<CosmosAlbum>(ca, new PartitionKey(ca.Id));

                // Note that after creating the item, we can access the body of the item with the Resource property off the ItemResponse. We can also access the RequestCharge property to see the amount of RUs consumed on this request.
                Console.WriteLine("Created item in database with id: {0} Operation consumed {1} RUs.\n", albumResponse.Resource.Id, albumResponse.RequestCharge);
            }
        }

        public async Task Delete(int id)
        {
            var partitionKeyValue = id;
            var identifier = id.ToString();

            // Delete an item. Note we must provide the partition key value and id of the item to delete
            ItemResponse<CosmosAlbum> albumResponse = await _container.DeleteItemAsync<CosmosAlbum>(identifier, new PartitionKey(partitionKeyValue));
        }

        public async Task Edit(Album a)
        {
             ItemResponse<CosmosAlbum> albumResponse = await _container.ReadItemAsync<CosmosAlbum>(a.Id.ToString(), new PartitionKey(a.Id));
             CosmosAlbum ca = albumResponse.Resource;

            ca.Artist = a.Artist;
            ca.Title = a.Title;
            ca.Year = a.Year;
            ca.Price = a.Price;
            ca.Store = a.Store;
            ca.Symbol = a.Symbol;
            ca.Location = a.Location;
            ca.Format = a.Format;

             albumResponse = await _container.ReplaceItemAsync<CosmosAlbum>(ca, ca.Identifier, new PartitionKey(ca.Id));
        }

        public async Task<List<Album>> GetAll()
        {
            List<Album> result = new List<Album>();
            var sqlQueryText = "SELECT * FROM album";

            QueryDefinition queryDefinition = new QueryDefinition(sqlQueryText);
            FeedIterator<CosmosAlbum> queryResultSetIterator = _container.GetItemQueryIterator<CosmosAlbum>(queryDefinition);

            List<CosmosAlbum> albums = new List<CosmosAlbum>();

            while (queryResultSetIterator.HasMoreResults)
            {
                FeedResponse<CosmosAlbum> currentResultSet = await queryResultSetIterator.ReadNextAsync();
                foreach (CosmosAlbum ca in currentResultSet)
                {
                    result.Add(new Album()
                    {
                        Id = ca.Id,
                        Artist = ca.Artist,
                        Title = ca.Title,
                        Year = ca.Year,
                        Format = ca.Format,
                        Location = ca.Location,
                        Symbol = ca.Symbol,
                        Store = ca.Store,
                        Price = ca.Price
                    });
                }
            }
            return result;
        }

        public async Task<Album> GetBy(int id)
        {
            Album result = null;
            var sqlQueryText = "SELECT * FROM album a WHERE a.Id = @id";

            QueryDefinition queryDefinition = new QueryDefinition(sqlQueryText);
            queryDefinition.WithParameter("@id", id);
            FeedIterator<CosmosAlbum> queryResultSetIterator = _container.GetItemQueryIterator<CosmosAlbum>(queryDefinition);

            List<CosmosAlbum> albums = new List<CosmosAlbum>();

            while (queryResultSetIterator.HasMoreResults)
            {
                FeedResponse<CosmosAlbum> currentResultSet = await queryResultSetIterator.ReadNextAsync();
                foreach (CosmosAlbum ca in currentResultSet)
                {
                    result = new Album()
                    {
                        Id = ca.Id,
                        Artist = ca.Artist,
                        Title = ca.Title,
                        Year = ca.Year,
                        Format = ca.Format,
                        Location = ca.Location,
                        Symbol = ca.Symbol,
                        Store = ca.Store,
                        Price = ca.Price
                    };
                    break;
                }
            }
            return result;
        }
    }
}
