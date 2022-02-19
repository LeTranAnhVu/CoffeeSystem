using Microsoft.AspNetCore.SignalR;
using SignalRService.Hubs;

namespace SignalRService.BackgroundServices;

public class SignalRTestMessageBackgroundService : BackgroundService
{
    private readonly ILogger<SignalRTestMessageBackgroundService> _logger;
    private readonly IHubContext<CommonHub, ICommonHub> _hubContext;

    public SignalRTestMessageBackgroundService(ILogger<SignalRTestMessageBackgroundService> logger,
        IHubContext<CommonHub, ICommonHub> hubContext)
    {
        _logger = logger;
        _hubContext = hubContext;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("Background service: Send test signalr message");
        while (!stoppingToken.IsCancellationRequested)
        {
            var getRandom = new Random();
            await _hubContext.Clients.All.TestMessage(
                $"Welcome to Brian's coffee system - websocket is connected. Current temperature is {getRandom.Next(-10, 20)}° C - reported by from Mr.Background service");
            await Task.Delay(TimeSpan.FromSeconds(2), stoppingToken);
        }
    }
}