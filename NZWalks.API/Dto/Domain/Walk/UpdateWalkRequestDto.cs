using System.ComponentModel.DataAnnotations;

namespace NZWalks.API.Dto.Domain.Walk;

public class UpdateWalkRequestDto
{
    [Required]
    [MaxLength(64, ErrorMessage = "The {0} field must be less than {1} characters.")]
    public string Name { get; set; }

    [Required]
    [MaxLength(1000, ErrorMessage = "The {0} field must be less than {1} characters.")]
    public string Description { get; set; }

    [Required]
    [Range(0, 50, ErrorMessage = "The {0} field must be between {1} and {2}.")]
    public double LengthInKm { get; set; }

    public string? WalkImageUrl { get; set; }

    [Required]
    public Guid DifficultyId { get; set; }

    [Required]
    public Guid RegionId { get; set; }
}