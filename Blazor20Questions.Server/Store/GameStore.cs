using Blazor20Questions.Server.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Blazor20Questions.Server.Store
{
    public class GameStore : IGameStore
    {
        public async Task CreateNewGame(GameModel model)
        {

        }

        public async Task<GameModel> GetGame(Guid id)
        {
            return null;
        }

        public async Task UpdateGame(GameModel model)
        {

        }
    }
}
