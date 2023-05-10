using System.Text.Json.Serialization;

namespace ECommerce.Models
{
    public class Product
    {
        [JsonPropertyName("name")]
        public string name { get; set; }

        [JsonPropertyName("price")]
        public string price { get; set; }

        [JsonPropertyName("condition")]
        public string condition { get; set; }

        [JsonPropertyName("thumbnail")]
        public string thumbnail { get; set; }
    }
}
