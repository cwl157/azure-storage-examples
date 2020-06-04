using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using MusicManager.App.Models;
using MusicManager.App.FileAccess;

namespace MusicManager.App.Repositories
{
    public class JsonRepository : IAlbumRepository
    {
        private string _dbFilePath;
        private JsonSerializerOptions _options;
        private readonly IFileAccess _fileAccess;
        public JsonRepository(string jsonFilePath, IFileAccess fa)
        {
            _fileAccess = fa;
            _dbFilePath = jsonFilePath;

            _options = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                WriteIndented = true
            };
        }

        public async Task Add(Album a)
        {
            List<Album> all = await GetAll();

            int lastId = 0;
            Album last = all.OrderByDescending(a => a.Id).FirstOrDefault();

            if (last != null)
            {
                lastId = last.Id;
            }

            a.Id = lastId + 1;

            all.Add(a);

            string jsonAlbums = JsonSerializer.Serialize(all, _options);

            await _fileAccess.Write(_dbFilePath, jsonAlbums);
        }

        public async Task Delete(int id)
        {
            List<Album> all = await GetAll();
            var left = all.Where(aa => aa.Id != id);

            string jsonAlbums = JsonSerializer.Serialize(left, _options);

            await _fileAccess.Write(_dbFilePath, jsonAlbums);
        }

        public async Task Edit(Album a)
        {
            List<Album> all = await GetAll();
            Album edited = all.FirstOrDefault(e => e.Id == a.Id);
            if (edited != null)
            {
                edited.Artist = a.Artist;
                edited.Title = a.Title;
                edited.Year = a.Year;
                edited.Format = a.Format;
                edited.Store = a.Store;
                edited.Price = a.Price;
                edited.Symbol = a.Symbol;
                edited.Location = a.Location;
            }
            string jsonAlbums = JsonSerializer.Serialize(all, _options);

            await _fileAccess.Write(_dbFilePath, jsonAlbums);
        }

        public async Task<List<Album>> GetAll()
        {
            return await Task.Run(() =>
            {
                List<Album> albums = new List<Album>();
                string initAlbums = _fileAccess.Read(_dbFilePath).Result;
                albums = JsonSerializer.Deserialize<List<Album>>(initAlbums, _options);

                return albums;
            });
        }

        public async Task<Album> GetBy(int id)
        {
            List<Album> albums = await GetAll();
            return albums.FirstOrDefault(a => a.Id == id);
        }  
    }
}
