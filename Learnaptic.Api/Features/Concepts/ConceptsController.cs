using Learnaptic.Api.Data;
using Learnaptic.Api.Features.Concepts.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace Learnaptic.Api.Features.Concepts
{
    [Authorize]
    [ApiController]
    [Route("api/study-guides/{studyGuideId}/concepts")]
    public class ConceptsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public ConceptsController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpPost]
        public async Task<IActionResult> CreateConcept(int studyGuideId, CreateConceptDto createConceptDto)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var studyGuide = await _context.StudyGuides.FirstOrDefaultAsync(sg => sg.Id == studyGuideId && sg.UserId == userId);

            if (studyGuide == null)
                return NotFound();

            var nextPosition = await _context.Concepts
                .Where(c => c.StudyGuideId == studyGuideId)
                .MaxAsync(c => (int?)c.Position) ?? -1;

            var concept = new Concept
            {
                Title = createConceptDto.Title,
                Content = createConceptDto.Content,
                Position = nextPosition + 1,
                StudyGuideId = studyGuideId
            };

            _context.Concepts.Add(concept);

            var now = DateTime.UtcNow;

            studyGuide.UpdatedAt = now;
            studyGuide.LastAccessedAt = now;

            await _context.SaveChangesAsync();

            var response = new GetConceptDto
            {
                Id = concept.Id,
                Title = concept.Title,
                Content = concept.Content,
            };

            return StatusCode(StatusCodes.Status201Created, response);
        }

        [HttpPut("{conceptId}")]
        public async Task<IActionResult> UpdateConcept(int conceptId, int studyGuideId, UpdateConceptDto updateConceptDto)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var concept = await _context.Concepts
                .Include(c => c.StudyGuide)
                .FirstOrDefaultAsync(c =>
                c.Id == conceptId &&
                c.StudyGuideId == studyGuideId
                && c.StudyGuide.UserId == userId);

            if (concept == null)
                return NotFound();

            concept.Title = updateConceptDto.Title;
            concept.Content = updateConceptDto.Content;

            var now = DateTime.UtcNow;

            concept.StudyGuide.UpdatedAt = now;
            concept.StudyGuide.LastAccessedAt = now;

            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpPut("reorder")]
        public async Task<IActionResult> ReorderConcepts(int studyGuideId, List<int> conceptIds)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var studyGuide = await _context.StudyGuides.FirstOrDefaultAsync(sg => sg.Id == studyGuideId && sg.UserId == userId);

            if (studyGuide == null)
                return NotFound();

            if (conceptIds.Count != conceptIds.Distinct().Count())
                return BadRequest("Duplicate concept IDs are not allowed.");

            var concepts = await _context.Concepts
                .Where(c => c.StudyGuideId == studyGuideId)
                .ToListAsync();

            if (concepts.Count != conceptIds.Count)
            {
                return BadRequest("The request must contain every concept exactly once.");
            }

            var conceptsById = concepts.ToDictionary(c => c.Id);

            if (conceptIds.Any(id => !conceptsById.ContainsKey(id)))
            {
                return BadRequest(
                    "One or more concepts do not belong to this study guide.");
            }

            for (int i = 0; i < conceptIds.Count; i++)
            {
                conceptsById[conceptIds[i]].Position = i;
            }

            var now = DateTime.UtcNow;
            studyGuide.UpdatedAt = now;
            studyGuide.LastAccessedAt = now;

            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpDelete("{conceptId}")]
        public async Task<IActionResult> DeleteConcept(int conceptId, int studyGuideId)
        {

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var concept = await _context.Concepts
                .Include(c => c.StudyGuide)
                .FirstOrDefaultAsync(c =>
                    c.Id == conceptId &&
                    c.StudyGuideId == studyGuideId &&
                    c.StudyGuide.UserId == userId);

            if (concept == null)
            {
                return NotFound();
            }

            _context.Concepts.Remove(concept);

            var now = DateTime.UtcNow;

            concept.StudyGuide.UpdatedAt = now;
            concept.StudyGuide.LastAccessedAt = now;

            await _context.SaveChangesAsync();

            return NoContent();
        }  
    }
}
