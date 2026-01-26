using Application.IServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class GradedAttemptController : ControllerBase
    {
        private IGradedAttemptService _service;

        public GradedAttemptController(IGradedAttemptService service)
        {
            _service = service;
        }

        //[]
    }
}
