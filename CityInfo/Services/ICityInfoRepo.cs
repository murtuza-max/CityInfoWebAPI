using CityInfo.Entity;

namespace CityInfo.Services
{
    public interface ICityInfoRepo
    {
        IEnumerable<City> GetCities();
        City GetCity(int cityid);
        bool CityExist(int cityid);
        IEnumerable<PointOfInterest> GetPointsOfInterestforcity(int cityid);
        PointOfInterest GetPointOfInterestforcity(int cityid,int poiid);

        void AddPointOfInterestForCity(int cityId,PointOfInterest pointOfInterest);
        void DeletePointOfInterestForCity(int cityId,PointOfInterest pointOfInterest);

        bool Save();
    }
}
