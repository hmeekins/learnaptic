namespace Learnaptic.Api.Dtos.ConceptDtos
{
    public class GetConceptDto
    {
        public int Id { get; set; }

        public string Title { get; set; } = string.Empty;

        public string Content { get; set; } = string.Empty;

        public int Position { get; set; }
    }
}
