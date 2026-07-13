using Learnaptic.Api.Data;
using Learnaptic.Api.Dtos;
using Learnaptic.Api.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Learnaptic.Api.Controllers
{
    [ApiController]
    [Route("api/study-guides/{studyGuideId}/concepts")]
    public class ConceptController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public ConceptController(ApplicationDbContext context)
        {
            _context = context;
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

            studyGuide.LastAccessedAt = DateTime.UtcNow;

            _context.Concepts.Add(newConcept);
            await _context.SaveChangesAsync();

            var createdConceptDto = new GetConceptDto
            {
                Id = newConcept.Id,
                StudyGuideId = newConcept.StudyGuideId,
                Title = newConcept.Title,
                Content = newConcept.Content
            };

            return CreatedAtAction(nameof(GetConceptById), new { studyGuideId = studyGuideId, id = newConcept.Id }, createdConceptDto);
        }
    }
}
