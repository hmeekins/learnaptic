namespace Learnaptic.Api.Models
{
    public class StudyGuide
    {
        public int Id { get; set; }
        public string Title { get; set; } = "";
        public string Description { get; set; } = "";
        public string Subject { get; set; } = "";
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public List<Concept> Concepts { get; set; } = new();
        public List<Flashcard> Flashcards { get; set; } = new();
    }
}
