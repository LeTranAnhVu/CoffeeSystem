namespace OrderService.Exceptions
{
    public class OrderNotFoundException : Exception
    {
        static string _defaultMessage = "Cannot found the order";
        public OrderNotFoundException(): base(_defaultMessage)
        {
        }

        public OrderNotFoundException(int orderId): base($"{_defaultMessage} with id is {orderId}")
        {
        }

        public OrderNotFoundException(string? message) : base(message)
        {
        }
    }
}