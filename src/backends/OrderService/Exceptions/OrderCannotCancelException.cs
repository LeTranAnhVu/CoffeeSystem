namespace OrderService.Exceptions
{
    public class OrderCannotCancelException : Exception
    {
        static string _defaultMessage = "Cannot cancel the order";
        public OrderCannotCancelException(): base(_defaultMessage)
        {
        }

        public OrderCannotCancelException(int orderId): base($"{_defaultMessage} with id is {orderId}. Because the order was processed.")
        {
        }

        public OrderCannotCancelException(string? message) : base(message)
        {
        }
    }
}