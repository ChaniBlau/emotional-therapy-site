using Microsoft.AspNetCore.Mvc;
using BL.Api;
using Dal.Models;

namespace Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TherapistsController : ControllerBase
    {
        private readonly IBLTherapist _blTherapist;

        public TherapistsController(IBLTherapist blTherapist)
        {
            _blTherapist = blTherapist;
        }

        [HttpGet]
        public async Task<ActionResult<List<Therapist>>> GetAllTherapists()
        {
            var therapists = await _blTherapist.GetAllTherapists();
            return Ok(therapists);
        }
    }
}
