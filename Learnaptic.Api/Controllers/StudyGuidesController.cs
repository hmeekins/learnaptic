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
                .Select(sg => new GetStudyGuideSummaryDto
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
                Subject = studyGuide.Subject,

                Concepts = studyGuide.Concepts
                    .OrderBy(c => c.Position)
                    .Select(c => new GetConceptDto
                    {
                        Id = c.Id,
                        Title = c.Title,
                        Content = c.Content,
                    })
                    .ToList(),

                StudySets = studyGuide.StudySets
                    .Select(ss => new GetStudySetListDto
                    {
                        Id = ss.Id,
                        Title = ss.Title
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
            DateTime now = DateTime.UtcNow;
            var studyGuide = new StudyGuide
            {
                Title = dto.Title,
                Subject = dto.Subject,
                CreatedAt = now,
                UpdatedAt = now,
                LastAccessedAt = now
            };

            _context.StudyGuides.Add(studyGuide);
            await _context.SaveChangesAsync();

            var responseStudyGuide = new GetStudyGuideSummaryDto
            {
                Id = studyGuide.Id,
                Title = studyGuide.Title,
                Subject = studyGuide.Subject,
                LastAccessedAt = studyGuide.LastAccessedAt
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
            var studyGuide = await _context.StudyGuides.FindAsync(id);

            if (studyGuide == null)
                return NotFound();

            studyGuide.Title = dto.Title;
            studyGuide.Subject = dto.Subject;

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