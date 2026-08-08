namespace Learnaptic.Api.Features.Concepts
{
    public class Concept
    {
        public int Id { get; set; }

        public int StudyGuideId { get; set; }

        public StudyGuide StudyGuide { get; set; } = null!;

        public string Title { get; set; } = string.Empty;

        public string Content { get; set; } = string.Empty;

        public int Position { get; set; }
    }
}
