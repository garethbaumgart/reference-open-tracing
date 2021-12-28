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

        [HttpGet(Name = "GetValuesFromServer")]
        public async Task<ActionResult<IEnumerable<string>>> Get()
        {
            try
            {
                using var activity = _activitySource.StartActivity("GetValuesFromServer");
                activity?.SetTag("TestTag", "Value one");
                activity?.SetTag("TestTag2", "Value two");
                activity?.SetTag("TestTag3", "Value three");

                var response = await _httpClient.GetStringAsync("server");
                var values = JsonSerializer.Deserialize<IEnumerable<string>>(response) ?? Enumerable.Empty<string>();
                //throw new Exception("boom");
                return Ok(values);
            }
            catch (Exception ex)
            {
                // todo add to span log
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
