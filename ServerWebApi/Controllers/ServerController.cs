using Microsoft.AspNetCore.Mvc;

namespace ServerWebApi.Controllers
{
    [ApiController]
    [Route("api/[Controller]")]
    public class ServerController
    {
        [HttpGet(Name = "GetValuesFromDatabase")]
        public Task<IEnumerable<string>> Get()
        {
            var tempValues = new List<string>() { "Value One", "Value Two" };
            return Task.FromResult(tempValues.AsEnumerable<string>());
        }
    }
}
