using System.ComponentModel.DataAnnotations;

namespace CityInfo.Models
{
    public class PointOfInterestCreation
    {
        [Required(ErrorMessage ="you should provide name value..")]
        [MaxLength(20)]
        public string Name { get; set; }

        [MaxLength(100)]
        public string Description { get; set; }
    }
}
