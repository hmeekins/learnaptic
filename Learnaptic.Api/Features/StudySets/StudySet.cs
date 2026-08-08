using Learnaptic.Api.Features.Auth;
using Learnaptic.Api.Features.Flashcards;

namespace Learnaptic.Api.Features.StudySets
{
    public class StudySet
    {
        public int Id { get; set; }

        public string Title { get; set; } = string.Empty;

        public DateTime CreatedAt { get; set; }

        public DateTime UpdatedAt { get; set; }

        public DateTime LastAccessedAt { get; set; }

        public string UserId { get; set; } = string.Empty;

        public ApplicationUser User { get; set; } = null!;

        public List<StudyGuide> StudyGuides { get; set; } = new();

        public List<Flashcard> Flashcards { get; set; } = new();
    }
}
