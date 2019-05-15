using Blazor20Questions.Shared;
using Microsoft.AspNetCore.SignalR;
using Newtonsoft.Json;
using System;
using System.Threading.Tasks;

namespace Blazor20Questions.Server.Hubs
{
    public class GameHub : Hub
    {
        public Task Register(string gameId)
        {
            return Groups.AddToGroupAsync(Context.ConnectionId, gameId);
        } 

        public static async Task SendUpdate(IHubClients clients, Guid id, GameResponse game)
        {
            var message = JsonConvert.SerializeObject(game);
            await clients.Group(id.ToString()).SendAsync("UpdateGame", message);
        }
    }
}
