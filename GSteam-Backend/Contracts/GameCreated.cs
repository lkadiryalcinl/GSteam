namespace Contracts
{
    public class GameCreated
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Author { get; set; }

        public decimal Price { get; set; }
        public string Description { get; set; }
        public string MinimumSystemRequirement { get; set; }
        public string RecommendedSystemRequirement { get; set; }

        public Guid CategoryId { get; set; }
    }
}
