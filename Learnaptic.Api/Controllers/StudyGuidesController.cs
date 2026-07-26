using Learnaptic.Api.Data;
using Learnaptic.Api.Dtos.StudyGuideDtos;
using Learnaptic.Api.Dtos.ConceptDtos;
using Learnaptic.Api.Dtos.StudySetDtos;
using Learnaptic.Api.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Learnaptic.Api.Controllers
{
    [ApiController]
    [Route("api/study-guides")]
    public class StudyGuidesController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public StudyGuidesController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetStudyGuides()
        {
            var studyGuides = await _context.StudyGuides
                .OrderByDescending(sg => sg.LastAccessedAt)
                .Select(sg => new GetStudyGuideListDto
                {
                    Id = sg.Id,
                    Title = sg.Title,
                    Subject = sg.Subject,
                    LastAccessedAt = sg.LastAccessedAt
                })
                .ToListAsync();

            return Ok(studyGuides);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetStudyGuideById(int id)
        {
            var studyGuide = await _context.StudyGuides
                .Include(sg => sg.StudySets)
                .Include(sg => sg.Concepts)
                .FirstOrDefaultAsync(sg => sg.Id == id);

            if (studyGuide == null)
                return NotFound();

            var studyGuideDto = new GetStudyGuideDto
            {
                Id = studyGuide.Id,
                Title = studyGuide.Title,
                Description = studyGuide.Description,
                Subject = studyGuide.Subject,

                StudySets = studyGuide.StudySets
                    .Select(ss => new GetStudySetListDto
                    {
                        Id = ss.Id,
                        Title = ss.Title
                    })
                    .ToList(),

                Concepts = studyGuide.Concepts
                    .OrderBy(c => c.Position)
                    .Select(c => new GetConceptDto
                    {
                        Id = c.Id,
                        Title = c.Title,
                        Content = c.Content,
                        Position = c.Position
                    })
                    .ToList(),

                UpdatedAt = studyGuide.UpdatedAt,
                CreatedAt = studyGuide.CreatedAt
            };

            studyGuide.LastAccessedAt = DateTime.UtcNow;
            await _context.SaveChangesAsync();

            return Ok(studyGuideDto);
        }

        [HttpPost]
        public async Task<IActionResult> CreateStudyGuide(CreateStudyGuideDto dto)
        {
            var requestedStudySetIds = dto.StudySetIds
                .Distinct()
                .ToList();

            var linkedStudySets = await _context.StudySets
                .Where(ss => requestedStudySetIds.Contains(ss.Id))
                .ToListAsync();

            if (linkedStudySets.Count != requestedStudySetIds.Count)
            {
                return BadRequest(
                    "One or more selected study sets do not exist.");
            }

            DateTime now = DateTime.UtcNow;

            var studyGuide = new StudyGuide
            {
                Title = dto.Title,
                Description = dto.Description,
                Subject = dto.Subject,
                CreatedAt = now,
                UpdatedAt = now,
                LastAccessedAt = now,
                StudySets = linkedStudySets,

                Concepts = dto.Concepts
                    .Select((c, position) => new Concept
                    {
                        Title = c.Title,
                        Content = c.Content,
                        Position = position
                    })
                    .ToList()
            };

            _context.StudyGuides.Add(studyGuide);
            await _context.SaveChangesAsync();

            var responseStudyGuide = new GetStudyGuideDto
            {
                Id = studyGuide.Id,
                Title = studyGuide.Title,
                Description = studyGuide.Description,
                Subject = studyGuide.Subject,

                StudySets = studyGuide.StudySets
                    .Select(ss => new GetStudySetListDto
                    {
                        Id = ss.Id,
                        Title = ss.Title
                    })
                    .ToList(),

                Concepts = studyGuide.Concepts
                    .OrderBy(c => c.Position)
                    .Select(c => new GetConceptDto
                    {
                        Id = c.Id,
                        Title = c.Title,
                        Content = c.Content,
                        Position = c.Position
                    })
                    .ToList(),

                UpdatedAt = studyGuide.UpdatedAt,
                CreatedAt = studyGuide.CreatedAt
            };

            return CreatedAtAction(
                nameof(GetStudyGuideById),
                new { id = studyGuide.Id },
                responseStudyGuide);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateStudyGuide(
            int id,
            UpdateStudyGuideDto dto)
        {
            var studyGuide = await _context.StudyGuides
                .Include(sg => sg.StudySets)
                .Include(sg => sg.Concepts)
                .FirstOrDefaultAsync(sg => sg.Id == id);

            if (studyGuide == null)
                return NotFound();

            var requestedStudySetIds = dto.StudySetIds
                .Distinct()
                .ToList();

            var linkedStudySets = await _context.StudySets
                .Where(ss => requestedStudySetIds.Contains(ss.Id))
                .ToListAsync();

            if (linkedStudySets.Count != requestedStudySetIds.Count)
            {
                return BadRequest(
                    "One or more selected study sets do not exist.");
            }

            var existingConcepts = studyGuide.Concepts
                .ToDictionary(c => c.Id);

            var incomingConceptIds = dto.Concepts
                .Where(c => c.Id.HasValue)
                .Select(c => c.Id!.Value)
                .ToList();

            if (incomingConceptIds.Count !=
                incomingConceptIds.Distinct().Count())
            {
                return BadRequest(
                    "Duplicate concept IDs are not allowed.");
            }

            var incomingConceptIdSet =
                incomingConceptIds.ToHashSet();

            var invalidConceptId = incomingConceptIdSet
                .FirstOrDefault(
                    conceptId =>
                        !existingConcepts.ContainsKey(conceptId));

            if (invalidConceptId != 0)
            {
                return BadRequest(
                    $"Concept {invalidConceptId} does not belong to this study guide.");
            }

            studyGuide.Title = dto.Title;
            studyGuide.Description = dto.Description;
            studyGuide.Subject = dto.Subject;

            studyGuide.StudySets.Clear();

            foreach (var studySet in linkedStudySets)
            {
                studyGuide.StudySets.Add(studySet);
            }

            for (int position = 0; position < dto.Concepts.Count; position++)
            {
                var incomingConcept = dto.Concepts[position];

                if (incomingConcept.Id.HasValue)
                {
                    var existingConcept = existingConcepts[incomingConcept.Id.Value];

                    existingConcept.Title = incomingConcept.Title;

                    existingConcept.Content = incomingConcept.Content;

                    existingConcept.Position = position;
                }
                else
                {
                    var newConcept = new Concept
                    {
                        Title = incomingConcept.Title,
                        Content = incomingConcept.Content,
                        Position = position
                    };

                    studyGuide.Concepts.Add(newConcept);
                }
            }

            var conceptsToRemove = existingConcepts.Values
                .Where(c =>
                    !incomingConceptIdSet.Contains(c.Id))
                .ToList();

            _context.Concepts.RemoveRange(conceptsToRemove);

            DateTime now = DateTime.UtcNow;
            studyGuide.UpdatedAt = now;
            studyGuide.LastAccessedAt = now;

            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteStudyGuide(int id)
        {
            var studyGuide =
                await _context.StudyGuides.FindAsync(id);

            if (studyGuide == null)
                return NotFound();

            _context.StudyGuides.Remove(studyGuide);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}