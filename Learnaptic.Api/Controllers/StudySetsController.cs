using Learnaptic.Api.Data;
using Learnaptic.Api.Dtos.StudySetDtos;
using Learnaptic.Api.Dtos.FlashcardDtos;
using Learnaptic.Api.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Learnaptic.Api.Controllers
{
    [Route("api/study-sets")]
    [ApiController]
    public class StudySetsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public StudySetsController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetStudySets()
        {
            var studySets = await _context.StudySets
                .OrderByDescending(ss => ss.LastAccessedAt)
                .Select(ss => new GetStudySetListDto
                {
                    Id = ss.Id,
                    Title = ss.Title
                })
                .ToListAsync();

            return Ok(studySets);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetStudySet(int id)
        {
            var studySet = await _context.StudySets
                .Include(ss => ss.Flashcards)
                .FirstOrDefaultAsync(ss => ss.Id == id);

            if (studySet == null)
            {
                return NotFound();
            }

            var studySetDto = new GetStudySetDto
            {
                Id = studySet.Id,
                Title = studySet.Title,
                CreatedAt = studySet.CreatedAt,
                UpdatedAt = studySet.UpdatedAt,
                Flashcards = studySet.Flashcards.Select(fc => new GetFlashcardDto
                {
                    Id = fc.Id,
                    Question = fc.Question,
                    Answer = fc.Answer
                }).ToList()
            };

            studySet.LastAccessedAt = DateTime.UtcNow;
            await _context.SaveChangesAsync();

            return Ok(studySetDto);
        }

        [HttpPost]
        public async Task<IActionResult> CreateStudySet(CreateStudySetDto createStudySetDto)
        {
            DateTime now = DateTime.UtcNow;
            var studySet = new StudySet
            {
                Title = createStudySetDto.Title,
                CreatedAt = now,
                UpdatedAt = now,
                LastAccessedAt = now,
                Flashcards = createStudySetDto.Flashcards
                    .Select(fc => new Flashcard
                    {
                        Question = fc.Question,
                        Answer = fc.Answer
                    })
                    .ToList()
            };

            _context.StudySets.Add(studySet);
            await _context.SaveChangesAsync();

            var studySetDto = new GetStudySetDto
            {
                Id = studySet.Id,
                Title = studySet.Title,
                CreatedAt = studySet.CreatedAt,
                UpdatedAt = studySet.UpdatedAt,
                Flashcards = studySet.Flashcards.Select(fc => new GetFlashcardDto
                {
                    Id = fc.Id,
                    Question = fc.Question,
                    Answer = fc.Answer
                }).ToList()
            };

            return CreatedAtAction(nameof(GetStudySet), new { id = studySet.Id }, studySetDto);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateStudySet(int id, UpdateStudySetDto updateStudySetDto)
        {
            var studySet = await _context.StudySets
                .Include(ss => ss.Flashcards)
                .FirstOrDefaultAsync(ss => ss.Id == id);

            if (studySet == null)
                return NotFound();

            var existingFlashcards = studySet.Flashcards
                .ToDictionary(fc => fc.Id);

            var incomingIds = updateStudySetDto.Flashcards
                .Where(fc => fc.Id.HasValue)
                .Select(fc => fc.Id.Value)
                .ToHashSet();

            foreach (var incomingFlashcard in updateStudySetDto.Flashcards)
            {
                if (incomingFlashcard.Id.HasValue)
                {
                    if (!existingFlashcards.TryGetValue(incomingFlashcard.Id.Value, out var existingFlashcard))
                    {
                        return BadRequest(
                            $"Flashcard {incomingFlashcard.Id.Value} does not belong to this study set.");
                    }

                    existingFlashcard.Question = incomingFlashcard.Question;
                    existingFlashcard.Answer = incomingFlashcard.Answer;
                }
                else
                {
                    var newFlashcard = new Flashcard
                    {
                        Question = incomingFlashcard.Question,
                        Answer = incomingFlashcard.Answer
                    };

                    studySet.Flashcards.Add(newFlashcard);
                }
            }

            var flashcardsToRemove = existingFlashcards.Values
                .Where(fc => !incomingIds.Contains(fc.Id))
                .ToList();

            _context.Flashcards.RemoveRange(flashcardsToRemove);

            DateTime now = DateTime.UtcNow;
            studySet.Title = updateStudySetDto.Title;
            studySet.UpdatedAt = now;
            studySet.LastAccessedAt = now;

            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteStudySet(int id)
        {
            var studySet = await _context.StudySets.FindAsync(id);
            if (studySet == null)
            {
                return NotFound();
            }

            _context.StudySets.Remove(studySet);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
