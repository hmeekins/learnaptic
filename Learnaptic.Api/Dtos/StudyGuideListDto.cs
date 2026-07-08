namespace Learnaptic.Api.Dtos
{
    public class StudyGuideListDto
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string? Description { get; set; }
        public string? Subject { get; set; }
        public DateTime LastAccessed { get; set; }
    }
}
