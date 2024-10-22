using System.Text.Json.Serialization;

namespace FilterService.Models
{
    public class GameFilterItem
    {
        [JsonPropertyName("gameId")]

        public string GameId { get; set; }
        [JsonPropertyName("Name")]
        public string Name { get; set; }
        [JsonPropertyName("Author")]

        public string Author { get; set; }
        [JsonPropertyName("minPrice")]


        public decimal MinPrice { get; set; }
        [JsonPropertyName("maxPrice")]


        public decimal MaxPrice { get; set; }
        [JsonPropertyName("price")]


        public decimal Price { get; set; }
        [JsonPropertyName("Description")]

        public string Description { get; set; }
        [JsonPropertyName("minimumSystemRequirement")]

        public string MinimumSystemRequirement { get; set; }
        [JsonPropertyName("recommendedSystemRequirement")]

        public string RecommendedSystemRequirement { get; set; }
        [JsonPropertyName("categoryId")]

        public Guid CategoryId { get; set; }
    }
}
