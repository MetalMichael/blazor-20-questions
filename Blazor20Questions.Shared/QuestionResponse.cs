namespace Blazor20Questions.Shared
{
    public class QuestionResponse
    {
        /// <summary>
        /// The question that was asked
        /// </summary>
        public string Question { get; set; }

        /// <summary>
        /// The answer provided by the host, if any
        /// </summary>
        public bool? Answer { get; set; }

        /// <summary>
        /// Whether the question has been answered
        /// </summary>
        public bool HasAnswer => Answer.HasValue;
    }
}