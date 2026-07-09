namespace Learnaptic.Api.Models
{
    public class Flashcard
    {
        public int Id { get; set; }

        public int StudyGuideId { get; set; }

        public StudyGuide StudyGuide { get; set; } = null!;

        public string Question { get; set; } = string.Empty;

        public string Answer { get; set; } = string.Empty;
    }
}
