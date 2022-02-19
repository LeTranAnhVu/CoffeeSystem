namespace OrderService.FailResults
{
    public class NotFoundResult : FailResult
    {
        public NotFoundResult()
        {
            Message = "Entity is not found";
            StatusCode = 404;
        }

        public NotFoundResult(int id, string entityName)
        {
            Message = $"{entityName} with Id {id} is not found";
            StatusCode = 404;
        }

        public NotFoundResult(string message)
        {
            Message = message;
            StatusCode = 404;
        }
    }
}