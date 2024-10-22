namespace GameService.Base
{
    public abstract class BaseModel
    {
        protected BaseModel()
        {
            Id = Guid.NewGuid();
            CreatedDate = DateTime.UtcNow;
        }

        public Guid Id { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
