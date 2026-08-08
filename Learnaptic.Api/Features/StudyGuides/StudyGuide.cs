using Learnaptic.Api.Features.Auth;
using Learnaptic.Api.Features.Concepts;
using Learnaptic.Api.Features.StudySets;

public class StudyGuide
{
    public int Id { get; set; }

    public string Title { get; set; } = string.Empty;

    public string? Subject { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }

    public DateTime LastAccessedAt { get; set; }

    public string UserId { get; set; } = string.Empty;

    public ApplicationUser User { get; set; } = null!;

    public List<Concept> Concepts { get; set; } = new();

    public List<StudySet> StudySets { get; set; } = new();
}