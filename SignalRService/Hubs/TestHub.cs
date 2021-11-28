using Microsoft.AspNetCore.SignalR;

namespace SignalRService.Hubs;

public interface ITestHub
{
    Task ReceiveMessage(string who, string message);
}

public class TestHub : Hub<ITestHub>
{
    // public async Task Send(string name, string message)
    // {
    //     Console.WriteLine("Call from TestHub");
    //     await Clients.All.SendAsync("broadcastMessage", name, message);
    // }

    // public async Task Ahihi(string name, string message)
    // {
    //     await Clients.OthersInGroup("ahihi").SendAsync("asd", "asdf");
    // }

    public async Task JoinQuestionGroup(string groupName)
    {
        await Groups.AddToGroupAsync(Context.ConnectionId, groupName);
    }

    public async Task LeaveQuestionGroup(string groupName)
    {
        await Groups.RemoveFromGroupAsync(Context.ConnectionId, groupName);
    }
}