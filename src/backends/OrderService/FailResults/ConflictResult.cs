namespace OrderService.FailResults
{
    public class ConflictResult : FailResult
    {
        public ConflictResult()
        {
            Message = "Conflict Entity";
            StatusCode = 409;
        } 
        
        public ConflictResult(string message)
        {
            Message = message;
            StatusCode = 409;
        } 
    }
}