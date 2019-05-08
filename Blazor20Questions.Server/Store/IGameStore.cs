using System;
using System.Threading.Tasks;
using Blazor20Questions.Server.Models;

namespace Blazor20Questions.Server.Store
{
    public interface IGameStore
    {
        Task CreateNewGame(GameModel model);
        Task<GameModel> GetGame(Guid id);
        Task UpdateGame(GameModel model);
    }
}