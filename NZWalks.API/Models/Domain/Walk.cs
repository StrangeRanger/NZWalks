// ReSharper disable EntityFramework.ModelValidation.UnlimitedStringLength
namespace NZWalks.API.Models.Domain;

public class Walk
{
    public Guid Id { get; init; }
    public required string Name { get; set; }
    public required string Description { get; set; }
    public double LengthInKm { get; set; }
    public string? WalkImageUrl { get; set; }
    public Guid DifficultyId { get; set; }
    public Guid RegionId { get; set; }

    // ------ Navigation properties: ------ //

    // Indicate to Entity Framework that there is a relationship between the classes/entities, which is represented by
    // foreign keys.
    public required Difficulty Difficulty { get; init; }
    public required Region Region { get; init; }
}