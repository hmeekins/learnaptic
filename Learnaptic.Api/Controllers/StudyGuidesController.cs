using Learnaptic.Api.Data;
using Learnaptic.Api.Dtos;
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
                .OrderByDescending(sg => sg.LastAccessed)
                .Select(sg => new GetStudyGuideListDto
                {
                    Id = sg.Id,
                    Title = sg.Title,
                    Description = sg.Description,
                    Subject = sg.Subject,
                    LastAccessed = sg.LastAccessed
                }).ToListAsync();
                

            return Ok(studyGuides);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetStudyGuideById(int id)
        {
            var studyGuide = await _context.StudyGuides
                .Where(sg => sg.Id == id)
                .Select(sg => new GetStudyGuideDto
                {
                    Id = sg.Id,
                    Title = sg.Title,
                    Description = sg.Description,
                    Subject = sg.Subject,
                    UpdatedAt = sg.UpdatedAt
                })
                .FirstOrDefaultAsync();

            if (studyGuide == null)
                return NotFound();

            return Ok(studyGuide);
        }

        [HttpPost]
        public async Task<IActionResult> CreateStudyGuide(CreateStudyGuideDto dto)
        {
            var studyGuide = new StudyGuide
            {
                Title = dto.Title,
                Description = dto.Description,
                Subject = dto.Subject,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
                LastAccessed = DateTime.UtcNow
            };

            _context.StudyGuides.Add(studyGuide);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetStudyGuideById), new { id = studyGuide.Id }, studyGuide);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateStudyGuide(int id, UpdateStudyGuideDto dto)
        {
            var studyGuide = await _context.StudyGuides.FindAsync(id);

            if (studyGuide == null)
            {
                return NotFound();
            }

            studyGuide.Title = dto.Title;
            studyGuide.Description = dto.Description;
            studyGuide.Subject = dto.Subject;
            studyGuide.UpdatedAt = DateTime.UtcNow;
            studyGuide.LastAccessed = DateTime.UtcNow;

            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteStudyGuide(int id)
        {
            var studyGuide = await _context.StudyGuides.FindAsync(id);

            if (studyGuide == null)
            {
                return NotFound();
            }

            _context.StudyGuides.Remove(studyGuide);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}
