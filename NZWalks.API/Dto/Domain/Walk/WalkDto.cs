using NZWalks.API.Dto.Domain.Difficulty;
using NZWalks.API.Dto.Domain.Region;

namespace NZWalks.API.Dto.Domain.Walk;

public class WalkDto
{
    public Guid Id { get; init; }
    public required string Name { get; init; }
    public required string Description { get; init; }
    public double LengthInKm { get; init; }
    public string? WalkImageUrl { get; init; }

    // We include the navigation properties to provide detailed information about related entities. We can include these
    // properties from within the WalkDto because:
    //  1. The Walk class has navigation properties for Difficulty and Region, representing relationships in the domain
    //     model.
    //  2. The related entities are included in the query results using the Include method in the 'SqlWalkRepository'
    //     class.
    public required RegionDto Region { get; init; }
    public required DifficultyDto Difficulty { get; init; }
}