using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace ClientWebApi.Controllers
{
    [ApiController]
    [Route("api/[Controller]")]
    public class ClientController
    {
        HttpClient _httpClient;
        public ClientController(HttpClient httpClient)
        {
            _httpClient = httpClient;
            _httpClient.BaseAddress = new Uri("http://localhost:5000/api");
        }

        [HttpGet(Name = "GetValuesFromServer")]
        public async Task<IEnumerable<string>> Get()
        {
            var response = await _httpClient.GetStringAsync("server");
            return JsonSerializer.Deserialize<IEnumerable<string>>(response) ?? Enumerable.Empty<string>();
        }
    }
}
