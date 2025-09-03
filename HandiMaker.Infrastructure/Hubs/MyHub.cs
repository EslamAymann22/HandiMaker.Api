using Microsoft.AspNetCore.SignalR;

namespace HandiMaker.Infrastructure.Hubs
{
    public class MyHub : Hub
    {

        public MyHub() { }

        public override async Task OnConnectedAsync()
        {
            await base.OnConnectedAsync();
        }

        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            await base.OnDisconnectedAsync(exception);
        }

    }
}
