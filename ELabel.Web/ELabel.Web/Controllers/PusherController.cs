using System.Web.Http;
using PusherServer;

namespace ELabel.Web.Controllers
{
    public class PusherController : ApiController
    {
        // GET: api/Pusher/rejectAlert
        public IHttpActionResult GetPusher(string id)
        {
            var pusher = new Pusher("112258", "0a062137e6bd1304c414", "27c853b5d9494a1c8c2b");
            var result = pusher.Trigger("test_channel", "my_event", new {message = id});
            return Ok();
        }
    }
}
