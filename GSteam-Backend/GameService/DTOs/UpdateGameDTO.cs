namespace GameService.DTOs
{
    public class UpdateGameDTO
    {
        public string Name { get; set; }
        public string Author { get; set; }

        public decimal Price { get; set; }
        public string Description { get; set; }
        public string MinimumSystemRequirement { get; set; }
        public string RecommendedSystemRequirement { get; set; }
    }
}
