using System.ComponentModel.DataAnnotations;

namespace Learnaptic.Api.Dtos
{
    public class UpdateStudyGuideDto
    {
        [Required]
        [StringLength(100, MinimumLength = 3)]
        public string Title { get; set; } = string.Empty;

        public string? Description { get; set; }

        [StringLength(40, MinimumLength = 1)]
        public string? Subject { get; set; }
    }
}
