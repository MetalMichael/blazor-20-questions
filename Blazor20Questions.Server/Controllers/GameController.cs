using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Blazor20Questions.Shared;
using Blazor20Questions.Server.Models;
using Blazor20Questions.Server.Store;
using System.Linq;
using System.Collections.Generic;
using Microsoft.AspNetCore.SignalR;
using Blazor20Questions.Server.Hubs;

namespace Blazor20Questions.Server.Controllers
{
    [Route("api/[controller]")]
    public class GameController : Controller
    {
        private readonly IGameStore _store;
        private readonly IHubClients _clients;

        public GameController(IGameStore gameStore, IHubContext<GameHub> context)
        {
            _store = gameStore;
            _clients = context.Clients;
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> Create([FromBody] CreateGameModel model)
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
                AllowConcurrentQuestions = model.AllowConcurrentQuestions,
                Questions = new List<QuestionModel>(0),
                Guesses = new List<string>(0)
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
        public async Task<IActionResult> Guess(Guid id, [FromBody] GuessModel guess)
        {
            try
            {
                var game = await _store.GetGame(id);

                if (game.IsComplete)
                {
                    return BadRequest("This game has ended");
                }

                game.Guesses.Add(guess.Guess);
                if (game.GuessMatches(guess.Guess))
                {
                    game.Won = true;
                }
                else if (game.GuessesCountAsQuestions && game.QuestionsTaken >= game.TotalQuestions)
                {
                    game.Lost = true;
                }
                await _store.UpdateGame(game);

                var response = game.ToResponseModel();
                await GameHub.SendUpdate(_clients, id, response);
                return Ok(response);
            }
            catch (NotFoundException)
            {
                return NotFound();
            }
        }

        [HttpPost("{id:Guid}/ask")]
        public async Task<IActionResult> Ask(Guid id, [FromBody] AskQuestionModel question)
        {
            try
            {
                var game = await _store.GetGame(id);

                if (game.IsComplete)
                {
                    return BadRequest("This game has ended");
                }

                if (game.QuestionsTaken >= game.TotalQuestions)
                {
                    return BadRequest("No more questions allowed");
                }

                if (!game.AllowConcurrentQuestions && game.Questions.Count > 0 && !game.Questions.Last().HasAnswer)
                {
                    return BadRequest("Only one question allowed at a time");
                }

                var questionModel = new QuestionModel
                {
                    Question = question.Question
                };

                game.Questions.Add(questionModel);
                await _store.UpdateGame(game);

                var response = game.ToResponseModel();
                await GameHub.SendUpdate(_clients, id, response);
                return Ok(response);
            }
            catch (NotFoundException)
            {
                return NotFound();
            }
        }

        [HttpPost("{id:Guid}/answer/{index:int}")]
        public async Task<IActionResult> Answer(Guid id, int index, [FromBody] AnswerModel answer)
        {
            try
            {
                var game = await _store.GetGame(id);

                if (game.IsComplete)
                {
                    return BadRequest("This game has ended");
                }

                if (index > game.Questions.Count - 1)
                {
                    return NotFound();
                }

                var question = game.Questions[index];
                if (question.HasAnswer)
                {
                    return BadRequest("Question already answered");
                }

                question.Answer = answer.Answer;
                await _store.UpdateGame(game);

                var response = game.ToResponseModel();
                await GameHub.SendUpdate(_clients, id, response);
                return Ok(response);
            }
            catch (NotFoundException)
            {
                return NotFound();
            }
        }
    }
}