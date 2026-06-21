namespace Learnaptic.Api.Models
{
    public class Flashcard
    {
        public int Id { get; set; }
        public int StudyGuideId { get; set; }
        public StudyGuide StudyGuide { get; set; }
        public string Question { get; set; } = "";
        public string Answer { get; set; } = "";
    }
}
