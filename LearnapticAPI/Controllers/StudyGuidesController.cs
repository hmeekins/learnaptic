using Microsoft.AspNetCore.Mvc;
using Learnaptic.Api.Models;

namespace Learnaptic.Api.Controllers
{
    [ApiController]
    [Route("api/study-guides")]
    public class StudyGuidesController : ControllerBase
    {
        private readonly List<StudyGuide> _studyGuides = [new() { Id = 1, Title = "Sample Study Guide", Description = "A sample study guide for demonstration purposes.", Subject = "Sample Subject", CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow}];
        
        [HttpGet]
        public IActionResult GetStudyGuides()
        {
            return Ok(_studyGuides);
        }
    }
}
