using Learnaptic.Api.Data;
using Learnaptic.Api.Dtos.StudySetDtos;
using Learnaptic.Api.Dtos.FlashcardDtos;
using Learnaptic.Api.Dtos.StudyGuideDtos;
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
                .Include(ss => ss.StudyGuides)
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

                StudyGuides = studySet.StudyGuides.Select(sg => new GetStudyGuideSummaryDto
                {
                    Id = sg.Id,
                    Title = sg.Title
                }).ToList(),

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
            var requestedStudyGuideIds = createStudySetDto.StudyGuideIds
                .Distinct()
                .ToList();

            var linkedStudyGuides = await _context.StudyGuides
                .Where(sg => requestedStudyGuideIds.Contains(sg.Id))
                .ToListAsync();

            if (linkedStudyGuides.Count != requestedStudyGuideIds.Count)
            {
                return BadRequest("One or more selected study guides do not exist.");
            }

            var studySet = new StudySet
            {
                Title = createStudySetDto.Title,
                CreatedAt = now,
                UpdatedAt = now,
                LastAccessedAt = now,

                StudyGuides = linkedStudyGuides,

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

                StudyGuides = studySet.StudyGuides.Select(sg => new GetStudyGuideSummaryDto
                {
                    Id = sg.Id,
                    Title = sg.Title
                }).ToList(),

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
                .Include(ss => ss.StudyGuides)
                .FirstOrDefaultAsync(ss => ss.Id == id);

            if (studySet == null)
                return NotFound();

            var requestedStudyGuideIds = updateStudySetDto.StudyGuideIds
                .Distinct()
                .ToList();

            var linkedStudyGuides = await _context.StudyGuides
                .Where(sg => requestedStudyGuideIds.Contains(sg.Id))
                .ToListAsync();

            if (linkedStudyGuides.Count != requestedStudyGuideIds.Count)
            {
                return BadRequest("One or more selected study guides do not exist.");
            }

            var existingFlashcards = studySet.Flashcards
                .ToDictionary(fc => fc.Id);

            var incomingIds = updateStudySetDto.Flashcards
                .Where(fc => fc.Id.HasValue)
                .Select(fc => fc.Id.Value)
                .ToHashSet();

            var invalidFlashcardId = incomingIds
                .FirstOrDefault(id => !existingFlashcards.ContainsKey(id));

            if (invalidFlashcardId != 0)
            {
                return BadRequest(
                    $"Flashcard {invalidFlashcardId} does not belong to this study set.");
            }

            DateTime now = DateTime.UtcNow;
            studySet.Title = updateStudySetDto.Title;
            studySet.UpdatedAt = now;
            studySet.LastAccessedAt = now;

            studySet.StudyGuides.Clear();

            foreach (var studyGuide in linkedStudyGuides)
            {
                studySet.StudyGuides.Add(studyGuide);
            }

            foreach (var incomingFlashcard in updateStudySetDto.Flashcards)
            {
                if (incomingFlashcard.Id.HasValue)
                {
                    var existingFlashcard =
                        existingFlashcards[incomingFlashcard.Id.Value];

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
