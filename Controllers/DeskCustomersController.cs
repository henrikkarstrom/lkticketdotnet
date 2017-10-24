using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using LKTicket.Models;
using LKTicket.Repositoires;

namespace LKTicket.Controllers
{
    [Produces("application/json")]
    [Route("api/Desk/Customers")]
    public class CustomersController : Controller
    {
        // GET: api/Customers
        [HttpGet]
        [ProducesResponseType(typeof(List<CustomerResponse>), 200)]
        public IActionResult Get()
        {
            using (var db = new AppDb())
            {
                db.Connection.Open();
                return Ok(CustomersRepository.GetCustomers(db));
            }
        }

        // GET: api/Customers/5
        [HttpGet("{id}", Name = "Get")]
        [ProducesResponseType(typeof(CustomerResponse),200)]
        public IActionResult Get(int id)
        {
            using (var db = new AppDb())
            {
                db.Connection.Open();
                var customer = CustomersRepository.GetCustomer(db, id);

                if (customer != null)
                    return Ok(customer);
                return NotFound();
            }
        }
        [HttpGet("{id}/oders", Name = "GetOrders")]
        [ProducesResponseType(typeof(List<OrderResponse>), 200)]
        public IActionResult GetOrders(int id)
        {
            using (var db = new AppDb())
            {
                db.Connection.Open();
                var customer = OrderRespository.GetOrdersForCustomer(db, id);

                if (customer != null)
                    return Ok(customer);
                return NotFound();
            }
        }

        // POST: api/Customers
        [HttpPost]
        public IActionResult Post([FromBody]Customer value)
        {
            if (ModelState.IsValid)
                using (var db = new AppDb())
                {
                    db.Connection.Open();
                    var id = CustomersRepository.CreateCustomer(db, value);
                    return CreatedAtAction("Get", new { id = id });
                }
            else
                return BadRequest(ModelState);
        }
        
        // PUT: api/Customers/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {
        }
        
        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
