using Blazor20Questions.Shared;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Blazor20Questions.Server.Models
{
    public class GameModel
    {
        public Guid Id { get; set; }
        public bool Won { get; set; }
        public bool Lost { get; set; }
        public DateTime Expires { get; set; }
        public string Subject { get; set; }
        public int TotalQuestions { get; set; }
        public int GuessesTaken => Guesses.Count;
        public int QuestionsTaken => Questions.Count + (GuessesCountAsQuestions ? GuessesTaken : 0);
        public bool GuessesCountAsQuestions { get; set; }
        public bool AllowConcurrentQuestions { get; set; }

        public IList<QuestionModel> Questions { get; set; }
        public IList<string> Guesses { get; set; }

        public bool IsComplete => Won || Lost || DateTime.UtcNow > Expires;

        public GameResponse ToResponseModel()
        {
            var response = new GameResponse
            {
                Id = Id,
                Won = Won,
                Complete = IsComplete,
                EndTime = Expires,
                QuestionsRemaining = TotalQuestions - QuestionsTaken,
                TotalQuestions = TotalQuestions,
                GuessesCountAsQuestions = GuessesCountAsQuestions,
                AllowConcurrentQuestions = AllowConcurrentQuestions,
                Questions = Questions.Select(q => q.ToResponseModel()).ToList(),
                Guesses = Guesses
            };

            if (IsComplete) {
                response.Subject = Subject;
            }

            return response;
        }

        private static string Fuzz(string s)
        {
            return s.ToLower().Replace(" ", "");
        }

        public bool GuessMatches(string guess)
        {
            return Fuzz(guess) == Fuzz(Subject);
        }
    }
}
