using AutoMapper;

namespace CityInfo.Profiles
{
    public class CityProfile : Profile
    {
        public CityProfile()
        {
            CreateMap<Entity.City, Models.CityWithoutPointofInterestDto>();
        }
    }
}
