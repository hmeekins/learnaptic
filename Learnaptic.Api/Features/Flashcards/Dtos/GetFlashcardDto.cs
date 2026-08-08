namespace Learnaptic.Api.Features.Flashcards.Dtos
{
    public class GetFlashcardDto
    {
        public int Id { get; set; }

        public string Question { get; set; } = string.Empty;

        public string Answer { get; set; } = string.Empty;
    }
}
