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
    public class standardReferencesController : ApiController
    {
        private readonly IRepository<standardReference> repository;

        public standardReferencesController()
        {
            repository = new StandardReferenceRepository();
        }

        // GET: api/standardReferences
        public IEnumerable<standardReference> GetstandardReferences(int pageIndex, int pageSize)
        {
            return repository.GetAll().Skip(pageIndex * pageSize).Take(pageSize);
        }

        // GET: api/standardReferences/5
        [ResponseType(typeof(standardReference))]
        public IHttpActionResult GetstandardReference(string id)
        {
            standardReference standardReference = repository.Find(id);
            if (standardReference == null)
            {
                return NotFound();
            }

            return Ok(standardReference);
        }

        // PUT: api/standardReferences/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutstandardReference(string id, standardReference standardReference)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != standardReference.SAMPLEID)
            {
                return BadRequest();
            }

            repository.Put(standardReference);

            try
            {
                repository.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!standardReferenceExists(id))
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

        // POST: api/standardReferences
        [ResponseType(typeof(standardReference))]
        public IHttpActionResult PoststandardReference(standardReference standardReference)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            repository.Add(standardReference);

            try
            {
                repository.SaveChanges();
            }
            catch (DbUpdateException)
            {
                if (standardReferenceExists(standardReference.SAMPLEID))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtRoute("DefaultApi", new { id = standardReference.SAMPLEID }, standardReference);
        }

        // DELETE: api/standardReferences/5
        [ResponseType(typeof(standardReference))]
        public IHttpActionResult DeletestandardReference(string id)
        {
            standardReference standardReference = repository.Find(id);
            if (standardReference == null)
            {
                return NotFound();
            }

            repository.Delete(standardReference);
            repository.SaveChanges();

            return Ok(standardReference);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                repository.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool standardReferenceExists(string id)
        {
            return repository.Count(e => e.SAMPLEID == id) > 0;
        }
    }
}