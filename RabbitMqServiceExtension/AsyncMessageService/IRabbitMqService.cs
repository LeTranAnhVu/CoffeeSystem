namespace RabbitMqServiceExtension.AsyncMessageService;

public interface IRabbitMqService
{
    public void SendMessage<T>(string topic, T content, bool persistent = true);
    public void ReceiveMessageOnTopic<T>(string topic, Action<T?> callback, string? queueName = null);
}