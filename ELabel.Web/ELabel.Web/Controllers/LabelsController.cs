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
    public class LabelsController : ApiController
    {
        private readonly IRepository<Label> repository;

        public LabelsController()
        {
            repository = new LabelRepository();
        }

        // GET: api/Labels
        public IEnumerable<Label> GetLabels(int pageIndex, int pageSize)
        {
            return repository.GetAll().Skip(pageIndex * pageSize).Take(pageSize);
        }

        // GET: api/Labels/5
        [ResponseType(typeof(Label))]
        public IHttpActionResult GetLabel(string id)
        {
            Label label = repository.Find(id);
            if (label == null)
            {
                return NotFound();
            }

            return Ok(label);
        }

        // GET: api/Labels/status
        [ResponseType(typeof (IEnumerable<SampleDTO>))]
        [Route("api/Labels/status")]
        public IHttpActionResult GetStatus()
        {
            var items = repository.GetAll();
            var remoteSite = items.Where(x => x.Location == "Remote Site");
            var coreShack = items.Where(x => x.Location == "Core Shack");
            var transport = items.Where(x => x.Location == "Transport");
            var lab = items.Where(x => x.Location == "Lab");

            var status = new List<SampleDTO>
            {
                new SampleDTO
                {
                    Stage = "Remote Site",
                    Scanned = remoteSite.Count(x => (bool) x.Scanned),
                    Unscanned = remoteSite.Count(x => !(bool) x.Scanned),
                    Percentage =
                        (double) remoteSite.Count(x => (bool) x.Scanned)/remoteSite.Count(x => !(bool) x.Scanned)*100.0
                },
                new SampleDTO
                {
                    Stage = "Core Shack",
                    Scanned = coreShack.Count(x => (bool) x.Scanned),
                    Unscanned = coreShack.Count(x => !(bool) x.Scanned),
                    Percentage =
                        (double) coreShack.Count(x => (bool) x.Scanned)/coreShack.Count(x => !(bool) x.Scanned)*100.0
                },
                new SampleDTO
                {
                    Stage = "Transport",
                    Scanned = transport.Count(x => (bool) x.Scanned),
                    Unscanned = transport.Count(x => !(bool) x.Scanned),
                    Percentage =
                        (double) transport.Count(x => (bool) x.Scanned)/transport.Count(x => !(bool) x.Scanned)*100.0
                },
                new SampleDTO
                {
                    Stage = "Lab",
                    Scanned = lab.Count(x => (bool) x.Scanned),
                    Unscanned = lab.Count(x => !(bool) x.Scanned),
                    Percentage = (double) lab.Count(x => (bool) x.Scanned)/lab.Count(x => !(bool) x.Scanned)*100.0
                }
            };

            return Ok(status);
        }

        // PUT: api/Labels/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutLabel(string id, Label label)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != label.SampleID)
            {
                return BadRequest();
            }

            repository.Put(label);

            try
            {
                repository.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!LabelExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            var pusher = new Pusher("112258", "0a062137e6bd1304c414", "27c853b5d9494a1c8c2b");
            var result = pusher.Trigger("test_channel", "my_event", new { message = "labelUpdate" });
            return Ok();

            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST: api/Labels
        [ResponseType(typeof(Label))]
        public IHttpActionResult PostLabel(Label label)
        {

            repository.Add(label);

            try
            {
                repository.SaveChanges();
            }
            catch (DbUpdateException)
            {
                if (LabelExists(label.SampleID))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtRoute("DefaultApi", new { id = label.SampleID }, label);
        }

        // DELETE: api/Labels/5
        [ResponseType(typeof(Label))]
        public IHttpActionResult DeleteLabel(string id)
        {
            Label label = repository.Find(id);
            if (label == null)
            {
                return NotFound();
            }

            repository.Delete(label);
            repository.SaveChanges();

            return Ok(label);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                repository.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool LabelExists(string id)
        {
            return repository.Count(e => e.SampleID == id) > 0;
        }
    }
}