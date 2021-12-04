using Microsoft.AspNetCore.SignalR;
using SignalRService.Dtos;

namespace SignalRService.Hubs;

public interface ICommonHub
{
    Task ReceiveMessage(string who, string message);
    Task ChangeOrderStatus(OrderStatusChangedDto dto);
    Task CreateNewOrder(OrderCreatedDto dto);
    Task CancelOrderStatus(OrderStatusChangedDto dto);

    Task TestMessage(string message);
}

public class CommonHub : Hub<ICommonHub>
{
    public async Task JoinGroup(string groupName)
    {
        await Groups.AddToGroupAsync(Context.ConnectionId, groupName);
    }

    public async Task LeaveGroup(string groupName)
    {
        await Groups.RemoveFromGroupAsync(Context.ConnectionId, groupName);
    }
}