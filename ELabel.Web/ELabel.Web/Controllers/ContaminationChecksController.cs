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
    public class ContaminationChecksController : ApiController
    {
        private readonly IRepository<ContaminationCheck> repository;

        public ContaminationChecksController()
        {
            repository = new ContaminationCheckRepository();
        }

        // GET: api/ContaminationChecks
        public IEnumerable<ContaminationCheck> GetContaminationChecks(int pageIndex, int pageSize)
        {
            return repository.GetAll().Skip(pageIndex * pageSize).Take(pageSize);
        }

        // GET: api/ContaminationChecks/5
        [ResponseType(typeof(ContaminationCheck))]
        public IHttpActionResult GetContaminationCheck(string id)
        {
            ContaminationCheck contaminationCheck = repository.Find(id);
            if (contaminationCheck == null)
            {
                return NotFound();
            }

            return Ok(contaminationCheck);
        }

        // PUT: api/ContaminationChecks/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutContaminationCheck(string id, ContaminationCheck contaminationCheck)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != contaminationCheck.SAMPLEID)
            {
                return BadRequest();
            }

            repository.Put(contaminationCheck);

            try
            {
                repository.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ContaminationCheckExists(id))
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

        // POST: api/ContaminationChecks
        [ResponseType(typeof(ContaminationCheck))]
        public IHttpActionResult PostContaminationCheck(ContaminationCheck contaminationCheck)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            repository.Add(contaminationCheck);

            try
            {
                repository.SaveChanges();
            }
            catch (DbUpdateException)
            {
                if (ContaminationCheckExists(contaminationCheck.SAMPLEID))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtRoute("DefaultApi", new { id = contaminationCheck.SAMPLEID }, contaminationCheck);
        }

        // DELETE: api/ContaminationChecks/5
        [ResponseType(typeof(ContaminationCheck))]
        public IHttpActionResult DeleteContaminationCheck(string id)
        {
            ContaminationCheck contaminationCheck = repository.Find(id);
            if (contaminationCheck == null)
            {
                return NotFound();
            }

            repository.Delete(contaminationCheck);
            repository.SaveChanges();

            return Ok(contaminationCheck);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                repository.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool ContaminationCheckExists(string id)
        {
            return repository.Count(e => e.SAMPLEID == id) > 0;
        }
    }
}