namespace Learnaptic.Api.Models
{
    public class Concept
    {
        public int Id { get; set; }

        public int StudyGuideId { get; set; }

        public StudyGuide StudyGuide { get; set; } = null!;

        public string Title { get; set; } = string.Empty;

        public string Explanation { get; set; } = string.Empty;

        public List<KeyPoint> KeyPoints { get; set; } = new();
    }
}
