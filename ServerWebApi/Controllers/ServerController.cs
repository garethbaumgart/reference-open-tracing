using Microsoft.AspNetCore.Mvc;
using ServerWebApi.Repositories;
using System.Diagnostics;

namespace ServerWebApi.Controllers
{
    [ApiController]
    [Route("api/[Controller]")]
    public class ServerController
    {
        private ActivitySource _activitySource;
        private IValueRepository _valueRepository;

        public ServerController(ActivitySource activitySource, IValueRepository valueRepository)
        {
            _activitySource = activitySource;
            _valueRepository = valueRepository;
        }

        [HttpGet(Name = "GetValuesFromDatabase")]
        public async Task<IEnumerable<string>> Get()
        {
            using var activity = _activitySource.StartActivity("GetValuesFromDatabase");
            var values = await _valueRepository.GetValues();
            activity?.SetTag("ReturnedValues", values);

            return values;
        }
    }
}
