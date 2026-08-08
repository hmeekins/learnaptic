using System.ComponentModel.DataAnnotations;

namespace Learnaptic.Api.Features.Flashcards.Dtos
{
    public class UpdateFlashcardDto
    {
        public int? Id { get; set; }

        [Required]
        [StringLength(300, MinimumLength = 1)]
        public string Question { get; set; } = string.Empty;

        [Required]
        [StringLength(1000, MinimumLength = 1)]
        public string Answer { get; set; } = string.Empty;
    }
}
