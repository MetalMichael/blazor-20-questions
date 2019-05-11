using Blazor20Questions.Shared;
using System;

namespace Blazor20Questions.Server.Models
{
    public class GameModel
    {
        public Guid Id { get; set; }
        public bool Won { get; set; }
        public DateTime Expires { get; set; }
        public string Subject { get; set; }
        public int TotalQuestions { get; set; }
        public int QuestionsTaken { get; set; }
        public bool GuessesCountAsQuestions { get; set; }

        public GameResponse ToResponseModel()
        {
            return new GameResponse
            {
                Id = Id,
                Won = Won,
                EndTime = Expires,
                QuestionsRemaining = TotalQuestions - QuestionsTaken,
                GuessesCountAsQuestions = GuessesCountAsQuestions
            };
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
