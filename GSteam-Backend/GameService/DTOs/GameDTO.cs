namespace GameService.DTOs
{
    public class GameDTO
    {
        public string Name { get; set; }
        public string Author { get; set; }

        public decimal Price { get; set; }
        public IFormFile VideoFile { get; set; }
        public IFormFile GameFile { get; set; }
        public string Info { get; set; }

        public string Description { get; set; }
        public string MinimumSystemRequirement { get; set; }
        public string RecommendedSystemRequirement { get; set; }
        public Guid CategoryId { get; set; }
    }
}
