using EventInfo.Service;
using Microsoft.AspNetCore.Mvc;

namespace EventInfo.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherData : ControllerBase
    {
        private readonly MqttService service;

        public WeatherData(MqttService service)
        {
            this.service = service;
        }

        [HttpGet]
        [Route("GetAll")]
        public ActionResult<List<WeatherData>> Index()
        {
            var pom = this.service.GetData();
            return Ok(pom);
        }
    }
}
