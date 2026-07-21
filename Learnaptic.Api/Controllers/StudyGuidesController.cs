using Learnaptic.Api.Data;
using Learnaptic.Api.Dtos.StudyGuideDtos;
using Learnaptic.Api.Dtos.ConceptDtos;
using Learnaptic.Api.Dtos.StudySetDtos;
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
                    Description = sg.Description,
                    Subject = sg.Subject,
                    LastAccessedAt = sg.LastAccessedAt
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

                    StudySets = sg.StudySets
                        .Select(ss => new GetStudySetListDto
                        {
                            Id = ss.Id,
                            Title = ss.Title
                        })
                        .ToList(),

                    Concepts = sg.Concepts
                        .Select(c => new GetConceptDto
                        {
                            Id = c.Id,
                            Title = c.Title,
                            Content = c.Content
                        })
                        .ToList(),

                    UpdatedAt = sg.UpdatedAt,
                    CreatedAt = sg.CreatedAt
                })
                .FirstOrDefaultAsync();

            if (studyGuide == null)
                return NotFound();

            return Ok(studyGuide);
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
                return BadRequest("One or more selected study sets do not exist.");
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
                StudySets = linkedStudySets
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

                Concepts = new(),

                UpdatedAt = studyGuide.UpdatedAt,
                CreatedAt = studyGuide.CreatedAt
            };

            return CreatedAtAction(
                nameof(GetStudyGuideById),
                new { id = studyGuide.Id },
                responseStudyGuide);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateStudyGuide(int id, UpdateStudyGuideDto dto)
        {
            var studyGuide = await _context.StudyGuides
                .Include(sg => sg.StudySets)
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
                return BadRequest("One or more selected study sets do not exist.");
            }

            studyGuide.Title = dto.Title;
            studyGuide.Description = dto.Description;
            studyGuide.Subject = dto.Subject;

            studyGuide.StudySets.Clear();

            foreach (var studySet in linkedStudySets)
            {
                studyGuide.StudySets.Add(studySet);
            }

            DateTime now = DateTime.UtcNow;
            studyGuide.UpdatedAt = now;
            studyGuide.LastAccessedAt = now;

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
