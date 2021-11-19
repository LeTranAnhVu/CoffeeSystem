namespace ProductService.FailResults
{
    public class BadRequestResult : FailResult
    {
        public BadRequestResult()
        {
            Message = "Bad request";
            StatusCode = 400;
        } 
        
        public BadRequestResult(string message)
        {
            Message = message;
            StatusCode = 400;
        } 
    }
}