namespace Learnaptic.Api.Features.StudyGuides.Dtos
{
    public class GetStudyGuideSummaryDto
    {
        public int Id { get; set; }

        public string Title { get; set; } = string.Empty;

        public string? Subject { get; set; }

        public DateTime LastAccessedAt { get; set; }
    }
}
