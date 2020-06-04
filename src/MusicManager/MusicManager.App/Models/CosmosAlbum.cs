using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace MusicManager.App.Models
{
    public class CosmosAlbum
    {
        [JsonProperty(PropertyName = "id")]
        public string Identifier { get; set; }
        public int Id { get; set; }
        public string Artist { get; set; }
        public string Title { get; set; }
        public string Year { get; set; }
        public string Format { get; set; }
        public string Store { get; set; }
        public decimal Price { get; set; }
        public string Symbol { get; set; }
        public Category Location { get; set; }
    }
}
