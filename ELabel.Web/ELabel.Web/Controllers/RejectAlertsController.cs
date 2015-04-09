using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Web.Http;
using System.Web.Http.Description;
using ELabel.Web.DataAccess;
using ELabel.Web.Models;
using ELabel.Web.Repositories;
using PusherServer;

namespace ELabel.Web.Controllers
{
    public class RejectAlertsController : ApiController
    {
        private readonly IRepository<RejectAlert> repository;

        public RejectAlertsController()
        {
            repository = new RejectAlertRepository();
        }

        // GET: api/RejectAlerts
        public IEnumerable<RejectAlert> GetRejectAlerts(int pageIndex, int pageSize)
        {
            return repository.GetAll().Skip(pageIndex * pageSize).Take(pageSize);
        }

        // GET: api/RejectAlerts/latest
        [Route("api/RejectAlerts/latest")]
        [ResponseType(typeof(RejectAlert))]
        public IHttpActionResult GetRejectAlerts()
        {
            return Ok(repository.GetAll().OrderByDescending(x => x.ID).FirstOrDefault());
        }

        // GET: api/RejectAlerts/5
        [ResponseType(typeof(RejectAlert))]
        public IHttpActionResult GetRejectAlert(string id)
        {
            var rejectAlert = repository.Find(id);
            if (rejectAlert == null)
            {
                return NotFound();
            }

            return Ok(rejectAlert);
        }

        // PUT: api/RejectAlerts/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutRejectAlert(int id, RejectAlert rejectAlert)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != rejectAlert.ID)
            {
                return BadRequest();
            }

            repository.Put(rejectAlert);

            try
            {
                repository.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!RejectAlertExists(id))
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

        // POST: api/RejectAlerts
        [ResponseType(typeof(RejectAlert))]
        public IHttpActionResult PostRejectAlert(RejectAlert rejectAlert)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            repository.Add(rejectAlert);
            repository.SaveChanges();

            var pusher = new Pusher("112258", "0a062137e6bd1304c414", "27c853b5d9494a1c8c2b");
            var result = pusher.Trigger("test_channel", "my_event", new { message = "rejectAlert" });
            return Ok();

            return CreatedAtRoute("DefaultApi", new { id = rejectAlert.ID }, rejectAlert);
        }

        // DELETE: api/RejectAlerts/5
        [ResponseType(typeof(RejectAlert))]
        public IHttpActionResult DeleteRejectAlert(string id)
        {
            RejectAlert rejectAlert = repository.Find(id);
            if (rejectAlert == null)
            {
                return NotFound();
            }

            repository.Delete(rejectAlert);
            repository.SaveChanges();

            return Ok(rejectAlert);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                repository.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool RejectAlertExists(int id)
        {
            return repository.Count(e => e.ID == id) > 0;
        }
    }
}