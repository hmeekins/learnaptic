namespace Learnaptic.Api.Data
{
    using Microsoft.EntityFrameworkCore;
    using Learnaptic.Api.Models;
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<StudyGuide> StudyGuides { get; set; }
        public DbSet<Flashcard> Flashcards { get; set; }
        public DbSet<Concept> Concepts { get; set; }
        public DbSet<KeyPoint> KeyPoints { get; set; }
    }
}
