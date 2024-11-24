using GameService.Base;
using System.Text.Json.Serialization;

namespace GameService.Entities
{
    public class Game : BaseModel
    {
        public Game() 
        { 
            GameImages = [];
        }
        public string Name { get; set; }
        public string Author { get; set; }

        public decimal Price { get; set; }
        public string VideoUrl { get; set; }
        public string Info { get; set; }

        public string Description { get; set; }
        public string MinimumSystemRequirement { get; set; }
        public string RecommendedSystemRequirement { get; set; }
        
        #nullable enable
        public string UserId { get; set; }

        public Guid CategoryId { get; set; }
        [JsonIgnore]
        public Category Category { get; set; }
        public ICollection<GameImage> GameImages { get; set; }
    }
}
