using Learnaptic.Api.Dtos.FlashcardDtos;        
using System.ComponentModel.DataAnnotations;

namespace Learnaptic.Api.Dtos.StudySetDtos
{
    public class CreateStudySetDto
    {
        [Required]
        [StringLength(200, MinimumLength = 2)]
        public string Title { get; set; } = string.Empty;

        [MinLength(2)]
        public List<CreateFlashcardDto> Flashcards { get; set; } = new();
    }
}
