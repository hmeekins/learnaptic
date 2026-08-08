namespace Learnaptic.Api.Features.Concepts.Dtos
{
    public class GetConceptDto
    {
        public int Id { get; set; }

        public string Title { get; set; } = string.Empty;

        public string Content { get; set; } = string.Empty;
    }
}
