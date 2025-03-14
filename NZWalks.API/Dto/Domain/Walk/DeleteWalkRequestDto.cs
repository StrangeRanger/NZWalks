namespace NZWalks.API.Dto.Domain.Walk;

public class DeleteWalkRequestDto
{
    public required string Name { get; set; }
    public required string Description { get; set; }
    public double LengthInKm { get; set; }
    public string? WalkImageUrl { get; set; }
    public Guid DifficultyId { get; set; }
    public Guid RegionId { get; set; }
}