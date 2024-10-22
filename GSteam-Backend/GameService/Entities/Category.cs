using GameService.Base;

namespace GameService.Entities
{
    public class Category : BaseModel
    {
        public string? Name { get; set; }
        public string? Description { get; set; }
        public ICollection<Game> Games { get; set; }
    }
}
