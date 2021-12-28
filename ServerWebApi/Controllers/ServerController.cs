using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace ServerWebApi.Controllers
{
    [ApiController]
    [Route("api/[Controller]")]
    public class ServerController
    {
        private ActivitySource _activitySource;
        public ServerController(ActivitySource activitySource)
        {
            _activitySource = activitySource;
        }

        [HttpGet(Name = "GetValuesFromDatabase")]
        public Task<IEnumerable<string>> Get()
        {
            using var activity = _activitySource.StartActivity("GetValuesFromDatabase");
            var tempValues = new List<string>() { "Value One", "Value Two" };
            activity?.SetTag("TempValues", tempValues);

            return Task.FromResult(tempValues.AsEnumerable<string>());
        }
    }
}
