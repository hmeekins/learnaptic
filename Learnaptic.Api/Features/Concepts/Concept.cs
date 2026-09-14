using Learnaptic.Api.Features.Notebooks;
using System.Text.Json;

namespace Learnaptic.Api.Features.Concepts
{
    public class Concept
    {
        public int Id { get; set; }

        public int NotebookId { get; set; }

        public Notebook Notebook { get; set; } = null!;

        public string Title { get; set; } = "Untitled Concept";

        public JsonDocument Content { get; set; } =
            JsonDocument.Parse(
                """
                {
                    "type": "doc",
                    "content": [
                        {
                            "type": "paragraph"
                        }
                    ]
                }
                """
            );

        public int Position { get; set; }
    }
}