using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace ClientWebApi.Controllers
{
    [ApiController]
    [Route("api/[Controller]")]
    public class ClientController : ControllerBase
    {
        HttpClient _httpClient;
        public ClientController(HttpClient httpClient)
        {
            _httpClient = httpClient;
            _httpClient.BaseAddress = new Uri("http://serverwebapi/api/");
        }

        [HttpGet(Name = "GetValuesFromServer")]
        public async Task<ActionResult<IEnumerable<string>>> Get()
        {
            try
            {
                var response = await _httpClient.GetStringAsync("server");
                var values = JsonSerializer.Deserialize<IEnumerable<string>>(response) ?? Enumerable.Empty<string>();
                throw new Exception("boom");
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
