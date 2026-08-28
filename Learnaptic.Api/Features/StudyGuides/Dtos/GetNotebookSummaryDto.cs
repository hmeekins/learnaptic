namespace Learnaptic.Api.Features.Notebooks.Dtos
{
    public class GetNotebookSummaryDto
    {
        public int Id { get; set; }

        public string Title { get; set; } = string.Empty;

        public string? Subject { get; set; }

        public DateTime LastAccessedAt { get; set; }
    }
}
