using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using LKTicket.Models;
using LKTicket.Repositoires;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace LKTicket.Controllers.Admin
{
    [Route("api/admin/users")]
    public class AdminUsers : Controller
    {
        [Produces(typeof(List<UserResponse>))]
        [HttpGet]
        public IActionResult Get()
        {
            using (var db = new AppDb())
            {
                db.Connection.Open();
                return Ok(UserRespository.GetUsers(db));
            }
        }
        [Produces(typeof(List<UserResponse>))]
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            using (var db = new AppDb())
            {
                db.Connection.Open();
                return Ok(UserRespository.GetUsers(db));
            }
        }
        [Produces(typeof(int))]
        [HttpPost]
        public IActionResult Post([FromBody] UserRequest value)
        {
            if (ModelState.IsValid)
            {
                using (var db = new AppDb())
                {
                    db.Connection.Open();
                    return Ok(UserRespository.Create(db, value));
                }
            }
            return BadRequest(ModelState);
        }
    }
}
