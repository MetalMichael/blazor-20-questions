using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Blazor20Questions.Shared;
using Blazor20Questions.Server.Models;
using Blazor20Questions.Server.Store;

namespace Blazor20Questions.Server.Controllers
{
    [Route("api/[controller]")]
    public class GameController : Controller
    {
        private IGameStore _store;

        public GameController(IGameStore gameStore)
        {
            _store = gameStore;
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> Create(CreateGameModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var game = new GameModel
            {
                Id = Guid.NewGuid(),
                Subject = model.Subject,
                Expires = DateTime.UtcNow.AddMinutes(model.Minutes),
                TotalQuestions = model.Questions,
                GuessesCountAsQuestions = model.GuessesCountAsQuestions,
                QuestionsTaken = 0
            };

            await _store.CreateNewGame(game);

            return Ok(game.ToResponseModel());
        }

        [HttpGet("{id:Guid}")]
        public async Task<IActionResult> Get(Guid id)
        {
            try
            {
                var game = await _store.GetGame(id);
                return Ok(game.ToResponseModel());
            }
            catch (Exception)
            {
                return NotFound();
            }
        }

        [HttpPost("{id:Guid}/guess")]
        public async Task<IActionResult> Guess(Guid id, [FromBody] string guess)
        {
            try
            {
                var game = await _store.GetGame(id);

                if (game.Won || game.QuestionsTaken >= game.TotalQuestions)
                {
                    return BadRequest("This game has ended");
                }

                if (game.GuessMatches(guess))
                {
                    game.Won = true;
                    await _store.UpdateGame(game);
                }
                else if (game.GuessesCountAsQuestions)
                {
                    game.QuestionsTaken++;
                    await _store.UpdateGame(game);
                }

                return Ok(game);
            }
            catch (Exception)
            {
                return NotFound();
            }
        }
    }
}