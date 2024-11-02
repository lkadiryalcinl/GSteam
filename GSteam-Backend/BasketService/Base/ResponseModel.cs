namespace BasketService.Base
{
    public class ResponseModel<T>
    {
        public bool IsSuccess { get; set; } = false;
        public string Message { get; set; }
        public T Data { get; set; }
    }
}
