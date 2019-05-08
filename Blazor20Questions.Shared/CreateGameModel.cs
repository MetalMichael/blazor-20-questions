using System;
using System.Collections.Generic;
using System.Text;

namespace Blazor20Questions.Shared
{
    public class CreateGameModel
    {
        public string Subject { get; set; }
        public int Minutes { get; set; }
        public int Questions { get; set; }
        public bool GuessesCountAsQuestions { get; set; }
    }
}
