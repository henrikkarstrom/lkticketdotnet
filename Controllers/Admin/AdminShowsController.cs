using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using LKTicket.Models;
using LKTicket.Repositoires;
using static LKTicket.Repositoires.ShowsRepository;

namespace LKTicket.Controllers.Admin
{
    [Produces("application/json")]
    [Route("api/Admin/Shows")]
    public class AdminShowsController : Controller
    {
        // GET: api/AdminShows
        [HttpGet]
        [Produces(typeof(List<ShowResponse>))]
        public IActionResult Get()
        {
            using (var db = new AppDb())
            {
                db.Connection.Open();
                return Ok(ShowsRepository.GetShows(db));
            }
        }

        // GET: api/AdminShows/5
        [HttpGet("{id}", Name = "GetShow")]
        [Produces(typeof(ShowResponse))]
        public IActionResult Get(int id)
        {
            using (var db = new AppDb())
            {
                db.Connection.Open();
                return Ok(ShowsRepository.GetShow(db,id));
            }
        }

        [HttpGet("{id}/performances", Name = "GetPerformancesForShow")]
        [Produces(typeof(List<PerformanceResponse>))]
        public IActionResult GetPerformance(int id)
        {
            using (var db = new AppDb())
            {
                db.Connection.Open();
                return Ok(PerformancesRepository.GetPerformancesForShow(db, id));
            }
        }

        [HttpGet("{id}/performance/profiles", Name = "GetPerformancesProfileForShow")]
        [Produces(typeof(List<PerformanceWithProfileData>))]
        public IActionResult GetPerformanceProfileData(int id)
        {
            using (var db = new AppDb())
            {
                db.Connection.Open();
                return Ok(ShowsRepository.GetPerformancesForShowWhitProfileData(db, id));
            }
        }

        [HttpPost("{id}/performances", Name = "CreatePerformancesForShow")]
        [Produces(typeof(PerformanceResponse))]
        public IActionResult CreatePerformance(int id, [FromBody] PerformanceRequest value)
        {
            if (ModelState.IsValid)
            {
                using (var db = new AppDb())
                {
                    db.Connection.Open();
                    return Ok(PerformancesRepository.CreatePerformance(db, id, value));

                }
            }
            return BadRequest(ModelState);
        }

        [HttpGet("{id}/rates", Name = "GetRatesForShow")]
        [Produces(typeof(List<RateResponse>))]
        public IActionResult GetRates(int id)
        {
            using (var db = new AppDb())
            {
                db.Connection.Open();
                return Ok(RatesRepository.GetRatesForShow(db, id));
            }
        }

        [HttpGet("{id}/prices", Name = "GetPricesForShow")]
        [Produces(typeof(List<PriceWithNameResponse>))]
        public IActionResult GetPrices(int id)
        {
            try
            {
                using (var db = new AppDb())
                {
                    db.Connection.Open();
                    return Ok(PricesRepository.GetPricesForShow(db, id));
                }
            }
            catch(Exception exception)
            {
                return Ok();
            }
        }

        [HttpPost("{id}/rates", Name = "InsertRateForShow")]
        [Produces(typeof(List<RateResponse>))]
        public IActionResult PostRate(int id, [FromBody] RateRequest value)
        {
            using (var db = new AppDb())
            {
                db.Connection.Open();
                return Ok(RatesRepository.CreateRate(db, id, value));
            }
        }

        [HttpGet("{id}/categories", Name = "GetCatgoryForShow")]
        [Produces(typeof(List<RateResponse>))]
        public IActionResult GetCategories(int id)
        {
            using (var db = new AppDb())
            {
                db.Connection.Open();
                return Ok(CategoriesRepository.GetCategoriesForShow(db, id));
            }
        }

        [HttpPost("{id}/categories", Name = "InsertCategoryForShow")]
        [Produces(typeof(List<RateResponse>))]
        public IActionResult PostCategories(int id, [FromBody] CategoryRequest value)
        {
            if (ModelState.IsValid)
            {
                using (var db = new AppDb())
                {
                    db.Connection.Open();
                    return Ok(CategoriesRepository.CreateCategory(db, id, value));
                }
            }
            return BadRequest(ModelState);
        }


        // POST: api/AdminShows
        [HttpPost]
        [Produces(typeof(ShowResponse))]
        public IActionResult Post([FromBody]ShowRequest value)
        {
            if(ModelState.IsValid)
            {
                using (var db = new AppDb())
                {
                    db.Connection.Open();
                    return Ok(ShowsRepository.CreateShow(db, value));
                }
            }
            return BadRequest(ModelState);
        }
    }
}
