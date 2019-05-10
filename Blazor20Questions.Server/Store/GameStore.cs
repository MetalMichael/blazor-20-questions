using System;
using System.Threading.Tasks;
using MongoDB.Driver;
using Blazor20Questions.Server.Config;
using Blazor20Questions.Server.Models;

namespace Blazor20Questions.Server.Store
{
    public class GameStore : IGameStore
    {
        private readonly IMongoDatabase _db;
        public GameStore(MongoDBConfig config)
        {
            var client = new MongoClient(config.ConnectionString);
            _db = client.GetDatabase(config.Database);
        }

        private IMongoCollection<GameModel> _games => _db.GetCollection<GameModel>("Games");

        public async Task CreateNewGame(GameModel model)
        {
            await _games.InsertOneAsync(model);
        }

        public async Task<GameModel> GetGame(Guid id)
        {
            return await _games.Find(g => g.Id == id).FirstAsync();
        }

        public async Task UpdateGame(GameModel model)
        {
            await _games.ReplaceOneAsync(g => g.Id == model.Id, model);
        }
    }
}
