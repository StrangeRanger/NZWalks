// ReSharper disable EntityFramework.ModelValidation.UnlimitedStringLength
namespace NZWalks.API.Models.Domain;

public class Region
{
    public Guid Id { get; init; }
    public required string Code { get; set; }
    public required string Name { get; set; }
    public string? RegionImageUrl { get; set; }
}