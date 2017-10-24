using Microsoft.AspNetCore.Mvc;
using LKTicket.Models;
using LKTicket.Repositoires;

namespace LKTicket.Controllers
{
    [Produces("application/json")]
    [Route("api/Desk/Shows")]
    public class DeskShowsController : Controller
    {


        [HttpGet("", Name = "GetShowsForDesk")]
        [ProducesResponseType(typeof(ShowResponse), 200)]
        public IActionResult Get()
        {
            using (var db = new AppDb())
            {
                db.Connection.Open();
                var shows = ShowsRepository.GetShows(db);
                return Ok(shows);
            }
        }

        [HttpGet("{id}/performances", Name = "GetPerformanceForDesk")]
        [ProducesResponseType(typeof(ShowResponse), 200)]
        public IActionResult GetPerformances(int id, int profileId)
        {
            using (var db = new AppDb())
            {
                db.Connection.Open();
                var shows = ShowsRepository.GetPerformancesForShowWhitAvibility(db, id, profileId);
                return Ok(shows);
            }
        }
    }
}
