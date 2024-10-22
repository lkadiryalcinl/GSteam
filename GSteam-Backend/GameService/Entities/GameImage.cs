using GameService.Base;

namespace GameService.Entities
{
    public class GameImage : BaseModel
    {
        public string URL { get; set; }
        public Guid GameId { get; set; }
        public Game Game { get; set; }
    }
}
