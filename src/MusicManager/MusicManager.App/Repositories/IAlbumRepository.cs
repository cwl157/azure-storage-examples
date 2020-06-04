using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using MusicManager.App.Models;

namespace MusicManager.App.Repositories
{
    public interface IAlbumRepository
    {
        public Task Add(Album a);

        public Task Edit(Album a);

        public Task<List<Album>> GetAll();

        public Task<Album> GetBy(int id);


        public Task Delete(int id);
    }
}
