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
            return Ok(await _context.StudyGuides.ToListAsync());
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetStudyGuideById(int id)
        {
            var studyGuide = await _context.StudyGuides.FindAsync(id);
            if (studyGuide == null)
            {
                return NotFound();
            }
            return Ok(studyGuide);
        }

        [HttpPost]
        public async Task<IActionResult> CreateStudyGuide(CreateStudyGuideDto dto)
        {
            var studyGuide = new StudyGuide
            {
                Title = dto.Title,
                Description = dto.Description,
                Subject = dto.Subject
            };

            _context.StudyGuides.Add(studyGuide);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetStudyGuideById), new { id = studyGuide.Id }, studyGuide);
        }
    }
}
