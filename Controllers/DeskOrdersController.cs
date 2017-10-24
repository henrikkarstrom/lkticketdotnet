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
    [Route("api/Desk/Orders")]
    public class DeskOrdersController : Controller
    {        
        [HttpGet("{id}", Name = "GetOrder")]
        [ProducesResponseType(typeof(OrderResponse),200)]
        public IActionResult Get(int id)
        {
            using (var db = new AppDb())
            {
                db.Connection.Open();
                var customer = OrderRespository.GetOrder(db, id);

                if (customer != null)
                    return Ok(customer);
                return NotFound();
            }
        }
        
        [HttpPost]
        [ProducesResponseType(typeof(OrderResponse), 200)]
        public IActionResult Post()
        {
            using (var db = new AppDb())
            {
                db.Connection.Open();
                var order = OrderRespository.CreateOrder(db);
                return Ok(order);
            }
        }


        [HttpDelete("{id}")]
        [ProducesResponseType(typeof(void), 200)]
        public IActionResult Delete(int id)
        {
            using (var db = new AppDb())
            {
                db.Connection.Open();
                OrderRespository.DeleteTicketsForOrder(db,id,1,1);
                return Ok();
            }
        }

        [HttpPost("{id}/tickets", Name = "AddTickets")]
        [ProducesResponseType(typeof(void), 200)]
        public IActionResult AddTickets(int id, [FromBody]TicketRequest ticketRequest)
        {
            if (ModelState.IsValid)
            {
                using (var db = new AppDb())
                {
                    db.Connection.Open();
                    OrderRespository.AddTicketsToOrder(db, id, ticketRequest);
                    return Ok();
                }
            }
            return BadRequest(ModelState);
        }

        [HttpPost("{id}/payment", Name = "AddPayment")]
        [ProducesResponseType(typeof(void), 200)]
        public IActionResult AddPayment(int id, [FromBody]PaymentRequest paymentRequest)
        {
            if (ModelState.IsValid)
            {
                using (var db = new AppDb())
                {
                    db.Connection.Open();
                    PaymentRepository.CreatePayment(db, id, paymentRequest);
                    return Ok();
                }
            }
            return BadRequest(ModelState);
        }

        [HttpGet("{id}/tickets", Name = "ListTickets")]
        [ProducesResponseType(typeof(List<TicketResponse>), 200)]
        public IActionResult ListTickets(int id, bool onlyActive = false)
        {
            using (var db = new AppDb())
            {
                db.Connection.Open();
                return Ok(TicketRepository.GetTicketsForOrder(db, id, onlyActive));
            }
        }
    }
}
