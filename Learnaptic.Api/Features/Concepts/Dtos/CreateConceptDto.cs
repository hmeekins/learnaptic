using System.ComponentModel.DataAnnotations;
using System.Text.Json;

namespace Learnaptic.Api.Features.Concepts.Dtos
{
    public class CreateConceptDto
    {
        [StringLength(100, MinimumLength = 3)]
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
    }
}
