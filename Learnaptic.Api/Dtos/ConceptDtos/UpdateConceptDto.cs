using System.ComponentModel.DataAnnotations;

namespace Learnaptic.Api.Dtos.ConceptDtos
{
    public class UpdateConceptDto
    {
        [Required]
        [StringLength(100, MinimumLength = 3)]
        public string Title { get; set; } = string.Empty;

        [Required]
        [StringLength(20000, MinimumLength = 20)]
        public string Content { get; set; } = string.Empty;
    }
}
