using Learnaptic.Api.Features.StudySets;

namespace Learnaptic.Api.Features.Flashcards
{
    public class Flashcard
    {
        public int Id { get; set; }

        public int StudySetId { get; set; }

        public StudySet StudySet { get; set; } = null!;

        public int Position { get; set; }

        public string Question { get; set; } = string.Empty;

        public string Answer { get; set; } = string.Empty;
    }
}
