using Microsoft.AspNetCore.SignalR;

namespace E_Commerce_Shop_Api.Hubs
{
    public class ChatHub : Hub<IChatClient>
    {
        private static Dictionary<string, string> _connections = new Dictionary<string, string>();

        public override Task OnConnectedAsync()
        {
            var userId = Context.User.Identity.Name;

            _connections.Add(Context.ConnectionId, userId);
            return base.OnConnectedAsync();
        }

        public override Task OnDisconnectedAsync(Exception? exception)
        {
            _connections.Remove(Context.ConnectionId);
            return base.OnDisconnectedAsync(exception);
        }

        //need to fix to dont send for all user message
        public async Task SendMessage(string message)
        {
            await Clients.All.ReceiveMessage(message);
        }

        public async Task Send1on1Message(string userId, string message)
        {
            var connectionId = _connections.FirstOrDefault(x => x.Value == userId).Key;
            await Clients.Client(connectionId).ReceiveMessage(message);
        }



    }



    public interface IChatClient
    {
        //need to fix to dont send for all user message
        Task ReceiveMessage(string message);
    }


}
