using AutoMapper;
using CityInfo.Entity;
using CityInfo.Models;
using CityInfo.Services;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using System.Drawing;


namespace CityInfo.Controllers
{
    [ApiController]
    [Route("api/cities/{cityid}/pointOfInterest")]
    public class PointOfInterestController : ControllerBase
    {
        private readonly IMapper mapper;
        private readonly ILogger<PointOfInterestController> _logger;
        private readonly IMailService _localMailservice;
        private readonly ICityInfoRepo _cityInfoRepo;

        public PointOfInterestController(ILogger<PointOfInterestController> _logger, IMailService _localMailService,ICityInfoRepo cityInfoRepo,
            IMapper mapper) // constructor DI
        {
            this.mapper = mapper;
            this._logger= _logger ?? throw new ArgumentNullException(nameof(_logger));
            this._localMailservice= _localMailService ?? throw new ArgumentNullException(nameof(_logger));
            this._cityInfoRepo = cityInfoRepo ?? throw new ArgumentNullException(nameof(_logger));
        }
        [HttpGet("{poiid}")]
        public IActionResult GetPointOfInterestforcity(int cityid, int poiid)
        {
            var city = _cityInfoRepo.CityExist(cityid);
            if (!city)
            {
                _logger.LogInformation($"City with id {cityid} wasn't found when " +
                        $"accessing points of interest.");
                return NotFound();
            }

            var poifc = _cityInfoRepo.GetPointOfInterestforcity(cityid, poiid);
            if (poifc == null)
            {
                _logger.LogInformation($"point of interest with id {poiid} wasn't found when " +
                       $"accessing points of interest.");
                return NotFound();
            }
            //var res = new PointOfInterestDto()
            //{
            //    Id = poifc.Id,
            //    Description = poifc.Description,
            //    Name = poifc.Name

            //};
            var res = mapper.Map<PointOfInterestDto>(poifc);
            return Ok(res);
        }

        [HttpGet]
        public IActionResult GetPointsOfInterestforcity(int cityid)
        {

            var city = _cityInfoRepo.CityExist(cityid);
            if (!city)
            {
                _logger.LogInformation($"City with id {cityid} wasn't found when " +
                       $"accessing points of interest.");
                return NotFound();
            }
            var poisfc = _cityInfoRepo.GetPointsOfInterestforcity(cityid);
            //var res = new List<PointOfInterestDto>();
            //foreach(var poi in poisfc)
            //{
            //    res.Add(new PointOfInterestDto
            //    {
            //        Id= poi.Id,
            //        Description= poi.Description,
            //        Name= poi.Name
            //    });
            //}
            var res = mapper.Map<IEnumerable<PointOfInterestDto>>(poisfc);
            return Ok(res);
        }

        [HttpPost]
        public IActionResult CreatePointOfInterest(int cityId,
            [FromBody] PointOfInterestCreation pointOfInterest)
        {
            if (pointOfInterest.Description == pointOfInterest.Name)
            {
                ModelState.AddModelError(
                    "Description",
                    "The provided description should be different from the name.");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            //var city = CitiesDataStore.Instance.Cities.FirstOrDefault(c => c.Id == cityId);
            //if (city == null)
            //{
            //    return NotFound();
            //}
            if (!_cityInfoRepo.CityExist(cityId))
            {
                return NotFound();
            }

            //var finalPointOfInterest = new PointOfInterestDto()
            //{
            //    Id = ++maxPointOfInterestId,
            //    Name = pointOfInterest.Name,
            //    Description = pointOfInterest.Description
            //};
            var finalPointOfInterest = mapper.Map<Entity.PointOfInterest>(pointOfInterest);

            _cityInfoRepo.AddPointOfInterestForCity(cityId, finalPointOfInterest);
            _cityInfoRepo.Save();
            var createdPointOfInterestToReturn = mapper
                .Map<PointOfInterestDto>(finalPointOfInterest);
            return CreatedAtRoute(
                "GetPointOfInterest",
                new { cityId, id = createdPointOfInterestToReturn.Id },
                createdPointOfInterestToReturn);
        }

        [HttpPut("{id}")]
        public IActionResult UpdatePointOfInterest(int cityId, int id,
           [FromBody] PointOfInterestCreation pointOfInterest)
        {
            if (pointOfInterest.Description == pointOfInterest.Name)
            {
                ModelState.AddModelError(
                    "Description",
                    "The provided description should be different from the name.");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (!_cityInfoRepo.CityExist(cityId))
            {
                return NotFound();
            }

            var poifc = _cityInfoRepo.GetPointOfInterestforcity(cityId, id);
            if (poifc == null)
            {
                _logger.LogInformation($"point of interest with id {id} wasn't found when " +
                       $"accessing points of interest.");
                return NotFound();
            }
            mapper.Map(pointOfInterest, poifc);
            _cityInfoRepo.Save();
            
            return NoContent();
        }

        [HttpPatch("{id}")]
        public IActionResult PartiallyUpdatePointOfInterest(int cityId, int id,
            [FromBody] JsonPatchDocument<PointOfInterestUpdate> patchDoc)
        {
            var city = CitiesDataStore.Instance.Cities
                .FirstOrDefault(c => c.Id == cityId);
            if (city == null)
            {
                return NotFound();
            }

            var pointOfInterestFromStore = city.PointOfInterest
                .FirstOrDefault(c => c.Id == id);
            if (pointOfInterestFromStore == null)
            {
                return NotFound();
            }

            var pointOfInterestToPatch =
                   new PointOfInterestUpdate()
                   {
                       Name = pointOfInterestFromStore.Name,
                       Description = pointOfInterestFromStore.Description
                   };

            patchDoc.ApplyTo(pointOfInterestToPatch,ModelState);

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (pointOfInterestToPatch.Description == pointOfInterestToPatch.Name)
            {
                ModelState.AddModelError(
                    "Description",
                    "The provided description should be different from the name.");
            }

            if (!TryValidateModel(pointOfInterestToPatch))
            {
                return BadRequest(ModelState);
            }

            pointOfInterestFromStore.Name = pointOfInterestToPatch.Name;
            pointOfInterestFromStore.Description = pointOfInterestToPatch.Description;

            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult DeletePointOfInterest(int cityId, int id)
        {
            if (!_cityInfoRepo.CityExist(cityId))
            {
                return NotFound();
            }


            var pointOfInterestFromEntity = _cityInfoRepo.GetPointOfInterestforcity(cityId, id);
               
            if (pointOfInterestFromEntity == null)
            {
                return NotFound();
            }

            _cityInfoRepo.DeletePointOfInterestForCity(cityId, pointOfInterestFromEntity);
            _cityInfoRepo.Save();
            _localMailservice.Send("Point of interest deleted..",$"Point of interest with Name:{pointOfInterestFromEntity.Name} & ID:{pointOfInterestFromEntity.Id} was deleted");
            return NoContent();
        }

    }
}
