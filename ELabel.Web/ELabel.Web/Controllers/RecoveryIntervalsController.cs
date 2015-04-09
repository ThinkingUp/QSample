using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Web.Http;
using System.Web.Http.Description;
using ELabel.Web.DataAccess;
using ELabel.Web.Models;
using ELabel.Web.Repositories;

namespace ELabel.Web.Controllers
{
    public class RecoveryIntervalsController : ApiController
    {
        private readonly IRepository<RecoveryIntervalsDTO> repository;

        public RecoveryIntervalsController()
        {
            repository = new RecoveryIntervalsRepository();
        }

        // GET: api/RecoveryIntervals?pageIndex=1&pageSize=10
        public IEnumerable<RecoveryIntervalsDTO> GetRecoveryIntervals(int pageIndex, int pageSize)
        {
            return repository.GetAll().Skip(pageIndex * pageSize).Take(pageSize);
        }

        // GET: api/RecoveryIntervals/5
        [ResponseType(typeof(IEnumerable<RecoveryIntervalsDTO>))]
        public IHttpActionResult GetRecoveryInterval(string id)
        {
            var recoveryInterval = repository.FindAll(x => x.HOLEID == id);
            if (recoveryInterval == null)
            {
                return NotFound();
            }

            return Ok(recoveryInterval);
        }

        // PUT: api/RecoveryIntervals/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutRecoveryInterval(int id, RecoveryIntervalsDTO recoveryInterval)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != recoveryInterval.ID)
            {
                return BadRequest();
            }

            repository.Put(recoveryInterval);

            try
            {
                repository.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!RecoveryIntervalExists(id))
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

        // POST: api/RecoveryIntervals
        [ResponseType(typeof(RecoveryIntervalsDTO))]
        public IHttpActionResult PostRecoveryInterval(RecoveryIntervalsDTO recoveryInterval)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            repository.Add(recoveryInterval);
            repository.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = recoveryInterval.ID }, recoveryInterval);
        }

        // DELETE: api/RecoveryIntervals/5
        [ResponseType(typeof(RecoveryIntervalsDTO))]
        public IHttpActionResult DeleteRecoveryInterval(string id)
        {
            var recoveryInterval = repository.Find(id);
            if (recoveryInterval == null)
            {
                return NotFound();
            }

            repository.Delete(recoveryInterval);
            repository.SaveChanges();

            return Ok(recoveryInterval);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                repository.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool RecoveryIntervalExists(int id)
        {
            return repository.Count(e => e.ID == id) > 0;
        }
    }
}