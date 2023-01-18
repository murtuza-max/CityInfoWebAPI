using CityInfo.Context;
using CityInfo.Entity;

namespace CityInfo.Services
{
    public class CityInfoRepo : ICityInfoRepo
    {
        private readonly CityInfoContext cityInfoContext;

        public CityInfoRepo(CityInfoContext cityInfoContext) {
            this.cityInfoContext = cityInfoContext; 
        }
        public IEnumerable<City> GetCities()
        {
           return cityInfoContext.cities.OrderBy(c => c.Name).ToList(); 
        }

        public City GetCity(int cityid)
        {
            return cityInfoContext.cities.Where(c => c.Id == cityid).FirstOrDefault();
        } 
        public bool CityExist(int cityid)
        {
            return cityInfoContext.cities.Any(c => c.Id == cityid);
        }

        public PointOfInterest GetPointOfInterestforcity(int cityid, int poiid)
        {
            return cityInfoContext.pointOfInterests.Where(p=> p.Id ==poiid && p.CityId == cityid).FirstOrDefault();
        }

        public IEnumerable<PointOfInterest> GetPointsOfInterestforcity(int cityid)
        {
            return cityInfoContext.pointOfInterests.Where(p => p.CityId == cityid).ToList();
        }

        public void AddPointOfInterestForCity(int cityId, PointOfInterest pointOfInterest)
        {
            var city = GetCity(cityId);
            city.PointsOfInterest.Add(pointOfInterest);
        }

        public bool Save()
        {
            return (cityInfoContext.SaveChanges() >=0);
        }

        public void DeletePointOfInterestForCity(int cityId, PointOfInterest pointOfInterest)
        {
            cityInfoContext.pointOfInterests.Remove(pointOfInterest);
        }
    }
}
