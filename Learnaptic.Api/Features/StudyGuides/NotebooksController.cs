using Learnaptic.Api.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using Learnaptic.Api.Features.Notebooks.Dtos;
using Learnaptic.Api.Features.Concepts.Dtos;
using Learnaptic.Api.Features.StudySets.Dtos;

namespace Learnaptic.Api.Features.Notebooks
{
    [Authorize]
    [ApiController]
    [Route("api/notebooks")]
    public class NotebooksController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public NotebooksController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetNotebooks()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var notebooks = await _context.Notebooks
                .Where(nb => nb.UserId == userId)
                .OrderByDescending(nb => nb.LastAccessedAt)
                .Select(nb => new GetNotebookSummaryDto
                {
                    Id = nb.Id,
                    Title = nb.Title,
                    Subject = nb.Subject,
                    LastAccessedAt = nb.LastAccessedAt
                })
                .ToListAsync();

            return Ok(notebooks);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetNotebookById(int id)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var notebook = await _context.Notebooks
                .Include(nb => nb.StudySets)
                .Include(nb => nb.Concepts)
                .FirstOrDefaultAsync(nb => nb.Id == id && nb.UserId == userId);

            if (notebook == null)
                return NotFound();

            var notebookDto = new GetNotebookDto
            {
                Id = notebook.Id,
                Title = notebook.Title,
                Subject = notebook.Subject,

                Concepts = notebook.Concepts
                    .OrderBy(c => c.Position)
                    .Select(c => new GetConceptDto
                    {
                        Id = c.Id,
                        Title = c.Title,
                        Content = c.Content,
                    })
                    .ToList(),

                StudySets = notebook.StudySets
                    .Select(ss => new GetStudySetListDto
                    {
                        Id = ss.Id,
                        Title = ss.Title
                    })
                    .ToList(),

                UpdatedAt = notebook.UpdatedAt,
                CreatedAt = notebook.CreatedAt
            };

            notebook.LastAccessedAt = DateTime.UtcNow;
            await _context.SaveChangesAsync();

            return Ok(notebookDto);
        }

        [HttpPost]
        public async Task<IActionResult> CreateNotebook(CreateNotebookDto dto)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (userId == null)
                return Unauthorized();

            DateTime now = DateTime.UtcNow;
            var notebook = new Notebook
            {
                Title = dto.Title,
                Subject = dto.Subject,
                CreatedAt = now,
                UpdatedAt = now,
                LastAccessedAt = now,
                UserId = userId
            };

            _context.Notebooks.Add(notebook);
            await _context.SaveChangesAsync();

            var responseNotebook = new GetNotebookSummaryDto
            {
                Id = notebook.Id,
                Title = notebook.Title,
                Subject = notebook.Subject,
                LastAccessedAt = notebook.LastAccessedAt
            };

            return CreatedAtAction(
                nameof(GetNotebookById),
                new { id = notebook.Id },
                responseNotebook);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateNotebook(int id, UpdateNotebookDto dto)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var notebook = await _context.Notebooks.FirstOrDefaultAsync(nb => nb.Id == id && nb.UserId == userId);

            if (notebook == null)
                return NotFound();

            notebook.Title = dto.Title;
            notebook.Subject = dto.Subject;

            DateTime now = DateTime.UtcNow;
            notebook.UpdatedAt = now;
            notebook.LastAccessedAt = now;

            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteNotebook(int id)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var notebook = await _context.Notebooks.FirstOrDefaultAsync(nb => nb.Id == id && nb.UserId == userId);

            if (notebook == null)
                return NotFound();

            _context.Notebooks.Remove(notebook);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}