namespace Learnaptic.Api.Dtos
{
    public class GetConceptDto
    {
        public int Id { get; set; }

        public int StudyGuideId { get; set; }

        public string Title { get; set; } = string.Empty;

        public string Content { get; set; } = string.Empty;
    }
}
