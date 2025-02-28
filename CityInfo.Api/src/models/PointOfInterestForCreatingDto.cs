

using System.ComponentModel.DataAnnotations;

namespace CityInfo.Api.src.models;

public class PointOfInterestForCreatingDto
{
    [Required]
    [MaxLength(50)]
    public string? Name { get; set; }
    [Required]
    [MaxLength(200)]
    public string? Description { get; set; }
}


