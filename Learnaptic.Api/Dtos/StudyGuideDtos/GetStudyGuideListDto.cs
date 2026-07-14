namespace Learnaptic.Api.Dtos.StudyGuideDtos
{
    public class GetStudyGuideListDto
    {
        public int Id { get; set; }

        public string Title { get; set; } = string.Empty;

        public string? Description { get; set; }

        public string? Subject { get; set; }

        public DateTime LastAccessedAt { get; set; }
    }
}
