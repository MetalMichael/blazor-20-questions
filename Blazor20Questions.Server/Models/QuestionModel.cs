namespace Blazor20Questions.Server.Models
{
    public class QuestionModel
    {
        public string Question { get; set; }
        public bool? Answer { get; set; }
        public bool HasAnswer => Answer.HasValue;
    }
}
