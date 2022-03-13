namespace RabbitMqServiceExtension.AsyncMessageService;

public interface IRabbitMqService
{
    public void SendMessage<T>(string topic, T content, bool persistent = true);
    public void ReceiveMessageOnTopic<T>(string topic, Action<T?> callback, string? queueName = null);
    public void ReceiveMessageOnTopicWithCallbackAsync<T>(string topic, Func<T?, Task> callbackAsync, string? queueName = null);
    public Task TryToCreateConnection();
}