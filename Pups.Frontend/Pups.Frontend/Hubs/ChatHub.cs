using Microsoft.AspNetCore.SignalR;

namespace Pups.Frontend.Hubs;

public class ChatHub : Hub
{
    public async Task SendMessage(string user, string message)
    {
        await Clients.All.SendAsync("ReceiveMessage", user, message);
    }
    public async Task SendGroupMessage(string user, string groupName, string message)
    {
        await Groups.AddToGroupAsync(Context.ConnectionId, groupName);
        await Clients.Group(groupName).SendAsync("ReceiveGroupMessage", user, message);
    }
}