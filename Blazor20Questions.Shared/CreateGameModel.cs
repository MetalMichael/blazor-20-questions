using System.ComponentModel.DataAnnotations;

namespace Blazor20Questions.Shared
{
    public class CreateGameModel
    {
        public const int MinQuestions = 1;
        public const int MaxQuestions = 50;

        public const int MinMinutes = 1;
        public const int MaxMinutes = 30;

        [Required]
        [RegularExpression("/^[a-z\\s0-9]+$/i", ErrorMessage = "Only letters, numbers and spaces are allowed")]
        public string Subject { get; set; }

        [Required]
        [Range(MinMinutes, MaxMinutes, ErrorMessage = "Game must be between 1 and 30 minutes")]
        public int Minutes { get; set; }

        [Required]
        [Range(MinQuestions, MaxQuestions, ErrorMessage = "Must allow between 1 and 50 questions")]
        public int Questions { get; set; }

        public bool GuessesCountAsQuestions { get; set; }
    }
}
