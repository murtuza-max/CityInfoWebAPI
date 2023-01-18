namespace CityInfo.Models
{
    public class CitiesDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int NoofPointofInterest { 
            get 
            {
                return PointOfInterest.Count;
            }  
        }
        public ICollection<PointOfInterestDto> PointOfInterest { get; set; } = new List<PointOfInterestDto>();
    }
}
