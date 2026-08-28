using Learnaptic.Api.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using Learnaptic.Api.Features.StudySets.Dtos;
using Learnaptic.Api.Features.Flashcards.Dtos;
using Learnaptic.Api.Features.Notebooks.Dtos;
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
                .Include(ss => ss.Notebooks)
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

                Notebooks = studySet.Notebooks.Select(nb => new GetNotebookSummaryDto
                {
                    Id = nb.Id,
                    Title = nb.Title
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

            var requestedNotebookIds = createStudySetDto.NotebookIds
                .Distinct()
                .ToList();

            var linkedNotebooks = await _context.Notebooks
                .Where(nb => requestedNotebookIds.Contains(nb.Id) && nb.UserId == userId)
                .ToListAsync();

            if (linkedNotebooks.Count != requestedNotebookIds.Count)
            {
                return BadRequest("One or more selected notebooks do not exist.");
            }

            DateTime now = DateTime.UtcNow;

            var studySet = new StudySet
            {
                Title = createStudySetDto.Title,
                CreatedAt = now,
                UpdatedAt = now,
                LastAccessedAt = now,
                UserId = userId,
                Notebooks = linkedNotebooks,

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

                Notebooks = studySet.Notebooks.Select(nb => new GetNotebookSummaryDto
                {
                    Id = nb.Id,
                    Title = nb.Title
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
                .Include(ss => ss.Notebooks)
                .FirstOrDefaultAsync(ss =>
                    ss.Id == id &&
                    ss.UserId == userId);

            if (studySet == null)
                return NotFound();

            var requestedNotebookIds = updateStudySetDto.NotebookIds
                .Distinct()
                .ToList();

            var linkedNotebooks = await _context.Notebooks
                .Where(nb =>
                    requestedNotebookIds.Contains(nb.Id) &&
                    nb.UserId == userId)
                .ToListAsync();

            if (linkedNotebooks.Count != requestedNotebookIds.Count)
            {
                return BadRequest(
                    "One or more selected notebooks do not exist.");
            }

            if (!AreFlashcardsValid(
                studySet,
                updateStudySetDto.Flashcards))
            {
                return BadRequest("Invalid flashcard IDs.");
            }

            var now = DateTime.UtcNow;

            studySet.Title = updateStudySetDto.Title;
            studySet.UpdatedAt = now;
            studySet.LastAccessedAt = now;

            studySet.Notebooks.Clear();

            foreach (var notebook in linkedNotebooks)
            {
                studySet.Notebooks.Add(notebook);
            }

            SyncFlashcards(
                studySet,
                updateStudySetDto.Flashcards);

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

        private bool AreFlashcardsValid(StudySet studySet, List<UpdateFlashcardDto> incomingFlashcards)
        {
            var existingFlashcardIds = studySet.Flashcards
                .Select(fc => fc.Id)
                .ToHashSet();

            var incomingIds = incomingFlashcards
                .Where(fc => fc.Id.HasValue)
                .Select(fc => fc.Id.GetValueOrDefault())
                .ToList();

            if (incomingIds.Count != incomingIds.Distinct().Count())
                return false;

            if (incomingIds.Any(id => !existingFlashcardIds.Contains(id)))
                return false;

            return true;
        }

        private void SyncFlashcards(StudySet studySet, List<UpdateFlashcardDto> incomingFlashcards)
        {
            var existingFlashcards = studySet.Flashcards
                .ToDictionary(fc => fc.Id);

            var incomingIds = incomingFlashcards
                .Where(fc => fc.Id.HasValue)
                .Select(fc => fc.Id.GetValueOrDefault())
                .ToHashSet();

            int position = 0;

            foreach (var incomingFlashcard in incomingFlashcards)
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
                .Where(fc => !incomingIds.Contains(fc.Id))
                .ToList();

            _context.Flashcards.RemoveRange(flashcardsToRemove);
        }
    }
}
