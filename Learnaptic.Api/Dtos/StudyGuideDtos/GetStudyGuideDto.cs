using Learnaptic.Api.Dtos.ConceptDtos;
using Learnaptic.Api.Dtos.StudySetDtos;

namespace Learnaptic.Api.Dtos.StudyGuideDtos
{
    public class GetStudyGuideDto
    {
        public int Id { get; set; }

        public string Title { get; set; } = string.Empty;

        public string? Description { get; set; }

        public string? Subject { get; set; }

        public List<GetStudySetListDto> StudySets { get; set; } = new();

        public List<GetConceptDto> Concepts { get; set; } = new();

        public DateTime UpdatedAt { get; set; }

        public DateTime CreatedAt { get; set; }
    }
}
