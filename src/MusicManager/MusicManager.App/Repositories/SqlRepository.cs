using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;
using System.Threading.Tasks;
using MusicManager.App.Models;

namespace MusicManager.App.Repositories
{
    public class SqlRepository : IAlbumRepository
    {
        private string _connectionString;
        public SqlRepository(string connString)
        {
            _connectionString = connString;
        }
        public async Task Add(Album a)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                using (SqlCommand command = conn.CreateCommand())
                {
                    command.Connection.Open();
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    command.CommandText = "addAlbum";
                    command.Parameters.AddWithValue("@artist", a.Artist);
                    command.Parameters.AddWithValue("@title", a.Title);
                    command.Parameters.AddWithValue("@year", a.Year);
                    command.Parameters.AddWithValue("@format", a.Format);
                    command.Parameters.AddWithValue("@store", a.Store);
                    command.Parameters.AddWithValue("@price", a.Price);
                    command.Parameters.AddWithValue("@location", a.Location);
                    command.Parameters.AddWithValue("@symbol", a.Symbol);

                    await command.ExecuteNonQueryAsync();
                }
            }
        }

        public async Task Delete(int id)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                using (SqlCommand command = conn.CreateCommand())
                {
                    command.Connection.Open();
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    command.CommandText = "deleteAlbum";

                    command.Parameters.AddWithValue("@id", id);

                    await command.ExecuteNonQueryAsync();
                }
            }
        }

        public async Task Edit(Album a)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                using (SqlCommand command = conn.CreateCommand())
                {
                    command.Connection.Open();
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    command.CommandText = "editAlbum";
                    command.Parameters.AddWithValue("@id", a.Id);
                    command.Parameters.AddWithValue("@artist", a.Artist);
                    command.Parameters.AddWithValue("@title", a.Title);
                    command.Parameters.AddWithValue("@year", a.Year);
                    command.Parameters.AddWithValue("@format", a.Format);
                    command.Parameters.AddWithValue("@store", a.Store);
                    command.Parameters.AddWithValue("@price", a.Price);
                    command.Parameters.AddWithValue("@location", a.Location);
                    command.Parameters.AddWithValue("@symbol", a.Symbol);

                    await command.ExecuteNonQueryAsync();
                }
            }
        }

        public async Task<List<Album>> GetAll()
        {
            List<Album> result = new List<Album>();
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                using (SqlCommand command = conn.CreateCommand())
                {
                    command.Connection.Open();
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    command.CommandText = "getAlbums";


                    using (SqlDataReader reader = await command.ExecuteReaderAsync())
                    {
                        while (reader.Read())
                        {
                            //result.Add(new Album()
                            Album tmp = new Album
                            {
                                Id = int.Parse(reader["Id"].ToString()),
                                Artist = reader["Artist"].ToString(),
                                Title = reader["Title"].ToString(),
                                Year = reader["Year"].ToString(),
                                Format = reader["Format"].ToString(),
                                Store = reader["Store"].ToString(),
                                Symbol = reader["Symbol"].ToString(),
                                Location = (Category)int.Parse(reader["Location"].ToString())

                            };

                            decimal outPrice = 0;
                            decimal.TryParse(reader["Price"].ToString(), out outPrice);
                            tmp.Price = outPrice;

                            result.Add(tmp);
                        }
                    }
                }
                return result;
            }

        }

        public async Task<Album> GetBy(int id)
        {
            Album result = null;
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                using (SqlCommand command = conn.CreateCommand())
                {
                    command.Connection.Open();
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    command.CommandText = "getAlbum";
                    command.Parameters.AddWithValue("@id", id);
                    using (SqlDataReader reader = await command.ExecuteReaderAsync())
                    {
                        while (reader.Read())
                        {
                            result = new Album
                            {
                                Id = int.Parse(reader["Id"].ToString()),
                                Artist = reader["Artist"].ToString(),
                                Title = reader["Title"].ToString(),
                                Year = reader["Year"].ToString(),
                                Format = reader["Format"].ToString(),
                                Store = reader["Store"].ToString(),
                                Symbol = reader["Symbol"].ToString(),
                                Location = (Category)int.Parse(reader["Location"].ToString())

                            };

                            decimal outPrice = 0;
                            decimal.TryParse(reader["Price"].ToString(), out outPrice);
                            result.Price = outPrice;
                        }
                    }
                }

            }
            return result;
        }
    }
}
