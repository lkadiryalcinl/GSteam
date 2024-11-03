namespace BasketService.Models
{
    public class CheckoutModel
    {
        public Guid GameId { get; set; }
        public string Name { get; set; }
        public string Author { get; set; }
        public decimal Price { get; set; }
        public string Info { get; set; }
        public string Description { get; set; }
        public Guid UserId { get; set; }
    }
}
