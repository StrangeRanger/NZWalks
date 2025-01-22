using System.ComponentModel.DataAnnotations;

namespace NZWalks.API.Dto.Domain.Region;

public class AddRegionRequestDto
{
    [Required]
    [MinLength(3, ErrorMessage = "Code must be at least 3 characters long")]
    [MaxLength(3, ErrorMessage = "Code must be at most 3 characters long")]
    // Null forgiving symbol used, because non-nullability is already enforced via the Required attribute.
    public string Code { get; set; } = null!;

    [Required]
    [MaxLength(64, ErrorMessage = "Name must be at most 32 characters long")]
    // Null forgiving symbol used, because non-nullability is already enforced via the Required attribute.
    public string Name { get; set; } = null!;

    public string? RegionImageUrl { get; set; }
}