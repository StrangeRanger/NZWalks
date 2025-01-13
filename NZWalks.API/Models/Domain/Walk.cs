namespace NZWalks.API.Models.Domain;

public class Walk
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public double LengthInKm { get; set; }
    public string? WalkImageUrl { get; set; }
    public Guid DifficultyId { get; set; }
    public Guid RegionId { get; set; }
    
    /// Navigation properties:
    /// Indicate to Entity Framework that there is a relationship between the classes, which is represented by
    /// foreign keys.
    public Difficulty Difficulty { get; set; }
    public Region Region { get; set; }
}  