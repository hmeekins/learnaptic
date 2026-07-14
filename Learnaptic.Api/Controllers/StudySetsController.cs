using Learnaptic.Api.Data;
using Learnaptic.Api.Dtos.StudySetDtos;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Learnaptic.Api.Controllers
{
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
            var studySets = await _context.StudySets
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
            var studySet = await _context.StudySets.FindAsync(id);

            if (studySet == null)
            {
                return NotFound();
            }

            var studySetDto = new GetStudySetDto
            {
                Id = studySet.Id,
                Title = studySet.Title,
                CreatedAt = studySet.CreatedAt,
                UpdatedAt = studySet.UpdatedAt
            };

            studySet.LastAccessedAt = DateTime.Now;
            await _context.SaveChangesAsync();

            return Ok(studySetDto);
        }
    }
}
