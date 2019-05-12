using System;
using System.Threading.Tasks;
using MongoDB.Driver;
using Blazor20Questions.Server.Config;
using Blazor20Questions.Server.Models;
using System.Runtime.Serialization;

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
            var game = await _games.Find(g => g.Id == id).FirstOrDefaultAsync();
            if (game == null)
            {
                throw new NotFoundException();
            }
            return game;
        }

        public async Task UpdateGame(GameModel model)
        {
            await _games.ReplaceOneAsync(g => g.Id == model.Id, model);
        }
    }

    public class NotFoundException : Exception
    {
        public NotFoundException()
        {
        }

        public NotFoundException(string message) : base(message)
        {
        }

        public NotFoundException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected NotFoundException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
