namespace OrderService.FailResults
{
    public abstract class FailResult
    {
        public string Message { get; set; }
        public int StatusCode { get; set; }
    }
}