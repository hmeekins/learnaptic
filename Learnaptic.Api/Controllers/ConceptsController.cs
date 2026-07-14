using Learnaptic.Api.Data;
using Learnaptic.Api.Dtos;
using Learnaptic.Api.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Learnaptic.Api.Controllers
{
    [ApiController]
    [Route("api/study-guides/{studyGuideId}/concepts")]
    public class ConceptsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public ConceptsController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetConceptList(int studyGuideId)
        {
            bool studyGuideExists = await _context.StudyGuides.AnyAsync(sg => sg.Id == studyGuideId);

            if (!studyGuideExists)
                return NotFound();

            var concepts = await _context.Concepts
                .Where(c => c.StudyGuideId == studyGuideId)
                .OrderBy(c => c.Id)
                .Select(c => new GetConceptDto
                {
                    Id = c.Id,
                    StudyGuideId = c.StudyGuideId,
                    Title = c.Title,
                    Content = c.Content
                })
                .ToListAsync();

            return Ok(concepts);
        }

        [HttpGet("{id}")] 
        public async Task<IActionResult> GetConceptById(int studyGuideId, int id)
        {
            var concept = await _context.Concepts
                .Where(c => c.StudyGuideId == studyGuideId && c.Id == id)
                .Select(c => new GetConceptDto
                {
                    Id = c.Id,
                    StudyGuideId = c.StudyGuideId,
                    Title = c.Title,
                    Content = c.Content
                })
                .FirstOrDefaultAsync();

            if (concept == null)
                return NotFound();

            return Ok(concept);
        }

        [HttpPost]
        public async Task<IActionResult> CreateConcept(int studyGuideId, CreateConceptDto createConceptDto)
        {
            var studyGuide = await _context.StudyGuides
                .FindAsync(studyGuideId);

            if (studyGuide == null)
                return NotFound();

            var newConcept = new Concept
            {
                StudyGuideId = studyGuideId,
                Title = createConceptDto.Title,
                Content = createConceptDto.Content
            };

            DateTime now = DateTime.UtcNow;
            studyGuide.UpdatedAt = now;
            studyGuide.LastAccessedAt = now;
            
            _context.Concepts.Add(newConcept);
            await _context.SaveChangesAsync();

            var createdConceptDto = new GetConceptDto
            {
                Id = newConcept.Id,
                StudyGuideId = newConcept.StudyGuideId,
                Title = newConcept.Title,
                Content = newConcept.Content
            };

            return CreatedAtAction(nameof(GetConceptById), new { studyGuideId, id = newConcept.Id }, createdConceptDto);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateConcept(int studyGuideId, int id, UpdateConceptDto updateConceptDto)
        {
            var studyGuide = await _context.StudyGuides.FindAsync(studyGuideId);
            if (studyGuide == null)
                return NotFound();

            var concept = await _context.Concepts
                .FirstOrDefaultAsync(c =>
                    c.StudyGuideId == studyGuideId &&
                    c.Id == id);

            if (concept == null)
                return NotFound();

            concept.Title = updateConceptDto.Title;
            concept.Content = updateConceptDto.Content;

            DateTime now = DateTime.UtcNow;
            studyGuide.UpdatedAt = now;
            studyGuide.LastAccessedAt = now;

            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteConcept(int studyGuideId, int id)
        {
            var studyGuide = await _context.StudyGuides.FindAsync(studyGuideId);
            if (studyGuide == null)
                return NotFound();

            var concept = await _context.Concepts
                .FirstOrDefaultAsync(c =>
                    c.StudyGuideId == studyGuideId &&
                    c.Id == id);

            if (concept == null)
                return NotFound();

            _context.Concepts.Remove(concept);

            DateTime now = DateTime.UtcNow;
            studyGuide.UpdatedAt = now;
            studyGuide.LastAccessedAt = now;

            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}
