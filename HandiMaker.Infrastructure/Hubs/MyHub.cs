using HandiMaker.Data.Entities;
using HandiMaker.Infrastructure.DbContextData;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace HandiMaker.Infrastructure.Hubs
{
    [Authorize]
    public class MyHub : Hub
    {
        private readonly HandiMakerDbContext _handiMakerDb;

        public MyHub(HandiMakerDbContext handiMakerDb)
        {
            this._handiMakerDb = handiMakerDb;
        }

        public override async Task OnConnectedAsync()
        {
            var UserId = Context.User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (!string.IsNullOrEmpty(UserId))
            {
                var connectionId = Context.ConnectionId;
                var user = await _handiMakerDb.Users.FindAsync(UserId);
                if (user != null)
                {
                    user.Connections!.Add(new Connection
                    {
                        ConnectionId = connectionId,
                        UserId = UserId
                    });
                    await _handiMakerDb.SaveChangesAsync();
                }
                await base.OnConnectedAsync();
            }
        }

        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            var ConnectionId = await _handiMakerDb.Connections.Where(C => C.ConnectionId == Context.ConnectionId).FirstOrDefaultAsync();
            if (ConnectionId is not null)
            {
                _handiMakerDb.Connections.Remove(ConnectionId);
                await _handiMakerDb.SaveChangesAsync();
            }
            await base.OnDisconnectedAsync(exception);
        }

    }
}
