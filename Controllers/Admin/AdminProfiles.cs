using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using LKTicket.Models;
using LKTicket.Repositoires;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace LKTicket.Controllers.Admin
{
    [Route("api/admin/profiles")]
    public class AdminProfiles : Controller
    {
        [Produces(typeof(List<ProfileResponse>))]
        [HttpGet]
        public IActionResult Get()
        {
            using (var db = new AppDb())
            {
                db.Connection.Open();
                return Ok(ProfileRepository.GetProfiles(db));
            }
        }
        [Produces(typeof(int))]
        [HttpPost]
        public IActionResult Post([FromBody] ProfileRequest value)
        {
            if (ModelState.IsValid)
            {
                using (var db = new AppDb())
                {
                    db.Connection.Open();
                    return Ok(ProfileRepository.CreateProfile(db, value));
                }
            }
            return BadRequest(ModelState);
        }
    }
}
