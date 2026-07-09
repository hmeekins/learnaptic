using System.ComponentModel.DataAnnotations;

namespace Learnaptic.Api.Dtos
{
    public class CreateConceptDto
    {
        [Required]
        [StringLength(100, MinimumLength = 3)]
        public string Title { get; set; } = string.Empty;

        [Required]
        [StringLength(5000, MinimumLength = 20)]
        public string Explanation { get; set; } = string.Empty;

        public List<string> KeyPoints { get; set; } = new();
    }
}
