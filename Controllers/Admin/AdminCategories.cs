using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using LKTicket.Models;
using LKTicket.Repositoires;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace LKTicket.Controllers.Admin
{


    [Route("api/admin/categories")]
    public class AdminCategories : Controller
    {
        [Produces(typeof(List<CategoryResponse>))]
        [HttpGet]
        public IActionResult Get()
        {
            using (var db = new AppDb())
            {
                db.Connection.Open();
                return Ok(CategoriesRepository.GetCategories(db));
            }
        }

        // GET api/values/5
        [HttpGet("{id}")]
        [Produces(typeof(CategoryResponse))]
        public IActionResult Get(int id)
        {
            using (var db = new AppDb())
            {
                db.Connection.Open();
                return Ok(CategoriesRepository.GetCategory(db, id));
            }
        }

        // PUT api/values/5
        [HttpPut("{id}/ticketCount/{nbr}")]
        public void Put(int id, int nbr)
        {

        }

        [HttpPut("{id}/prices/{rate_id}/")]
        public IActionResult PutPrice(int id, int rate_id, [FromBody] PriceRequest value )
        {
            if(ModelState.IsValid)
            {
                using (var db = new AppDb())
                {
                    db.Connection.Open();
                    return Ok(PricesRepository.CreatePrice(db, id, rate_id, value.Price));
                }
            }
            return BadRequest(ModelState);
        }

        [HttpDelete("{id}/prices/{rate_id}/")]
        public IActionResult DeltePrice(int id, int rate_id)
        {
            if (ModelState.IsValid)
            {
                using (var db = new AppDb())
                {
                db.Connection.Open();
                    PricesRepository.DeletePrice(db, id, rate_id);
                    return Ok();
                }
            }
            return BadRequest(ModelState);
        }
    }
}
