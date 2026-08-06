using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Learnaptic.Api.Models;

namespace Learnaptic.Api.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<StudyGuide> StudyGuides { get; set; }
        public DbSet<Flashcard> Flashcards { get; set; }
        public DbSet<Concept> Concepts { get; set; }
        public DbSet<StudySet> StudySets { get; set; }
    }
}
