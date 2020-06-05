using System;
using System.Collections;
using System.Globalization;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Linq.Expressions;
using System.Threading.Tasks;
using MusicManager.App.Models;
using MusicManager.App.FileAccess;
using MusicManager.App.Repositories;

namespace MusicManager.App
{
    class Program
    {
        static async Task Main(string[] args)
        {
            string input = "";
            Dictionary<string, Category> categoryLookup = new Dictionary<string, Category>() {
                {"wishlist", Category.Wishlist},
                {"collection", Category.Collection}
            };

            IFileAccess azureFileAccess = new AzureFileAccess(Constants.AzureFileConnectionString, Constants.AzureFileShareName);
            IFileAccess localFileAccess = new LocalFileAccess();
            IAlbumRepository repository = new SqlRepository(Constants.DbConnectionString);
           // IAlbumRepository repository = new JsonRepository(Constants.AzureFileFilepath, azureFileAccess);
            //IAlbumRepository repository = new JsonRepository(Constants.LocalFilePath, localFileAccess);
            //IAlbumRepository repository = new CosmosRepository(Constants.CosmosEndpointUri, Constants.CosmosPrimaryKey, Constants.CosmosDatabaseId, Constants.CosmosContainerId);

            while (input != "9")
            {
                Console.WriteLine("Main Menu");
                Console.WriteLine("1. Enter new Album");
                Console.WriteLine("2. Edit Album");
                Console.WriteLine("3. Display All");
                Console.WriteLine("4. Display Wishlist");
                Console.WriteLine("5. Display Collection");
                Console.WriteLine("6. Display filtered albums");
                Console.WriteLine("7. Display Totals");
                Console.WriteLine("8. Delete Album");
                Console.WriteLine("9. Exit");
                input = Console.ReadLine();

                if (input == "1")
                {
                    Console.Write("Artist: ");
                    string artist = Console.ReadLine();
                    Console.Write("Title: ");
                    string title = Console.ReadLine();
                    Console.Write("Year: ");
                    int year = 0;
                    int.TryParse(Console.ReadLine(), out year);
                    Console.Write("Format: ");
                    string format = Console.ReadLine();
                    Console.Write("Store: ");
                    string store = Console.ReadLine();
                    decimal price = 0;
                    Console.Write("Price: ");
                    decimal.TryParse(Console.ReadLine(), out price);
                    Console.Write("Symbol: ");
                    string symbol = Console.ReadLine();
                    Console.Write("Location: ");
                    string location = Console.ReadLine();
                    Category c = Category.Wishlist;
                    categoryLookup.TryGetValue(location.ToLower(), out c);

                    await repository.Add(new Album()
                    {
                        Id = 0,
                        Artist = artist,
                        Title = title,
                        Year = year == 0 ? "" : year.ToString(),
                        Format = format,
                        Store = store,
                        Price = price,
                        Symbol = symbol,
                        Location = c
                    });
                }

                else if (input == "2")
                {
                    int id = 0;
                    Console.WriteLine("Edit Album");
                    Console.Write("Id to edit: ");
                    int.TryParse(Console.ReadLine(), out id);

                    Album e = await repository.GetBy(id);
                    if (e != null)
                    {
                        string artist = e.Artist;
                        string title = e.Title;
                        string year = e.Year;
                        string format = e.Format;
                        string store = e.Store;
                        decimal price = e.Price;
                        string symbol = e.Symbol;
                        Category location = e.Location;

                        Console.Write("Artist (" + artist + "): ");
                        string artistIn = Console.ReadLine();
                        if (!string.IsNullOrEmpty(artistIn))
                        {
                            e.Artist = artistIn;
                        }
                        Console.Write("Title (" + title + "): ");
                        string titleIn = Console.ReadLine();
                        if (!string.IsNullOrEmpty(titleIn))
                        {
                            e.Title = titleIn;
                        }
                        Console.Write("Year (" + year + "): ");
                        string yearIn = Console.ReadLine();
                        if (!string.IsNullOrEmpty(yearIn))
                        {
                            int y = 0;
                            int.TryParse(yearIn, out y);

                            e.Year = y == 0 ? "" : y.ToString();
                        }
                        Console.Write("Format (" + format + "): ");
                        string formatIn = Console.ReadLine();
                        if (!string.IsNullOrEmpty(formatIn))
                        {
                            e.Format = formatIn;
                        }
                        Console.Write("Store (" + store + "): ");
                        string storeIn = Console.ReadLine();
                        if (!string.IsNullOrEmpty(storeIn))
                        {
                            e.Store = storeIn;
                        }

                        Console.Write("Price (" + price + "): ");
                        string priceIn = Console.ReadLine();
                        if (!string.IsNullOrEmpty(priceIn))
                        {
                            decimal p = 0;
                            decimal.TryParse(priceIn, out p);

                            e.Price = p;
                        }

                        Console.Write("Symbol (" + symbol + "): ");
                        string symbolIn = Console.ReadLine();
                        if (!string.IsNullOrEmpty(symbolIn))
                        {
                            e.Symbol = symbolIn;
                        }
                        Console.Write("Location (" + location + "): ");
                        string locationIn = Console.ReadLine();
                        if (!string.IsNullOrEmpty(locationIn))
                        {
                            Category c = Category.Wishlist;
                            categoryLookup.TryGetValue(locationIn.ToLower(), out c);
                            e.Location = c;
                        }

                        await repository.Edit(e);
                    }
                    else
                    {
                        Console.WriteLine("Could not find album with id = " + id);
                    }
                }

                else if (input == "3")
                {
                    printAlbums(await repository.GetAll());
                }
                else if (input == "4")
                {
                    printAlbums(repository.GetAll().Result.Where(a => a.Location == Category.Wishlist));
                }
                else if (input == "5")
                {
                    printAlbums(repository.GetAll().Result.Where(a => a.Location == Category.Collection));
                }
                else if (input == "6")
                {
                    Console.Write("Filter Expression: ");
                    string filter = Console.ReadLine();

                    // var result = repository.GetFilteredAlbums(filter);
                    var result = GetFilteredAlbums(filter, repository.GetAll().Result);
                    printAlbums(result);
                }
                else if (input == "7")
                {
                    decimal wishListTotal = repository.GetAll().Result.Where(a => a.Location == Category.Wishlist).Sum(a => a.Price);
                    decimal collectionTotal = repository.GetAll().Result.Where(a => a.Location == Category.Collection).Sum(a => a.Price);

                    Console.WriteLine("Wishlist total: " + wishListTotal.ToString("C", CultureInfo.CurrentCulture));
                    Console.WriteLine("Collection total: " + collectionTotal.ToString("C", CultureInfo.CurrentCulture));

                }
                else if (input == "8")
                {
                    Console.WriteLine("Delete album");
                    Console.Write("Id to delete: ");
                    string id = Console.ReadLine();
                    int parsedId = 0;
                    if (!int.TryParse(id, out parsedId))
                    {
                        Console.WriteLine("Id must be a number");
                        continue;
                    }
                    var a = await repository.GetBy(parsedId);
                    if (a != null)
                    {
                        Console.WriteLine("*** WARNING *** This operation cannot be undone. Please type the name of the album to delete below");
                        string titleToDelete = Console.ReadLine();
                        if (a.Title == titleToDelete)
                        {
                            await repository.Delete(a.Id);
                        }
                        else
                        {
                            Console.WriteLine("Album not found, nothing deleted");
                        }
                    }
                }

                else if (input == "9")
                {
                    Console.WriteLine("Bye");
                }
            }
        }

