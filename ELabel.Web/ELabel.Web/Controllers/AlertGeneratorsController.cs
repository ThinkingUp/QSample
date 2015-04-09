using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Web.Http;
using System.Web.Http.Description;
using ELabel.Web.DataAccess;
using ELabel.Web.Repositories;
using Newtonsoft.Json;
using ELabel.QC;
using ELabel.QC.AlertClasses;

namespace ELabel.Web.Controllers
{
    public class AlertGeneratorsController : ApiController
    {
        private readonly IRepository<AlertGenerator> repository;

        public AlertGeneratorsController()
        {
            repository = new AlertGeneratorRepository();
        }

        // GET: api/AlertGenerators
        public IEnumerable<AlertGenerator> GetAlertGenerators(int pageIndex, int pageSize)
        {
            return repository.GetAll().Skip(pageIndex * pageSize).Take(pageSize);
        }

        // GET: api/AlertGenerators/AlertStandardReference
        [Route("api/AlertGenerators/AlertStandardReference")]
        [ResponseType(typeof(IEnumerable<RejectAlert>))]
        public IHttpActionResult GetAlertStandardReference()
        {
            var str = repository.GetAll();
            var obj = JsonConvert.DeserializeObject<Alerts>(str.First().Message);
            return Ok(obj.AlertsStandardReference);
        }

        // GET: api/AlertGenerators/AlertsMethod
        [Route("api/AlertGenerators/AlertsMethod")]
        [ResponseType(typeof(IEnumerable<MethodAlert>))]
        public IHttpActionResult GetAlertsMethod()
        {
            var str = repository.GetAll();
            var obj = JsonConvert.DeserializeObject<Alerts>(str.First().Message);
            return Ok(obj.AlertsMethod);
        }

        // GET: api/AlertGenerators/AlertsBasic
        [Route("api/AlertGenerators/AlertsBasic")]
        [ResponseType(typeof(IEnumerable<BasicAlert>))]
        public IHttpActionResult GetAlertsBasic()
        {
            var str = repository.GetAll();
            var obj = JsonConvert.DeserializeObject<Alerts>(str.First().Message);
            return Ok(obj.AlertsBasic);
        }

        // GET: api/AlertGenerators/AlertsContaminationCheck
        [Route("api/AlertGenerators/AlertsContaminationCheck")]
        [ResponseType(typeof(IEnumerable<RangeAlert>))]
        public IHttpActionResult GetAlertsContaminationCheck()
        {
            var str = repository.GetAll();
            var obj = JsonConvert.DeserializeObject<Alerts>(str.First().Message);
            return Ok(obj.AlertsContaminationCheck);
        }

        // GET: api/AlertGenerators/AlertsDeuplicates
        [Route("api/AlertGenerators/AlertsDeuplicates")]
        [ResponseType(typeof(IEnumerable<DuplicatesAlert>))]
        public IHttpActionResult GetAlertsDeuplicates()
        {
            var str = repository.GetAll();
            var obj = JsonConvert.DeserializeObject<Alerts>(str.First().Message);
            return Ok(obj.AlertsDuplicates);
        }

        // GET: api/AlertGenerators/5
        [ResponseType(typeof(AlertGenerator))]
        public IHttpActionResult GetAlertGenerator(string id)
        {
            AlertGenerator alertGenerator = repository.Find(id);
            if (alertGenerator == null)
            {
                return NotFound();
            }

            return Ok(alertGenerator);
        }

        // PUT: api/AlertGenerators/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutAlertGenerator(long id, AlertGenerator alertGenerator)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != alertGenerator.ID)
            {
                return BadRequest();
            }

            repository.Put(alertGenerator);

            try
            {
                repository.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AlertGeneratorExists(id))
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

        // POST: api/AlertGenerators
        [ResponseType(typeof(AlertGenerator))]
        public IHttpActionResult PostAlertGenerator(AlertGenerator alertGenerator)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            repository.Add(alertGenerator);
            repository.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = alertGenerator.ID }, alertGenerator);
        }

        // DELETE: api/AlertGenerators/5
        [ResponseType(typeof(AlertGenerator))]
        public IHttpActionResult DeleteAlertGenerator(string id)
        {
            AlertGenerator alertGenerator = repository.Find(id);
            if (alertGenerator == null)
            {
                return NotFound();
            }

            repository.Delete(alertGenerator);
            repository.SaveChanges();

            return Ok(alertGenerator);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                repository.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool AlertGeneratorExists(long id)
        {
            return repository.Count(e => e.ID == id) > 0;
        }
    }
}