using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BokArkiv.Models

{

    public class Book
    {
        [JsonProperty("id")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string? Id { get; set; }

        [JsonProperty("author")]
        public string Author { get; set; }

        [JsonProperty("title")]
        public string Title { get; set; }

        [JsonProperty("genre")]
        public string Genre { get; set; }

        [JsonProperty("price")]
        public double Price { get; set; }
        
        [JsonProperty("publish_date")]
        public DateTime PublishDate { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }
    }
}
