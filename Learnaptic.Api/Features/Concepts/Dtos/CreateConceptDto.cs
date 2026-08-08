using System.ComponentModel.DataAnnotations;

namespace Learnaptic.Api.Features.Concepts.Dtos
{
    public class CreateConceptDto
    {
        [Required]
        [StringLength(100, MinimumLength = 3)]
        public string Title { get; set; } = string.Empty;

        [Required]
        [StringLength(20000, MinimumLength = 20)]
        public string Content { get; set; } = string.Empty;
    }
}
