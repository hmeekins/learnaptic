namespace Learnaptic.Api.Models
{
    public class Flashcard
    {
        public int Id { get; set; }

        public int StudySetId { get; set; }

        public StudySet StudySet { get; set; } = null!;

        public string Question { get; set; } = string.Empty;

        public string Answer { get; set; } = string.Empty;
    }
}