        private static void printAlbums(IEnumerable<Album> albums)
        {
            albums = albums.OrderBy(a => a.Artist);
            Console.WriteLine("{0,-20}{1,-20}{2,40}{3,10}{4,10}{5,20}{6,10}{7,20}{8,10}", "Id", "Artist", "Title", "Year", "Format", "Store", "Price", "Location", "Sy");
                    for (int i =0; i < 160; i++)
                    {
                        Console.Write("-");
                    }
                    Console.WriteLine();
            int length = 0;
                    foreach (Album a in albums)
                    {
                       Console.WriteLine("{0,-20}{1,-20}{2,40}{3,10}{4,10}{5,20}{6,10}{7,20}{8,10}",
                       a.Id,
                        a.Artist,
                        a.Title,
                        a.Year,
                        a.Format,
                        a.Store,
                        a.Price.ToString("C", CultureInfo.CurrentCulture),
                        a.Location,
                        a.Symbol);
                length++;
                    }
            Console.WriteLine("Total: " + length+"; Total Price: " + albums.Sum(a => a.Price).ToString("C", CultureInfo.CurrentCulture));
            Console.WriteLine("");
        }

        private static List<Album> GetFilteredAlbums(string filter, IEnumerable<Album> albums)
        {
            filter = filter.ToLower();
            string[] sep = filter.Split("&&");
            IEnumerable<Album> result = albums;
            foreach (string s in sep)
            {
                Console.WriteLine(s);
                string[] each = s.Split(new char[] { '=' });
                string typeToken = each[0].Trim();
                string valueToken = each[1].Trim();

                if (typeToken == "artist")
                {
                    result = result.Where(a => a.Artist.ToLower() == valueToken);
                }
                else if (typeToken == "year")
                {
                    result = result.Where(a => a.Year.ToLower() == valueToken);
                }
                else if (typeToken == "store")
                {
                    result = result.Where(a => a.Store.ToLower() == valueToken);
                }
                else if (typeToken == "format")
                {
                    result = result.Where(a => a.Format.ToLower() == valueToken);
                }
                else if (typeToken == "title")
                {
                    result = result.Where(a => a.Title.ToLower() == valueToken);
                }
                else if (typeToken == "price")
                {
                    decimal price = 0;
                    decimal.TryParse(valueToken, out price);
                    result = result.Where(a => a.Price == price);
                }
                else if (typeToken == "location")
                {
                    if (valueToken == "wishlist")
                    {
                        result = result.Where(a => a.Location == Category.Wishlist);
                    }
                    else if (valueToken == "collection")
                    {
                        result = result.Where(a => a.Location == Category.Collection);
                    }
                }


            }
            return result.ToList();
        }
    }
}
