using Microsoft.AspNetCore.SignalR;

namespace SignalRService.Hubs;

public class ChatHub : Hub
{
    public async Task Send(string name, string message)
    {
        await Clients.All.SendAsync("broadcastMessage", name, message);
    }
}