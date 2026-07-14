namespace Learnaptic.Api.Dtos.FlashcardDtos
{
    public class GetFlashcardDto
    {
        public int Id { get; set; }

        public int StudySetId { get; set; }

        public string Question { get; set; } = string.Empty;

        public string Answer { get; set; } = string.Empty;
    }
}
