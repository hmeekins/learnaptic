using Learnaptic.Api.Features.Flashcards.Dtos;
using System.ComponentModel.DataAnnotations;

namespace Learnaptic.Api.Features.StudySets.Dtos
{
    public class UpdateStudySetDto
    {
        [Required]
        [StringLength(200, MinimumLength = 2)]
        public string Title { get; set; } = string.Empty;

        [Required]
        [MinLength(2)]
        public List<UpdateFlashcardDto> Flashcards { get; set; } = new();

        public List<int> StudyGuideIds { get; set; } = new();
    }
}
