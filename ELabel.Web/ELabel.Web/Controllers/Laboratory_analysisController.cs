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
    public class Laboratory_analysisController : ApiController
    {
        private readonly IRepository<Laboratory_analysis> repository;

        public Laboratory_analysisController()
        {
            repository = new LabAnalysesRepository();
        }

        // GET: api/Laboratory_analysis?pageIndex=1&pageSize=10
        public IEnumerable<Laboratory_analysis> GetLaboratory_Analyses(int pageIndex, int pageSize)
        {
            return repository.GetAll().Skip(pageIndex * pageSize).Take(pageSize);
        }

        // GET: api/Laboratory_analysis/5
        [ResponseType(typeof(Laboratory_analysis))]
        public IHttpActionResult GetLaboratory_analysis(string id)
        {
            var laboratory_analysis = repository.Find(id);
            if (laboratory_analysis == null)
            {
                return NotFound();
            }

            return Ok(laboratory_analysis);
        }

        // PUT: api/DrillholeSamplesViews/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutLaboratory_analysis(string id, Laboratory_analysis laboratory_analysis)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != laboratory_analysis.SAMPLEID)
            {
                return BadRequest();
            }

            repository.Put(laboratory_analysis);

            try
            {
                repository.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!Laboratory_analysisExists(id))
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

        // POST: api/Laboratory_analysis
        [ResponseType(typeof(Laboratory_analysis))]
        public IHttpActionResult PostLaboratory_analysis(Laboratory_analysis laboratory_analysis)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            repository.Add(laboratory_analysis);

            try
            {
                repository.SaveChanges();
            }
            catch (DbUpdateException)
            {
                if (Laboratory_analysisExists(laboratory_analysis.SAMPLEID))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtRoute("DefaultApi", new { id = laboratory_analysis.SAMPLEID }, laboratory_analysis);
        }

        // DELETE: api/Laboratory_analysis/5
        [ResponseType(typeof(Laboratory_analysis))]
        public IHttpActionResult DeleteLaboratory_analysis(string id)
        {
            Laboratory_analysis laboratory_analysis = repository.Find(id);
            if (laboratory_analysis == null)
            {
                return NotFound();
            }

            repository.Delete(laboratory_analysis);
            repository.SaveChanges();

            return Ok(laboratory_analysis);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                repository.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool Laboratory_analysisExists(string id)
        {
            return repository.Count(e => e.SAMPLEID == id) > 0;
        }
    }
}