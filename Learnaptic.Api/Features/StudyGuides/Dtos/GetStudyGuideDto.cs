using Learnaptic.Api.Features.Concepts.Dtos;
using Learnaptic.Api.Features.StudySets.Dtos;

namespace Learnaptic.Api.Features.StudyGuides.Dtos
{
    public class GetStudyGuideDto
    {
        public int Id { get; set; }

        public string Title { get; set; } = string.Empty;

        public string? Subject { get; set; }

        public List<GetConceptDto> Concepts { get; set; } = new();

        public List<GetStudySetListDto> StudySets { get; set; } = new();

        public DateTime UpdatedAt { get; set; }

        public DateTime CreatedAt { get; set; }
    }
}
