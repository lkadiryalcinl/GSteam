using MongoDB.Entities;

namespace SearchService.Models
{
    public class GameItem : Entity
    {
        public string Name { get; set; }
        public string Author { get; set; }

        public decimal Price { get; set; }
        public string Description { get; set; }
        public string MinimumSystemRequirement { get; set; }
        public string RecommendedSystemRequirement { get; set; }

        public Guid CategoryId { get; set; }
    }
}
