using Microsoft.AspNetCore.SignalR;

namespace ApiIsocare2.Utilities.SignalR
{
    public class NotificationHub : Hub
    {
        public async Task SendMessage(string message)
        {
            await Clients.All.SendAsync("ReceiveMessage", message);
        }
    }
}
