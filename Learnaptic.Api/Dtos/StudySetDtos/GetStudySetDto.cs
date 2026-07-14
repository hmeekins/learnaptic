using Learnaptic.Api.Models;
using Learnaptic.Api.Dtos.FlashcardDtos;

namespace Learnaptic.Api.Dtos.StudySetDtos
{
    public class GetStudySetDto
    {
        public int Id { get; set; }

        public string Title { get; set; } = string.Empty;

        public DateTime CreatedAt { get; set; }

        public DateTime UpdatedAt { get; set; }

        public List<GetFlashcardDto> Flashcards { get; set; } = new List<GetFlashcardDto>();
    }
}
