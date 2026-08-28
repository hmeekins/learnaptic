using System.ComponentModel.DataAnnotations;

namespace Learnaptic.Api.Features.Notebooks.Dtos
{
    public class CreateNotebookDto
    {
        [Required]
        [StringLength(100, MinimumLength = 3)]
        public string Title { get; set; } = string.Empty;

        [StringLength(40, MinimumLength = 1)]
        public string? Subject { get; set; }
    }
}
