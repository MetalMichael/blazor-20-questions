using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Blazor20Questions.Shared;
using Blazor20Questions.Server.Models;
using Blazor20Questions.Server.Store;
using System.Linq;
using System.Collections.Generic;

namespace Blazor20Questions.Server.Controllers
{
    [Route("api/[controller]")]
    public class GameController : Controller
    {
        private readonly IGameStore _store;

        public GameController(IGameStore gameStore)
        {
            _store = gameStore;
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
                GuessesTaken = 0
            };

            await _store.CreateNewGame(game);

            return Ok(game.ToResponseModel());
        }

        [HttpGet("{id:Guid}")]
        public async Task<IActionResult> Get(Guid id)
        {
            try
            {
                //var game = await _store.GetGame(id);
                var game = new GameModel
                {
                    Id = id,
                    AllowConcurrentQuestions = false,
                    Expires = DateTime.Now.AddDays(1),
                    GuessesCountAsQuestions = true,
                    TotalQuestions = 10,
                    GuessesTaken = 5,
                    Lost = false,
                    Won = false,
                    Questions = new List<QuestionModel>
                    {
                        new QuestionModel{Question = "is it a bird?", Answer=true },
                        new QuestionModel{Question = "is it a plane?", Answer= false },
                        new QuestionModel{Question = "is it a fish?" }
                    }
                };
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

                if (game.GuessMatches(guess.Guess))
                {
                    game.Won = true;
                    await _store.UpdateGame(game);
                }
                else if (game.GuessesCountAsQuestions)
                {
                    game.GuessesTaken++;
                    await _store.UpdateGame(game);
                }

                return Ok(game);
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

                if(!game.AllowConcurrentQuestions && game.Questions.Count > 0 && !game.Questions.Last().HasAnswer)
                {
                    return BadRequest("Only one question allowed at a time");
                }

                var questionModel = new QuestionModel
                {
                    Question = question.Question
                };

                game.Questions.Add(questionModel);
                await _store.UpdateGame(game);

                return Ok(game);
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

                return Ok(game);
            }
            catch (NotFoundException)
            {
                return NotFound();
            }
        }
    }
}