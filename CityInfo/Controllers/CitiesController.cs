using AutoMapper;
using CityInfo.Context;
using CityInfo.Entity;
using CityInfo.Models;
using CityInfo.Services;
using Microsoft.AspNetCore.Mvc;

namespace CityInfo.Controllers
{
    [ApiController]
    [Route("api/cities")]
    public class CitiesController : ControllerBase
    {
        private readonly ICityInfoRepo cityInfoRepo;
        private readonly IMapper mappper;

        public CitiesController(ICityInfoRepo cityInfoRepo, IMapper mapper) 
        {
           this.cityInfoRepo = cityInfoRepo;
           this.mappper = mapper;
        }
        [HttpGet] 
        public IActionResult GetCities()
        {
            //var cities = CitiesDataStore.Instance.Cities;
            var cities = cityInfoRepo.GetCities();
            if(cities == null)
            {
                return NotFound();
            }
            //var res = new List<CityWithoutPointofInterestDto>();
            // foreach (var c in cities)
            // {
            //     res.Add(new CityWithoutPointofInterestDto
            //     {
            //         Id = c.Id,
            //         Description= c.Description,
            //         Name= c.Name
            //     });
            // }

            var res = mappper.Map<IEnumerable<CityWithoutPointofInterestDto>>(cities);
            return Ok(res);
        }
        [HttpGet("{id}")] 
        public IActionResult GetCity(int id)
        {
            var city = cityInfoRepo.GetCity(id);
            if (city == null)
            {
                return NotFound();
            }
            return Ok(city);     
        }

    }
}
