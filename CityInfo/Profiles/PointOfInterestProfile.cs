using AutoMapper;

namespace CityInfo.Profiles
{
    public class PointOfInterestProfile : Profile
    {
        public PointOfInterestProfile()
        {
            CreateMap<Entity.PointOfInterest, Models.PointOfInterestDto>();
            CreateMap<Models.PointOfInterestCreation, Entity.PointOfInterest>();
            CreateMap<Models.PointOfInterestUpdate, Entity.PointOfInterest>()
                .ReverseMap();

        }
    }
}
