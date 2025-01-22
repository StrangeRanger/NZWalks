using System.ComponentModel.DataAnnotations;

namespace NZWalks.API.Dto.Domain.Walk;

public class AddWalkRequestDto
{
    [Required]
    [MaxLength(64, ErrorMessage = "The {0} field must be less than {1} characters.")]
    // Null forgiving symbol used, because non-nullability is already enforced via the Required attribute.
    public string Name { get; set; } = null!;

    [Required]
    [MaxLength(1000, ErrorMessage = "The {0} field must be less than {1} characters.")]
    // Null forgiving symbol used, because non-nullability is already enforced via the Required attribute.
    public string Description { get; set; } = null!;

    [Required]
    [Range(0, 50, ErrorMessage = "The {0} field must be between {1} and {2}.")]
    public double LengthInKm { get; set; }

    public string? WalkImageUrl { get; set; }

    [Required]
    public Guid DifficultyId { get; set; }

    [Required]
    public Guid RegionId { get; set; }
}