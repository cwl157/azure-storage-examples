namespace MusicManager.App.Models
{
    public enum Category {
        Wishlist = 0,
        Collection = 1
    }
    public class Album
    {
        public int Id {get;set;}
        public string Artist {get;set;}
        public string Title {get;set;}
        public string Year {get;set;}
        public string Format {get;set;}
        public string Store {get;set;}
        public decimal Price {get;set;}
        public string Symbol {get;set;}
        public Category Location {get;set;}

        public Album()
        {
            Artist = "";
            Title = "";
            Year = "";
            Format = "";
            Store = "";
            Price = 0;
            Symbol = "";
            Location = Category.Wishlist;
        }
    }
}