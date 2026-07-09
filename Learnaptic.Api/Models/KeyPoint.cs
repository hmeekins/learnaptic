namespace Learnaptic.Api.Models
{
    public class KeyPoint
    {
        public int Id { get; set; }

        public int ConceptId { get; set; }

        public Concept Concept { get; set; } = null!;

        public string Content { get; set; } = string.Empty;
    }
}
