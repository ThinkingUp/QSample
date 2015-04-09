using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Web.Http;
using System.Web.Http.Description;
using ELabel.Web.DataAccess;
using ELabel.Web.Repositories;

namespace ELabel.Web.Controllers
{
    public class DrillholeSamplesViewsController : ApiController
    {
        private readonly IRepository<DrillholeSamplesView> repository;

        public DrillholeSamplesViewsController()
        {
            repository = new DrillholeSamplesViewRespository();
        }

        // GET: api/DrillholeSamplesViews?pageIndex=1&pageSize=10
        public IEnumerable<DrillholeSamplesView> GetDrillholeSamplesViews(int pageIndex, int pageSize)
        {
            return repository.GetAll().Skip(pageIndex * pageSize).Take(pageSize);
        }

        // GET: api/DrillholeSamplesViews/5
        [ResponseType(typeof(DrillholeSamplesView))]
        public IHttpActionResult GetDrillholeSamplesView(string id)
        {
            DrillholeSamplesView drillholeSamplesView = repository.Find(id);
            if (drillholeSamplesView == null)
            {
                return NotFound();
            }

            return Ok(drillholeSamplesView);
        }

        // PUT: api/DrillholeSamplesViews/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutDrillholeSamplesView(string id, DrillholeSamplesView drillholeSamplesView)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != drillholeSamplesView.HOLEID)
            {
                return BadRequest();
            }

            repository.Put(drillholeSamplesView);

            try
            {
                repository.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!DrillholeSamplesViewExists(id))
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

        // POST: api/DrillholeSamplesViews
        [ResponseType(typeof(DrillholeSamplesView))]
        public IHttpActionResult PostDrillholeSamplesView(DrillholeSamplesView drillholeSamplesView)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            repository.Add(drillholeSamplesView);

            try
            {
                repository.SaveChanges();
            }
            catch (DbUpdateException)
            {
                if (DrillholeSamplesViewExists(drillholeSamplesView.HOLEID))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtRoute("DefaultApi", new { id = drillholeSamplesView.HOLEID }, drillholeSamplesView);
        }

        // DELETE: api/DrillholeSamplesViews/5
        [ResponseType(typeof(DrillholeSamplesView))]
        public IHttpActionResult DeleteDrillholeSamplesView(string id)
        {
            DrillholeSamplesView drillholeSamplesView = repository.Find(id);
            if (drillholeSamplesView == null)
            {
                return NotFound();
            }

            repository.Delete(drillholeSamplesView);
            repository.SaveChanges();

            return Ok(drillholeSamplesView);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                repository.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool DrillholeSamplesViewExists(string id)
        {
            return repository.Count(e => e.HOLEID == id) > 0;
        }
    }
}