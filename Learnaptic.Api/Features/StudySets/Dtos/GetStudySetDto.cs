using Learnaptic.Api.Features.Flashcards.Dtos;
using Learnaptic.Api.Features.StudyGuides.Dtos;

namespace Learnaptic.Api.Features.StudySets.Dtos
{
    public class GetStudySetDto
    {
        public int Id { get; set; }

        public string Title { get; set; } = string.Empty;

        public DateTime CreatedAt { get; set; }

        public DateTime UpdatedAt { get; set; }

        public List<GetStudyGuideSummaryDto> StudyGuides { get; set; } = new();

        public List<GetFlashcardDto> Flashcards { get; set; } = new();
    }
}
