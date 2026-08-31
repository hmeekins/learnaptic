using Learnaptic.Api.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using Learnaptic.Api.Features.Concepts.Dtos;

namespace Learnaptic.Api.Features.Concepts
{
    [Authorize]
    [ApiController]
    [Route("api/notebooks/{notebookId}/concepts")]
    public class ConceptsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public ConceptsController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpPost]
        public async Task<IActionResult> CreateConcept(int notebookId, CreateConceptDto createConceptDto)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var notebook = await _context.Notebooks.FirstOrDefaultAsync(nb => nb.Id == notebookId && nb.UserId == userId);

            if (notebook == null)
                return NotFound();

            var nextPosition = await _context.Concepts
                .Where(c => c.NotebookId == notebookId)
                .MaxAsync(c => (int?)c.Position) ?? -1;

            var concept = new Concept
            {
                Title = createConceptDto.Title,
                Content = createConceptDto.Content,
                Position = nextPosition + 1,
                NotebookId = notebookId
            };

            _context.Concepts.Add(concept);

            var now = DateTime.UtcNow;

            notebook.UpdatedAt = now;
            notebook.LastAccessedAt = now;

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
        public async Task<IActionResult> UpdateConcept(int conceptId, int notebookId, UpdateConceptDto updateConceptDto)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var concept = await _context.Concepts
                .Include(c => c.Notebook)
                .FirstOrDefaultAsync(c =>
                c.Id == conceptId &&
                c.NotebookId == notebookId
                && c.Notebook.UserId == userId);

            if (concept == null)
                return NotFound();

            concept.Title = updateConceptDto.Title;
            concept.Content = updateConceptDto.Content;

            var now = DateTime.UtcNow;

            concept.Notebook.UpdatedAt = now;
            concept.Notebook.LastAccessedAt = now;

            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpPut("reorder")]
        public async Task<IActionResult> ReorderConcepts(int notebookId, List<int> conceptIds)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var notebook = await _context.Notebooks.FirstOrDefaultAsync(nb => nb.Id == notebookId && nb.UserId == userId);

            if (notebook == null)
                return NotFound();

            if (conceptIds.Count != conceptIds.Distinct().Count())
                return BadRequest("Duplicate concept IDs are not allowed.");

            var concepts = await _context.Concepts
                .Where(c => c.NotebookId == notebookId)
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
            notebook.UpdatedAt = now;
            notebook.LastAccessedAt = now;

            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpDelete("{conceptId}")]
        public async Task<IActionResult> DeleteConcept(int conceptId, int notebookId)
        {

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var concept = await _context.Concepts
                .Include(c => c.Notebook)
                .FirstOrDefaultAsync(c =>
                    c.Id == conceptId &&
                    c.NotebookId == notebookId &&
                    c.Notebook.UserId == userId);

            if (concept == null)
            {
                return NotFound();
            }

            _context.Concepts.Remove(concept);

            var now = DateTime.UtcNow;

            concept.Notebook.UpdatedAt = now;
            concept.Notebook.LastAccessedAt = now;

            await _context.SaveChangesAsync();

            return NoContent();
        }  
    }
}
