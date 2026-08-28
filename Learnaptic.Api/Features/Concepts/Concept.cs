using Learnaptic.Api.Features.Notebooks;

namespace Learnaptic.Api.Features.Concepts
{
    public class Concept
    {
        public int Id { get; set; }

        public int NotebookId { get; set; }

        public Notebook Notebook { get; set; } = null!;

        public string Title { get; set; } = string.Empty;

        public string Content { get; set; } = string.Empty;

        public int Position { get; set; }
    }
}
