using System.ComponentModel.DataAnnotations;

namespace Blazor20Questions.Shared
{
    public class CreateGameModel
    {
        [Required]
        [RegularExpression("")]
        public string Subject { get; set; }

        [Required]
        [Range(1, 30, ErrorMessage = "Game must be between 1 and 30 minutes")]
        public int Minutes { get; set; }

        [Required]
        [Range(1, 50, ErrorMessage = "Must allow between 1 and 50 questions")]
        public int Questions { get; set; }

        public bool GuessesCountAsQuestions { get; set; }
    }
}
