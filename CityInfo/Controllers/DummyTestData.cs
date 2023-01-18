using CityInfo.Context;
using Microsoft.AspNetCore.Mvc;

namespace CityInfo.Controllers
{
    [ApiController]
    [Route("cities/testData")]
    public class DummyTestData : ControllerBase
    {
        private CityInfoContext _cityinfo;

        public DummyTestData(CityInfoContext cityInfoContext) {
          _cityinfo = cityInfoContext;
        }

        [HttpGet]
        public IActionResult TestData()
        {
            return Ok();
        }

    }
}
