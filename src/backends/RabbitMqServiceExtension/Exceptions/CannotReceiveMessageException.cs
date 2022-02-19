namespace RabbitMqServiceExtension.Exceptions;

public class CannotReceiveMessageException : Exception
{
    static string _defaultMessage = "Cannot listen to receive message in Rabbit Mq ";
    public CannotReceiveMessageException(string? message): base(message ?? _defaultMessage)
    {
    }
}