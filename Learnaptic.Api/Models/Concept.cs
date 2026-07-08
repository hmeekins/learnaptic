namespace Learnaptic.Api.Models
{
    public class Concept
    {
        public int Id { get; set; }
        public int StudyGuideId { get; set; }
        public StudyGuide StudyGuide { get; set; }
        public string Title { get; set; } = "";
        public string Explanation { get; set; } = "";
        public List<KeyPoint> KeyPoints { get; set; } = new();
    }
}
