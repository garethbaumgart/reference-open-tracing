using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Text.Json;

namespace ClientWebApi.Controllers
{
    [ApiController]
    [Route("api/[Controller]")]
    public class ClientController : ControllerBase
    {
        HttpClient _httpClient;
        ActivitySource _activitySource;

        public ClientController(HttpClient httpClient, ActivitySource activitySource)
        {
            _httpClient = httpClient;
            _httpClient.BaseAddress = new Uri("http://serverwebapi/api/");
            _activitySource = activitySource;
        }

        [HttpGet]
        [Route("GetValues")]
        public async Task<ActionResult<IEnumerable<string>>> GetValuesFromServer()
        {
            using var activity = _activitySource.StartActivity();
            activity?.SetTag("TestTag", "Value one");
            activity?.SetTag("TestTag2", "Value two");
            activity?.SetTag("TestTag3", "Value three");

            var response = await _httpClient.GetStringAsync("server");
            var values = JsonSerializer.Deserialize<IEnumerable<string>>(response) ?? Enumerable.Empty<string>();
            return Ok(values);
        }

        [HttpGet]
        [Route("GetError")]
        public async Task<ActionResult<IEnumerable<string>>> GetError()
        {
            using var activity = _activitySource.StartActivity();
            try
            {
                var response = await _httpClient.GetStringAsync("server/values");
                var values = JsonSerializer.Deserialize<IEnumerable<string>>(response) ?? Enumerable.Empty<string>();
                
                var count = 1;
                foreach (var value in values)
                {
                    activity?.SetTag($"value-{count++}", value);
                }

                throw new Exception("This is a contrived exception.");
            }
            catch (Exception ex)
            {
                var errorEvent = new ActivityEvent("application-error", tags: new ActivityTagsCollection() { new("exception", ex) });

                activity?.AddEvent(errorEvent);
                activity?.SetTag("otel.status_code", "ERROR");
                activity?.SetTag("otel.status_description", ex.ToString());

                return new ObjectResult(
                    new
                    {
                        Value = ex.Message,
                        StatusCode = 500
                    });
            }
        }
    }
}
