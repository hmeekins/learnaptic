using Microsoft.AspNetCore.Mvc;

namespace Learnaptic.Api.Controllers
{
    [ApiController]
    [Route("api/study-guides")]
    public class StudyGuidesController : ControllerBase
    {
        [HttpGet]
        public string Get()
        {
            return "Study guides endpoint is working!";
        }
    }
}
