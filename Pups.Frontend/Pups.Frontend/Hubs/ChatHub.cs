using Microsoft.AspNetCore.SignalR;
using Pups.Frontend.Models.Domain;

namespace Pups.Frontend.Hubs;

public class ChatHub : Hub
{
    public async Task RegisterUser(Guid groupName)
    {
        await Groups.AddToGroupAsync(Context.ConnectionId, groupName.ToString());
    }
    public async Task SendGroupMessage(Guid groupName, Message message)
    {
        await Clients.Group(groupName.ToString()).SendAsync("ReceiveMessage", message);
    }
}