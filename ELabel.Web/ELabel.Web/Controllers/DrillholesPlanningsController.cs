using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Web.Http;
using System.Web.Http.Description;
using ELabel.Web.DataAccess;
using ELabel.Web.Repositories;

namespace ELabel.Web.Controllers
{
    public class DrillholesPlanningsController : ApiController
    {
        private readonly IRepository<DrillholesPlanning> repository;

        public DrillholesPlanningsController()
        {
            repository = new DrillholesPlanningRespository();
        }

        // GET: api/DrillholesPlannings?pageIndex=1&pageSize=10
        public IEnumerable<DrillholesPlanning> GetDrillholesPlannings(int pageIndex, int pageSize)
        {
            return repository.GetAll().GroupBy(x => x.HOLEID).Select(x => x.First()).Skip(pageIndex * pageSize).Take(pageSize);
        }

        // GET: api/DrillholesPlannings/5
        [ResponseType(typeof(IEnumerable<DrillholesPlanning>))]
        public IHttpActionResult GetDrillholesPlannings(string id)
        {
            var plannings = repository.FindAll(x => x.HOLEID == id);
            if (plannings == null)
            {
                return NotFound();
            }

            return Ok(plannings);
        }

        // PUT: api/DrillholesPlannings/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutDrillholesPlanning(string id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            repository.Put(new DrillholesPlanning {SAMPLEID = id});

            try
            {
                repository.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!DrillholesPlanningExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST: api/DrillholesPlannings
        [ResponseType(typeof(DrillholesPlanning))]
        public IHttpActionResult PostDrillholesPlanning(DrillholesPlanning drillholesPlanning)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            repository.Add(drillholesPlanning);

            try
            {
                repository.SaveChanges();
            }
            catch (DbUpdateException)
            {
                if (DrillholesPlanningExists(drillholesPlanning.SAMPLEID))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtRoute("DefaultApi", new { id = drillholesPlanning.SAMPLEID }, drillholesPlanning);
        }

        // DELETE: api/DrillholesPlannings/5
        [ResponseType(typeof(DrillholesPlanning))]
        public IHttpActionResult DeleteDrillholesPlanning(string id)
        {
            DrillholesPlanning drillholesPlanning = repository.Find(id);
            if (drillholesPlanning == null)
            {
                return NotFound();
            }

            repository.Delete(drillholesPlanning);
            repository.SaveChanges();

            return Ok(drillholesPlanning);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                repository.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool DrillholesPlanningExists(string id)
        {
            return repository.Count(e => e.SAMPLEID == id) > 0;
        }
    }
}