using System;

namespace Blazor20Questions.Shared
{
    public class GameResponse
    {
        public Guid Id { get; set; }
        public DateTime EndTime { get; set; }
        public int QuestionsRemaining { get; set; }
        public bool GuessesCountAsQuestions { get; set; }
    }
}
