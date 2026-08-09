using Learnaptic.Api.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using Learnaptic.Api.Features.StudySets.Dtos;
using Learnaptic.Api.Features.Flashcards.Dtos;
using Learnaptic.Api.Features.StudyGuides.Dtos;
using Learnaptic.Api.Features.Flashcards;

namespace Learnaptic.Api.Features.StudySets
{
    [Authorize]
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
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var studySets = await _context.StudySets
                .Where(ss => ss.UserId == userId)
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
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var studySet = await _context.StudySets
                .Include(ss => ss.Flashcards)
                .Include(ss => ss.StudyGuides)
                .FirstOrDefaultAsync(ss => ss.Id == id && ss.UserId == userId);

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

                Flashcards = studySet.Flashcards
                .OrderBy(fc => fc.Position)
                .Select(fc => new GetFlashcardDto
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
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (userId == null)
                return Unauthorized();

            var requestedStudyGuideIds = createStudySetDto.StudyGuideIds
                .Distinct()
                .ToList();

            var linkedStudyGuides = await _context.StudyGuides
                .Where(sg => requestedStudyGuideIds.Contains(sg.Id) && sg.UserId == userId)
                .ToListAsync();

            if (linkedStudyGuides.Count != requestedStudyGuideIds.Count)
            {
                return BadRequest("One or more selected study guides do not exist.");
            }

            DateTime now = DateTime.UtcNow;

            var studySet = new StudySet
            {
                Title = createStudySetDto.Title,
                CreatedAt = now,
                UpdatedAt = now,
                LastAccessedAt = now,
                UserId = userId,
                StudyGuides = linkedStudyGuides,

                Flashcards = createStudySetDto.Flashcards
                    .Select((fc, index) => new Flashcard
                    {
                        Question = fc.Question,
                        Answer = fc.Answer,
                        Position = index
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

                Flashcards = studySet.Flashcards
                    .OrderBy(fc => fc.Position)
                    .Select(fc => new GetFlashcardDto
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
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var studySet = await _context.StudySets
                .Include(ss => ss.Flashcards)
                .Include(ss => ss.StudyGuides)
                .FirstOrDefaultAsync(ss => ss.Id == id && ss.UserId == userId);

            if (studySet == null)
                return NotFound();

            var requestedStudyGuideIds = updateStudySetDto.StudyGuideIds
                .Distinct()
                .ToList();

            var linkedStudyGuides = await _context.StudyGuides
                .Where(sg => requestedStudyGuideIds.Contains(sg.Id) && sg.UserId == userId)
                .ToListAsync();

            if (linkedStudyGuides.Count != requestedStudyGuideIds.Count)
            {
                return BadRequest("One or more selected study guides do not exist.");
            }

            var existingFlashcards = studySet.Flashcards
                .ToDictionary(fc => fc.Id);

            var incomingIds = updateStudySetDto.Flashcards
                .Where(fc => fc.Id.HasValue)
                .Select(fc => fc.Id.GetValueOrDefault())
                .ToList();

            if (incomingIds.Count != incomingIds.Distinct().Count())
            {
                return BadRequest("Duplicate flashcard IDs are not allowed.");
            }
            
            var incomingIdsSet = incomingIds.ToHashSet();

            var invalidFlashcardId = incomingIdsSet
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

            int position = 0;

            foreach (var incomingFlashcard in updateStudySetDto.Flashcards)
            {
                if (incomingFlashcard.Id.HasValue)
                {
                    var existingFlashcard =
                        existingFlashcards[incomingFlashcard.Id.Value];

                    existingFlashcard.Question = incomingFlashcard.Question;
                    existingFlashcard.Answer = incomingFlashcard.Answer;
                    existingFlashcard.Position = position;
                }
                else
                {
                    var newFlashcard = new Flashcard
                    {
                        Question = incomingFlashcard.Question,
                        Answer = incomingFlashcard.Answer,
                        Position = position
                    };

                    studySet.Flashcards.Add(newFlashcard);
                }

                position++;
            }

            var flashcardsToRemove = existingFlashcards.Values
                .Where(fc => !incomingIdsSet.Contains(fc.Id))
                .ToList();

            _context.Flashcards.RemoveRange(flashcardsToRemove);

            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteStudySet(int id)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var studySet = await _context.StudySets.FirstOrDefaultAsync(ss => ss.Id == id && ss.UserId == userId);

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
